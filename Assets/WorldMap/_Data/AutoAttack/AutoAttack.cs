using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GOA.Config;
using GOA.Item;
using GOA.UserData;
using GOA.WorldMap;
using NTFunctions_old;
using NTPackage_old.EventDispatcher;
using Rubik.Combat;
using Rubik.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AutoAttack
{
    public enum State
    {
        Unknow,
        FindingMob,
        Attacking,
        UsingPotion,
    }

    public class AutoAttack : MonoBehaviour
    {
        public State State = State.Unknow;
        public bool IsAuto = false;
        public float TimeScale = 2;
        const float delayNextState = 1;
        public float countDelayNextState = 0;

        public string skill = "";
        public bool IsUseSkill = false;

        public bool outPotion = false;

        private void OnEnable() {
            this.State = State.Unknow;
        }
        private void OnDisable() {
            this.State = State.Unknow;
        }
        void Start()
        {
            if(!this.IsAuto){
                gameObject.SetActive(false);
            }
            EventListenerManager.instance.Register(EventCode.ViewInforMobOn,"AutoAttack",this.ClickViewInforMob);
            EventListenerManager.instance.Register(EventCode.EndCombat,"AutoAttack",this.EndCombat);
        }

        public List<CharacterEnemy> Enemy;

        void FixedUpdate()
        {
            if(!this.IsAuto){
                gameObject.SetActive(false);
                return;
            }
            this.countDelayNextState -= Time.fixedDeltaTime;
            if(countDelayNextState > 0) return;
            this.countDelayNextState = delayNextState;

            if (this.State == State.Unknow)
            {
                Scene scene = SceneManager.GetActiveScene();
                if(scene.name.Equals(Configs.Login_Screen)){
                    return;
                }
                if (scene.name.Equals(Configs.WorldMap_Screen))
                {
                    bl_SceneLoaderManager.LoadScene(Configs.WorldMap_Screen);
                }
                this.State = State.FindingMob;
            }
            if(this.State == State.FindingMob){
                if((float)UserData.Instance.characterData.CurrentHP/(float)UserData.Instance.characterData.HP < 0.5f && !this.outPotion){
                    this.State = State.UsingPotion;
                    this.UsePotion();
                    return;
                }
                try
                {
                    if(MobManager.instance.GreaterMobs.Count == 0) return;
                    Mob mob = MobManager.instance.GreaterMobs[0];
                    foreach (Mob item in MobManager.instance.GreaterMobs)
                    {
                        if(item.mobData.Lv > mob.mobData.Lv){
                            mob = item;
                        }
                    }
                    mob.mobCtrl.mobLock.ChoseMob();
                }
                catch (System.Exception)
                {
                    
                    throw;
                }
            }
            if(this.State == State.Attacking){
                Scene scene = SceneManager.GetActiveScene();
                if(scene.name.Equals(Configs.Combat_Screen)){
                    if(this.Enemy.Count == 0){
                        CharacterEnemy[] enemy = FindObjectsOfType<CharacterEnemy>();
                        foreach (CharacterEnemy item in enemy)
                        {
                            if(item.CharacterState.data.typeMob!= MobTypeCode.unknowType){
                                this.Enemy.Add(item);
                            }
                        }
                    }
                    foreach (CharacterEnemy item in Enemy)
                    {
                        if(item.gameObject.activeSelf){
                            if(this.IsUseSkill && this.skill.Length > 0){
                                MainCombatUI.Instance.OnChooseSkill(this.skill);
                            }
                            item.Touch();
                            item.Touch();
                            return;
                        }
                    }
                }
            }
        }

        public void ClickViewInforMob(object data = null){
            if(!this.IsAuto){
                gameObject.SetActive(false);
                return;
            }
            ViewInforMob viewInforMob = (ViewInforMob) UIManager.instance.GetPopupUIByCode(PopupCode.ViewInforMobUI);
            this.State = State.Attacking;
            this.Enemy.Clear();
            viewInforMob.btnAttack_Onclick();
            Time.timeScale = this.TimeScale;
        }

        public void EndCombat(object data = null){
            Time.timeScale = 1;
            if(!this.IsAuto){
                gameObject.SetActive(false);
                return;
            }
            try
            {
                this.State = State.FindingMob;
                EndCombatUI endCombatUI = FindObjectOfType<EndCombatUI>();
                endCombatUI.OnClick();
            }
            catch (System.Exception e)
            {
                Debug.LogWarning(e);
            }
            Time.timeScale = 1;
        }

        public void UsePotion(){
            ItemCode itemCode = ItemCode.Unknow;
            if(UserData.Instance.Inventory.GetInventoryByCode(ItemCode.Potion) > 0){
                itemCode = ItemCode.Potion;
            }else if(UserData.Instance.Inventory.GetInventoryByCode(ItemCode.HiPotion) > 0){
                itemCode = ItemCode.HiPotion;
            }else if(UserData.Instance.Inventory.GetInventoryByCode(ItemCode.Elixir) > 0){
                itemCode = ItemCode.Elixir;
            }
            if(itemCode == ItemCode.Unknow){
                this.IsAuto = false;
                this.State = State.Unknow;
                this.outPotion = true;
                return;
            }
            APIManager.Instance.UserItem(itemCode, DoneUseItem);
        }
        
        public void DoneUseItem(object data = null){
            this.State = State.FindingMob;
        }
    }
}