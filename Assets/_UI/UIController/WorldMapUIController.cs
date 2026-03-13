using Lean.Localization;
using NTPackage.EventDispatcher;
using NTPackage.Functions;
using NTPackage.UI;
using Pixelplacement;
using Rubik.BattleEngine;
using Rubik.Combat;
using Rubik.Format;
using Rubik.Myrk.BattleTeam;
using Rubik.Myrk.PackageIAP;
using Rubik.UI;
using Rubik.UserDataPlayer;
using Rubik.UserProfile;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Rubik.UIController
{
    public class WorldMapUIController : NTBehaviour
    {

        [Header("Player Data")]

        public TextMeshProUGUI TextName;
        public TextMeshProUGUI TextLevel;
        public TextMeshProUGUI TextExp;
        public AvatarPlayerUI AvatarPlayerUI;
        public PopupUI coomingSoon;
        public GameObject coinFly, gemFly,energyFly;
        public Transform coinTran, gemTrans,enrgyTrans;

        public Transform RightSide;
        public Transform LeftSide;
        public static WorldMapUIController Instance;
        public GameObject heroLock,summonLock,clanlock,shopLock,mailLock,arenaLock,invenLock,questLock,monsterLock,lineUplock;
        protected override void Awake()
        {
            Time.timeScale = 1;
            base.Awake();
            if (WorldMapUIController.Instance != null)
            {
                NTLog.LogError("Only 1 Instance allow");
                return;
            }
            WorldMapUIController.Instance = this;
        }

        protected override void Start()
        {
            base.Start();
            EventListenerManager.instance.Register(EventCode.ChangeDisplayName, "WorldMapUIController", this.UpdatePlayerData);
            EventListenerManager.instance.Register(EventCode.ChangeAvatar, "WorldMapUIController", this.UpdatePlayerData);
            EventListenerManager.instance.Register(EventCode.UpdateUserData, "WorldMapUIController", this.UpdatePlayerData);
            ObjectPoolingManager.Instance.PushChildObjectIntoPooling(this.RightSide);
            ObjectPoolingManager.Instance.PushChildObjectIntoPooling(this.LeftSide);
            this.InitBtnPackageIAP();
            this.UpdatePlayerData();
            for (int i = 0; i < 11; i++)
            {
                int temp = i;

                if (!UserProfileManager.Instance.TutorialDone.Contains((TutorialType)i))
                {
                    var popup = PopupManager.Instance.GetPopupUI(PopupCode.TutorialUI).GetComponent<TutorialUI>();
                    Debug.Log("Lock type : "+UserProfileManager.Instance.IsFunctionLocked(popup.Tuts[temp].tuts[0].locktype));
                    if (!UserProfileManager.Instance.IsFunctionLocked(popup.Tuts[temp].tuts[0].locktype))
                    {
                        if(temp == 0)
                        {
                            AppsFlyerManager.TrackingEvent(AppsflyerEvents.player_start_tutorial, 1, 1);
                        }
                        popup.tutType = (TutorialType)temp;
                        popup.indexOftut = 0;
                        PopupManager.Instance.OnUI(PopupCode.TutorialUI, null, (popup) => {
                          
                        });
                        break;
                    }
                   
                }
            }
           
            heroLock.SetActive(UserProfileManager.Instance.IsFunctionLocked(LockFunctionType.Hero));
            summonLock.SetActive(UserProfileManager.Instance.IsFunctionLocked(LockFunctionType.Summon));
            clanlock.SetActive(UserProfileManager.Instance.IsFunctionLocked(LockFunctionType.Clan));
            shopLock.SetActive(UserProfileManager.Instance.IsFunctionLocked(LockFunctionType.Market));
            mailLock.SetActive(UserProfileManager.Instance.IsFunctionLocked(LockFunctionType.Mail));
            invenLock.SetActive(UserProfileManager.Instance.IsFunctionLocked(LockFunctionType.Bag));
            questLock.SetActive(UserProfileManager.Instance.IsFunctionLocked(LockFunctionType.Quest));
            monsterLock.SetActive(UserProfileManager.Instance.IsFunctionLocked(LockFunctionType.Monster));
            lineUplock.SetActive(UserProfileManager.Instance.IsFunctionLocked(LockFunctionType.LineUp));
            arenaLock.SetActive(UserProfileManager.Instance.IsFunctionLocked(LockFunctionType.Arena));
            if (BattleEngineController.Instance.BattleType == BattleType.Arena)
            {
                StartCoroutine(NTFunction.WaitSecond(0.5f, () => {
                    _OnclickArena();
                }));
            }

            //OnLevelComplete();
            if (StaticData.level == -1)
            {
                StaticData.level = UserDataManager.Instance.UserData.Level;
            }
            else
            {
                if(StaticData.level< UserDataManager.Instance.UserData.Level)
                {
                    AppsFlyerManager.TrackingLevel( UserDataManager.Instance.UserData.Level);
                    StaticData.level = UserDataManager.Instance.UserData.Level;
                }
            }
#if UNITY_ANDROID
            if (PlayerPrefs.GetInt("android_install", 0) == 0)
            {
                PlayerPrefs.SetInt("android_install", 1);
                AppsFlyerManager.TrackingEvent(AppsflyerEvents.android_install, PlayerPrefs.GetInt("android_install", 0));
            }
#endif
#if UNITY_ANDROID
            if (PlayerPrefs.GetInt("ios_install", 0) == 0)
            {
                PlayerPrefs.SetInt("ios_install", 1);
                AppsFlyerManager.TrackingEvent(AppsflyerEvents.ios_install, PlayerPrefs.GetInt("ios_install", 0));
            }
#endif

        }

        public void UpdatePlayerData(object data = null)
        {
            this.TextName.text = UserDataManager.Instance.UserData.DisplayName;
            (long exp, long expNext, int level) = UserDataManager.Instance.GetPlayerLevel();
            this.TextLevel.text = (level+1).ToString();
            this.TextExp.text = $"{FormatData.GetFriendlyShortNumber(exp)}/{FormatData.GetFriendlyShortNumber(expNext)}";
            this.AvatarPlayerUI.SetData(UserProfileManager.Instance.AvatarPlayer.Current, UserProfileManager.Instance.AvatarBorderPlayer.Current, UserDataManager.Instance.UserData.DisplayName, 1);
        }

        public void _OnclickSummon()
        {
            if (UserProfileManager.Instance.IsFunctionLocked(LockFunctionType.Summon))
            {
                PopupManager.Instance.OnUI(PopupCode.MessagePanel, null, (PopupUI popupUI) =>
                {
                    popupUI.GetComponent<MessagePanel>().SetData(LeanLocalization.GetTranslationText("lock_title", "Feature Locked"),string.Format( LeanLocalization.GetTranslationText("unlock_feature", "Feature unlocks at Level {0}"),UserProfileManager.Instance.GetLevelUnlockFunction(LockFunctionType.Summon)));
                });
                return;
            }
            PopupManager.Instance.OnUI(PopupCode.SummonUI);
        }
        public void _OnclickAchievement()
        {
            if (UserProfileManager.Instance.IsFunctionLocked(LockFunctionType.LineUp))
            {
                PopupManager.Instance.OnUI(PopupCode.MessagePanel, null, (PopupUI popupUI) =>
                {
                    popupUI.GetComponent<MessagePanel>().SetData(LeanLocalization.GetTranslationText("lock_title", "Feature Locked"), string.Format(LeanLocalization.GetTranslationText("unlock_feature", "Feature unlocks at Level {0}"), UserProfileManager.Instance.GetLevelUnlockFunction(LockFunctionType.Summon)));
                });
                return;
            }
            PopupManager.Instance.OnUI(PopupCode.AchievementUI);
        }

        public void _OnclickQuest()

        {
            if (UserProfileManager.Instance.IsFunctionLocked(LockFunctionType.Quest))
            {
                PopupManager.Instance.OnUI(PopupCode.MessagePanel, null, (PopupUI popupUI) =>
                {
                    popupUI.GetComponent<MessagePanel>().SetData(LeanLocalization.GetTranslationText("lock_title", "Feature Locked"), string.Format(LeanLocalization.GetTranslationText("unlock_feature", "Feature unlocks at Level {0}"), UserProfileManager.Instance.GetLevelUnlockFunction(LockFunctionType.Quest)));
                });
                return;
            }
            PopupManager.Instance.OnUI(PopupCode.QuestUI);
        }

        public void _OnclickChangeAvatar()
        {
            PopupManager.Instance.OnUI(PopupCode.AvatarChangeUI);
        }
        public void _OnclickEventUI()
        {
            PopupManager.Instance.OnUI(PopupCode.EventUI);
        }
        public void _OnclickChangeName()
        {
            PopupManager.Instance.OnUI(PopupCode.NameChangeUI);
        }
        public void OnButtonCharacter_Onclick()
        {
            if (UserProfileManager.Instance.IsFunctionLocked(LockFunctionType.Hero))
            {
                PopupManager.Instance.OnUI(PopupCode.MessagePanel, null, (PopupUI popupUI) =>
                {
                    popupUI.GetComponent<MessagePanel>().SetData(LeanLocalization.GetTranslationText("lock_title", "Feature Locked"), string.Format(LeanLocalization.GetTranslationText("unlock_feature", "Feature unlocks at Level {0}"), UserProfileManager.Instance.GetLevelUnlockFunction(LockFunctionType.Hero)));
                });
                return;
            }
            PopupManager.Instance.OnUI(NTPackage.UI.PopupCode.CharacterGear_UI, 0);
            
        }
        public void _OnclickSetting()
        {
            PopupManager.Instance.OnUI(PopupCode.SettingUI);
        }
        public void OnButtonInven()
        {
            if (UserProfileManager.Instance.IsFunctionLocked(LockFunctionType.Bag))
            {
                PopupManager.Instance.OnUI(PopupCode.MessagePanel, null, (PopupUI popupUI) =>
                {
                    popupUI.GetComponent<MessagePanel>().SetData(LeanLocalization.GetTranslationText("lock_title", "Feature Locked"), string.Format(LeanLocalization.GetTranslationText("unlock_feature", "Feature unlocks at Level {0}"), UserProfileManager.Instance.GetLevelUnlockFunction(LockFunctionType.Bag)));
                });
                return;
            }
            PopupManager.Instance.OnUI(PopupCode.Inventory_UI);
        }
        public void OnButtonMonster()
        {
            if (UserProfileManager.Instance.IsFunctionLocked(LockFunctionType.Monster))
            {
                PopupManager.Instance.OnUI(PopupCode.MessagePanel, null, (PopupUI popupUI) =>
                {
                    popupUI.GetComponent<MessagePanel>().SetData(LeanLocalization.GetTranslationText("lock_title", "Feature Locked"), string.Format(LeanLocalization.GetTranslationText("unlock_feature", "Feature unlocks at Level {0}"), UserProfileManager.Instance.GetLevelUnlockFunction(LockFunctionType.Monster)));
                });
                return;
            }
            PopupManager.Instance.OnUI(PopupCode.MonsterUI);
        }
        public void OnButtonLineUp()
        {
            if (UserProfileManager.Instance.IsFunctionLocked(LockFunctionType.LineUp))
            {
                PopupManager.Instance.OnUI(PopupCode.MessagePanel, null, (PopupUI popupUI) =>
                {
                    popupUI.GetComponent<MessagePanel>().SetData(LeanLocalization.GetTranslationText("lock_title", "Feature Locked"), string.Format(LeanLocalization.GetTranslationText("unlock_feature", "Feature unlocks at Level {0}"), UserProfileManager.Instance.GetLevelUnlockFunction(LockFunctionType.LineUp)));
                });
                return;
            }
            PopupManager.Instance.OnUI(PopupCode.LineUpUI, 0);
        }
        public void OnCommingSoon()
        {
            //HUDCanvas.Instance.ShowNotification("Comming soon !");
            coomingSoon.OnUI();
        }

        public void OnChat(int tabIndex = 0){
            PopupManager.Instance.OnUI(PopupCode.ChatUI, tabIndex);
        }
        public void OnClan()
        {
            if (UserProfileManager.Instance.IsFunctionLocked(LockFunctionType.Clan))
            {
                PopupManager.Instance.OnUI(PopupCode.MessagePanel, null, (PopupUI popupUI) =>
                {
                    popupUI.GetComponent<MessagePanel>().SetData(LeanLocalization.GetTranslationText("lock_title", "Feature Locked"), string.Format(LeanLocalization.GetTranslationText("unlock_feature", "Feature unlocks at Level {0}"), UserProfileManager.Instance.GetLevelUnlockFunction(LockFunctionType.Clan)));
                });
                return;
            }
            PopupManager.Instance.OnUI(PopupCode.ClanHomeUI);
        }
        public void _OnclickIAPShop(){
            if (UserProfileManager.Instance.IsFunctionLocked(LockFunctionType.Market))
            {
                PopupManager.Instance.OnUI(PopupCode.MessagePanel, null, (PopupUI popupUI) =>
                {
                    popupUI.GetComponent<MessagePanel>().SetData(LeanLocalization.GetTranslationText("lock_title", "Feature Locked"), string.Format(LeanLocalization.GetTranslationText("unlock_feature", "Feature unlocks at Level {0}"), UserProfileManager.Instance.GetLevelUnlockFunction(LockFunctionType.Market)));
                });
                return;
            }
            PopupManager.Instance.OnUI(PopupCode.IAPShopUI);
        }
        public void _OnclickArena()
        {
            if (UserProfileManager.Instance.IsFunctionLocked(LockFunctionType.Arena))
            {
                PopupManager.Instance.OnUI(PopupCode.MessagePanel, null, (PopupUI popupUI) =>
                {
                    popupUI.GetComponent<MessagePanel>().SetData(LeanLocalization.GetTranslationText("lock_title", "Feature Locked"), string.Format(LeanLocalization.GetTranslationText("unlock_feature", "Feature unlocks at Level {0}"), UserProfileManager.Instance.GetLevelUnlockFunction(LockFunctionType.Arena)));
                });
                return;
            }
            PopupManager.Instance.OnUI(PopupCode.ArenaUI);
        }

        public async void IncreaseEffect(Vector3 spawnPos, bool isCoin = true)
        {
            var coinIcon = isCoin ? coinTran : gemTrans;
            // var coinEffect = lsElements[(int)effect].icon.GetComponentInChildren<ParticleSystem>();
            int temp = 2;
           
            for (int i = 0; i < temp; i++)
            {
                GameObject go = isCoin ? coinFly : gemFly;
                GameObject coin = Instantiate(go, spawnPos + new Vector3(0, 0, -0.1f), Quaternion.identity);
                Vector3 target = coin.transform.position + new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), 0);//generate a random pos
                Tween.Position(coin.transform, target, 0.5f + 1 * 0.06f, 0, Tween.EaseOutStrong);
                Tween.Position(coin.transform, coinIcon.position, 0.3f + 0.1f * i, 0.1f + 0.1f * i, Tween.EaseInStrong, Tween.LoopType.None, null, () =>
                {
                    
                    Destroy(coin);
                   coinIcon.GetComponentInChildren<ParticleSystem>().Play();
                });
            }
            float timeStamp = Time.time;
            while (Time.time - timeStamp < 1)
                await System.Threading.Tasks.Task.Yield();


        }
        public async void IncreaseEffect(Vector3 spawnPos,Transform targetPos, bool isCoin = true)
        {
           
            // var coinEffect = lsElements[(int)effect].icon.GetComponentInChildren<ParticleSystem>();
            int temp = 2;

            for (int i = 0; i < temp; i++)
            {
                GameObject go = isCoin ? coinFly : gemFly;
                GameObject coin = Instantiate(go, spawnPos + new Vector3(0, 0, -0.1f), Quaternion.identity);
                Vector3 target = coin.transform.position + new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), 0);//generate a random pos
                Tween.Position(coin.transform, target, 0.5f + 1 * 0.06f, 0, Tween.EaseOutStrong);
                Tween.Position(coin.transform, targetPos.position, 0.3f + 0.1f * i, 0.1f + 0.1f * i, Tween.EaseInStrong, Tween.LoopType.None, null, () =>
                {

                    Destroy(coin);
                    targetPos.GetComponentInChildren<ParticleSystem>().Play();
                });
            }
            float timeStamp = Time.time;
            while (Time.time - timeStamp < 1)
                await System.Threading.Tasks.Task.Yield();


        }
        public async void IncreaseEnergy(Vector3 spawnPos)
        {

            // var coinEffect = lsElements[(int)effect].icon.GetComponentInChildren<ParticleSystem>();
            int temp = 2;

            for (int i = 0; i < temp; i++)
            {
                GameObject go =energyFly;
                GameObject coin = Instantiate(go, spawnPos + new Vector3(0, 0, -0.1f), Quaternion.identity);
                Vector3 target = coin.transform.position + new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), 0);//generate a random pos
                Tween.Position(coin.transform, target, 0.5f + 1 * 0.06f, 0, Tween.EaseOutStrong);
                Tween.Position(coin.transform, enrgyTrans.position, 0.3f + 0.1f * i, 0.1f + 0.1f * i, Tween.EaseInStrong, Tween.LoopType.None, null, () =>
                {

                    Destroy(coin);
                    enrgyTrans.GetComponentInChildren<ParticleSystem>().Play();
                });
            }
            float timeStamp = Time.time;
            while (Time.time - timeStamp < 1)
                await System.Threading.Tasks.Task.Yield();


        }

        public void InitBtnPackageIAP(){
            List<PackageIAP> packageIAPs = PackageIAPManager.Instance.GetPackageIAPAvailable();
            foreach (PackageIAP packageIAP in packageIAPs)
            {
                BtnPackageIAP prefab = PackageIAPManager.Instance.GetBtnPackageIAP(packageIAP.Index);
                if(prefab == null) continue;
                BtnPackageIAP btnPackageIAP = ObjectPoolingManager.Instance.InstantiateObject<BtnPackageIAP>("Btn" + packageIAP.Index, prefab.transform);
                btnPackageIAP.SetData(packageIAP);
                btnPackageIAP.transform.SetParent(this.LeftSide);
                NTFunction.ResetPosition(btnPackageIAP.transform);
            }
        }

        public void OnClickDailyReward(){
            PopupManager.Instance.OnUI(PopupCode.DailyRewardUI);
        }
    }
}
