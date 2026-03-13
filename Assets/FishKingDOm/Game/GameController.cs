using DG.Tweening;
using GOA.UserData;
using Minimalist.Bar.UI;
using NTPackage;
using NTPackage.Functions;
using Pixelplacement;
using Rubik._2DGPS.Campaign;
using Rubik._2DGPS.UserData;
using Rubik.Battle;
using Rubik.BattleEngine;
using Rubik.CardPlayer;
using Rubik.Combat;
using Rubik.Common.AudioHelper;
using Rubik.Config;
using Rubik.Myrk.Clan;
using Rubik.UserDataPlayer;
using Sirenix.OdinInspector;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public enum GamePlayState
{
    Start,
    PlayerTurn,
    EnemyTurn,
    End
}

public class GameController : MonoBehaviour
{
    public TurnController turnController;
    public EndGameUI endGame;
    public BaseCharacterDataSO baseData, baseEnemy;
    public CardPosition heroPos, enemyPos;
    public GameObject charPreb, skillCover, x2Speed, x2Speed1, apearEffect, gameStart, gameStartPanel, mapGo;
    //  public GameState gameState;
    public BaseCharacterDataSO[] playerData;
    public HeroController[] listCardCharacter, lsCardEnemy;
    HeroController heroCharacter, enemyCharacter;
    public Animator CamAnim;
    [SerializeField] CinemachineCamera cam1, cam2, cam3;
    [SerializeField] BarBhv hpBar;
    Coroutine playerCoroutine, enemyCoroutine;
    public string[] CardTeam = new string[] { "1", "2", "3" };
    public Transform skillPosition, skillRangePosition, skillEnemyRange,skillStartHero,skillStartEnemy;

    public GamePlayState state;
    // UIController uiController;
    // Start is called before the first frame update
    public static GameController Instance;
    public int indexSkill = 0;
    public int SkillCost = -30;
    [SerializeField] BattleShortData battleData;
    public NTDictionary<string, List<HeroController>> characterDics = new NTDictionary<string, List<HeroController>>();
    public DameBossGamePlayUI dameBoss;
    void Awake()
    {
        
        listCardCharacter = new HeroController[9];
        lsCardEnemy = new HeroController[9];
        //StaticData.GameMode = GameMode.Campaign;
        Instance = this;
        Time.timeScale = PlayerPrefs.GetFloat("Speed", 1.5f);
        if (BattleEngineController.Instance.BattleType!= BattleType.Clan)
        {
            battleData = BattleEngineController.Instance.Result;
            dameBoss.gameObject.SetActive(false);
        }
           
        else
        {
            battleData = BattleEngineController.Instance.ClanBattleResult.BattleResult;
            dameBoss.gameObject.SetActive(true);
            dameBoss.Setup(totalDameBoss, ClanManager.Instance.GetClanBossReward(totalDameBoss).AmountReward, ClanManager.Instance.GetClanBossReward(totalDameBoss).MaxDamage);
        }
       if(BattleEngineController.Instance.BattleType == BattleType.Portal)
        {
            AppsFlyerManager.TrackingEvent(AppsflyerEvents.attack_astral_gate, 1, 1);
        }
        else if (BattleEngineController.Instance.BattleType == BattleType.Arena)
        {
            AppsFlyerManager.TrackingEvent(AppsflyerEvents.arena_battle_start, 1, 1);
        }
        else if (BattleEngineController.Instance.BattleType == BattleType.Clan)
        {
            AppsFlyerManager.TrackingEvent(AppsflyerEvents.attack_clan_boss, 1, 1);
        }


    }
    public void OnEvents()
    {
      
       
    }
    int totalDameBoss = 0;
    public void SetTotalDame(int dame)
    {
        totalDameBoss += dame;
        dameBoss.Setup(totalDameBoss, ClanManager.Instance.GetClanBossReward(totalDameBoss).AmountReward, ClanManager.Instance.GetClanBossReward(totalDameBoss).MaxDamage);
    }
    private void Start()
    {

        //if (StaticData.isGameInit)
        {
            StartNewGame();
        }
        if (PlayerPrefs.GetFloat("Speed")  ==1)
        {
            Time.timeScale = 1f;
            isSpeed = false;
            x2Speed.SetActive(isSpeed); x2Speed1.SetActive(isSpeed);
            
        }
        else
        {
            isSpeed = true;
            x2Speed.SetActive(isSpeed); x2Speed1.SetActive(isSpeed);
            Time.timeScale = 1.5f;
           
        }
        //  UIGamePlayBattle.Instance.ShowPanelButton();

    }
    bool isSpeed = false;
    public void OnSpeed()
    {
        isSpeed = !isSpeed;
        x2Speed.SetActive(isSpeed); x2Speed1.SetActive(isSpeed);
        if (isSpeed)
        {
            Time.timeScale = 1.5f;
            PlayerPrefs.SetFloat("Speed", 1.5f);
        }
        else
        {
            Time.timeScale = 1f;
            PlayerPrefs.SetFloat("Speed", 1f);
        }
    }
    public void StartNewGame()
    {

        waveNumber = 0;
       
        StartCoroutine(LoadGame());

    }
    [Button]
    public void StartReLoadGame()
    {
        // this.CardTeam = CardManager.instance.CardTeam;
        StaticData.CardTeam = this.CardTeam;
        waveNumber = 0;

        Debug.Log("Reload game ");
    }
    void SetRsize()
    {
        float Ratio;
        float x = (float)1920 / 1080;
        float y = (float)Screen.width / Screen.height;
        if (x > y)
            Ratio = (y / x);
        else
            Ratio = 1;
        // Debug.Log("tem : " + Ratio);
        mapGo.transform.localScale = new Vector2(mapGo.transform.localScale.x * Ratio, mapGo.transform.localScale.y * Ratio);
    }
    IEnumerator LoadGame()
    {
        SetRsize();
        skillCover.SetActive(false);
        
        yield return StartCoroutine(LoadPlayerCard());
        yield return StartCoroutine(LoadEnemyCard());
        AudioCtrl.Instance.Play(AudioName.UI_Button_Battle_Start);
        foreach (HeroController hero in lsCardEnemy)
        {
            if(hero!=null)
             hero.positionStart.Begin();
        }
        foreach (HeroController hero in listCardCharacter)
        {
            if (hero != null)
                hero.positionStart.Begin();
        }
        turnController.SetState(lsTurn);
        StartGameInServer();

    }
    public int enemyNumer = 0, playerNumber = 0;
    CardPlayerIndex[] lsCardPlayerIndexs = new CardPlayerIndex[10];
    CardPlayerIndex[] lsCardEnemyIndexs = new CardPlayerIndex[10];
    IEnumerator LoadPlayerCard()
    {

        int loadNumber = CardTeam.Length;
        //var lsCardsplayer = CardPlayerManager.Instance.GetCardPlayerByTeam();
        var lsCardsplayer = battleData.TeamA.Cards;
        var herodata = battleData.TeamA.Hero;
        heroCharacter = Rubik.Common.Common.SpawnCardToPosition(charPreb, Vector3.zero, heroPos.lsPos[10].position, heroPos.lsPos[10], 10, null, 0.2f);
        heroCharacter.heroID = herodata.TeamID;
        SetAnimHero(herodata.GearIndexs.ToList(), heroCharacter);

        heroCharacter.Begin(true);
        heroCharacter.OffBar();
        AssetLoader.Instance.MixSkinWithGears(heroCharacter.GetAnim(), herodata.GearIndexs);
        yield return null;
        for (int i = 0; i < lsCardsplayer.Length; i++)
        {
            if (lsCardsplayer[i]._id != null && !lsTurn.ContainsKey(lsCardsplayer[i]._id))
            {
                // var index = (int)CardManager.instance.GetCardByID(CardManager.instance.CardTeam[i]).Index;

                //Debug.Log("Index card : " + index);
                //GameObject effect1 = ObjectPool.Spawn(apearEffect);
                //effect1.transform.position = heroPos.lsPos[lsCardsplayer[i].Slot].position;
                heroPos.lsPos[lsCardsplayer[i].Slot].GetComponent<CharacterPositionBattle>().PlaySpwanEffect();
                yield return null;
                var go = Rubik.Common.Common.SpawnCardToPosition(charPreb, Vector3.zero, heroPos.lsPos[i].position, heroPos.lsPos[lsCardsplayer[i].Slot], lsCardsplayer[i].Slot, null, 0.2f + i * 0.2f);
                listCardCharacter[lsCardsplayer[i].Slot] = (go);
                go.heroIndex = (int)lsCardsplayer[i].Index;
                //go.heroID = CardManager.instance.GetCardByID(CardManager.instance.CardTeam[i])._id;
                go.heroID = lsCardsplayer[i]._id;
                go.SetHeroData(1, CardPlayerManager.Instance.GetCharacterByIndex((int)lsCardsplayer[i].Index), lsCardsplayer[i]);
                lsCardPlayerIndexs[i] = lsCardsplayer[i].Index;
                go.slotID = lsCardsplayer[i].Slot;
                go.star = lsCardsplayer[i].Star;
                go.lv = lsCardsplayer[i].Level;
                go.Begin(true);
                lsTurn.Add(lsCardsplayer[i]._id, go);
                yield return null;

            }

        }
        characterDics.Add(battleData.TeamA.TeamID, listCardCharacter.ToList());


        //AssetLoader.Instance.MixSkin(heroCharacter.GetAnim());
        //heroCharacter.GetAnim().skeleton.SetSkin("basic");
    }//
    public int waveNumber = 0;
    IEnumerator LoadEnemyCard()
    {
      
        var lsCardsplayer = battleData.TeamB.Cards;
        var enemydata = battleData.TeamB.Hero;
        if(enemydata!=null&& !string.IsNullOrEmpty(enemydata._id))
        {
            enemyCharacter = Rubik.Common.Common.SpawnCardToPosition(charPreb, Vector3.zero, enemyPos.lsPos[10].position, enemyPos.lsPos[10], 10, null, 0.2f);
            enemyCharacter.heroID = enemydata.TeamID;
            SetAnimHero(enemydata.GearIndexs.ToList(), enemyCharacter);
            enemyCharacter.isEnemy = true;
            enemyCharacter.Begin(false);
            enemyCharacter.OffBar();
            AssetLoader.Instance.MixSkinWithGears(enemyCharacter.GetAnim(), enemydata.GearIndexs);
        }
       
        for (int i = 0; i < lsCardsplayer.Length; i++)
        {
            if (lsCardsplayer[i]._id != null)
            {
                //GameObject effect1 = ObjectPool.Spawn(apearEffect);
                //effect1.transform.position = enemyPos.lsPos[lsCardsplayer[i].Slot].position;\
                enemyPos.lsPos[lsCardsplayer[i].Slot].GetComponent<CharacterPositionBattle>().PlaySpwanEffect();
                yield return null;
                var go = Rubik.Common.Common.SpawnCardToPosition(charPreb, Vector2.zero, Vector2.zero, enemyPos.lsPos[lsCardsplayer[i].Slot], i, null, 0.2f + i * 0.2f);
                go.isEnemy = true;
                lsCardEnemy[lsCardsplayer[i].Slot] = (go);
                go.heroIndex = (int)lsCardsplayer[i].Index;
                go.heroID = lsCardsplayer[i]._id;
                go.SetHeroData(1, CardPlayerManager.Instance.GetCharacterByIndex((int)lsCardsplayer[i].Index), lsCardsplayer[i]);
                lsCardEnemyIndexs[i] = lsCardsplayer[i].Index;
                go.Begin(false);
                go.slotID = lsCardsplayer[i].Slot;
                go.star = lsCardsplayer[i].Star;
                go.lv = lsCardsplayer[i].Level;
                lsTurn.Add(lsCardsplayer[i]._id, go);
                //go.transform.localPosition = pos;
                yield return null;
            }


        }

        characterDics.Add(battleData.TeamB.TeamID, lsCardEnemy.ToList());
        //var herodata = battleData.TeamB.Hero;
        //if (!string.IsNullOrEmpty(herodata.TeamID))
        //{
        //    yield return new WaitForSeconds(0.01f);
        //    GameObject effect = ObjectPool.Spawn(apearEffect);
        //    effect.transform.position = enemyPos.lsPos[10].position;
        //    yield return new WaitForSeconds(0.01f);
        //    enemyCharacter = Rubik.Common.Common.SpawnCardToPosition(charPreb, Vector3.zero, enemyPos.lsPos[9].position, enemyPos.lsPos[9], 9, null, 0.2f);
        //    enemyCharacter.isEnemy = true;
        //    enemyCharacter.heroID = herodata._id;
        //    //enemyCharacter.SetHeroData(1, playerData);
        //    enemyCharacter.Begin(false);
        //    lsTurn.Add(herodata._id, enemyCharacter);
        //    AssetLoader.Instance.MixSkinWithGears(enemyCharacter.GetAnim(), herodata.GearIndexs);
        //}



    }
    void SetAnimHero(List<int> lsGear, HeroController hero)
    {

        AnimationConfigs.IDLE = "axe/idle";
        AnimationConfigs.ATTACK = "axe/attack 1";
        AnimationConfigs.SKILL_ATTACK = "axe/attack 1";
        AnimationConfigs.HIT = "axe/get_hit";
        hero.SetHeroData(1, playerData[0]);
        if (lsGear.Contains(0))
        {
            AnimationConfigs.IDLE = "sword/idle";
            AnimationConfigs.ATTACK = "sword/attack 1";
            AnimationConfigs.SKILL_ATTACK = "sword/attack 1";
            AnimationConfigs.HIT = "sword/get_hit";
            hero.SetHeroData(1, playerData[0]);
            
        }
        if (lsGear.Contains(7))
        {
            AnimationConfigs.IDLE = "hook/idle";
            AnimationConfigs.ATTACK = "hook/attack 1";
            AnimationConfigs.SKILL_ATTACK = "hook/attack1";
            AnimationConfigs.HIT = "hook/get_hit";
            hero.SetHeroData(1, playerData[1]);
        }
        if (lsGear.Contains(14))
        {
            AnimationConfigs.IDLE = "axe/idle";
            AnimationConfigs.ATTACK = "axe/attack 1";
            AnimationConfigs.SKILL_ATTACK = "axe/attack 1";
            AnimationConfigs.HIT = "axe/get_hit";
            hero.SetHeroData(1, playerData[2]);

        }

    }
    public void CamZoomIn(bool isEnemy = false)
    {
        cam1.gameObject.SetActive(false);
        cam3.gameObject.SetActive(isEnemy);
        cam2.gameObject.SetActive(!isEnemy);
    }
    public void CamZoomOut()
    {
        cam1.gameObject.SetActive(true);
        cam3.gameObject.SetActive(false);
        cam2.gameObject.SetActive(false);
    }
    void SetPlayerTurn()
    {
        if (StaticData.state == GamePlayState.PlayerTurn)
        {
            StaticData.state = GamePlayState.EnemyTurn;
        }
        else if (StaticData.state == GamePlayState.EnemyTurn)
        {
            StaticData.state = GamePlayState.PlayerTurn;

        }
        switch (StaticData.state)
        {
            case GamePlayState.PlayerTurn:
                //uiController.playerTurnTxt.text = "Your Turn";
                // uiController.playerPanel.SetActive(false);
                // uiController.enemyPanel.SetActive(true);
                break;
            case GamePlayState.EnemyTurn:
                //uiController.playerTurnTxt.text = "Enemy Turn";
                //uiController.playerPanel.SetActive(true);
                //uiController.enemyPanel.SetActive(false);
                break;
            case GamePlayState.Start:
                break;
            case GamePlayState.End:
                break;
        }
    }
    // public LeanTweenType leanType = LeanTweenType.easeInOutBack;
    void AttackCharacter(HeroController go, List<HeroController> lsTargets, Vector2 targetPos)
    {
        go.AttackOneHit(lsTargets, targetPos, () => { isAttack = false; });
    }



    bool CheckLose()
    {
        foreach (HeroController hero in listCardCharacter)
        {
            if (hero.curInfo.hp > 0)
                return false;
        }
        return true;
    }
    public bool CheckWin()
    {

        foreach (HeroController hero in lsCardEnemy)
        {
            if (hero.curInfo.hp > 0)
                return false;
        }
        return true;
    }
    public void CheckGameEnd()
    {
        if (CheckLose() && !isAttack)
        {
            Debug.Log("Lose ");
            //StopCoroutine(playerCoroutine);
            StopAllCoroutines();
            foreach (HeroController hero in lsCardEnemy)
            {
                hero.SetLayer(1);
            }
            // ReLoad();
            endGame.End(false);
            skillCover.SetActive(false);
        }
        if (CheckWin())
        {
            //Debug.Log("win ");
            if (playerCoroutine != null)
                StopCoroutine(playerCoroutine);
            StopAllCoroutines();
            foreach (HeroController hero in listCardCharacter)
            {
                if (hero.curInfo.hp > 0)
                {
                    hero.ChangeState(CharacterState.Win);
                    hero.SetLayer(1);
                }
            }
            skillCover.SetActive(false);
            endGame.End(true);

        }
    }
    void ChangeState()
    {
        SetPlayerTurn();
        Debug.Log(StaticData.state);
        if (StaticData.state == GamePlayState.EnemyTurn)
        {
            enemyCoroutine = StartCoroutine(CoEnemyTurn());
        }
        else
        {
            //playerCoroutine = StartCoroutine(CoPlayerTurn());
        }
        CheckGameEnd();
    }
    //public void ChangeTurn()
    //{
    //    indexturn++;
    //    turnController.SetTurn(indexturn, lsTurn);

    //}
    public bool isAttack = false;
    public int indexturn = 0;
    IEnumerator CoEnemyTurn()
    {
        // turnController.SetTurn(indexturn, lsTurn);
        yield return new WaitForSeconds(LevelController.Instance.levelConfig.timeBetWeenTurn / LevelController.Instance.levelConfig.speedGame);

        for (int i = 0; i < lsCardEnemy.Length; i++)
        {

            if (lsCardEnemy[i].curInfo.hp > 0)
            {
                turnController.SetTurn(lsCardEnemy[i].heroID);
                indexturn++;
                var enemyAttack = lsCardEnemy[i];
                var target = EnemyAIAttackTarget();
                Debug.Log(isAttack + "  Enemy turn " + target);
                if (target == null)
                    break;
                if (enemyAttack.curInfo.ap >= 100)
                {
                    skillCover.SetActive(true);
                    enemyAttack.SetLayer(60);
                    //UIGamePlayBattle.Instance.ShowStateTxt(string.Format("<color=red>{0}</color> skill", lsCardEnemy[i].model.Name));
                    foreach (HeroController hero in listCardCharacter)
                    {
                        hero.SetLayer(60);
                    }
                    enemyAttack.AttackSkill(GetTargets(enemyAttack.model.SkillAction.TargetType, listCardCharacter.ToList(), target), GetSkillCastPosition(enemyAttack), () =>
                    {
                        skillCover.SetActive(false);
                        isAttack = false;
                        enemyAttack.zfx.StopZfx();
                        enemyAttack.SetLayer(1);
                        foreach (HeroController hero in listCardCharacter)
                        {
                            hero.SetLayer(1);
                        }
                    });
                }
                else
                {
                    // AttackCharacter(enemyAttack, GetTargets(enemyAttack.model.SkillAction.TargetType, listCardCharacter.ToList(), target), EnemyAIAttackTarget());
                    UIGamePlayBattle.Instance.ShowStateTxt(string.Format("<color=red>{0}</color> attack", lsCardEnemy[i].model.Name));
                }

                isAttack = true;
                yield return new WaitUntil(() => isAttack == false);
                //yield return new WaitForSeconds(LevelController.Instance.levelConfig.timeBetWeenCharacterAttack / LevelController.Instance.levelConfig.speedGame);
                yield return new WaitForSeconds(1);
                //ield return new WaitForSeconds(LevelController.Instance.levelConfig.timeBetWeenCharacterAttack );
                //CheckGameEnd();
            }
            Debug.Log(i + "  Enemy turn " + lsCardEnemy[i].curInfo.hp);

            // ChangeTurn();


        }
        ChangeState();
    }

    IEnumerator CoCharacterTurn(TurnData currentData, ListTurnState targetDatas)
    {
        Debug.Log("ID : " + currentData.TurnCardData.TeamID );
        HeroController currentChar = characterDics.Get(currentData.TurnCardData.TeamID)[currentData.TurnCardData.ActionSlot];
        var playerAttack = currentChar;
        turnController.SetTurn(playerAttack.heroID);
        // yield return new WaitForSeconds(.3f);
        var target = GetTargetTurnData(currentData.TurnCardData.TurnStates);
        if (target == null)
            yield break;
        if (currentData.TurnCardData.ActionType == Rubik.BattleEngine.ActionType.BaseAttack)
        {
            // ZfxGameplayController.Instance.ShowNotiText("Attack");
            CardPlayerManager.Instance.BaseAttackAudio((CardPlayerIndex)playerAttack.heroIndex);
            AttackCharacter(playerAttack, target, GetSkillCastPosition(playerAttack));
            isAttack = true;
        }
        else if (currentData.TurnCardData.ActionType == Rubik.BattleEngine.ActionType.SkillAttack)
        {
            // playerAttack.AttackSkill(lsCardEnemy,GetSkillCastPosition(playerAttack) ,() => { isAttack = false; });
            try
            {
                if (currentChar.isEnemy)
                    ZfxGameplayController.Instance.ShowNotiText(CardPlayerManager.Instance.GetActiveSkillNameByCardIndex(lsCardEnemyIndexs[currentData.TurnCardData.ActionSlot]));
                else
                    ZfxGameplayController.Instance.ShowNotiText(CardPlayerManager.Instance.GetActiveSkillNameByCardIndex(lsCardPlayerIndexs[currentData.TurnCardData.ActionSlot]));
            }
            catch
            {
                Debug.Log("Skill miss");
            }

            skillCover.SetActive(true);
            playerAttack.SetLayer(60);
            isAttack = true;
            //foreach (HeroController hero in lsCardEnemy)
            //{
            //    if (hero != null)
            //        hero.SetLayer(40);
            //}
            Debug.Log("Skill 1 ");
            CardPlayerManager.Instance.SkillActiveAudio((CardPlayerIndex)playerAttack.heroIndex);
            Vector2 pos;
            if (!playerAttack.model.isMoveToEnemyPosition)
            {
                pos = GetSkillCastPosition(playerAttack);
            }
            else
            {
                pos = target[0].positionStart.positionHit(); 
            }
            Debug.Log("Skill pos : " + playerAttack.model.isMoveToEnemyPosition);
            playerAttack.AttackSkill(target, pos, () =>
            {
                skillCover.SetActive(false);
                isAttack = false;
                playerAttack.zfx.StopZfx();
                playerAttack.SetLayer(playerAttack.slotID);
                foreach (HeroController hero in lsCardEnemy)
                {
                    if (hero != null)
                        hero.SetLayer(hero.slotID);
                }
            });
        }

        yield return new WaitUntil(() => isAttack == false);
        yield return new WaitForSeconds(.5f);
        //yield return new WaitForSeconds(LevelController.Instance.levelConfig.timeBetWeenCharacterAttack / LevelController.Instance.levelConfig.speedGame);


    }
    List<HeroController> GetTargets(TargetType type, List<HeroController> lsTargets, HeroController target)
    {
        List<HeroController> lstargetActack = new List<HeroController>();
        switch (type)
        {
            case TargetType.SINGLE_TARGET:
                lstargetActack.Add(target);
                break;
            case TargetType.BACK_ROW_TARGETS:
                if (lsTargets.Count < 3)
                {
                    for (int i = 0; i < lsTargets.Count; i++)
                    {
                        if (lsTargets[i].curInfo.hp > 0)
                        {
                            lstargetActack.Add(lsTargets[i]);
                        }
                    }
                }
                else
                {
                    for (int i = 2; i < lsTargets.Count; i++)
                    {
                        if (lsTargets[i].curInfo.hp > 0)
                        {
                            lstargetActack.Add(lsTargets[i]);
                        }
                    }
                    if (lstargetActack.Count == 0)
                    {
                        for (int i = 0; i < lsTargets.Count; i++)
                        {
                            if (lsTargets[i].curInfo.hp > 0)
                            {
                                lstargetActack.Add(lsTargets[i]);
                            }
                        }
                    }
                }


                for (int i = 2; i < lsTargets.Count; i++)
                {
                    if (lsTargets[i].curInfo.hp > 0)
                    {
                        lstargetActack.Add(lsTargets[i]);
                    }
                }
                break;
            case TargetType.FRONT_ROW_TARGETS:
                if (lsTargets.Count <= 2)
                {
                    for (int i = 0; i < lsTargets.Count; i++)
                    {
                        if (lsTargets[i].curInfo.hp > 0)
                        {
                            lstargetActack.Add(lsTargets[i]);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < 2; i++)
                    {
                        if (lsTargets[i].curInfo.hp > 0)
                        {
                            lstargetActack.Add(lsTargets[i]);
                        }
                    }
                    if (lstargetActack.Count == 0)
                    {
                        for (int i = 0; i < lsTargets.Count; i++)
                        {
                            if (lsTargets[i].curInfo.hp > 0)
                            {
                                lstargetActack.Add(lsTargets[i]);
                            }
                        }
                    }
                }

                break;
            case TargetType.RANDOM_THREE_TARGETS:

                for (int i = 0; i < lsTargets.Count; i++)
                {
                    if (lsTargets[i].curInfo.hp > 0)
                    {
                        lstargetActack.Add(lsTargets[i]);
                    }
                }
                while (lstargetActack.Count > 3)
                {
                    int rd = Random.Range(0, lstargetActack.Count);
                    lstargetActack.RemoveAt(rd);
                }
                break;
            case TargetType.ALL_TARGETS:
                for (int i = 0; i < lsTargets.Count; i++)
                {
                    if (lsTargets[i].curInfo.hp > 0)
                    {
                        lstargetActack.Add(lsTargets[i]);
                    }
                }
                break;
        }

        return lstargetActack;
    }
    HeroController EnemyAIAttackTarget()
    {
        List<HeroController> lsPlayerCardActive = new List<HeroController>();
        for (int i = 0; i < listCardCharacter.Length; i++)
        {
            if (listCardCharacter[i].curInfo.hp > 0)
                lsPlayerCardActive.Add(listCardCharacter[i]);
        }
        if (lsPlayerCardActive.Count > 0)
        {
            return lsPlayerCardActive[Random.Range(0, lsPlayerCardActive.Count)];
        }

        else
        {
            return null;
        }
    }
    Vector2 GetSkillCastPosition(HeroController hero)
    {
        switch (hero.model.CardType)
        {
            case AttackType.Melee:
                if (hero.model.SkillType == SkillAction.SKILL_STAND)
                {
                    Vector3 pos1 = hero.isEnemy ? GameController.Instance.skillEnemyRange.position : GameController.Instance.skillRangePosition.position;
                    return pos1;
                }
                else
                {
                  
                    return skillPosition.position;
                }
                break;
            case AttackType.Ranged:
                Vector3 pos2 = hero.isEnemy ? GameController.Instance.skillEnemyRange.position : GameController.Instance.skillRangePosition.position;
                return pos2;
                break;
            case AttackType.Arrow:
                Vector3 pos3 = hero.isEnemy ? GameController.Instance.skillEnemyRange.position : GameController.Instance.skillRangePosition.position;
                return pos3;
                break;
            default:
                return skillPosition.position;
                break;
        }

    }
    HeroController GetEnemyAttack()
    {
        List<HeroController> lsEnemyCardActive = new List<HeroController>();
        for (int i = 0; i < lsCardEnemy.Length; i++)
        {
            if (lsCardEnemy[i].curInfo.hp > 0)
                lsEnemyCardActive.Add(lsCardEnemy[i]);
        }
        if (lsEnemyCardActive.Count > 0)
        {
            return lsEnemyCardActive[Random.Range(0, lsEnemyCardActive.Count)];
        }
        else
        {
            return null;
        }
    }
    List<GameObject> lsCoins = new List<GameObject>();
    public void SpawnCoin(Vector2 spawnPos, int number)
    {
        for (int i = 0; i < number; i++)
        {
            lsCoins.Add(Rubik.Common.Common.SpawnCoin(spawnPos, number));
        }

    }
    public bool CheckPlayerLose()
    {
        var checkWin = EnemyAIAttackTarget();
        //Debug.Log("Check win : " + checkWin);
        if (checkWin == null)
        {
            return true;
        }
        else
            return false;
    }
    public HeroController PlayerAIAttackTarget()
    {
        List<HeroController> lsEnemyCardActive = new List<HeroController>();
        for (int i = 0; i < lsCardEnemy.Length; i++)
        {
            if (lsCardEnemy[i].curInfo.hp > 0)
                lsEnemyCardActive.Add(lsCardEnemy[i]);
        }
        if (lsEnemyCardActive.Count > 0)
        {
            return lsEnemyCardActive[Random.Range(0, lsEnemyCardActive.Count)];
        }
        else
        {
            return null;
        }
    }
    public void SetTurnEffect(string teamID, int slot, BuffDataShort[] buffs)
    {
        HeroController currentChar = characterDics.Get(teamID)[slot];
        turnController.SetEffect(currentChar.heroID, buffs);
    }
    public List<HeroController> GetTargetTurnData(ListTurnState target)
    {
        List<HeroController> lsTargets = new List<HeroController>();
        foreach (TurnCardState turn in target.TurnCardStates)
        {
            if (turn.Dmg != null && turn.Dmg.Length > 0)
            {
                //for(int i = 0; i < turn.Dmg.Length; i++)
                {
                    characterDics.Get(turn.TeamID)[turn.Slot].atk = turn.Dmg;
                    characterDics.Get(turn.TeamID)[turn.Slot].typeDame = turn.TypeDmg;
                    lsTargets.Add(characterDics.Get(turn.TeamID)[turn.Slot]);
                }
                
            }
            if (turn.Buff != null && turn.Buff.Length > 0)
            {
                //characterDics[turn.TeamID][turn.Slot].TurnEffectBuff(turn.Buff);
                SetTurnEffect(turn.TeamID, turn.Slot, turn.Buff);
                // characterDics[turn.TeamID][turn.Slot].SetHP(turn.HP);
                //  characterDics[turn.TeamID][turn.Slot].SetHP(turn.AP);
            }
            try
            {

            }
            catch
            {

            }

            characterDics.Get(turn.TeamID)[turn.Slot].SetHP(turn.HP, turn.MaxHP);
            characterDics.Get(turn.TeamID)[turn.Slot].SetAP(turn.AP);
            //characterDics[turn.TeamID][turn.Slot].SetHPBar();
            // characterDics[turn.TeamID][turn.Slot].SetAPBar();
            if (waveNumber == 7)
            {

                Debug.Log(" team : " + turn.TeamID + " HP : " + turn.HP + " slot : " + turn.Slot);
                Debug.Log(" team : " + turn.TeamID + " HP-char : " + characterDics.Get(turn.TeamID)[turn.Slot].curInfo.hp + " slot : " + turn.Slot);
            }
        }
        return lsTargets;
    }
    public void SetCurrentAllCharacter()
    {
        foreach (HeroController hero in lsCardEnemy)
        {
            if (hero != null)
            {
                hero.SetHPBar();
                hero.SetAPBar();
            }

        }
        foreach (HeroController hero in listCardCharacter)
        {
            if (hero != null)
            {
                hero.SetHPBar();
                hero.SetAPBar();
            }

        }
    }
    HeroController GetPlayerAttack()
    {
        List<HeroController> lsPlayerCardActive = new List<HeroController>();
        for (int i = 0; i < listCardCharacter.Length; i++)
        {
            if (listCardCharacter[i].curInfo.hp > 0)
                lsPlayerCardActive.Add(listCardCharacter[i]);
        }
        if (lsPlayerCardActive.Count > 0)
        {
            return lsPlayerCardActive[Random.Range(0, lsPlayerCardActive.Count)];
        }
        else
        {
            return null;
        }
    }
    public void SetCurrentHpBar()
    {
        float current = 0, max = 0;
        for (int i = 0; i < lsCardEnemy.Length; i++)
        {
            max += lsCardEnemy[i].model.MaxHP;
            current += lsCardEnemy[i].curInfo.hp;
        }
        // Debug.Log(current + " --- " + max);
        SetHPBar(current, max);
    }
    void SetHPBar(float curInfoHP, float curInfoMaxHP)
    {
        if (hpBar == null) return;
        if (curInfoMaxHP != 0)
        {

            hpBar.Quantity.Amount = curInfoHP;
            hpBar.Quantity.MaximumAmount = curInfoMaxHP;
        }
        if (hpBar.Quantity.Amount < 0) hpBar.Quantity.Amount = 0;
    }

    public void ShakeCamera()
    {
        //CamAnim.Play("ShakeCam");
    }
    public void Replay()
    {
        // UnityEngine.SceneManagement.SceneManager.LoadScene("GamePlay");
        SceneController.Instance.LoadScene(SceneConfig.Battle_Screen);
    }
    public void SetMaxCanvas()
    {
     //   GetComponentInParent<Canvas>().lay
    }
    public void BackToHomeScene()
    {
        //UnityEngine.SceneManagement.SceneManager.LoadScene("Home Screen");
        SceneController.Instance.LoadScene(SceneConfig.WorldMap_Screen);
    }
    public Dictionary<string, HeroController> lsTurn = new Dictionary<string, HeroController>();
    public void AddTurn()
    {
        int index = 0;
        lsTurn.Add(heroCharacter.heroID, heroCharacter);
        index++;
        //while (index < count)
        {
            foreach (HeroController hero in listCardCharacter)
            {
                if (hero.curInfo.hp > 0)
                {
                    lsTurn.Add(hero.heroID, hero);
                    index++;
                }
            }
            foreach (HeroController hero in lsCardEnemy)
            {
                if (hero.curInfo.hp > 0)
                {
                    lsTurn.Add(hero.heroID, hero);
                    index++;
                }
            }
        }
        turnController.SetState(lsTurn);
    }


    void StartGameInServer()
    {
        StartCoroutine(CoStartGame());
    }
    IEnumerator CoStartGame()
    {
        //int index = 0;
        var startgameData = battleData.StartBattleStates;
        foreach (TurnCardState state in startgameData.TurnCardStates)
        {
            if (state.Buff != null && state.Buff.Length > 0)
            {
                //characterDics[state.TeamID][state.Slot].TurnEffectBuff(state.Buff);
                SetTurnEffect(state.TeamID, state.Slot, state.Buff);
            }
            Debug.Log(state.TeamID + " id ");
                Debug.Log(state.Slot + " slot ");
            characterDics.Get(state.TeamID)[state.Slot].SetHP(state.HP, state.MaxHP);
            characterDics.Get(state.TeamID)[state.Slot].SetAP(state.AP);
            characterDics.Get(state.TeamID)[state.Slot].SetHPBar();
            characterDics.Get(state.TeamID)[state.Slot].SetAPBar();
        }
        int countTime = 1;
        while (countTime > 0)
        {
            gameStart.SetActive(true);
            //  gameStart.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Start in : " + countTime;
            yield return new WaitForSeconds(1);
            // gameStart.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Start in : " + countTime;
            countTime--;
        }
        AudioCtrl.Instance.Play(AudioName.UI_Button_Battle_Start);
        gameStartPanel.transform.DOMoveY(gameStart.transform.position.y + 10, .5f).OnComplete(() =>
        {
            gameStart.SetActive(false);
        }).SetEase(Ease.InBack);

        yield return new WaitForSeconds(1f);
        while (waveNumber < battleData.RoundDatas.Length)
        {
            //start turn state
            UIGamePlayBattle.Instance.txtTurn.text = Lean.Localization.LeanLocalization.GetTranslationText("turn", "Turn") + " " + (waveNumber + 1).ToString() + "/15";
            var turnData = battleData.RoundDatas[waveNumber].TurnDatas;
            Debug.Log("Wave number  : " + waveNumber);
            foreach (TurnData turn in turnData)
            {
                yield return StartCoroutine(CoCharacterTurn(turn, turn.TurnCardData.TurnStates));
                //check hero attack
                if (!string.IsNullOrEmpty(turn.TurnHeroData.TeamID))
                {

                    Debug.Log("hero attack ! " + waveNumber);
                    //if (currentData.TurnCardData.ActionType == Rubik.BattleEngine.ActionType.SkillAttack && !CheckLose() && !CheckWin())
                    {
                        var target = GetTargetTurnData(turn.TurnHeroData.TurnStates);
                        //turnController.SetTurn(heroCharacter.heroID);
                        // playerAttack.AttackSkill(lsCardEnemy,GetSkillCastPosition(playerAttack) ,() => { isAttack = false; });
                        skillCover.SetActive(true);
                        var heroChar = turn.TurnHeroData.TeamID == heroCharacter.heroID ? heroCharacter : enemyCharacter;
                        heroChar.SetLayer(60);
                        //foreach (HeroController hero in lsCardEnemy)
                        //{
                        //    if (hero != null)
                        //        hero.SetLayer(60);
                        //}
                        CardPlayerManager.Instance.SkillActiveAudio((CardPlayerIndex)heroChar.heroIndex);
                        heroChar.AttackSkill(target, GetSkillCastPosition(heroChar), () =>
                        {
                            skillCover.SetActive(false);
                            isAttack = false;
                            heroChar.zfx.StopZfx();
                            heroChar.SetLayer(heroChar.slotID);
                            foreach (HeroController hero in lsCardEnemy)
                            {
                                if (hero != null)
                                    hero.SetLayer(hero.slotID);
                            }
                        });
                        isAttack = true;
                        yield return new WaitUntil(() => isAttack == false);
                    }

                }

            }

            //end turn state 
            var endTurnData = battleData.RoundDatas[waveNumber].EndTurnStates;
            if (endTurnData.TurnCardStates.Length > 0)
                yield return new WaitForSeconds(.5f);
            foreach (TurnCardState turn in endTurnData.TurnCardStates)
            {
                if (turn.Buff != null && turn.Buff.Length > 0)
                {
                    // characterDics[turn.TeamID][turn.Slot].TurnEffectBuff(turn.Buff);
                    SetTurnEffect(turn.TeamID, turn.Slot, turn.Buff);
                }
                if (turn.Dmg != null && turn.Dmg.Length > 0)
                {
                    for (int i = 0; i < turn.Dmg.Length; i++)
                    {

                        characterDics.Get(turn.TeamID)[turn.Slot].SetHP(turn.HP, turn.MaxHP);
                        characterDics.Get(turn.TeamID)[turn.Slot].SetAP(turn.AP);
                        characterDics.Get(turn.TeamID)[turn.Slot].HitDame(turn.Dmg[i], turn.TypeDmg[i]);
                        // yield return new WaitForSeconds(.2f);
                    }
                    //characterDics[turn.TeamID][turn.Slot].TurnEffectBuff(turn.Buff);
                    SetTurnEffect(turn.TeamID, turn.Slot, turn.Buff);

                }
            }
            waveNumber++;
            yield return new WaitForSeconds(1);


        }
        Debug.Log("End game");
        EndGame(battleData.TeamWin == battleData.TeamA.TeamID);




    }

    public void Skip(){
        if(!Rubik.UserDataPlayer.UserDataManager.Instance.CanSkipBattle()) return;
        EndGame(battleData.TeamWin == battleData.TeamA.TeamID);
    }

    void EndGame(bool isWin)
    {
        if (playerCoroutine != null)
            StopCoroutine(playerCoroutine);
        StopAllCoroutines();

        skillCover.SetActive(false);
        endGame.OnUI();
        switch (BattleEngineController.Instance.BattleType)
        {
            case BattleType.Map:
                endGame.End(isWin);
                break;
            case BattleType.Clan:
                endGame.EndBoss(isWin);
                break;
            case BattleType.Portal:
                endGame.EndPortal(isWin);
                break;
            case BattleType.Outpost:
                endGame.EndOutPost(isWin);
                break;
            case BattleType.Arena:
                endGame.EndArena(isWin);
                break;
            default:
                endGame.EndPortal(isWin);
                break;
        }
      
           

    }
    public void AttackPlayer()
    {
        if (isAttack)
            return;
        isAttack = true;
        var playerAttack = listCardCharacter[0];
        // yield return new WaitForSeconds(.3f);
        var target = PlayerAIAttackTarget();
        UIGamePlayBattle.Instance.ShowStateTxt("<color=blue>Player</color> attack ");
        UIGamePlayBattle.Instance.ExitPanelButton();
        Debug.Log(target.name + "--");
        // AttackCharacter(playerAttack, GetTargets(playerAttack.model.SkillAction.TargetType, lsCardEnemy.ToList(), target), target);
        StartCoroutine(WaitForPlayerEndState());
        // ChangeTurn();
    }
    public IEnumerator WaitForPlayerEndState()
    {
        yield return new WaitUntil(() => isAttack == false);
        ChangeState();
    }
    public void SkillPlayer()
    {
        if (isAttack)
            return;
        var playerAttack = listCardCharacter[0];
        // yield return new WaitForSeconds(.3f);
        var target = PlayerAIAttackTarget();

        if (playerAttack.curInfo.ap >= SkillCost)
        {
            playerAttack.AddAP(-SkillCost);
            UIGamePlayBattle.Instance.ShowStateTxt("<color=blue>Player</color> skill ");
            UIGamePlayBattle.Instance.ExitPanelButton();
            // playerAttack.AttackSkill(lsCardEnemy,GetSkillCastPosition(playerAttack) ,() => { isAttack = false; });
            skillCover.SetActive(true);
            playerAttack.SetLayer(60);
            //foreach (HeroController hero in lsCardEnemy)
            //{
            //    hero.SetLayer(60);
            //}
            isAttack = true;
            playerAttack.AttackSkill(GetTargets(playerAttack.model.SkillAction.TargetType, lsCardEnemy.ToList(), target), GetSkillCastPosition(playerAttack), () =>
            {
                skillCover.SetActive(false);
                isAttack = false;
                playerAttack.zfx.StopZfx();
                playerAttack.SetLayer(playerAttack.slotID);
                foreach (HeroController hero in lsCardEnemy)
                {
                    hero.SetLayer(hero.slotID);
                }
            });
            StartCoroutine(WaitForPlayerEndState());
            //ChangeTurn();
        }
        else
        {
            ZfxGameplayController.Instance.ShowNotiText("Don't enough mana ! ");
        }

    }
}
