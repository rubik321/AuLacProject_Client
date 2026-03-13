using System;
using System.Collections;
using System.Collections.Generic;
using GOA.Config;
using GOA.Item;
using GOA.UserData;
using NTFunctions_old;
using NTPackage_old.Functions;
using SimpleJSON;
using UnityEngine;

namespace Rubik.GOA.Blacksmith
{
    public class BlacksmithManager : LoadBehaviour
    {
        public NTDictionary<GearTypeData> GearTypeDataDic;
        public TextAsset GearTypeAsset;
        public List<ListUpgradeLevelData> ListUpgradeLevelData;
        public TextAsset UpgradeLevelDataAsset;
        public NTDictionary<FusionGear> FusionGearDic;
        public TextAsset FusionDataAsset;
        public NTDictionary<FragmentData> FragmentDataDic;
        public TextAsset FragmentDataAsset;

        public static BlacksmithManager instance;
        protected override void Awake()
        {
            base.Awake();
            if (BlacksmithManager.instance != null){
               NTLog.LogWarning("Only 1 instance allow", gameObject);
               return;
             }
            BlacksmithManager.instance = this;
        }

        protected override void Start(){
            this.GearTypeDataDic = new NTDictionary<GearTypeData>();
            this.ListUpgradeLevelData = new();
            JSONNode jGearType = JSONNode.Parse(GearTypeAsset.text);
            foreach (JSONNode item in jGearType)
            {
                GearTypeData gearType = JsonUtility.FromJson<GearTypeData>(item.ToString());
                this.GearTypeDataDic.Add(gearType.Type.ToString(), gearType);
            }
            JSONNode jUpgradeLevelData = JSONNode.Parse(UpgradeLevelDataAsset.text);
            foreach (JSONNode item in jUpgradeLevelData)
            {
                ListUpgradeLevelData list = new();
                foreach (JSONNode ele in item)
                {
                    UpgradeLevelData upgradeLevelData = JsonUtility.FromJson<UpgradeLevelData>(ele.ToString());
                    list.Data.Add(upgradeLevelData);
                }
                ListUpgradeLevelData.Add(list);
            }
            JSONNode jFusionData= JSONNode.Parse(FusionDataAsset.text);
            foreach (JSONNode item in jFusionData)
            {
                FusionGear fusionGear = JsonUtility.FromJson<FusionGear>(item.ToString());
                this.FusionGearDic.Add(fusionGear.Rarity.ToString(), fusionGear);
            }
            JSONNode jFragmentData = JSONNode.Parse(FragmentDataAsset.text);
            foreach (JSONNode item in jFragmentData)
            {
                FragmentData fragmentData = JsonUtility.FromJson<FragmentData>(item.ToString());
                this.FragmentDataDic.Add(fragmentData.Rarity.ToString(), fragmentData);
            }
        }

        public void UpgradeLv(string gearID, bool isUpgradeStone, Action<bool> done = null){
            JSONNode jdata = new JSONObject();
            jdata["userID"] = UserData.Instance.data.UserId;
            jdata["gearID"] = gearID;
            jdata["isUpgradeStone"] = isUpgradeStone;
            jdata["lands"] = UserData.Instance.CountLand;
            StartCoroutine(APIManager.Instance.PostData(jdata.ToString(),SeverConfigs.Blacksmith_UpgradeGear,(data)=>{
                jdata = JSONNode.Parse(data.downloadHandler.text);
                done?.Invoke(jdata["Data"]["Success"]);
                foreach (JSONNode item in jdata["Data"]["ItemUpdates"])
                {
                    ItemValue itemValue = JsonUtility.FromJson<ItemValue>(item.ToString());
                    UserData.Instance.Inventory.SetInventoryByCode(itemValue.Type, itemValue.Amount);
                }
            }));
        }

        public void UpStar(string gearID, Action<GearData> done = null){
            JSONNode jdata = new JSONObject();
            jdata["userID"] = UserData.Instance.data.UserId;
            jdata["gearID"] = gearID;
            StartCoroutine(APIManager.Instance.PostData(jdata.ToString(),SeverConfigs.Blacksmith_UpStarGear,(data)=>{
                jdata = JSONNode.Parse(data.downloadHandler.text);
                foreach (JSONNode item in jdata["Data"]["ItemUpdates"])
                {
                    ItemValue itemValue = JsonUtility.FromJson<ItemValue>(item.ToString());
                    UserData.Instance.Inventory.SetInventoryByCode(itemValue.Type, itemValue.Amount);
                }
                GearData gearData = JsonUtility.FromJson<GearData>(jdata["Data"]["Gear"].ToString());
                done?.Invoke(gearData);
            }));
        }
        public void FusionGear(string[] gearIDs, bool isFusionStone,Action<ResultFusionGear> done = null){
            JSONNode jdata = new JSONObject();
            jdata["userID"] = UserData.Instance.data.UserId;
            jdata["gearIDs"] = gearIDs;
            jdata["isFusionStone"] = isFusionStone;
            StartCoroutine(APIManager.Instance.PostData(jdata.ToString(),SeverConfigs.Blacksmith_FusionGear,(data)=>{
                jdata = JSONNode.Parse(data.downloadHandler.text);
                ResultFusionGear resultFusionGear = JsonUtility.FromJson<ResultFusionGear>(jdata["Data"].ToString());
                foreach (ItemValue item in resultFusionGear.ItemUpdates)
                {
                    UserData.Instance.Inventory.SetInventoryByCode(item.Type, item.Amount);
                }
                done?.Invoke(resultFusionGear);
            }));
        }
        public void FragmentGear(string gearID,Action done = null){
            JSONNode jdata = new JSONObject();
            jdata["userID"] = UserData.Instance.data.UserId;
            jdata["gearID"] = gearID;
            StartCoroutine(APIManager.Instance.PostData(jdata.ToString(),SeverConfigs.Blacksmith_FragmentGear,(data)=>{
                jdata = JSONNode.Parse(data.downloadHandler.text);
                foreach (JSONNode item in jdata["Data"]["Rewards"])
                {
                    ItemValue itemValue = JsonUtility.FromJson<ItemValue>(item.ToString());
                    UserData.Instance.Inventory.AddInventoryByCode(itemValue.Type, itemValue.Amount);
                }
                done?.Invoke();
            }));
        }

        public ItemCode GetOrbName(int classType)
        {
            if (classType == 2)
            {
                return ItemCode.ProtectorSoul;
            }
            if (classType == 1)
            {
                return ItemCode.VanguardSoul;
            }
            return ItemCode.DestroyerSoul;
        }
        
    }
}