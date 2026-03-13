using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NTFunctions_old;
using Rubik.UI;
using GOA.Config;
using NTPackage.UI;

namespace NTFunctions_old
{
    public class UIManager : LoadBehaviour
    {
        public float currentLvUI = 0;
        [SerializeField] private List<PopupUI> popupUIs;
        public List<Transform> collectionsUI;

        public static UIManager instance;
        public GameObject MainCanvas,lineUpUI,characgearUI;
        protected override void Awake()
        {
            base.Awake();
            if (UIManager.instance != null) Debug.LogError("Only 1 UIManager allow");
            UIManager.instance = this;
            MainCanvas = gameObject.GetComponentsInChildren<Canvas>()[0].gameObject;
            Time.timeScale = 1;

        }

        public override void LoadComponents()
        {
            base.LoadComponents();
            this.LoadCollectionsUI();
            this.LoadPopupUI();
        }

        protected void LoadCollectionsUI(){
            this.collectionsUI.Clear();
            Transform col = transform.Find("CanvasCollection");
            if(col == null) return;
            foreach (Transform item in col)
            {
                this.collectionsUI.Add(item);
            }
        }

        protected void LoadPopupUI(){
            this.popupUIs.Clear();
            foreach (PopupUI popupUI in transform.GetComponentsInChildren<PopupUI>())
            {
                this.popupUIs.Add(popupUI);
               
            }
        }

        //Function
        public void StartGameCOntroller()
        {

        }
        protected override void Start()
        {
            base.Start();
            this.OffAllUI();
            if (GOA.UserData.UserData.Instance.isDailyRewardShow&& GOA.UserData.UserData.Instance.dailyIncome>0)
            {
                {
                    StartCoroutine(StartShowDaily());
                }
                
            }
            if (GOA.UserData.UserData.Instance.LevelUpData.isLevelup)
            {
                OnLevelUp_Onclick();
            }
        }
        IEnumerator StartShowDaily()
        {
            yield return new WaitUntil(() => GOA.UserData.UserData.Instance.isNotifyShow==false);
            GOA.UserData.UserData.Instance.isDailyRewardShow = false;
            OnclickDailyReward();
        }
        public virtual void OffAllUI(){
            foreach (PopupUI popupUI in this.popupUIs)
            {
                popupUI.HideNone();
            }
        }

        //Get

        public float GetCurrentLvUI(){
            this.currentLvUI = 0;
            foreach (PopupUI popupUI in this.popupUIs)
            {
                if(popupUI.IsShow() && this.currentLvUI < popupUI.lvUI){
                    this.currentLvUI = popupUI.lvUI;
                }
            }
            return this.currentLvUI;
        }
        public void OnLevelUp_Onclick()
        {
            Debug.Log("Show level Up");
            GOA.UIMenu.ClassesUI levelUp = (GOA.UIMenu.ClassesUI)GetPopupUIByCode(PopupCode.LevelUpUI);
            if (levelUp == null) return;
            levelUp.Show();
            levelUp.UpdateData();
            
        }
        public void OnclickDailyReward()
        {
            DailyReward dailyReward = (DailyReward)GetPopupUIByCode(PopupCode.DailyRewardUI);
            if (dailyReward == null) return;
                dailyReward.Show();
        }

        public PopupUI GetPopupUIByCode(PopupCode popupCode){
            return this.popupUIs.Find((popupUI) => (popupUI.popupCode == popupCode));
        }
        public void OnButtonCharacter_Onclick()
        {
            PopupManager.Instance.OnUI(NTPackage.UI.PopupCode.CharacterGear_UI);
            //characgearUI.SetActive(true);

            //CharacterUIController.Instance.CameraUI.SetActive(true);
            // CharacterUIController.Instance.SwitchPanel(2);
        }
        public void OnButtonAdventure_Onclick()
        {
            Rubik.Common.AudioHelper.AudioCtrl.Instance.Play(Rubik.Common.AudioHelper.AudioName.UI_Button_Worldmap);
            //CharacterUIController.Instance.CameraUI.SetActive(true);
            //CharacterUIController.Instance.SwitchPanel(5);
            //CharacterUIController.Instance.bgGo.SetActive(true);

            bl_SceneLoaderManager.LoadScene(Configs.Adventure_Screen);

        }
        public void OnButtonQuest_Onclick()
        {
            CharacterUIController.Instance.bgGo.SetActive(true);
            CharacterUIController.Instance.CameraUI.SetActive(true);
        }
        public void OnButtonQuestQuit()
        {
            CharacterUIController.Instance.bgGo.SetActive(false);
            CharacterUIController.Instance.CameraUI.SetActive(false);
        }

        public void OnBtnInventory_Onclick(){
            InventoryUI.Manage.ShowInventory();
            CharacterUIController.Instance.bgGo.SetActive(false);
            CharacterUIController.Instance.CameraUI.SetActive(true);
        }

        public Transform GetUICollectionByName(string name){
            return this.collectionsUI.Find((ui)=>(ui.name.Equals(name)));
        }
        public void OnBtnDungeon_Onclick()
        {
            FindObjectOfType<Dungeon>().ShowDungeon();
        }
        public void OnButtonLineUp()
        {
            lineUpUI.SetActive(true);
        }
      
        public void PlayCampaign()
        {
            bl_SceneLoaderManager.LoadScene(Configs.Campaign_Screen);
        }
    }
}
