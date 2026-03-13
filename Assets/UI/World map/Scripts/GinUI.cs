using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOA.UserData;

namespace WorldMap.Header{
    public class GinUI : ResUI
    {
        protected override void UpdateResNumber()
        {
            base.UpdateResNumber();
            try
            {
                this.textNumberRes.text = UserData.Instance.data.Gin.ToString(); 
            }
            catch (System.Exception){}
        }
    }
}
