using System;
using System.Collections;
using System.Collections.Generic;

namespace GOA.Portal{
    [System.Serializable]
    public enum PortalStatus{
        Open = 1,
        Close = 2,
        Defeated = 3,
        Destroy = 4,
    }
    [System.Serializable]
    public class UserAttackData{
        public string userID;
        public string score;
        public string data;
    }
    [System.Serializable]
    public class PortalAttackDetail{
        public int TimeOpen;
        public PortalStatus Status;
        public string Version;
    }
    [System.Serializable]
    public class PortalAttackData{
        public List<UserAttackData> List;
        public int Rank;
        public string Score;
        public string Hp;
        public PortalAttackDetail Detail;
    }
}