using Rubik.Manager;
using Rubik.UserDataPlayer;

namespace Rubik.Friend
{
    public enum FriendStatus
    {
        None = 0,
        Request = 1,
        Friend = 2,
        Block = 3,
    }

    [System.Serializable]
    public class FriendData
    {
        public string _id;
        public string User1_ID;
        public string User2_ID;
        public FriendStatus Status;
        public int Time;
        public int Follower;

        public string PartnerDisplay;
        public int PartnerAvatar;
        public int PartnerBorder;
        public string PartnerCustom;
        public int PartnerLevel;
        public int PartnerLastLogin;

        public void UpdateData(FriendData data)
        {
            this._id = data._id;
            this.Status = data.Status;
            this.Time = data.Time;
            this.Follower = data.Follower;

            if (data.PartnerDisplay != null)
            {
                this.PartnerDisplay = data.PartnerDisplay;
                this.PartnerAvatar = data.PartnerAvatar;
                this.PartnerBorder = data.PartnerBorder;
                this.PartnerLevel = data.PartnerLevel;
                this.PartnerLastLogin = data.PartnerLastLogin;
            }
        }

        public string GetPartnerID(){
            if(UserDataManager.Instance.GetUserID().Equals(this.User1_ID)){
                return this.User2_ID;
            }else{
                return this.User1_ID;
            }
        }

        public long GetTimeOffline(){
            return ServerManager.Instance.GetTimeServer() - this.PartnerLastLogin;
        }

        public bool YouAreFollower(){
            if(this.User1_ID == UserDataManager.Instance.GetUserID()){
                return this.Follower != 1;
            }else{
                return this.Follower != 2;
            }
        }
    }
}

