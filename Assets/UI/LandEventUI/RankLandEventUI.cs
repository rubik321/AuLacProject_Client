using System.Collections;
using System.Collections.Generic;
using GOA.Portal;
using GOA.WorldMap;
using NTFunctions_old;
using NTPackage_old.Functions;
using Rubik.Chat;
using Rubik.Common.Popup;
using SimpleJSON;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace GOA.LandEvent
{
    public class RankLandEventUI : PopupUI
    {
        public List<UserAttackDataItemUI> UserAttackDataItemUIs;

        public TextMeshProUGUI TextTitle;

        public Transform UserContribute;
        public TextMeshProUGUI TextName;
        public TextMeshProUGUI TextRank;
        public TextMeshProUGUI TextScore;
        public TextMeshProUGUI TextTotalScore;
        public TextMeshProUGUI TextPlayerJoin;

        public TextMeshProUGUI TextTimeEnd;
        public TextMeshProUGUI TextEventName;


        public void OnUI(){
            // switch (GeoPointManager.Instance.EventType)
            // {
            //     case EventType.Hunter:
            //         this.TextEventName.text = Lean.Localization.LeanLocalization.GetTranslationText("hunter_event", "Hunting Event");
            //         this.TextTitle.text = Lean.Localization.LeanLocalization.GetTranslationText("hunter_event", "Hunting Event");
            //         break;
            //     case EventType.Explore:
            //         break;
            //     default:
            //         return;
            // }
            // this.UserContribute.gameObject.SetActive(false);
            // foreach (UserAttackDataItemUI item in UserAttackDataItemUIs)
            // {
            //     item.gameObject.SetActive(false);
            // }
            // APIManager.Instance.LandGetRankEvent((data)=>{
            //     JSONNode jdata = JSONNode.Parse(data);
            //     List<UserAttackData> userAttackDatas = new List<UserAttackData>();
            //     this.TextTotalScore.text = jdata["Data"]["Total"];
            //     this.TextPlayerJoin.text = jdata["Data"]["Players"];
            //     foreach (JSONNode item in jdata["Data"]["List"])
            //     {
            //         try
            //         {
            //             UserAttackData userAttackData = JsonUtility.FromJson<UserAttackData>(item.ToString());
            //             if(int.Parse(userAttackData.score) > 0){
            //                 userAttackDatas.Add(userAttackData);
            //             }
            //         }
            //         catch (System.Exception e)
            //         {
            //             Debug.LogWarning(e);
            //         }
            //     }
            //     for (int i = 0; i < UserAttackDataItemUIs.Count; i++)
            //     {
            //         try
            //         {
            //             UserAttackDataItemUIs[i].Init(userAttackDatas[i], i+1);
            //             UserAttackDataItemUIs[i].gameObject.SetActive(true);
            //         }
            //         catch (System.Exception)
            //         {
            //             UserAttackDataItemUIs[i].gameObject.SetActive(false);
            //         }
            //     }
            //     if(jdata["Data"]["Rank"] != null){
            //         this.UserContribute.gameObject.SetActive(true);
            //         this.TextName.text = UserData.UserData.Instance.data.DisplayName;
            //         this.TextScore.text = jdata["Data"]["Score"];
            //         if(jdata["Data"]["Rank"] > 98) this.TextRank.text = "99+";
            //         else this.TextRank.text = (jdata["Data"]["Rank"]+1).ToString();
            //     }
            //     StartCoroutine(this.CountDownTime());
            // });
            // this.Show();
        }

        IEnumerator CountDownTime(){
            while (true)
            {
                if(!gameObject.activeSelf) break;
                yield return new WaitForSeconds(1);
                LandEventData landEventData = LandEventManager.instance.LandGlobalEventDataDic.Get(ChatManager.Instance.EventChannelJoin);
                if(landEventData == null) break;
                this.TextTimeEnd.text = Lean.Localization.LeanLocalization.GetTranslationText("event_end_in", "Event Ending In:") +" "+ NTFunction.FormatTimeHour(landEventData.Duration + landEventData.Time - NTFunction.GetUtcTimestamp());
            }
        }

        public void OnClickDes(){
            // try
            // {
            //     ((LandEventDescUI) UIManager.instance.GetPopupUIByCode(PopupCode.LandEventDescUI)).OnUI(GeoPointManager.Instance.EventType);
            // }
            // catch (System.Exception e)
            // {
            //     NTLog.LogError(e.ToString(), gameObject);
            // }
        }
    }
}
