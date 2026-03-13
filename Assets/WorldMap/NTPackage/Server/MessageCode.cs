using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NTPackage_old.Server{
    public enum MessageCode
    {
        MessageServer_Test = 1000,
        MessageServer_Connect = 1001,
        MessageConnectResponse = 1002,
        MessageServer_ConnectFail = 1003,
        MessageServer_ConnectSuccess = 1004,
        MessageServer_Disconnect = 1005,


        AccountServer_Register = 2001,
        AccountServer_RegisterFail = 2002,
        AccountServer_RegisterSuccess = 2003,
        AccountServer_Login = 2004,
        AccountServer_LoginFail = 2005,
        AccountServer_LoginSuccess= 2006,
        AccountServer_LoginToken = 2007,

        UserPlayerServer_Login = 3001,
        UserPlayerServer_LoginFail = 3002,
        UserPlayerServer_LoginSuccess= 3003,

        Res_Login = 4001,
        Res_LoginFail = 4002,
        Res_LoginSuccess = 4003,
        Res_UpdateRes = 4004,

        Hero_Login = 5001,
        Hero_LoginFail = 5002,
        Hero_LoginSuccess = 5003,
        Hero_GetSummonResult = 5011,
        Hero_SendSummonResult = 5012,
        Hero_Summon = 5013,
        Hero_SummonFail = 5014,
        Hero_SummonSuccess = 5019,
        Hero_HireHero = 5015,
        Hero_HireHeroSuccess = 5016,
        Hero_HireHeroFail = 5017,
    }

}
