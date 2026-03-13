using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NTFunctions_old;
using Rubik.Friend;
using SimpleJSON;

namespace GOA.UIFriends{
    public class FriendTabUI : TabUI
    {
        public List<RelationshipData> RelationshipDatas = new List<RelationshipData>();
        
        public FriendBox FriendBoxSample;
        public Transform Holder;

        public bool IsLoad = false;

        protected override void Start()
        {
            base.Start();
            this.IsLoad = false;
        }

        public override void UpdateData(){
            base.UpdateData();
            // if(!this.IsLoad) 
            this.UpdateFriend();
        }

        public void UpdateFriend(){
            this.IsLoad = true;
            this.RelationshipDatas.Clear();
            NTFunction.ClearChild(this.Holder);
            FriendController.instance.GetFriendList(res=>{
                JSONNode data = JSONNode.Parse(res);
                JSONNode elements = data["Data"]["list"];
                for (int i = 0; i < elements.Count; i++)
                {
                    RelationshipData relationshipData = JsonUtility.FromJson<RelationshipData>(elements[i].ToString());
                    this.RelationshipDatas.Add(relationshipData);
                    FriendBox friendBox = Instantiate(this.FriendBoxSample);
                    friendBox.RelationshipData = relationshipData;
                    friendBox.UpdateData();
                    friendBox.transform.SetParent(this.Holder);
                    NTFunction.ResetPosition(friendBox.transform);
                }
            });
        }
    
    }
}
