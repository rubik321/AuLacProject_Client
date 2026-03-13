using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoShared;
using GOA.WorldMap;
using NTFunctions_old;
using Rubik.Combat;

namespace GOA.WorldMap
{
    public class PlayerData{
        public string Action;
        public bool Join;
        public string UserName;
        public string UserId;
        public List<string> GearCodes;
        public string Class;
        public double latitude;
        public double longitude;
    }

    public class Player : LoadBehaviour
    {

        public BaseCharacter character;

        public List<Sprite> ClassIcons;
        public Image Icon;

        public Coordinates Coordinates{
            get => Coordinates.convertVectorToCoordinates(transform.position);
        }
      
        [SerializeField]
        private double limitDis = 1;
        public double LimitDis{
            get => this.limitDis * NTConst.distanceUnit;
        }
        public Transform clouds;
        public Transform skin;
        protected override void Start()
        {
            base.Start();
        }
        public override void LoadComponents()
        {
            base.LoadComponents();
            this.LoadClouds();
            this.LoadSkin();
        }

        protected void LoadClouds(){
            if(clouds != null) return;
            this.clouds = transform.Find("Clouds");
        }

        protected void LoadSkin(){
            if(skin != null) return;
            this.skin = transform.Find("Skin");
        }

        //Function
        public float delayUpdateData = 0.5f;
        [SerializeField]
        private float countTimeUpdateData = 0f;

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            this.countTimeUpdateData -= Time.fixedDeltaTime;
            if(this.countTimeUpdateData < 0) this.UpdateData();
        }

        public void UpdateData(){
            try
            {
                this.Icon.sprite = this.ClassIcons[UserData.UserData.Instance.characterData.CharType % this.ClassIcons.Count];
                if (GameMaster.instance.viewModeCode == ViewModeCode.god) this.ViewModeGod();
                else this.ViewModePer();
            }
            catch { }
            
        }

        protected void ViewModeGod(){
            this.clouds.gameObject.SetActive(false);
            this.skin.gameObject.SetActive(false);
        }

        protected void ViewModePer(){
            this.clouds.gameObject.SetActive(true);
            this.skin.gameObject.SetActive(true);
        }
    }
}