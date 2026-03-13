using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOA.UserData;
using Rubik.Format;

namespace WorldMap.Header{
    public class CoinUI : ResUI
    {
        protected override void UpdateResNumber()
        {
            base.UpdateResNumber();
            try
            {
                this.textNumberRes.text = FormatData.GetFriendlyShortNumber(UserData.Instance.data.Coin) ; 
            }
            catch (System.Exception){}
        }
    }
}
