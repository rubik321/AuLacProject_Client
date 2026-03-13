
namespace Rubik.MsgDelivery
{
    public class MsgDeliveryKey
    {
        public const string System = "System";
        public const string Ping = "Ping";
        public const string Pong = "Pong";
        public const string RegisterChannel = "RegisterChannel";
        public const string LeaveChannel = "LeaveChannel";
        public const string LeaveAllChannel = "LeaveAllChannel";
        public const string CheckUserOnline = "CheckUserOnline";

        // Chat
        public const string ChatMsg = "ChatMsg";

        // Game Room
        public const string InviteGameRoom = "InviteGameRoom";

        // Portal
        public const string PortalMsg = "PortalMsg";
    }
}