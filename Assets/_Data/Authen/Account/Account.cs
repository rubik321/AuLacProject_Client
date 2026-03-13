using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rubik.Account
{
    [System.Serializable]
    public class Account
    {
        public string _id;
        public long AccountID;
        public string Username;
        public string Password;
        public string Email;
        public string DeviceID;
        public string GoogleEmail;
        public string GoogleName;
        public string GooglePlayID;
        public string GooglePlayName;
        public string AppleID;
        public string AppleName;
    }

    public class AuthenResponse
    {
        public int Status;
        public string Message;

        public Account Account;
        public string Token;
    }

}