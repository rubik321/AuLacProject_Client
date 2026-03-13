using System;
using System.Collections.Generic;
using System.Collections;
using NTPackage_old.Functions;
using SimpleJSON;
using Sirenix.OdinInspector;
using UnityEngine;
using NTFunctions_old;
using GOA.Config;

namespace Rubik._2DGPS.Gear
{
    using Rubik._2DGPS.DataCenter;
    using UserData;

    public class GearManager : LoadBehaviour
    {
        public NTDictionary<GearData> GearDataDic;
        public NTDictionary<Gear> GearDic;
        public List<GearLvCost> GearLvCosts;

        public static GearManager instance;
        protected override void Awake()
        {
            base.Awake();
            if (GearManager.instance != null){
               NTLog.LogWarning("Only 1 instance allow");
               return;
             }
            GearManager.instance = this;
        }

        public IEnumerator LoadData()
        {
            this.LoadGearData();
            this.LoadGearLvCost();
            yield return null;
        }

        public void LoadGearData()
        {
            this.GearDataDic = new NTDictionary<GearData>();
            JSONNode jdata = JSONNode.Parse(DataCenterManager.instance.GetData(DataName.GearData));
            foreach (JSONNode item in jdata)
            {
                GearData gearData = JsonUtility.FromJson<GearData>(item.ToString());
                this.GearDataDic.Add(gearData.Index.ToString(), gearData);
            }
        }
        public void LoadGearLvCost()
        {
            this.GearLvCosts = new List<GearLvCost>();
            JSONNode jdata = JSONNode.Parse(DataCenterManager.instance.GetData(DataName.GearLvCost));
            foreach (JSONNode item in jdata)
            {
                GearLvCost gearLvCost = JsonUtility.FromJson<GearLvCost>(item.ToString());
                this.GearLvCosts.Add(gearLvCost);
            }
        }

        public IEnumerator Init()
        {
            if (UserDataManager.instance.GetUserID().Length == 0)
            {
                Debug.LogWarning("Can't get userID");
            }
            else
            {
                this.GearDic = new NTDictionary<Gear>();
                yield return this.GetGears();
            }
        }

        public IEnumerator GetGears(Action<List<Gear>> callback = null)
        {
            JSONNode jdata = new JSONObject();
            jdata["userID"] = UserDataManager.instance.UserData._id;
            yield return APIManager.Instance.PostDataUrl(jdata.ToString(), SeverConfigs.GPS2D_Server + SeverConfigs._2DGPS_API_Gear_GetGears, (data) =>
                {
                    JSONNode jdata = JSONNode.Parse(data.downloadHandler.text);
                    APIManager.Instance.BaseAPIRespone(jdata);
                    List<Gear> gears = new List<Gear>();
                    foreach (JSONNode item in jdata["Data"]["Update_Gears"])
                    {
                        try
                        {
                            Gear gear = JsonUtility.FromJson<Gear>(item.ToString());
                            gears.Add(gear);
                        }
                        catch (System.Exception e)
                        {
                            NTLog.LogError(e.ToString(), gameObject);
                        }
                    }
                    callback?.Invoke(gears);
                }
            );
        }

        [Button]
        public void AddRandomGear(Action<List<Gear>> callback = null)
        {
            JSONNode jdata = new JSONObject();
            jdata["userID"] = UserDataManager.instance.UserData._id;
            StartCoroutine(APIManager.Instance.PostDataUrl(jdata.ToString(), SeverConfigs.GPS2D_Server + SeverConfigs._2DGPS_API_Gear_AddRandomGear, (data) =>
                {
                    JSONNode jdata = JSONNode.Parse(data.downloadHandler.text);
                    APIManager.Instance.BaseAPIRespone(jdata);
                    List<Gear> gears = new List<Gear>();
                    foreach (JSONNode item in jdata["Data"]["Update_Gears"])
                    {
                        try
                        {
                            Gear gear = JsonUtility.FromJson<Gear>(item.ToString());
                            gears.Add(gear);
                        }
                        catch (System.Exception e)
                        {
                            NTLog.LogError(e.ToString(), gameObject);
                        }
                    }
                    callback?.Invoke(gears);
                }
            ));
        }

        [Button]
        public void UpgradeLvGear(string gearID, Action<(List<Gear>, bool)> callback = null)
        {
            JSONNode jdata = new JSONObject();
            jdata["gearID"] = gearID;
            StartCoroutine(APIManager.Instance.PostDataUrl(jdata.ToString(), SeverConfigs.GPS2D_Server + SeverConfigs._2DGPS_API_Gear_UpgradeLvGear, (data) =>
                {
                    JSONNode jdata = JSONNode.Parse(data.downloadHandler.text);
                    APIManager.Instance.BaseAPIRespone(jdata);
                    List<Gear> gears = new List<Gear>();
                    foreach (JSONNode item in jdata["Data"]["Update_Gears"])
                    {
                        try
                        {
                            Gear gear = JsonUtility.FromJson<Gear>(item.ToString());
                            gears.Add(gear);
                        }
                        catch (System.Exception e)
                        {
                            NTLog.LogError(e.ToString(), gameObject);
                        }
                    }
                    callback?.Invoke((gears, jdata["Data"]["Success"]));
                }
            ));
        }

        public void UpdateGears(JSONNode jdata)
        {
            foreach (JSONNode item in jdata)
            {
                Gear gear = JsonUtility.FromJson<Gear>(item.ToString());
                if (this.GearDic.Get(gear._id) == null) this.GearDic.Add(gear._id, gear);
                else this.GearDic.Get(gear._id).UpdateGear(gear);
            }
        }

        public GearData GetGearDataByIndex(GearIndex index)
        {
            return this.GearDataDic.Get(index.ToString());
        }

    }
}