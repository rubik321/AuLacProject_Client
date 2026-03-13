using System;
using System.Collections;
using System.Collections.Generic;
using GOA.WorldMap;
using NTFunctions_old;
using NTPackage_old.Functions;
using Rubik.Chat;
using SimpleJSON;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GOA.LandEvent
{
    public class LandEventManager : LoadBehaviour
    {
        public long LastTimeUpdateEvent = 0;
        public const long DelayTimeUpdateEvent = 5;
        public const long DefaultTimeEvent = 60;

        public NTDictionary<LandEventData> LandUserEventDataDic;
        public NTDictionary<LandEventData> LandGlobalEventDataDic;

        public NTDictionary<LandEventInfo> LandEventInfoDic;
        public TextAsset DataLandEvent;

        public static LandEventManager instance;
        protected override void Awake()
        {
            base.Awake();
            if (LandEventManager.instance != null)
            {
                Debug.LogWarning("Only 1 instance allow");
                return;
            }
            LandEventManager.instance = this;
        }

        public override void LoadComponents()
        {
            base.LoadComponents();
            if (this.DataLandEvent == null) this.DataLandEvent = Resources.Load<TextAsset>("DataLandEvent");
        }

        protected override void Start(){
            this.LandEventInfoDic.Clear();
                JSONNode jdata = JSONNode.Parse(this.DataLandEvent.text);
                foreach (JSONNode item in jdata)
                {
                    LandEventInfo landEventInfo = JsonUtility.FromJson<LandEventInfo>(item.ToString());
                    this.LandEventInfoDic.Add(landEventInfo.EventType.ToString(), landEventInfo);
                }
        }

        public IEnumerator GetUserData()
        {
            while (true)
            {
                try
                {
                    this.GetUserEvent();
                    this.GetLandEvent();
                    break;
                }
                catch (System.Exception e)
                {
                    NTLog.LogError(e.ToString(), gameObject);
                }
                yield return new WaitForSeconds(1);
            }


        }

        public void GetUserEvent()
        {
            APIManager.Instance.GetUserEvent(UserData.UserData.Instance.data.UserId, (data) =>
            {
                this.LandUserEventDataDic.Clear();
                JSONNode jdata = JSONNode.Parse(data);
                foreach (JSONNode item in jdata["Data"])
                {
                    JSONNode jitem = JSONNode.Parse(item);
                    LandEventData landEventData = JsonUtility.FromJson<LandEventData>(jitem.ToString());
                    if (landEventData.Duration < 1) landEventData.Duration = DefaultTimeEvent;
                    this.AddLandEventData(landEventData);
                }
                Debug.LogWarning(data);
            });
        }

        public void GetLandEvent(Action done = null)
        {
            if (NTFunction.GetUtcTimestamp() - this.LastTimeUpdateEvent > DelayTimeUpdateEvent)
            {
                this.LandGlobalEventDataDic.Clear();
                APIManager.Instance.LandGetEvent((data) =>
                {
                    this.LastTimeUpdateEvent = NTFunction.GetUtcTimestamp();
                    JSONNode jdata = JSONNode.Parse(data);
                    foreach (JSONNode item in jdata["Data"])
                    {
                        LandEventData landEventData = JsonUtility.FromJson<LandEventData>(item.ToString());
                        if (landEventData.Duration < 1) landEventData.Duration = DefaultTimeEvent;
                        Debug.LogWarning(landEventData.Duration);
                        if (NTFunction.GetUtcTimestamp() - landEventData.Time < landEventData.Duration)
                        {
                            this.LandGlobalEventDataDic.Add(landEventData.GetLandEventID(), landEventData);
                        }
                    }
                    done?.Invoke();
                });
            }
            else
            {
                done?.Invoke();
            }
        }

        public void AddLandEventData(LandEventData landEventData)
        {
            this.LandUserEventDataDic.Add(landEventData.GetLandEventID(), landEventData);
        }

        public LandEventData GetLandEventData(string userID, string eventID)
        {
            return this.LandUserEventDataDic.Get(userID +":"+eventID);
        }

        [Button]
        public bool CheckEventIsEnd()
        {
            // if (GeoPointManager.Instance.EventType != EventType.None)
            // {
            //     LandEventData landEvent = LandEventManager.instance.LandGlobalEventDataDic.Get(ChatManager.Instance.EventChannelJoin);
            //     if (landEvent == null) return true;
            //     float time = landEvent.Duration + landEvent.Time - NTFunction.GetUtcTimestamp();
            //     if (time > 0) return false;
            //     return true;
            // }
            return false;
        }
    }
}