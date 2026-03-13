using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rubik.PlayerMail
{
    using ItemPlayer;

    public enum PlayerMailType{
        System = 0,
        Notification = 1,
    }

    [System.Serializable]
    public class PlayerMail
    {
        public string _id;
        public string Title;
        public PlayerMailType Type;
        public double TimeSend;
        public string Detail;
        public List<string> InjectString = new List<string>();
        public bool IsRecieve = false;
        public bool IsDelete = false;
        public bool IsRead = false;

        public PlayerMailAttached Attached = new PlayerMailAttached();

        public void UpdateData(PlayerMail playerMail)
        {
            this.Title = playerMail.Title;
            this.TimeSend = playerMail.TimeSend;
            this.Detail = playerMail.Detail;
            this.InjectString = playerMail.InjectString;
            this.IsRecieve = playerMail.IsRecieve;
            this.IsDelete = playerMail.IsDelete;
            this.Attached = playerMail.Attached;
            this.IsRead = playerMail.IsRead;

            if(this.Attached.IsEmpty()){
                this.IsRecieve = true;
            }else{
                if(this.IsRecieve){
                    this.IsRead = true;
                }
            }
        }
    }

    [System.Serializable]
    public class PlayerMailAttached
    {
        public List<ItemData> Items = new List<ItemData>();

        public bool IsEmpty()
        {
            if (Items.Count > 0) return false;
            return true;
        }
    }

}