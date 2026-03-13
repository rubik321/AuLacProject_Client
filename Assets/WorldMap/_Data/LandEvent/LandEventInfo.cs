using GOA.Item;

namespace GOA.LandEvent
{
    [System.Serializable]
    public class MatRequire{
        public ItemCode Code;
        public int Amount;
    }

    [System.Serializable]
    public class LandEventInfo{
        public EventType EventType;
        public MatRequire[] MatRequire;
    }
}