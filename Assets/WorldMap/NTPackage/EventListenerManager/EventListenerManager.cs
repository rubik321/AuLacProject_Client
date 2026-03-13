using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace NTPackage_old.EventDispatcher
{
    using System.Linq;
    using Functions;
    using NTFunctions_old;

    public enum EventCode{
        ReciveChatGlobal,
        ReciveChatFriend,
        ViewInforMobOn,
        EndCombat,
        ChangeNormalView,
        ChangeRegionView,
        OffAttackSound,
    }

    public class EventListenerManager : LoadBehaviour
    {
        public NTDictionary<EventCode, NTDictionary<string, Action<object>>> ActionsDictionary = new NTDictionary<EventCode, NTDictionary<string, Action<object>>>();

        public static EventListenerManager instance;
        protected override void Awake()
        {
            base.Awake();
            if (EventListenerManager.instance != null)
            {
                Debug.LogWarning("Only 1 instance allow");
                return;
            }
            EventListenerManager.instance = this;
        }

        public void PostEvent(EventCode eventCode,object data = null){
            NTDictionary<string,Action<object>> actions = this.ActionsDictionary.Get(eventCode);
            if(actions == null || actions.Dictionary.Count == 0) return;
            foreach (String item in actions.Dictionary.Keys.ToList())
            {
                try
                {
                    actions.Dictionary[item].Invoke(data);
                }
                catch (System.Exception)
                {
                    actions.Remove(item);
                }
            }
        }

        public void Register(EventCode eventCode, string key,Action<object> callback){
            
            NTDictionary<string, Action<object>> actions = this.ActionsDictionary.Get(eventCode);
            if(actions == null){
                actions = new NTDictionary<string, Action<object>>();
                this.ActionsDictionary.Add(eventCode, actions);
            }
            actions.Add(key, callback);
        }
    }
}

