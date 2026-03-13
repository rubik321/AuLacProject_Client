using System.Collections.Generic;
using NTPackage.Functions;
using UnityEngine;

namespace Rubik.UserDataPlayer
{
    public class RoleVisible : NTBehaviour
    {

        public List<Role> Roles;

        protected override void Awake(){
            base.Awake();
            this.IsVisible();
        }

        protected override void OnEnable(){
            base.OnEnable();
            this.IsVisible();
        }

        protected override void OnDisable(){
            base.OnDisable();
            this.IsVisible();
        }

        protected override void Start(){
            base.Start();
            this.IsVisible();
        }

        public void IsVisible(){
            if(UserDataManager.Instance.IsRole(this.Roles.ToArray())){
                gameObject.SetActive(true);
            }
            else{
                gameObject.SetActive(false);
            }
        }
    }
}