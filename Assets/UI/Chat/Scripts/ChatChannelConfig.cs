namespace Rubik.Chat
{
    public class ChatChannelConfig{
        public static string GetGlobalPortal(){
            return "Event:GlobalPortal";
        }

        public static string GetLandEvent(string landEventID){
            return landEventID;
        }
    }
}