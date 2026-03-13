using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using SimpleJSON;
using Rubik.Format;

namespace GOA.Portal{
    public class UserAttackDataItemUI : MonoBehaviour
    {
        public TextMeshProUGUI Rank;
        public TextMeshProUGUI Score;
        public TextMeshProUGUI Name;

        public void Init(UserAttackData userAttackData, int rank){
            this.Rank.text = rank.ToString();
            this.Score.text = FormatData.GetFriendlyShortNumberFromString(userAttackData.score);
            JSONNode data = JSONNode.Parse(userAttackData.data);
            if(data["Name"] != null) this.Name.text = data["Name"];
            else if(data["UserName"] != null) this.Name.text = data["UserName"];
            else this.Name.text = "Unknow";
        }
    }
}
