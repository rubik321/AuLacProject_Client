using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Newtonsoft.Json;
using System;
using Colyseus;
using SimpleJSON;
using Sirenix.OdinInspector;
using Rubik.UI;
using GOA.Config;
using GOA.UserData;
using UnityEngine.Events;
using System.Security.Cryptography;

using GOA.Portal;
using GOA.Item;
using System.Text;
using UnityEngine.SceneManagement;
using NTPackage_old.EventDispatcher;
using GOA.UIProfile;
using GOA.WorldMap;
using Rubik.Chat;

public class AttackMobInfo
{

    public string charID;
    public string mobID;
    public int HP;
    public int MP;
    public int mobLevel;
    public int mobType;
    public bool isVictory;
    public int Potion;
    public int HiPotion;
    public int Ether;
    public int HiEther;
    public int Elixir;
    public string outpostID;
    public string portalID;
    public int TotalDame;
    public int land;
    public int numOfMob;
    public int EventType;
    public string EventID;
    public string DisplayName;
}
public class QuestInfo
{

    public string userID;
    public string questID;
}
public class OrbInfo
{
    public string userID;
    public string charID;
    public string orbID;
    public int slot;
}
public class OrdID
{
    public string orbID;
}
public class GearInfo
{
    public string userID;
    public string charID;
    public string gearID;
    public string slot;
    public int equipedSlot;
    public bool isTwoHand;
}
public class UserGearInfo
{
    public string userID;
    public string gearID;
}
public class UserCompanionInfo
{
    public string userID;
    public string companionID;
    public int rarerity;
}
public class APIManager : MonoBehaviour
{
    public static APIManager Instance;
    string jsonAttackMod, jsonlogin, userID;
    ItemGearDrop gearRewards = new ItemGearDrop();
    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }
    public void AttackMobAPI(bool isWin, int hp, int mp, Action<ItemGearDrop> callback)
    {
        //gearRewards = new GearDatas();
        AttackMobInfo user = new AttackMobInfo();
        user.charID = UserData.Instance.characterData.CharacterID;
        user.mobLevel = UserData.Instance.DataInCombat.mobsInCombat[0].Lv;
        user.HP = hp;
        user.MP = mp;
        user.Potion = UserData.Instance.Inventory.Potion;
        user.HiPotion = UserData.Instance.Inventory.HiPotion;
        user.Ether = UserData.Instance.Inventory.Ether;
        user.HiEther = UserData.Instance.Inventory.HiEther;
        user.Elixir = UserData.Instance.Inventory.Elixir;
        user.isVictory = isWin;
        user.land = UserData.Instance.CountLand;
        user.numOfMob = UserData.Instance.DataInCombat.mobsInCombat.Count;
        // user.EventType = (int)GOA.WorldMap.GeoPointManager.Instance.EventType;
        user.EventID = ChatManager.Instance.EventChannelJoin;
        user.DisplayName = UserData.Instance.data.DisplayName.Length > 0 ? UserData.Instance.data.DisplayName : UserData.Instance.data.UserName;
        if (!String.IsNullOrEmpty(UserData.Instance.DataInCombat.OutpostId))
        {
            user.outpostID = UserData.Instance.DataInCombat.OutpostId;
        }
        if (!String.IsNullOrEmpty(UserData.Instance.DataInCombat.PortalId))
        {
            user.portalID = UserData.Instance.DataInCombat.PortalId;
            user.TotalDame = UserData.Instance.TotalDameInPortal;
        }
        switch ((int)UserData.Instance.DataInCombat.TypeCombat)
        {
            case 10001:
                user.mobType = 0;
                break;
            case 20001:
                user.mobType = 1;
                break;
            case 30001:
                user.mobType = 2;
                break;
            case 40001:
                user.mobType = 3;
                break;
            case 4:
                user.mobType = 4;
                break;
        }

        user.mobID = UserData.Instance.DataInCombat.mobsInCombat[0].Index;
        jsonAttackMod = JsonUtility.ToJson(user);
        StartCoroutine(PostData(jsonAttackMod, SeverConfigs.BASE_API_ATTACKMOB, (uwr) =>
        {
            JSONNode info = JSON.Parse(uwr.downloadHandler.text);
            string mess = uwr.downloadHandler.text;

            //if (!isWin)
            //{
            //    callback(gearRewards);
            //    return;
            //}


            gearRewards = JsonUtility.FromJson<ItemGearDrop>(mess);
            if (isWin)
            {
                UserData.Instance.GetCharacterData(info);
                UserData.Instance.GetStatsGearData();
                GetUserCompanion();
            }
            GOA.UserData.UserData.Instance.LevelUpData = gearRewards.Data.LevelUpStats;
            GOA.UserData.UserData.Instance.LevelUpData.isLevelup = gearRewards.Data.LevelUp;
            callback(gearRewards);
            user.portalID = "";
            user.TotalDame = 0;
        }));

    }
    public void EquipOrb(OrbData data, bool isEquip = true)
    {
        //gearRewards = new GearDatas();
        OrbInfo user = new OrbInfo();

        HUDCanvas.Instance.ShowLoadingPanel();
        if (isEquip)
        {
            user.userID = UserData.Instance.data.UserId;
            user.charID = UserData.Instance.characterData.CharacterID;
            user.orbID = data._id;
            user.slot = data.EquipedSlot;

            jsonAttackMod = JsonUtility.ToJson(user);
            StartCoroutine(PostData(jsonAttackMod, SeverConfigs.BASE_API_EQUIP_ORB, (uwr) =>
            {
                JSONNode info = JSON.Parse(uwr.downloadHandler.text);
                string mess = uwr.downloadHandler.text;
                GetUserOrb();

            }));
        }
        else
        {
            OrdID orb = new OrdID();
            orb.orbID = data._id;
            jsonAttackMod = JsonUtility.ToJson(orb);
            StartCoroutine(PostData(jsonAttackMod, SeverConfigs.BASE_API_UNEQUIP_ORB, (uwr) =>
            {
                JSONNode info = JSON.Parse(uwr.downloadHandler.text);
                string mess = uwr.downloadHandler.text;
                GetUserOrb();
            }));
        }

    }

    public void EquipCompanion(string companionID, bool isEquip = true, UnityAction callback = null)
    {
        //gearRewards = new GearDatas();
        UserCompanionInfo user = new UserCompanionInfo();

        HUDCanvas.Instance.ShowLoadingPanel();

        user.userID = UserData.Instance.data.UserId;
        user.companionID = companionID;
        jsonAttackMod = JsonUtility.ToJson(user);
        if (isEquip)
        {
            StartCoroutine(PostData(jsonAttackMod, SeverConfigs.BASE_API_EQUIP_COMPANION, (uwr) =>
            {
                JSONNode info = JSON.Parse(uwr.downloadHandler.text);
                string mess = uwr.downloadHandler.text;
                GetUserCompanion(() =>
                {
                    if (callback != null)
                        callback();
                });

            }));

        }
        else
        {
            StartCoroutine(PostData(jsonAttackMod, SeverConfigs.BASE_API_UNEQUIP_COMPANION, (uwr) =>
            {
                JSONNode info = JSON.Parse(uwr.downloadHandler.text);
                string mess = uwr.downloadHandler.text;
                GetUserCompanion(() =>
                {
                    if (callback != null)
                        callback();
                });

            }));
        }
    }
    public void GetDungeon( UnityAction callback = null)
    {
        return;
        //gearRewards = new GearDatas();
        JSONNode data = new JSONObject();
        data["userID"] = UserData.Instance.data.UserId;
        
        StartCoroutine(PostData(data.ToString(), SeverConfigs.BASE_API_GET_DUNGEON, (uwr) =>
        {
            JSONNode info = JSON.Parse(uwr.downloadHandler.text);
            string mess = uwr.downloadHandler.text;
            JSONNode res = JSONNode.Parse(mess);
            UserData.Instance.DungeonInfo = JsonUtility.FromJson<DungeonInfo>(res["Data"].ToString());
            //GetUserCompanion(() => {
            //    if (callback != null)
            //        callback();
            //});

        }));


    }
    public void JoinDungeon(UnityAction callback = null)
    {
        //gearRewards = new GearDatas();
        JSONNode data = new JSONObject();
        data["userID"] = UserData.Instance.data.UserId;

        HUDCanvas.Instance.ShowLoadingPanel();


        StartCoroutine(PostData(data.ToString(), SeverConfigs.BASE_API_JOIN_DUNGEON, (uwr) =>
        {
            JSONNode info = JSON.Parse(uwr.downloadHandler.text);
            string mess = uwr.downloadHandler.text;
            //JSONNode res = JSONNode.Parse(mess);
            //UserData.Instance.DungeonInfo = JsonUtility.FromJson<DungeonInfo>(res["Data"].ToString());
            //GetUserCompanion(() => {
            //   
            //});
            if (callback != null)
                   callback();

        }));


    }

    public void SendAdventure(string companionID, UnityAction callback = null)
    {
        //gearRewards = new GearDatas();
        UserCompanionInfo user = new UserCompanionInfo();

        HUDCanvas.Instance.ShowLoadingPanel();

        user.userID = UserData.Instance.data.UserId;
        user.companionID = companionID;


        jsonAttackMod = JsonUtility.ToJson(user);
        StartCoroutine(PostData(jsonAttackMod, SeverConfigs.BASE_API_SEND_ADVENTURE, (uwr) =>
        {
            JSONNode info = JSON.Parse(uwr.downloadHandler.text);
            string mess = uwr.downloadHandler.text;
            //GetUserCompanion(() => {
            //    if (callback != null)
            //        callback();
            //});

        }));


    }
    public void ClaimAdventure(CompanionData companionData, UnityAction callback = null)
    {
        //gearRewards = new GearDatas();
        UserCompanionInfo user = new UserCompanionInfo();

        HUDCanvas.Instance.ShowLoadingPanel();
        user.userID = UserData.Instance.data.UserId;
        user.companionID = companionData._id;
        user.rarerity = companionData.Rarity;

        jsonAttackMod = JsonUtility.ToJson(user);
        StartCoroutine(PostData(jsonAttackMod, SeverConfigs.BASE_API_CLAIM_ADVENTURE, (uwr) =>
        {
            JSONNode info = JSON.Parse(uwr.downloadHandler.text);
            string mess = uwr.downloadHandler.text;
            int coin = 0;
            int gin = 0;
            System.Collections.Generic.List<(string, int)> lsItem = new System.Collections.Generic.List<(string, int)>();
            if (info["Data"]["Coin"] != null)
            {
                coin = info["Data"]["Coin"];
                UserData.Instance.data.Coin += coin;
                lsItem.Add(new("icon_coin", coin));
            }

            if (info["Data"]["Gin"] != null)
            {

                gin = info["Data"]["Gin"];
                UserData.Instance.data.Gin += gin;
                lsItem.Add(new("icon_gin", gin));
            }


            string gear = null;
            if (info["Data"]["Gear"]["GearCode"] != null)
            {
                gear = info["Data"]["Gear"]["GearCode"];
                Debug.Log("Adventure : " + coin + " - " + gin + " - " + gear);
                lsItem.Add(new(gear, 1));
            }

            HUDCanvas.Instance.ShowReward(lsItem);
            GetUserGear(callback);
            //GetUserCompanion(() => {
            //    if (callback != null)
            //        callback();
            //});

        }));


    }
    public void GetUserOrb(UnityAction callback = null)
    {

        UserGearInfo user = new UserGearInfo();
        user.userID = UserData.Instance.data.UserId;

        string jsonTemp = JsonUtility.ToJson(user);
        UnityWebRequest www;
        StartCoroutine(PostData(jsonTemp, SeverConfigs.BASE_API_GET_USER_ORB, (uwr) =>
        {
            JSONNode info = JSON.Parse(uwr.downloadHandler.text);

            string mess = uwr.downloadHandler.text;

            UserData.Instance.GetSkillsData(info["Data"].ToString());
           

            HUDCanvas.Instance.HideLoadingPanel();
        }));
    }
    public void GetUserCompanion(UnityAction callback = null)
    {

        UserGearInfo user = new UserGearInfo();
        user.userID = UserData.Instance.data.UserId;

        string jsonTemp = JsonUtility.ToJson(user);
        UnityWebRequest www;
        StartCoroutine(PostData(jsonTemp, SeverConfigs.BASE_API_GET_USER_COMPANIONS, (uwr) =>
        {
            JSONNode info = JSON.Parse(uwr.downloadHandler.text);

            string mess = uwr.downloadHandler.text;
            UserData.Instance.GetCompanionsData(info["Data"].ToString());
            HUDCanvas.Instance.HideLoadingPanel();
            if (callback != null)
                callback();
        }));
    }

    public void ReloadCompanion()
    {
        JSONNode json = new JSONObject();
        json["token"] = UserData.Instance.Token;
        json["userId"] = UserData.Instance.data.UserId;
        StartCoroutine(PostData(json.ToString(), SeverConfigs.ReloadCompanionAPI, (uwr) =>
        {
            this.GetUserCompanion();
        }));
    }

    public void ClaimDailyQuestAPI(QuestData data)
    {
        //gearRewards = new GearDatas();
        QuestInfo user = new QuestInfo();
        user.userID = UserData.Instance.data.UserId;
        user.questID = data.Index;
        jsonAttackMod = JsonUtility.ToJson(user);
        HUDCanvas.Instance.ShowLoadingPanel();
        StartCoroutine(PostData(jsonAttackMod, SeverConfigs.BASE_API_CLAIM_DAILY_QUEST, (uwr) =>
        {
            JSONNode info = JSON.Parse(uwr.downloadHandler.text);
            string mess = uwr.downloadHandler.text;

        }));

    }
    public void SellGear(string idGear, UnityAction callback = null)
    {
        UserGearInfo user = new UserGearInfo();
        user.userID = UserData.Instance.data.UserId;
        user.gearID = idGear;
        string jsonTemp = JsonUtility.ToJson(user);
        UnityWebRequest www;
        StartCoroutine(PostData(jsonTemp, SeverConfigs.BASE_API_SELL_GEAR, (uwr) =>
        {
            JSONNode info = JSON.Parse(uwr.downloadHandler.text);

            string mess = uwr.downloadHandler.text;

            GetUserGear(callback);
        }));
    }
    public void ChangeGear(GearData gear, bool isChangeGear = true, UnityAction callback = null)
    {

        GearInfo userGear = new GearInfo();
        userGear.userID = UserData.Instance.data.UserId;
        userGear.charID = UserData.Instance.characterData.CharacterID;
        userGear.gearID = gear._id;
        userGear.equipedSlot = gear.EquipedSlot;
        userGear.slot = gear.Slot.ToString();
        if (gear.Restricted == 1)
        {
            userGear.isTwoHand = true;
        }
        else if (gear.Restricted == 0)
        {
            userGear.isTwoHand = false;
        }
        string jsonTemp = JsonUtility.ToJson(userGear);
        HUDCanvas.Instance.ShowLoadingPanel();
        UnityWebRequest www;
        if (isChangeGear)
            StartCoroutine(PostData(jsonTemp, SeverConfigs.BASE_API_EQUIP_GEAR, (uwr) =>
            {
                HUDCanvas.Instance.HideLoadingPanel();
                JSONNode info = JSON.Parse(uwr.downloadHandler.text);
                string mess = uwr.downloadHandler.text;
                GetUserGear(callback);
            }));
        else
        {
            StartCoroutine(PostData(jsonTemp, SeverConfigs.BASE_API_UNEQUIP_GEAR, (uwr) =>
            {
                HUDCanvas.Instance.HideLoadingPanel();
                JSONNode info = JSON.Parse(uwr.downloadHandler.text);
                string mess = uwr.downloadHandler.text;
                GetUserGear(callback);
            }));
        }
    }
    public void GetUserGear(UnityAction callback = null)
    {

        UserGearInfo user = new UserGearInfo();
        user.userID = UserData.Instance.data.UserId;
        string jsonTemp = JsonUtility.ToJson(user);
        UnityWebRequest www;
        StartCoroutine(PostData(jsonTemp, SeverConfigs.BASE_API_GET_USER_GEAR, (uwr) =>
        {
            JSONNode info = JSON.Parse(uwr.downloadHandler.text);

            string mess = uwr.downloadHandler.text;
            UserData.Instance.gearData = JsonUtility.FromJson<GearDatas>(mess);
            foreach (GearData data in UserData.Instance.gearData.Data.GearData)
            {
                data.SetUp();
            }
            UserData.Instance.GetStatsGearData();
            if (callback != null)
                callback();
        }));
    }


    public IEnumerator GetPortalData(Action<string> callback)
    {
        Debug.LogWarning("GetPortalData");
        JSONNode positionInfo = new JSONObject();
        positionInfo["longitude"] = GOA.WorldMap.GameMaster.instance.locationManager.currentLocation.longitude;
        positionInfo["latitude"] = GOA.WorldMap.GameMaster.instance.locationManager.currentLocation.latitude;
        positionInfo["distance"] = 2;
        positionInfo["userID"] = UserData.Instance.data.UserId;
        UnityWebRequest www;
        yield return StartCoroutine(PostData(positionInfo.ToString(), SeverConfigs.BASE_API_FIND_LOCATION, (uwr) =>
        {
            JSONNode info = JSON.Parse(uwr.downloadHandler.text);
            string mess = uwr.downloadHandler.text;
            callback(mess);
        }));
    }

    public IEnumerator GetRewards(Action<string> callback)
    {
        JSONNode json = new JSONObject();
        json["userID"] = GOA.UserData.UserData.Instance.data.UserId;
        UnityWebRequest www;
        yield return StartCoroutine(PostData(json.ToString(), SeverConfigs.GetRewardAPI, (uwr) =>
        {
            string mess = uwr.downloadHandler.text;
            callback(mess);
        }));
    }

    public IEnumerator GetGlobalPortal(Action<string> callback)
    {
        yield return StartCoroutine(PostDataUrl("", SeverConfigs.BASE_API_URL + SeverConfigs.GetGlobalPortalAPI, (uwr) =>
        {
            callback(uwr.downloadHandler.text);
        }));
    }

    public void UserItem(ItemCode itemCode, Action<string> callback)
    {
        string token = MD5Hash(UserData.Instance.data.UserId + itemCode + UserData.Instance.characterData.HP + UserData.Instance.characterData.MP + UserData.Instance.characterData.CurrentHP + UserData.Instance.characterData.CurrentMP + "Tipsy@GOA-GIN");

        // JSONNode data = new JSONObject();
        // data["userID"] = UserData.Instance.data.UserId;
        // data["itemID"] = (int)itemCode;
        // data["charID"] = UserData.Instance.characterData.CharacterID;
        // data["MaxHP"] = UserData.Instance.characterData.HP;
        // data["MaxMana"] = UserData.Instance.characterData.MP;
        // data["currentHP"] = UserData.Instance.characterData.CurrentHP;
        // data["currentMana"] = UserData.Instance.characterData.CurrentMP;
        // data["token"] = token;

        UserItemData userItemData = new UserItemData(UserData.Instance.data.UserId, (int)itemCode, UserData.Instance.characterData.CharacterID,
            UserData.Instance.characterData.HP, UserData.Instance.characterData.MP, UserData.Instance.characterData.CurrentHP, UserData.Instance.characterData.CurrentMP, token);
        StartCoroutine(this.PostData(JsonUtility.ToJson(userItemData), SeverConfigs.BASE_API_GET_USER_USE_ITEM, (uwr) =>
        {
            JSONNode info = JSON.Parse(uwr.downloadHandler.text);
            try
            {
                float currentHP = info["Data"]["updateData"]["CurrentHP"];
                if (currentHP > 0) UserData.Instance.characterData.CurrentHP = (int)currentHP;
            }
            catch (System.Exception) { }
            try
            {
                float currentMP = info["Data"]["updateData"]["CurrentMP"];
                if (currentMP > 0) UserData.Instance.characterData.CurrentMP = (int)currentMP;
            }
            catch (System.Exception) { }
            UserData.Instance.Inventory.AddInventoryByCode(itemCode, -1);
            string mess = uwr.downloadHandler.text;
            if (mess == null || mess.Length == 0) return;
            if (callback != null)
                callback(mess);
        }));
    }

    public IEnumerator NTPostData(string data, string path, Action<string> callback)
    {
        yield return StartCoroutine(PostData(data, path, (uwr) =>
        {
            JSONNode info = JSON.Parse(uwr.downloadHandler.text);
            string mess = uwr.downloadHandler.text;
            callback(mess);
        }));
    }

    public void GetPlayerShopData(string shopPlayerID, Action<string> callback)
    {
        JSONNode data = new JSONObject();
        data["userID"] = UserData.Instance.data.UserId;
        data["shopPlayerID"] = shopPlayerID;
        StartCoroutine(this.PostData(data.ToString(), SeverConfigs.GetPlayerShopData, uwr =>
        {
            JSONNode info = JSON.Parse(uwr.downloadHandler.text);
            Debug.LogWarning(uwr.downloadHandler.text);
            callback.Invoke(info["Data"].ToString());
        }));
    }
    public void BuyPlayerShopData(int itemId, string gearId, string slot, int amount, string shopPlayerID, Action<string> callback)
    {
        JSONNode data = new JSONObject();
        data["userId"] = UserData.Instance.data.UserId;
        data["itemId"] = itemId;
        data["gearId"] = gearId;
        data["slot"] = slot;
        data["amount"] = amount;
        data["shopPlayerID"] = shopPlayerID;
        StartCoroutine(this.PostData(data.ToString(), SeverConfigs.BuyPlayerShopItem, uwr =>
        {
            JSONNode info = JSON.Parse(uwr.downloadHandler.text);
            callback.Invoke(info["Data"].ToString());
        }));
    }

    public void GetNeutralShopData(string playerShopID, Action<string> callback)
    {
        JSONNode data = new JSONObject();
        data["userID"] = UserData.Instance.data.UserId;
        data["neutralShopID"] = playerShopID;
        StartCoroutine(this.PostData(data.ToString(), SeverConfigs.GetNeutralShopData, uwr =>
        {
            JSONNode info = JSON.Parse(uwr.downloadHandler.text);
            callback.Invoke(info["Data"].ToString());
        }));
    }
    public void BuyNeutralShopData(int itemId, string gearId, string slot, int amount, string playerShopID, Action<string> callback)
    {
        JSONNode data = new JSONObject();
        data["userId"] = UserData.Instance.data.UserId;
        data["itemId"] = itemId;
        data["gearId"] = gearId;
        data["slot"] = slot;
        data["amount"] = amount;
        data["neutralShopID"] = playerShopID;
        StartCoroutine(this.PostData(data.ToString(), SeverConfigs.BuyNeutralShopItem, uwr =>
        {
            JSONNode info = JSON.Parse(uwr.downloadHandler.text);
            callback.Invoke(info["Data"].ToString());
        }));
    }

    public void GetWanderingDealerData(string wanderingDealerID, Action<string> callback)
    {
        JSONNode data = new JSONObject();
        data["wanderingDealerID"] = wanderingDealerID;
        StartCoroutine(this.PostData(data.ToString(), SeverConfigs.GetWanderingDealerData, uwr =>
        {
            JSONNode info = JSON.Parse(uwr.downloadHandler.text);
            callback.Invoke(info["Data"].ToString());
        }));
    }

    public void BuyWanderingDealerData(int itemId, string slot, int amount, string playerShopID, Action<string> callback)
    {
        JSONNode data = new JSONObject();
        data["userId"] = UserData.Instance.data.UserId;
        data["itemId"] = itemId;
        data["slot"] = slot;
        data["amount"] = amount;
        data["wanderingDealerID"] = playerShopID;
        StartCoroutine(this.PostData(data.ToString(), SeverConfigs.BuyWanderingDealerItem, uwr =>
        {
            JSONNode info = JSON.Parse(uwr.downloadHandler.text);
            callback.Invoke(info["Data"].ToString());
        }));
    }

    public void BuildBuildingPlayer(double longitude, double latitude, GeoType buidingType, Action<string> callback)
    {
        JSONNode data = new JSONObject();
        data["longitude"] = longitude;
        data["latitude"] = latitude;
        data["userID"] = UserData.Instance.data.UserId;
        data["displayName"] = UserData.Instance.data.DisplayName;
        data["buidingType"] = (int)buidingType;
        StartCoroutine(this.PostData(data.ToString(), SeverConfigs.BuildBuildingPlayer, uwr =>
        {
            callback(uwr.downloadHandler.text);
        }));
    }
    int errorCount = 0;
    public IEnumerator PostData(string json, string path, Action<UnityWebRequest> callback)
    {
        //panelLock.SetActive(true);
        Debug.Log("Post send: " + json, gameObject);
        Debug.Log("Post url: " + path, gameObject);
        var uwr = new UnityWebRequest(SeverConfigs.BASE_API_URL + path, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");
        //Send the request then wait here until it returns
        HUDCanvas.Instance.HideLoadingPanel();
        yield return uwr.SendWebRequest();
        HUDCanvas.Instance.HideLoadingPanel();
        if (uwr.result == UnityWebRequest.Result.ConnectionError)
        {
            if (errorCount < 3)
            {
                StartCoroutine(PostData(json, path, callback));
                errorCount++;

            }
            else
            {
                // var mess = ("Error While Sending: " + uwr.error + "\n" + path);
                var mess = ("Connection Error!");
                Scene scene = SceneManager.GetActiveScene();
                UnityEngine.Events.UnityAction action = null;
                if (scene.name.Equals(Configs.Combat_Screen))
                {
                    action = () =>
                    {
                        bl_SceneLoaderManager.LoadScene(Configs.WorldMap_Screen);
                    };
                }
                if (action != null)
                {
                    HUDCanvas.Instance.ShowNotification(mess, "Error", null, () =>
                    {
                        action?.Invoke();
                    });
                }
                else
                {
                    HUDCanvas.Instance.ShowNotification(Lean.Localization.LeanLocalization.GetTranslationText(mess, mess));
                }
                errorCount = 0;
            }
        }
        else
        {

            errorCount = 0;
            JSONNode info = JSON.Parse(uwr.downloadHandler.text);
            Debug.Log("Received: " + uwr.downloadHandler.text);
            string resultLG = info["Status"];
            if (resultLG == "0")
            {
                string mess = info["Error"]["Message"];
                Debug.Log("Post url error: " + path + ":"+ mess, gameObject);
                HUDCanvas.Instance.ShowNotification(Lean.Localization.LeanLocalization.GetTranslationText(mess, mess));

            }
            else
            {

                callback(uwr);
                //UserData.Instance.GetCharacterData(info);

            }
        }
    }

    [Button]
    public void GetLand(int page = 1, int limit = 10, Action done = null)
    {
        string key = "Authorization";
        string data = "JWT " + UserData.Instance.Token;
        string url = SeverConfigs.GOA_API_URL + SeverConfigs.GetLandsAPI + "?page=" + page + "&limit=" + limit;
        StartCoroutine(APIManager.Instance.GetDataUrl(url, key, data, (uwr) =>
        {
            NTPackage_old.Functions.NTLog.LogMessage("Recive: " + uwr.downloadHandler.text, gameObject);
            JSONNode res = JSONNode.Parse(uwr.downloadHandler.text);
            if (res["count"])
            {
                UserData.Instance.CountLand = res["count"];
            }
            try
            {
                UserData.Instance.LandDatas.Clear();
                foreach (JSONNode item in res["data"])
                {
                    LandData landData = new LandData(item["_id"], item["center"]["coordinates"][0], item["center"]["coordinates"][1], item["areas"][0]["district"]);
                    UserData.Instance.LandDatas.Add(landData);
                }
            }
            catch (System.Exception)
            {
                return;
            }
            if (done != null) done.Invoke();
        }));
    }

    public void LandOrganizingEvent(string userID, string userName, string landID, double latitude, double longitude, int eventType = 0, Action<string> callback = null)
    {
        JSONNode data = new JSONObject();
        data["userID"] = userID;
        data["userName"] = userName;
        data["landID"] = landID;
        data["latitude"] = latitude;
        data["longitude"] = longitude;
        data["eventType"] = eventType;
        StartCoroutine(this.PostData(data.ToString(), SeverConfigs.Land_OrganizingEvent, uwr =>
        {
            callback?.Invoke(uwr.downloadHandler.text);
        }));
    }

    [Button]
    public void LandGetEvent(Action<string> callback)
    {
        StartCoroutine(this.PostData("", SeverConfigs.Land_GetEvent, uwr =>
        {
            callback(uwr.downloadHandler.text);
        }));
    }
    public void LandGetRankEvent(Action<string> callback)
    {
        JSONNode jdata = new JSONObject();
        jdata["userID"] = UserData.Instance.data.UserId;
        jdata["eventID"] = ChatManager.Instance.EventChannelJoin;
        StartCoroutine(this.PostData(jdata.ToString(), SeverConfigs.Land_GetEventRank, uwr =>
        {
            callback(uwr.downloadHandler.text);
        }));
    }

    [Button]
    public void GetUserEvent(string userID, Action<string> callback)
    {
        JSONNode data = new JSONObject();
        data["userID"] = userID;
        StartCoroutine(this.PostData(data.ToString(), SeverConfigs.Land_GetUserEvent, uwr =>
        {
            callback.Invoke(uwr.downloadHandler.text);
        }));
    }

    public IEnumerator GetDataUrl(string url, string key, string data, Action<UnityWebRequest> callback)
    {

        var uwr = UnityWebRequest.Get(url);
        uwr.SetRequestHeader(key, data);
        uwr.SetRequestHeader("Content-Type", "application/json");
        yield return uwr.SendWebRequest();
        callback(uwr);
    }

    [ContextMenu("GetOutpostList")]
    public void GetOutpostList()
    {
        JSONNode data = new JSONObject();
        data["userID"] = UserData.Instance.data.UserId;
        data["start"] = 0;
        data["end"] = -1;
        StartCoroutine(PostDataUrl(data.ToString(), SeverConfigs.BASE_API_URL + SeverConfigs.GetOutpostListAPI, (uwr) =>
        {
            JSONNode res = JSONNode.Parse(uwr.downloadHandler.text);
            UserData.Instance.OutpostsOccupied.Clear();
            try
            {
                for (int i = 0; i < res["Data"]["List"].Count; i++)
                {
                    UserData.Instance.OutpostsOccupied[res["Data"]["List"][i]] = 1;
                }
            }
            catch (System.Exception) { }
            try
            {
                GOA.WorldMap.WorldMapMaster.instance.UpdateStatusOutpost();
            }
            catch (System.Exception) { }
        }));
    }

    public void BuyItemInShop(string userID, int itemID, Action callback)
    {
        JSONNode data = new JSONObject();
        data["userID"] = userID;
        data["itemID"] = itemID;
        HUDCanvas.Instance.ShowLoadingPanel();
        StartCoroutine(PostDataUrl(data.ToString(), SeverConfigs.BASE_API_URL + SeverConfigs.BuyItemAPI, (uwr) =>
        {
            callback.Invoke();
        }));
    }

    public void ChangeDisplayName(string newName, Action callback)
    {
        JSONNode data = new JSONObject();
        data["userID"] = UserData.Instance.data.UserId;
        data["displayName"] = newName;
        HUDCanvas.Instance.ShowLoadingPanel();
        StartCoroutine(PostData(data.ToString(), SeverConfigs.ChangeDisplayName, (uwr) =>
        {
            callback.Invoke();
        }));
    }

    public IEnumerator PostDataUrl(string json, string url, Action<UnityWebRequest> callback)
    {
        HUDCanvas.Instance.ShowLoadingPanel();
        Debug.Log("Post send: " + json, gameObject);
        Debug.Log("Post url: " + url, gameObject);
        //panelLock.SetActive(true);
        var uwr = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        // Debug.Log(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");
        //Debug.Log(jsonAttackMod);
        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();
        if (uwr.result == UnityWebRequest.Result.ConnectionError)
        {
            NTPackage_old.Functions.NTLog.LogWarning("Post Error: " + uwr.error, gameObject);
            // var mess = ("Error While Sending: " + uwr.error);
            var mess = ("Connection Error!");
            HUDCanvas.Instance.ShowNotification(Lean.Localization.LeanLocalization.GetTranslationText(mess, mess));
            HUDCanvas.Instance.HideLoadingPanel();
        }
        else
        {


            JSONNode info = JSON.Parse(uwr.downloadHandler.text);
            Debug.Log("Received from: " + url + ": " + uwr.downloadHandler.text);
            string resultLG = info["Status"];
            if (resultLG == "0")
            {
                NTPackage_old.Functions.NTLog.LogWarning("Post Error: " + url + ": " + uwr.downloadHandler.text, gameObject);
                string mess = info["Error"]["Message"];
                HUDCanvas.Instance.ShowNotification(Lean.Localization.LeanLocalization.GetTranslationText(mess, mess));
                HUDCanvas.Instance.HideLoadingPanel();
            }
            else
            {
                HUDCanvas.Instance.HideLoadingPanel();
                NTPackage_old.Functions.NTLog.LogMessage("Post Recive: " + url + ": " + uwr.downloadHandler.text, gameObject);
                callback(uwr);
                //UserData.Instance.GetCharacterData(info);

            }
        }

    }
    private IEnumerator WaitAttackMob()
    {
        //panelLock.SetActive(true);
        var uwr = new UnityWebRequest(SeverConfigs.BASE_API_URL + SeverConfigs.BASE_API_ATTACKMOB, "POST");

        //var uwr = new UnityWebRequest(Config.IsTest ? Config.testAuth : Config.serverPublishLogin, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonAttackMod);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");
        //Send the request then wait here until it returns
        HUDCanvas.Instance.ShowLoadingPanel();
        yield return uwr.SendWebRequest();
        HUDCanvas.Instance.HideLoadingPanel();
        if (uwr.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log("Error While Sending: " + uwr.error);

        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);

            JSONNode info = JSON.Parse(uwr.downloadHandler.text);
            string resultLG = info["Status"];
            if (resultLG == "0")
            {
                string mess = info["Error"]["Message"];
                HUDCanvas.Instance.ShowNotification(mess);

            }
            else
            {
                UserData.Instance.GetCharacterData(info);

            }
        }

    }
    public string MD5Hash(string input)
    {
        StringBuilder hash = new StringBuilder();
        MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
        byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

        for (int i = 0; i < bytes.Length; i++)
        {
            hash.Append(bytes[i].ToString("x2"));
        }
        return hash.ToString();
    }

    public void BaseAPIRespone(JSONNode jdata){
        if(jdata["Data"]["Update_UserData"] != null){
            Rubik._2DGPS.UserData.UserDataManager.instance.UpdateData(jdata["Data"]["Update_UserData"]);
        }
        if(jdata["Data"]["Update_Cards"] != null){
            Rubik._2DGPS.Card.CardManager.instance.UpdateCards(jdata["Data"]["Update_Cards"]);
        }
        if(jdata["Data"]["Update_CardTeam"] != null){
            Rubik._2DGPS.UserData.UserDataManager.instance.UpdateCardTeam(jdata["Data"]["Update_CardTeam"]);
        }
        if(jdata["Data"]["Update_Currencies"] != null){
            Rubik._2DGPS.UserData.UserDataManager.instance.UpdateCurrencies(jdata["Data"]["Update_Currencies"]);
        }
        if(jdata["Data"]["Update_Characters"] != null){
            Rubik._2DGPS.Character.CharacterManager.instance.UpdateCharacters(jdata["Data"]["Update_Characters"]);
        }
        if(jdata["Data"]["Update_Gears"] != null){
            Rubik._2DGPS.Gear.GearManager.instance.UpdateGears(jdata["Data"]["Update_Gears"]);
        }
    }

}
