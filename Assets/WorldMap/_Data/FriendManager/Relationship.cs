using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rubik.Friend{
public enum RelationshipType {
    None = 0,
    Request = 1,
    Friend = 2,
    Block = 3,
}

[System.Serializable]
public class RelationshipData
    {
        public string _id;
        public string user1;
        public string user1Name = "Unknow";
        public string user2;
        public string user2Name = "Unknow";
        public bool User1FollowUser2;
        public bool User2FollowUser1;
        public RelationshipType type;
    }
}