using Minimalist.Bar.UI;
using Rubik.BattleEngine;
using Rubik.CardPlayer;
using Rubik.Config;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rubik.Battle
{
    public enum CharacterState
    {
        Idle, Run, Attack, AttackUp, AttackDown, Hit, Die, SkillBuff, SkillAtk, Jump, Win, Box, Box_Open
    }
    public enum StateAttackCharacter
    {
        Wait,
        Attacking,
        Done
    }
    public class CharacterBase : MonoBehaviour
    {
        public delegate void DelegateCharacter();
        public event DelegateCharacter CompleteAttack;
        public CharacterState state;
        public StateAttackCharacter stateAttack = StateAttackCharacter.Wait;
        public BaseCharacterData model;
        public HeroData heroData;
        public CurInfoHero curInfo = new CurInfoHero();
        public ZfxCharacter zfx;
        [SerializeField] GameObject nameTxt;
        //public CharacterInfoInGame infoIngame;
        public CharacterPositionBattle positionStart;
        [SerializeField] BarBhv hpBar;
        [SerializeField] BarBhv apBar;
        public bool isEnemy = false;
        [SerializeField] AnimationCharacterController animCtr;
        public UnityEngine.UI.Image avaCharacter, iconOrigin;
        public Sprite avaSpr;
        public GameObject[] efffectBuff;
        public string heroID;
        public virtual void Begin(bool isHeroSelf)
        {

            //model = _model;
            //positionStart = _pos;
            //transform.position = _pos.position();
            //GenCharacterGraphich();
            //animCtr = GetComponent<AnimationCharacterController>();
            animCtr.Begin();
            positionStart.Begin();
            positionStart.positionHeroSelf = isHeroSelf;
            //animCtr.SetSkeletonData(DataController.getInstance().characters.characters[_model.displayId].skeletonDataAsset);
            animCtr.skeletonAnimation.transform.localScale = model.ScaleData;
            zfx.Begin(isHeroSelf);
            if (!isHeroSelf)
            {
                animCtr.skeletonAnimation.gameObject.transform.localScale = new Vector2(-animCtr.skeletonAnimation.gameObject.transform.localScale.x, animCtr.skeletonAnimation.gameObject.transform.localScale.y);
            }
            if (model.CharacterType == CharacterType.Boss)
            {
                animCtr.skeletonAnimation.transform.localScale = 1.2f * new Vector2(-animCtr.skeletonAnimation.gameObject.transform.localScale.x, animCtr.skeletonAnimation.gameObject.transform.localScale.y); ;
            }
            else if (model.CharacterType == CharacterType.Box)
            {
                animCtr.skeletonAnimation.transform.localScale = .2f * Vector2.one;
            }
            ResetCurInfo();
            ShowGraphic();
        }
        public virtual void SetInfo(BaseCharacterDataSO data)
        {

        }
        public void FlipCharacter()
        {
            animCtr.skeletonAnimation.gameObject.transform.localScale = new Vector2(-animCtr.skeletonAnimation.gameObject.transform.localScale.x, animCtr.skeletonAnimation.gameObject.transform.localScale.y);
        }
        void ResetCurInfo()
        {
            curInfo.hp = model.MaxHP;
            curInfo.ap = model.AP;
            SetAPBar();
        }

        public void SetHeroData(int level, BaseCharacterDataSO data, CardBattleShordData svData = null)
        {

            heroData.level = level;
            heroData.characterData.ATK = data.baseData.ATK * level;
            heroData.characterData.Def = data.baseData.Def * level;
            heroData.characterData.CritRate = data.baseData.CritRate * level;
            heroData.characterData.CurHP = data.baseData.CurHP * level;
            heroData.characterData.MaxHP = data.baseData.MaxHP * level;
            heroData.characterData.Name = data.baseData.Name;
            heroData.characterData.AP = data.baseData.AP;
            heroData.characterData.Star = data.baseData.Star;
            heroData.characterData.Slot = data.baseData.Slot;
            heroData.characterData.Index = data.baseData.Index;
            heroData.characterData.isMultiTarget = data.baseData.isMultiTarget;
            heroData.characterData.CardType = data.baseData.CardType;
            heroData.characterData.CharacterType = data.baseData.CharacterType;
            heroData.characterData.SkillType = data.baseData.SkillType;
            heroData.characterData.AttackAction = data.baseData.AttackAction;
            heroData.characterData.SkillAction = data.baseData.SkillAction;
            heroData.characterData.MoveAction = data.baseData.MoveAction;
            heroData.characterData.ScaleData = data.baseData.ScaleData;
            heroData.characterData.isMoveToEnemyPosition = data.baseData.isMoveToEnemyPosition;
            if (svData != null)
            {
                heroData.characterData.CurHP = svData.CurHP;
                heroData.characterData.MaxHP = svData.MaxHP;
                heroData.characterData.AP = svData.AP;

                //hpBar.Quantity.MaximumAmount = svData.MaxHP;
                //hpBar.Quantity.IsSegmented = true;
                //hpBar.Quantity.SegmentAmount = 1000;
                //hpBar.Quantity.FillAmount = 1;
                //hpBar.GetComponentInParent<Canvas>().sortingLayerName = "Effect";
                //apBar.GetComponentInParent<Canvas>().sortingLayerName = "Effect";
            }

            model = heroData.characterData;
            //CardPlayerManager.Instance.SetSkeletonAnimationData(animCtr.skeletonAnimation, data.baseData.Index);
            animCtr.skeletonAnimation.skeletonDataAsset = data.skeAsset;

            animCtr.skeletonAnimation.Initialize(true);
            if (data.baseData.skinIndex > 0)
                animCtr.skeletonAnimation.Skeleton.SetSkin(data.baseData.skinIndex.ToString());
            animCtr.skeletonAnimation.skeletonDataAsset.GetSkeletonData(true);
            nameTxt.GetComponentInChildren<UnityEngine.UI.Text>().text = data.baseData.Name;
            nameTxt.SetActive(false);

            avaCharacter.sprite = SpriteHelper.Instance.ListOrigins[(int)data.baseData.Origin];
            //SpineEditorUtilities.ReloadSkeletonDataAssetAndComponent(animCtr.skeletonAnimation);
            zfx.SetEffects(data.listEffect);
            // zfx.listSkeleton = data.listSkeleton;
            if (StaticData.isTestMode)
            {
                heroData.characterData.CurHP = data.baseData.CurHP * 1000;
                heroData.characterData.MaxHP = data.baseData.MaxHP * 1000;
            }
        }
        void GenCharacterGraphich()
        {
            //GameObject obj = AxieInit.instance.GetAxie(model.Index).gameObject;
            //animCtr.skeletonAnimation = Instantiate(obj).GetComponent<SkeletonAnimation>();
            // animCtr.Begin();
        }
        public void RessetCharacter(BaseCharacterData _model)
        {
            model = _model;
           // animCtr.SetLayer(1);

        }
        public void OffBar(bool isOff = false)
        {
            hpBar.gameObject.SetActive(isOff);
            apBar.gameObject.SetActive(isOff);
            avaCharacter.gameObject.SetActive(isOff);
        }
        public void Attack()
        {
            animCtr.ChangeAnimation(AnimationCharacterState.Attack);
        }
        public virtual void ChangeState(CharacterState _state)
        {
            //if (state == _state) return;
            state = _state;
            switch (state)
            {
                case CharacterState.Attack:
                    animCtr.ChangeAnimation(AnimationCharacterState.Attack);
                    break;
                case CharacterState.AttackUp:
                    animCtr.ChangeAnimation(AnimationCharacterState.AttackUp);
                    break;
                case CharacterState.AttackDown:
                    animCtr.ChangeAnimation(AnimationCharacterState.AttackDown);
                    break;
                case CharacterState.Idle:
                    animCtr.ChangeAnimation(AnimationCharacterState.Idle);
                    break;
                case CharacterState.Hit:
                    animCtr.ChangeAnimation(AnimationCharacterState.Hit);
                    break;
                case CharacterState.Run:
                    animCtr.ChangeAnimation(AnimationCharacterState.Run);
                    break;
                case CharacterState.Die:
                    animCtr.ChangeAnimation(AnimationCharacterState.Die);
                    break;
                case CharacterState.SkillAtk:
                    animCtr.ChangeAnimation(AnimationCharacterState.SkillAttack);
                    break;
                case CharacterState.SkillBuff:
                    animCtr.ChangeAnimation(AnimationCharacterState.SkillBuff);
                    break;
                case CharacterState.Jump:
                    animCtr.ChangeAnimation(AnimationCharacterState.Jump);
                    break;
                case CharacterState.Win:
                    animCtr.ChangeAnimation(AnimationCharacterState.Win);
                    break;
                case CharacterState.Box:
                    animCtr.ChangeAnimation(AnimationCharacterState.Box);
                    break;
                case CharacterState.Box_Open:
                    animCtr.ChangeAnimation(AnimationCharacterState.Box_Open);
                    break;
            }
            this.SetAPBar();
            this.SetHPBar();
        }
        public virtual void UpdateInfo()
        {
            //APIManager.Instance.
        }
        public virtual void SetHP(float deltaHP, float maxHP)
        {
            curInfo.hp = deltaHP;
            model.MaxHP = maxHP;
            if (curInfo.hp < 0) curInfo.hp = 0;

        }
        public virtual void UpdateHP(float deltaHP)
        {
            curInfo.hp += deltaHP;
            if (curInfo.hp < 0) curInfo.hp = 0;

        }
        public virtual void UpdateDame(float deltaDame)
        {
            curInfo.dame += deltaDame;
            if (curInfo.dame < 0) curInfo.dame = 0;
        }
        public virtual void UpdateAP(float deltaAP)
        {

            if (isEnemy)
            {

                curInfo.ap += deltaAP;
                if (curInfo.ap < 0) curInfo.ap = 0;
                if (curInfo.ap > 100) curInfo.ap = 100;

            }
        }
        public virtual void SetAP(float deltaAP)
        {

            curInfo.ap = deltaAP;
            if (curInfo.ap < 0) curInfo.ap = 0;
            if (curInfo.ap > 100) curInfo.ap = 100;


        }
        public void AddAP(float deltaAP)
        {
            Debug.Log("AP : " + deltaAP);
            curInfo.ap += deltaAP;
            if (curInfo.ap < 0) curInfo.ap = 0;
            SetAPBar();
            // if (curInfo.ap > 100) curInfo.ap = 100;
        }
        public virtual void Die()
        {
            if (model.CharacterType != CharacterType.Box)
                ChangeState(CharacterState.Die);
            else
                ChangeState(CharacterState.Box_Open);
            positionStart.gameObject.SetActive(false);
            //else
            //    ChangeState(CharacterState.Box);
            //HideGraphic();
        }
        public void HideGraphic()
        {
            if (animCtr.skeletonAnimation != null)
                animCtr.skeletonAnimation.gameObject.SetActive(false);
            positionStart.gameObject.SetActive(false);
            hpBar.gameObject.SetActive(false);
            apBar.gameObject.SetActive(false);
            avaCharacter.gameObject.SetActive(false);

            if (nameTxt != null)
                nameTxt.gameObject.SetActive(false);
            foreach (GameObject idBuff in efffectBuff)
            {
                idBuff.SetActive(false);
            }
            ParticleSystem go = ObjectPool.Spawn<ParticleSystem>("DeathEffect", transform.position);
            go.Play();

        }
        public void ShowGraphic()
        {
            //if(animCtr.skeletonAnimation !=null)
            //animCtr.skeletonAnimation.gameObject.SetActive(false);
            positionStart.gameObject.SetActive(true);
            hpBar.gameObject.SetActive(true);
            apBar.gameObject.SetActive(true);
            //  hpBar.transform.localScale = new Vector2(0.003f, 0.003f);
            //  apBar.transform.localScale = new Vector2(0.003f, 0.003f);
            //if (nameTxt != null)
            //    nameTxt.gameObject.SetActive(true);

        }
        public virtual void SetHPBar()
        {
            if (hpBar == null) return;
            if (model.MaxHP != 0)
            {
                hpBar.Quantity.FillAmount = ((float)curInfo.hp / (float)model.MaxHP);//* (float)100;
                if (GameController.Instance.turnController.lsElement.Contains(heroID))
                    GameController.Instance.turnController.lsElement.Get(heroID).SetHpBar(hpBar.Quantity.FillAmount);
            }

            //if (hpBar.Quantity.Amount < 0) hpBar.Quantity.Amount = 0;
        }
        public virtual void SetAPBar()
        {
            if (apBar == null) return;
            apBar.Quantity.FillAmount = (float)((float)curInfo.ap / (float)100);
            if (GameController.Instance.turnController.lsElement.Contains(heroID))
            {
                GameController.Instance.turnController.lsElement.Get(heroID).SetApBar(apBar.Quantity.FillAmount);
            }


            //if (apBar.Quantity.Amount < 0) apBar.Quantity.Amount = 0;
            //if (apBar.Quantity.Amount > 100) apBar.Quantity.Amount = 100;
        }

        public void SetLayer(int layer)
        {
            animCtr.SetLayer(layer);
            //if (apBar.GetComponentInParent<BarCanvasBhv>()!=null)
            //    apBar.GetComponentInParent<BarCanvasBhv>().SetCanvasLayer(layer);
            //if (hpBar.GetComponentInParent<BarCanvasBhv>() != null)
            //    hpBar.GetComponentInParent<BarCanvasBhv>().SetCanvasLayer(layer);
        }
        public SkeletonAnimation GetAnim()
        {
            return animCtr.skeletonAnimation;
        }
    }
    [Serializable]
    public class CurInfoHero
    {
        public float hp;
        public float dame;
        public float ap;
        public float buffAP;
        public float deAP;

    }
}
