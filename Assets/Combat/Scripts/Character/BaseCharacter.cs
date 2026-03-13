using CodeHelper;
using DG.Tweening;
using GOA.UserData;
using GOA.WorldMap;
using NTPackage_old.EventDispatcher;
using Rubik.Common;
using Rubik.Common.AudioHelper;
using Rubik.UI;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace Rubik.Combat
{
    public abstract class BaseCharacter : MonoBehaviour, IMessageHandle
    {
        [SerializeField] protected Animator animator;
        [SerializeField] Transform attackerMoveTarget;
        [SerializeField] GearHolder[] gearHolders;
        [SerializeField] Transform root;
        [SerializeField] ClassAnimator[] classAnimatorOverride;
        [SerializeField] GameObject arrowChooseMain, arrowChooseSub;
        [SerializeField] float moveSpeed = 2;
        CharacterHealthBar healthBar;
        Action onCharacterDoneChoosingAction;
        ActionResponse actionData;
        bool isDefending = false;
        bool isChosen;

        Dictionary<BaseCharacter, (CharacterCombatState attacker, DamageFormula damageFormula, List<BaseEffectSO> effects)> hitTargetInfo =
            new Dictionary<BaseCharacter, (CharacterCombatState attacker, DamageFormula damageFormula, List<BaseEffectSO> effects)>();
        List<VFXInfo> projectileInfo = new List<VFXInfo>();
        [ReadOnly] public CharacterCombatState CharacterState;// { get; protected set; }
        public Vector3 DamageSpawnPosition => arrowChooseMain.transform.position;

        public void Handle(Message message)
        {
            switch (message.type)
            {
                case nameof(CodeHelper.MessageCollection.OnCharacterChooseTarget):
                    {
                        actionData = (ActionResponse)message.data[0];
                        arrowChooseMain.SetActive(actionData.targets.Contains(this) && actionData.mainTarget == this);
                        arrowChooseSub.SetActive(actionData.targets.Contains(this) && actionData.mainTarget != this);
                        isChosen = actionData.mainTarget == this;
                        break;
                    }
                case nameof(CodeHelper.MessageCollection.OnCharacterConfirmAction):
                    {
                        // AI don't send OnCharacterChooseTarget message. So we still have to assign actionData here
                        ResetArrow();
                        if (((ActionResponse)message.data[0]).attacker != this)
                        {
                            break;
                        }
                        actionData = (ActionResponse)message.data[0];
                       
                        switch (actionData.cost.statType)
                        {
                            case StatType.HP:
                                {
                                    if (actionData.cost.isFixed)
                                        CharacterState.hp -= (int)actionData.cost.rate;
                                    else
                                        CharacterState.hp -= (int)Mathf.Max(actionData.cost.rate * CharacterState.maxHp, 1);
                                    if (healthBar != null)
                                        healthBar.UpdateHealth(CharacterState.hp * 1f / CharacterState.maxHp);
                                    Debug.Log("Heal"+ (int)actionData.cost.rate);
                                    if (CharacterState.data.isBoss)
                                    {
                                        MainCombatUI.Instance.UpdateBossHealthBar(CharacterState.hp, CharacterState.maxHp);
                                    }
                                    break;
                                }
                            case StatType.MP:
                                {
                                    if (actionData.cost.isFixed)
                                        CharacterState.mp -= (int)actionData.cost.rate;
                                    else
                                        CharacterState.mp -= (int)Mathf.Max(actionData.cost.rate * CharacterState.maxMp, 1);
                                    if (healthBar != null)
                                        healthBar.UpdateMana(CharacterState.mp * 1f / CharacterState.maxMp);
                                    break;
                                }
                        }
                        onCharacterDoneChoosingAction?.Invoke();
                        onCharacterDoneChoosingAction = null;
                        break;
                    }
                case nameof(CodeHelper.MessageCollection.OnGameWin):
                    {
                        MessageManager.RemoveSubcriber<CodeHelper.MessageCollection.OnGameWin>(this);
                        animator.Play("victory");
                        break;
                    }
                case nameof(CodeHelper.MessageCollection.OnCharacterKilled):
                    {
                        if ((string)message.data[0] == CharacterState.data.ownerInstanceId)
                        {
                            animator.Play("death");
                            this.DelayInvoke(() =>
                            {
                                this.DelayInvoke(() => gameObject.SetActive(false), animator.GetCurrentAnimatorStateInfo(0).length);
                            }, 0);
                        }
                        break;
                    }
                case nameof(CodeHelper.MessageCollection.OnGameStart):
                    {
                        foreach (string skillId in CharacterState.data.skills)
                        {
                            BaseActionSO skill = ActionPool.Instance.GetActionSO(skillId);
                            if (skill == null || !skill.isPassive)
                                continue;
                            List<BaseCharacter> effectTarget;
                            switch (skill.actionTargetType)
                            {
                                case ActionTargetType.EnemyAlive:
                                    effectTarget = skill.GetTargets(this);
                                    break;
                                case ActionTargetType.AllyAlive:
                                    effectTarget = skill.GetTargets(CharacterManager.Instance.GetCharacterObject(CombatManager.Instance.GetAliveEnemies()[0].data.characterInstanceId));
                                    break;
                                default: // Self only
                                    effectTarget = new List<BaseCharacter>() { this };
                                    break;
                            }
                            foreach (BaseEffectSO effect in skill.effects)
                            {
                                foreach (BaseCharacter character in effectTarget)
                                {
                                    character.TakeEffect(new EffectState(effect, CharacterState));
                                }
                            }
                        }
                        break;
                    }
                case nameof(CodeHelper.MessageCollection.OnCharacterStartTurn):
                    if (TryGetComponent(out Collider col))
                    {
                        col.enabled = true;
                    }
                    break;
                case nameof(CodeHelper.MessageCollection.OnAttackHitFrame):
                    {
                        TriggerAttackHitFrame();
                        break;
                    }
            }
        }

        private void Start()
        {
            // Too many prefabs!!! Better just automate this
            if (animator.GetComponent<TriggerEnemyHitAnimation>() == null)
            {
                animator.AddComponent<TriggerEnemyHitAnimation>();
            }

            healthBar = GetComponentInChildren<CharacterHealthBar>();
            if (healthBar == null)
                return;
            healthBar.UpdateHealth((float)CharacterState.hp * 1f / CharacterState.maxHp);
            healthBar.UpdateMana((float)CharacterState.mp * 1f / CharacterState.maxMp);
        }

        private void OnEnable()
        {
            arrowChooseMain.SetActive(false);
            arrowChooseSub.SetActive(false);
            MessageManager.AddSubcriber<CodeHelper.MessageCollection.OnGameWin>(this);
            MessageManager.AddSubcriber<CodeHelper.MessageCollection.OnCharacterConfirmAction>(this);
            MessageManager.AddSubcriber<CodeHelper.MessageCollection.OnCharacterChooseTarget>(this);
            MessageManager.AddSubcriber<CodeHelper.MessageCollection.OnCharacterKilled>(this);
            MessageManager.AddSubcriber<CodeHelper.MessageCollection.OnGameStart>(this);
            MessageManager.AddSubcriber<CodeHelper.MessageCollection.OnCharacterStartTurn>(this);
        }

        private void OnDisable()
        {
            MessageManager.RemoveSubcriber<CodeHelper.MessageCollection.OnGameWin>(this);
            MessageManager.RemoveSubcriber<CodeHelper.MessageCollection.OnCharacterConfirmAction>(this);
            MessageManager.RemoveSubcriber<CodeHelper.MessageCollection.OnCharacterChooseTarget>(this);
            MessageManager.RemoveSubcriber<CodeHelper.MessageCollection.OnCharacterKilled>(this);
            MessageManager.RemoveSubcriber<CodeHelper.MessageCollection.OnGameStart>(this);
            MessageManager.RemoveSubcriber<CodeHelper.MessageCollection.OnCharacterStartTurn>(this);
        }

        public virtual void SetupData(CharacterCombatState characterState)
        {
            this.CharacterState = characterState;
            animator.runtimeAnimatorController = classAnimatorOverride.First(e => e.name == characterState.data.characterClass).animatorOverride;
            try
            {
                Rubik.Common.AudioHelper.TriggerAudio triggerAudio = animator.GetComponent<Rubik.Common.AudioHelper.TriggerAudio>();
                switch (WeaponTypeParse.FromString(characterState.data.characterClass))
                {
                    case WeaponType.Sword:
                        triggerAudio.NameSound = Rubik.Common.AudioHelper.AudioName.P_Atk_Sword;
                        break;
                    case WeaponType.Bow:
                        triggerAudio.NameSound = Rubik.Common.AudioHelper.AudioName.P_Atk_Bow;
                        break;
                    case WeaponType.Gun:
                        triggerAudio.NameSound = Rubik.Common.AudioHelper.AudioName.P_Atk_Gun;
                        break;
                    case WeaponType.Mace:
                        triggerAudio.NameSound = Rubik.Common.AudioHelper.AudioName.P_Atk_Mace;
                        break;
                    case WeaponType.Staff:
                        triggerAudio.NameSound = Rubik.Common.AudioHelper.AudioName.P_Atk_Staff;
                        break;
                    default:
                        triggerAudio.NameSound = Rubik.Common.AudioHelper.AudioName.P_Atk_Default;
                        break;
                }
            }
            catch (System.Exception)
            {
                
            }
            foreach (GearState gear in characterState.data.gears)
            {
                if (gear.gearType == (int)GearSlot.Accessory)
                    continue;
                LoadGear(gear.id);
            }
        }

        public void SetAnimator(string className){
            animator.runtimeAnimatorController = classAnimatorOverride.First(e => e.name == className).animatorOverride;
        }

        public void LoadGear(string id)
        {
            GearObject gearGO = Instantiate(AssetLoader.Instance.GetAsset(id), transform).GetComponent<GearObject>();

            // TODO: This logic not work for accessory. Must chagne logic when accessory can be worn
            foreach (GearHolder gearHolder in gearHolders)
            {
                gearHolder.holder.localScale = Vector3.one;
                if(gearHolder.weaponType == WeaponType.Shield)
                {
                    gearHolder.holder.localScale = new Vector3(1,-1,1);
                }
                if (gearHolder.gearType != gearGO.gearType)
                    continue;
                for (int i = gearHolder.holder.childCount - 1; i > -1; i--)
                {
                    if (gearHolder.holder.GetChild(i).TryGetComponent(out GearObject gear) && gear.gearType == gearGO.gearType)
                    {
                        Destroy(gearHolder.holder.GetChild(i).gameObject);
                    }
                }
            }

            Transform parent = gearHolders.First(e => e.gearType == gearGO.gearType && e.weaponType == gearGO.weaponType).holder;
            // Move new gear to its holder
            gearGO.transform.SetParent(parent);
            if (gearGO.skinnedMesh != null)
            {
                UpdateSkinnedMeshRealTime.UpdateSkinnedMesh(gearGO.skinnedMesh, root);
            }
            gearGO.gameObject.transform.localPosition = Vector3.zero;
            gearGO.gameObject.transform.localRotation = Quaternion.identity;
            gearGO.gameObject.transform.localScale = Vector3.one;
            gearGO.name = id;
        }

        public void LoadGearForUI(string id)
        {
            Debug.LogWarning(id);
            GearObject gearGO = Instantiate(AssetLoader.Instance.GetAsset(id), transform).GetComponent<GearObject>();
            UnloadGear(gearGO.gearType);
            // Move new gear to its holder
            Transform parent = gearHolders.First(e => e.gearType == gearGO.gearType && e.weaponType == gearGO.weaponType).holder;
            gearGO.transform.SetParent(parent);
            if (gearGO.skinnedMesh != null)
            {
                UpdateSkinnedMeshRealTime.UpdateSkinnedMesh(gearGO.skinnedMesh, root);
            }
            gearGO.gameObject.transform.localPosition = Vector3.zero;
            gearGO.gameObject.transform.localRotation = Quaternion.identity;
            gearGO.gameObject.transform.localScale = Vector3.one;
            gearGO.name = id;
            int LayerIgnoreRaycast = LayerMask.NameToLayer("UI");
            gearGO.gameObject.layer = LayerIgnoreRaycast;
            foreach (Transform child in gearGO.gameObject.transform)
            {
                child.gameObject.layer = LayerIgnoreRaycast;
                Transform _HasChildren = child.GetComponentInChildren<Transform>();
            }
            gearGO.transform.parent.localScale = Vector3.one;
        }

        public void UnloadGear(GearSlot gearSlot)
        {
            foreach (GearHolder gearHolder in gearHolders)
            {
                if (gearHolder.gearType != gearSlot)
                    continue;
                for (int i = gearHolder.holder.childCount - 1; i > -1; i--)
                {
                    if (gearHolder.holder.GetChild(i).TryGetComponent(out GearObject gear) && gear.gearType == gearSlot)
                    {
                        Destroy(gearHolder.holder.GetChild(i).gameObject);
                    }
                }
            }
        }

        public void ActivateTurn()
        {
            isDefending = false;
            MessageManager.SendMessage(new Message(nameof(CodeHelper.MessageCollection.OnCharacterStartTurn)));
        }

        private void EndTurn()
        {
            MessageManager.SendMessage(new Message(nameof(CodeHelper.MessageCollection.OnCharacterEndTurn),
                                                    new object[] { CharacterState.data.characterInstanceId }));
        }

        public void ApplyStartTurnEffect(Action onDone)
        {
            StartCoroutine(IEApplyStartTurnEffects(onDone));
        }

        private IEnumerator IEApplyStartTurnEffects(Action onDone)
        {
            Dictionary<string, int> effectStacks = new Dictionary<string, int>();
            int hpBeforeEffect = CharacterState.hp;
            int mpBeforeEffect = CharacterState.mp;
            foreach (EffectState effect in CharacterState.effects)
            {
                effect.ActivateOnStartTurn(CharacterState);
            }
            CharacterState.effects.RemoveAll(e => e.turnLeft <= 0);

            if (CharacterState.hp < hpBeforeEffect || CharacterState.mp < mpBeforeEffect)
            {
                animator.Play("gethit");
                yield return null; // wait for animator to change state to get info right
                yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
            }

            if (CharacterState.data.isPlayer)
                MainCombatUI.Instance.UpdatePlayerStatus();
            CheckDeath();

            onDone?.Invoke();
        }

        public void ApplyEndTurnEffect(Action onDone)
        {
            StartCoroutine(IEApplyEndTurnEffects(onDone));
        }

        private IEnumerator IEApplyEndTurnEffects(Action onDone)
        {
            Dictionary<string, int> effectStacks = new Dictionary<string, int>();
            int hpBeforeEffect = CharacterState.hp;
            int mpBeforeEffect = CharacterState.mp;
            foreach (EffectState effect in CharacterState.effects)
            {
                effect.ActivateOnEndTurn(CharacterState);
            }
            CharacterState.effects.RemoveAll(e => e.turnLeft <= 0);

            if (CharacterState.hp < hpBeforeEffect || CharacterState.mp < mpBeforeEffect)
            {
                animator.Play("gethit");
                yield return null; // wait for animator to change state to get info right
                yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
            }

            if (CharacterState.data.isPlayer)
                MainCombatUI.Instance.UpdatePlayerStatus();
            CheckDeath();

            onDone?.Invoke();
        }

        public void ChooseAction(Action onDone)
        {
            onCharacterDoneChoosingAction = onDone;
            if (CharacterState.data.isAI)
            {
                ChooseActionAI(); 
               CameraTrackController.Instance.SetPlayerCam();
            }
            else
            {
                CameraTrackController.Instance.SetBotCam();
            }
        }

        public void Action(Action onDone)
        {
            if (TryGetComponent(out Collider col))
            {
                col.enabled = false;
            }
            StartCoroutine(IEDoAction(onDone));
        }

        private IEnumerator IEDoAction(Action onDone)
        {
            // Camera moves to attacker and overlay effect shows
            // Note: Wait a bit more to fake choose action time
            if (CharacterState.data.isAI)
            {
                yield return new WaitForSeconds(1f);
            }
            else
            {
                yield return new WaitForSeconds(0.5f);
            }

            if (actionData.actionId == Constants.ID.DEFEND_ID)
            {
                Defend();
                yield return new WaitForSeconds(0.1f);
                onDone?.Invoke();
                yield break;
            }

            // Setup so TriggerEnemyHitAnimation will trigger when the right frame comes
            projectileInfo.Clear();
            MessageManager.AddSubcriber<CodeHelper.MessageCollection.OnAttackHitFrame>(this);
            hitTargetInfo.Clear();
            foreach (BaseCharacter enemy in actionData.targets)
            {
                SetupTargetHitInfo(enemy, CharacterState, actionData.damageFormula, actionData.effects);
            }

            // TODO: Camera Change
            // Phase 1: Ready or charge skill
            
            if (!string.IsNullOrEmpty(actionData.attackAnimationInfo[0].animationName))
            {
                animator.Play(actionData.attackAnimationInfo[0].animationName);
                foreach (VFXInfo vFXInfo in actionData.attackAnimationInfo[0].vfxInfo)
                {
                    if (vFXInfo.spawnTiming == SpawnTimingType.OnStartAnimation)
                        SpawnVfx(vFXInfo);
                }
                yield return null; // wait for animator to change state to get info right
                yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
                foreach (VFXInfo vFXInfo in actionData.attackAnimationInfo[0].vfxInfo)
                {
                    if (vFXInfo.spawnTiming == SpawnTimingType.OnEndAnimation)
                        SpawnVfx(vFXInfo);
                }
            }
            
            // Phase 2: Run to target
            if (!actionData.isLongRange)
            {
                string animName = actionData.attackAnimationInfo[1].animationName;
                if (string.IsNullOrEmpty(animName))
                {
                    animName = "run";
                }
                animator.Play(animName);
               
                foreach (VFXInfo vFXInfo in actionData.attackAnimationInfo[1].vfxInfo)
                {
                    if (vFXInfo.spawnTiming == SpawnTimingType.OnStartAnimation)
                        SpawnVfx(vFXInfo);
                }
                if (actionData.actionId == CharacterState.data.skills[0])
                {
                    yield return MoveToTarget(actionData.targets[0].attackerMoveTarget.position, moveSpeed, false);
                }
                else
                {
                    yield return MoveToTarget((actionData.targets[0].attackerMoveTarget.position + transform.parent.position) / 2, moveSpeed, false);
                }
                foreach (VFXInfo vFXInfo in actionData.attackAnimationInfo[1].vfxInfo)
                {
                    if (vFXInfo.spawnTiming == SpawnTimingType.OnEndAnimation)
                        SpawnVfx(vFXInfo);
                }
            }
           
           
            if (actionData.targets[0] != this)
            {
                transform.DOLookAt(actionData.targets[0].transform.position, 0.1f, AxisConstraint.Y);
            }

            // Phase 3: Attack
            animator.Play(actionData.attackAnimationInfo[2].animationName);
            Debug.Log("Animation  name : " + actionData.attackAnimationInfo[2].animationName + " long range : " + actionData.isLongRange);
            foreach (VFXInfo vFXInfo in actionData.attackAnimationInfo[2].vfxInfo)
            {
                if (vFXInfo.spawnTiming == SpawnTimingType.OnStartAnimation)
                    SpawnVfx(vFXInfo);
                else if (vFXInfo.spawnTiming == SpawnTimingType.Manual)
                    projectileInfo.Add(vFXInfo);
            }
            yield return null; // wait for animator to change state to get info right
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
            foreach (VFXInfo vFXInfo in actionData.attackAnimationInfo[2].vfxInfo)
            {
                if (vFXInfo.spawnTiming == SpawnTimingType.OnEndAnimation)
                    SpawnVfx(vFXInfo);
            }

            // Phase 4: Get Back to original position
            if (!string.IsNullOrEmpty(actionData.attackAnimationInfo[3].animationName))
            {
                animator.Play(actionData.attackAnimationInfo[3].animationName);
                foreach (VFXInfo vFXInfo in actionData.attackAnimationInfo[3].vfxInfo)
                {
                    if (vFXInfo.spawnTiming == SpawnTimingType.OnStartAnimation)
                        SpawnVfx(vFXInfo);
                }
            }
            else if (!actionData.isLongRange)
            {
                animator.Play("run");
            }

            if (!actionData.isLongRange)
            {
                yield return MoveToTarget(transform.parent.position, moveSpeed, actionData.attackAnimationInfo[3].flipDirection);
                animator.Play("idle");
            }
            transform.DOLocalRotateQuaternion(Quaternion.identity, 0.25f);
            foreach (VFXInfo vFXInfo in actionData.attackAnimationInfo[3].vfxInfo)
            {
                if (vFXInfo.spawnTiming == SpawnTimingType.OnEndAnimation)
                    SpawnVfx(vFXInfo);
            }

            onDone?.Invoke();
            yield return null;
            MessageManager.RemoveSubcriber<CodeHelper.MessageCollection.OnAttackHitFrame>(this);
        }
        private void SetupTargetHitInfo(BaseCharacter target, CharacterCombatState attacker, DamageFormula damageFormula, List<BaseEffectSO> effects)
        {
            hitTargetInfo.Add(target, (attacker, damageFormula, effects));
        }

        private void TriggerAttackHitFrame()
        {
            Debug.Log(projectileInfo.Count);
            if (projectileInfo.Count == 0)
            {
                foreach (BaseCharacter target in hitTargetInfo.Keys)
                {
                    TriggerEnemyHitAnimation(target);
                }
            }
            else
            {
                foreach (VFXInfo projectileVfx in projectileInfo)
                {
                    SpawnVfx(projectileVfx);
                }
            }
        }

        public void TriggerEnemyHitAnimation(BaseCharacter target)
        {
            target.TakeHit(hitTargetInfo[target].attacker, hitTargetInfo[target].damageFormula, hitTargetInfo[target].effects);
        }

        private void SpawnVfx(VFXInfo vfxInfo)
        {
            Debug.Log("Spawn vfx : " + vfxInfo.prefab.name);
            if (vfxInfo.spawnOnSelf)
            {
                GameObject particle = ObjectPool.Spawn(vfxInfo.prefab);
                particle.transform.position = transform.position;
                particle.transform.rotation = transform.rotation;
                this.DelayInvoke(particle.Recycle, particle.GetComponentInChildren<ParticleSystem>().main.duration);

                if (!particle.TryGetComponent(out FlyToTarget particleFly))
                {
                    return;
                }
                particleFly.SetTarget(actionData.targets[0].transform, () => TriggerEnemyHitAnimation(actionData.targets[0]));

                // Spawn more projectile if needed
                for (int i = 1; i < actionData.targets.Count; i++)
                {
                    GameObject subParticle = ObjectPool.Spawn(vfxInfo.prefab);
                    subParticle.transform.position = transform.position;
                    subParticle.transform.rotation = transform.rotation;
                    this.DelayInvoke(subParticle.Recycle, subParticle.GetComponentInChildren<ParticleSystem>().main.duration);
                    if (subParticle.TryGetComponent(out particleFly))
                    {
                        particleFly.SetTarget(actionData.targets[i].transform, () => TriggerEnemyHitAnimation(actionData.targets[i]));
                    }
                }
            }
            else if (vfxInfo.spawnOnAllEnemy)
            {
                GameObject particle = ObjectPool.Spawn(vfxInfo.prefab);
                StageObject stage = FindObjectOfType<StageObject>();
                Vector3 center = Vector3.zero;
                if (actionData.targets[0].CharacterState.data.isEnemy)
                {
                    foreach (StageObject.PositionHolder enemyHolder in stage.enemyHolders)
                    {
                        center += enemyHolder.characterHolder.position;
                    }
                    center = center * 1f / stage.enemyHolders.Length;
                }
                else
                {
                    foreach (StageObject.PositionHolder allyHolder in stage.allyHolders)
                    {
                        center += allyHolder.characterHolder.position;
                    }
                    center = center * 1f / stage.allyHolders.Length;

                }
                particle.transform.position = center;
                particle.transform.rotation = Quaternion.identity;
                this.DelayInvoke(particle.Recycle, particle.GetComponentInChildren<ParticleSystem>().main.duration);
                if (particle.TryGetComponent(out TimerTriggerHit delayHit))
                {
                    delayHit.SetupData(() =>
                    {
                        foreach (BaseCharacter target in hitTargetInfo.Keys)
                        {
                            TriggerEnemyHitAnimation(target);
                        }
                    });
                }
            }
            else
            {
                foreach (BaseCharacter target in actionData.targets)
                {
                    GameObject particle = ObjectPool.Spawn(vfxInfo.prefab);
                    if (particle == null)
                    {
                        Debug.LogError(vfxInfo.prefab.name + " not exist in pool");
                        continue;
                    }
                    particle.transform.position = target.transform.position;
                    particle.transform.rotation = target.transform.rotation;
                    this.DelayInvoke(particle.Recycle, particle.GetComponentInChildren<ParticleSystem>().main.duration);
                    if (particle.TryGetComponent(out TimerTriggerHit delayHit))
                    {
                        delayHit.SetupData(() =>
                        {
                            TriggerEnemyHitAnimation(target);
                        });
                    }
                }
            }
        }

        protected virtual void Defend()
        {
            animator.Play("Defend");
            isDefending = true;
        }

        private IEnumerator MoveToTarget(Vector3 target, float speed, bool flipLookAt)
        {
            transform.LookAt(target, Vector3.up);
            if (flipLookAt)
            {
                transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y + 180, 0);
            }
            bool completeMove = false;
            transform.DOMove(target, speed*UserData.Instance.movespeed).SetEase(Ease.Linear).SetSpeedBased().OnComplete(() => completeMove = true);
            while (!completeMove)
            {
                yield return null;
            }
        }

        private void TakeHit(CharacterCombatState attacker, DamageFormula damageFormula, List<BaseEffectSO> effects)
        {
            // Check evading attack
            // float hitRate = attacker.hit - CharacterState.evasion;
            float hitRate = attacker.hit -CharacterState.evasion;
            float rnd = UnityEngine.Random.Range(0, 1);
            //if (rnd > hitRate)
            //{
            //    ObjectPool.GetPrefabByName("MissText").GetComponent<DamageNumbersPro.DamageNumberMesh>()
            //                .Spawn(DamageSpawnPosition);
            //    return;
            //}
            
            // If attack hits
            int damage = (int)(damageFormula(attacker, CharacterState) * GetCritRatio(attacker.critRate, attacker.critDamage));
            if (isDefending)
                damage = Mathf.FloorToInt(damage * 0.5f);
            if (attacker.data.isPlayer && !String.IsNullOrEmpty(UserData.Instance.DataInCombat.PortalId))
            {
                if(damage > 0) UserData.Instance.TotalDameInPortal += damage;
            }
            if (attacker.data.isCompanion && !String.IsNullOrEmpty(UserData.Instance.DataInCombat.PortalId))
            {
                if(damage > 0) UserData.Instance.TotalDameInPortal += damage;
            }
            CharacterState.hp = Mathf.Clamp(CharacterState.hp - damage, 0, CharacterState.maxHp);
            if (healthBar != null)
            {
                healthBar.UpdateHealth(CharacterState.hp * 1f / CharacterState.maxHp);
            }
            if (CharacterState.data.isBoss)
            {
                MainCombatUI.Instance.UpdateBossHealthBar(CharacterState.hp, CharacterState.maxHp);
            }
            if (CharacterState.data.isPlayer)
            {
                MainCombatUI.Instance.UpdatePlayerStatus();
            }

            //if (damage == 0)
            //{
            //    // TODO: Effect invincible
            //    print("No damage");
            //}
            //else if (damage > 0)
            //{
            //    animator.Play("gethit");
            //    ObjectPool.GetPrefabByName("DamageNumber").GetComponent<DamageNumbersPro.DamageNumberMesh>()
            //                .Spawn(DamageSpawnPosition, damage);
            //}
            //else
            //{
            //    // TODO: animation heal
            //    Debug.Log("heal number : " + damage);
            //    animator.Play("idle");
            //    AudioCtrl.instance.Play(AudioName.Heal_Sound); 
            //    ObjectPool.GetPrefabByName("HealNumber").GetComponent<DamageNumbersPro.DamageNumberMesh>()
            //                .Spawn(DamageSpawnPosition, -damage);
            //}
            foreach (BaseEffectSO effectSO in effects)
            {
                EffectState effect = new EffectState(effectSO, attacker);
                TakeEffect(effect);
            }
            CheckDeath();
        }

        public void CheckDeath()
        {
            if (CharacterState.hp <= 0)
            {
                Debug.Log("Mob death " + CharacterState.data.typeMob);
                MessageManager.SendMessage(new Message(nameof(CodeHelper.MessageCollection.OnCharacterKilled),
                                                        new object[] { CharacterState.data.characterInstanceId }));
                animator.Play("death");
                this.DelayInvoke(() =>
                {
                    this.DelayInvoke(() => gameObject.SetActive(false), animator.GetCurrentAnimatorStateInfo(0).length - 0.1f);
                }, 0.1f);
                if(CharacterState.data.typeMob != MobTypeCode.unknowType)
                {
                    QuestManager.Instance.UpdateQuest(QuestType.Kill_Monster, 1);
                    switch (CharacterState.data.typeMob)
                    {
                        case MobTypeCode.RegularMobs:
                            QuestManager.Instance.UpdateQuest(QuestType.Kill_Regular_Mobs, 1);
                            break;
                        case MobTypeCode.GreaterMobs:
                            QuestManager.Instance.UpdateQuest(QuestType.Kill_Greater_Mobs, 1);
                            break;
                        default:
                            break;

                    }
                }
                    
            }
        }

        private float GetCritRatio(float critRate, float critDamage)
        {
            float rnd = UnityEngine.Random.Range(0, 100f);
           // Debug.LogWarning("Crittttt : " + rnd + "  - " +critRate);
            if (rnd < (int)(critRate*100))
            {
                print("CRIT!!!!");
                return critDamage;
            }
            return 1;
        }

        public void TakeEffect(EffectState effect)
        {
            int oldHP = CharacterState.hp;
            int oldMP = CharacterState.mp;
            effect.ActivateOnTaken(CharacterState);
            //if (CharacterState.hp > oldHP)
            //{
            //    ObjectPool.GetPrefabByName("HealNumber").GetComponent<DamageNumbersPro.DamageNumberMesh>()
            //                .Spawn(arrowChooseMain.transform.position, CharacterState.hp - oldHP);
            //}
            //if (CharacterState.mp > oldMP)
            //{
            //    ObjectPool.GetPrefabByName("HealMPNumber").GetComponent<DamageNumbersPro.DamageNumberMesh>()
            //                .Spawn(arrowChooseMain.transform.position, CharacterState.mp - oldMP);
            //}
            if (healthBar != null)
            {
                healthBar.UpdateHealth(CharacterState.hp * 1f / CharacterState.maxHp);
                healthBar.UpdateMana(CharacterState.mp * 1f / CharacterState.maxMp);
            }
            if (CharacterState.data.isBoss)
            {
                MainCombatUI.Instance.UpdateBossHealthBar(CharacterState.hp, CharacterState.maxHp);
            }


            if (effect.turnLeft == 0)
            {
                return;
            }
            BaseEffectSO effectSO = EffectPool.Instance.GetEffectSO(effect.effectId);
            if (effectSO.stackable)
            {
                CharacterState.effects.Add(effect);
            }
            else
            {
                foreach (EffectState effectState in CharacterState.effects)
                {
                    if (effectState.effectId == effect.effectId)
                    {
                        effectState.Deactivate(CharacterState);
                    }
                }
                CharacterState.effects.RemoveAll(e => e.effectId == effect.effectId);
                CharacterState.effects.Add(effect);
            }

            if (CharacterState.data.isPlayer)
            {
                MainCombatUI.Instance.UpdatePlayerStatus();
            }
        }

        private void CheckEffect(int oldHP, int oldMP)
        {

        }

        /// <summary>
        /// <para> 0 = Player </para>
        /// <para> 1 = Enemy </para>
        /// <para> 2 = Ally </para>
        /// </summary>
        /// <returns></returns>
        public int GetCharacterSideInfo()
        {
            if (CharacterState.data.isPlayer)
                return 0;
            if (CharacterState.data.isEnemy)
                return 1;
            return 2;
        }

        public bool IsCurrentAnimationName(string animName)
        {
            return animator.GetCurrentAnimatorStateInfo(0).IsName(animName);
        }

        // TODO: This should be in an other script
        #region AI

        private void ChooseActionAI()
        {
            CharacterCombatState attacker = CharacterState;
            (string actionId, CharacterCombatState target) = GetRandomMoveAI(CharacterState.data.isEnemy);
            
            ActionRequest request = new ActionRequest()
            {
                actionId = actionId, // Got from move pool
                attacker = this,
                target = CharacterManager.Instance.GetCharacterObject(target.data.characterInstanceId)
            };
            var target1 = CharacterManager.Instance.GetCharacterObject(target.data.characterInstanceId);
            MessageManager.SendMessage(new Message(nameof(CodeHelper.MessageCollection.OnCharacterConfirmAction),
                                                    new object[] { ActionPool.Instance.GetActionRespone(request) }));
        }
       
        private (string actionId, CharacterCombatState target) GetRandomMoveAI(bool isEnemy)
        {
            int rd = UnityEngine.Random.Range(0, 100);
            Debug.Log("random number : " + rd);
            int indexOfAction = 0;
            float rate = 0;
            for (int i = 0; i < CharacterState.data.skillsRate.Count; i++)
            {
                rate += CharacterState.data.skillsRate[i];
                if (rd< rate * 100)
                {
                    indexOfAction = i;
                    break;
                }
            }
            string actionId = CharacterState.data.skills[indexOfAction];
            List<CharacterCombatState> targetsAvailable;
            switch (ActionPool.Instance.GetActionSO(actionId).actionTargetType)
            {
                default:
                    targetsAvailable = new List<CharacterCombatState>() { CharacterState };
                    break;
                case ActionTargetType.Self:
                    targetsAvailable = new List<CharacterCombatState>() { CharacterState };
                    break;
                case ActionTargetType.AllyAlive:
                    if (isEnemy)
                        targetsAvailable = CombatManager.Instance.GetAliveEnemies();
                    else
                        targetsAvailable = CombatManager.Instance.GetAliveAllies();
                    break;
                case ActionTargetType.AllyDead:
                    if (isEnemy)
                        targetsAvailable = CombatManager.Instance.GetDeadEnemies();
                    else
                        targetsAvailable = CombatManager.Instance.GetDeadAllies();
                    break;
                case ActionTargetType.EnemyAlive:
                    if (isEnemy)
                        targetsAvailable = CombatManager.Instance.GetAliveAllies();
                    else
                        targetsAvailable = CombatManager.Instance.GetAliveEnemies();
                    break;
                case ActionTargetType.EnemyDead:
                    if (isEnemy)
                        targetsAvailable = CombatManager.Instance.GetDeadAllies();
                    else
                        targetsAvailable = CombatManager.Instance.GetDeadEnemies();
                    break;
                case ActionTargetType.AnyAlive:
                    targetsAvailable = CombatManager.Instance.GetAliveCharacters();
                    break;
                case ActionTargetType.AnyDead:
                    targetsAvailable = CombatManager.Instance.GetDeadCharacters();
                    break;
                case ActionTargetType.Owner:
                    targetsAvailable = new List<CharacterCombatState>() { CombatManager.Instance.GetCharacterCombatState(CharacterState.data.ownerInstanceId) };
                    break;
            }

            CharacterCombatState targetChosen = targetsAvailable.GetRandom();
            return (actionId, targetChosen);
        }

        private (string actionId, CharacterCombatState target) GetRandomMove(bool isEnemy)
        {
            string actionId = CharacterState.data.skills.GetRandom();
            List<CharacterCombatState> targetsAvailable;
            switch (ActionPool.Instance.GetActionSO(actionId).actionTargetType)
            {
                default:
                    targetsAvailable = new List<CharacterCombatState>() { CharacterState };
                    break;
                case ActionTargetType.Self:
                    targetsAvailable = new List<CharacterCombatState>() { CharacterState };
                    break;
                case ActionTargetType.AllyAlive:
                    if (isEnemy)
                        targetsAvailable = CombatManager.Instance.GetAliveEnemies();
                    else
                        targetsAvailable = CombatManager.Instance.GetAliveAllies();
                    break;
                case ActionTargetType.AllyDead:
                    if (isEnemy)
                        targetsAvailable = CombatManager.Instance.GetDeadEnemies();
                    else
                        targetsAvailable = CombatManager.Instance.GetDeadAllies();
                    break;
                case ActionTargetType.EnemyAlive:
                    if (isEnemy)
                        targetsAvailable = CombatManager.Instance.GetAliveAllies();
                    else
                        targetsAvailable = CombatManager.Instance.GetAliveEnemies();
                    break;
                case ActionTargetType.EnemyDead:
                    if (isEnemy)
                        targetsAvailable = CombatManager.Instance.GetDeadAllies();
                    else
                        targetsAvailable = CombatManager.Instance.GetDeadEnemies();
                    break;
                case ActionTargetType.AnyAlive:
                    targetsAvailable = CombatManager.Instance.GetAliveCharacters();
                    break;
                case ActionTargetType.AnyDead:
                    targetsAvailable = CombatManager.Instance.GetDeadCharacters();
                    break;
                case ActionTargetType.Owner:
                    targetsAvailable = new List<CharacterCombatState>() { CombatManager.Instance.GetCharacterCombatState(CharacterState.data.ownerInstanceId) };
                    break;
            }

            CharacterCombatState targetChosen = targetsAvailable.GetRandom();
            return (actionId, targetChosen);
        }

        #endregion
        [ContextMenu("Touch")]
        public void Touch(){
            this.OnMouseUpAsButton();
        }
        #region Interactive
        private void OnMouseUpAsButton()
        {
            if (CombatManager.Instance.CurrentGameState.GetTurnType() == typeof(StateChooseAction))
            {
                switch (MainCombatUI.CurrentState)
                {
                    case ChooseActionState.Attack:
                    case ChooseActionState.ChooseSkill:
                    case ChooseActionState.Defend:
                        {
                            if (string.IsNullOrEmpty(MainCombatUI.CurrentActionId))
                                break;

                            if (!CanMoveBeUsed(MainCombatUI.CurrentActionId))
                                break;

                            if (isChosen)
                            {
                                // TODO: Subtract MP and skill pool or something
                            }
                            ChooseThisAsTarget(MainCombatUI.CurrentActionId);
                            break;
                        }
                    case ChooseActionState.ChooseItem:
                        {
                            EventListenerManager.instance.PostEvent(EventCode.OffAttackSound);
                            if (string.IsNullOrEmpty(MainCombatUI.CurrentItemId))
                                break;

                            if (!CanMoveBeUsed(MainCombatUI.CurrentItemId))
                                break;

                            if (isChosen)
                            {
                                CombatManager.Instance.CurrentCharacterState.AddItem(MainCombatUI.CurrentItemId, -1);
                                MainCombatUI.Instance.UpdateItem(MainCombatUI.CurrentItemId);
                            }
                            ChooseThisAsTarget(MainCombatUI.CurrentItemId);
                            break;
                        }
                }
            }
        }

        private bool CanMoveBeUsed(string moveId)
        {
            // Companions can't be targeted
            if (!string.IsNullOrEmpty(CharacterState.data.ownerInstanceId))
                return false;

            BaseActionSO actionData = ActionPool.Instance.GetActionSO(moveId);
            bool canUseForAlly = actionData.actionTargetType == ActionTargetType.AnyAlive ||
                                 actionData.actionTargetType == ActionTargetType.AllyAlive;
            bool canUseForEnemy = actionData.actionTargetType == ActionTargetType.AnyAlive ||
                                 actionData.actionTargetType == ActionTargetType.EnemyAlive;
            bool canUseForSelf = actionData.actionTargetType == ActionTargetType.AnyAlive ||
                                 actionData.actionTargetType == ActionTargetType.AllyAlive ||
                                 actionData.actionTargetType == ActionTargetType.Self;
            return (canUseForAlly && !CharacterState.data.isEnemy)
                || (canUseForEnemy && CharacterState.data.isEnemy)
                || (canUseForSelf && CombatManager.Instance.CurrentCharacterState == CharacterState);
        }

        private void ChooseThisAsTarget(string actionId)
        {
            CharacterCombatState attacker = CombatManager.Instance.CurrentCharacterState;

            ActionRequest request = new ActionRequest()
            {
                actionId = actionId,
                attacker = CharacterManager.Instance.GetCharacterObject(attacker.data.characterInstanceId),
                target = this
            };
            if (isChosen)
            {
                isChosen = false; // Reset for next turn
                MessageManager.SendMessage(new Message(nameof(CodeHelper.MessageCollection.OnCharacterConfirmAction),
                                                        new object[] { ActionPool.Instance.GetActionRespone(request) }));
            }
            else
            {
                isChosen = true;
                MessageManager.SendMessage(new Message(nameof(CodeHelper.MessageCollection.OnCharacterChooseTarget),
                                                        new object[] { ActionPool.Instance.GetActionRespone(request) }));
            }
        }

        public void ResetArrow()
        {
            isChosen = false;
            arrowChooseMain.SetActive(false);
            arrowChooseSub.SetActive(false);
        }
        #endregion

        [Serializable]
        private class GearHolder
        {
            public GearSlot gearType;
            public WeaponType weaponType;
            public Transform holder;
        }

        [Serializable]
        private class ClassAnimator
        {
            public string name;
            public RuntimeAnimatorController animatorOverride;
        }
    }
}
