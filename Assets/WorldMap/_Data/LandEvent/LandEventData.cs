using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOA.LandEvent
{
   public enum EventType
    {
        None = -1,
        Hunter,
        Explore,
        Survival
    }

    [System.Serializable]
    public class LandEventData
    {
        public string landID;
        public long Time;
        public long Duration;
        public EventType EventType;
        public string UserID;
        public string UserName;
        public int PeopleJoined = 0;
        public double Latitude;
        public double Longitude;

        public string GetLandEventID(){
            if(this.landID == null || this.landID.Length == 0) return UserID + ":" + Latitude + Longitude;
            return UserID + ":" +this.landID;
        }
            

    }
}

