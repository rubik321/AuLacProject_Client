using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NTFunctions_old;
using Rubik.Friend;
using SimpleJSON;

namespace GOA.UIFriends{

    public class RequestTabUI : TabUI
    {
        public List<RelationshipData> RelationshipDatas = new List<RelationshipData>();
        
        public FriendRequestBox FriendRequestBoxSample;
        public Transform Holder;

        public bool IsLoad = false;

        public override void UpdateData(){
            base.UpdateData();
            // if(!this.IsLoad) 
            this.UpdateFriend();
        }

        public void UpdateFriend(){
            this.IsLoad = true;
            this.RelationshipDatas.Clear();
            NTFunction.ClearChild(this.Holder);
            FriendController.instance.GetFriendRequest(res=>{
                JSONNode data = JSONNode.Parse(res);
                JSONNode elements = data["Data"]["list"];
                for (int i = 0; i < elements.Count; i++)
                {
                    RelationshipData relationshipData = JsonUtility.FromJson<RelationshipData>(elements[i].ToString());
                    this.RelationshipDatas.Add(relationshipData);
                    FriendRequestBox friendRequestBox = Instantiate(this.FriendRequestBoxSample);
                    friendRequestBox.FriendUI = this;
                    friendRequestBox.RelationshipData = relationshipData;
                    friendRequestBox.UpdateData();
                    friendRequestBox.transform.SetParent(this.Holder);
                    NTFunction.ResetPosition(friendRequestBox.transform);
                }
            });
        }
    }
}
