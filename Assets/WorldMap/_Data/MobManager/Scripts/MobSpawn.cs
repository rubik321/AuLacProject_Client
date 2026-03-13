using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOA.WorldMap;
using NTFunctions_old;
using GoShared;
using GOA.UserData;
using GOA.Config;

namespace GOA.WorldMap
{
    public class MobSpawn : LoadBehaviour
    {
        public float delaySpawn = 90;
        public float countTimeSpawn = 0;

        public int countSpawn = 0;

        public bool maxSpawn = false;

        public bool CanSpawn = false;

        public System.DateTime TimeSpawnRegularMob;
        public const float DelaySpawnRegularMob = 270;
        public const string KeyTimeSpawnRegularMob = "TimeSpawnRegularMob";
        public System.DateTime TimeSpawnGreaterMob;
        public const float DelaySpawnGreaterMob = 2700;
        public const string KeyTimeSpawnGreaterMob = "TimeSpawnGreaterMob";

        public void Init()
        {
            if (this.CanSpawn) return;
            try
            {
                System.DateTime dateTime = System.DateTime.Parse(PlayerPrefs.GetString(UserData.UserData.Instance.data.UserId + ":" + KeyTimeSpawnRegularMob));
                double second = System.DateTime.Now.Subtract(dateTime).TotalSeconds;
                double time = second / DelaySpawnRegularMob;
                if (time > MobConfigs.LimitTopRegularMob) time = MobConfigs.LimitTopRegularMob;
                for (int i = 1; i < time; i++)
                {
                    Debug.LogWarning("MobSpawn GenerateRegularMob");
                    this.GenerateRegularMob();
                }
            }
            catch (System.Exception e)
            {
                Debug.LogWarning(e);
                PlayerPrefs.SetString(UserData.UserData.Instance.data.UserId + ":" + KeyTimeSpawnRegularMob, System.DateTime.Now.ToString());
            }
            try
            {
                System.DateTime dateTime = System.DateTime.Parse(PlayerPrefs.GetString(UserData.UserData.Instance.data.UserId + ":" + KeyTimeSpawnGreaterMob));
                double second = System.DateTime.Now.Subtract(dateTime).TotalSeconds;
                double time = second / DelaySpawnGreaterMob;
                if (time > MobConfigs.LimitTopGreaterMob) time = MobConfigs.LimitTopGreaterMob;
                for (int i = 1; i < time; i++)
                {
                    Debug.LogWarning("MobSpawn GenerateGreaterMob");
                    this.GenerateGreaterMob();
                }
            }
            catch (System.Exception e)
            {
                Debug.LogWarning(e);
                PlayerPrefs.SetString(UserData.UserData.Instance.data.UserId + ":" + KeyTimeSpawnGreaterMob, System.DateTime.Now.ToString());
            }
            this.CanSpawn = true;
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            return;
            if (!this.CanSpawn) return;
            try
            {
                if (MobManager.instance.RegularMobs.Count < MobConfigs.LimitDownRegularMob) this.GenerateRegularMob();
                if (System.DateTime.Now.Subtract(TimeSpawnRegularMob).Seconds > DelaySpawnRegularMob)
                {
                    this.GenerateRegularMob();
                }
                if (System.DateTime.Now.Subtract(TimeSpawnGreaterMob).Seconds > DelaySpawnGreaterMob)
                {
                    this.GenerateGreaterMob();
                }
            }
            catch (System.Exception) { }
        }

        protected void GenerateRegularMob()
        {
            // Debug.LogWarning("GenerateRegularMob");
            try
            {
                this.TimeSpawnRegularMob = System.DateTime.Now;
                if (MobManager.instance.RegularMobs.Count >= MobConfigs.LimitTopRegularMob)
                {
                    PlayerPrefs.SetString(UserData.UserData.Instance.data.UserId + ":" + KeyTimeSpawnRegularMob, System.DateTime.Now.ToString());
                    return;
                }
                PlayerPrefs.SetString(UserData.UserData.Instance.data.UserId + ":" + KeyTimeSpawnRegularMob, System.DateTime.Now.ToString());
                string mobIndex = MobManager.fixedIndexRegularMobs[Random.Range(0, MobManager.fixedIndexRegularMobs.Length)];
                Coordinates coordinates = MobManager.instance.GetRandomRange(GameMaster.instance.player.Coordinates, MobManager.instance.DisVisible);
                MobData mobData = new MobData(coordinates, mobIndex);
                mobData.SpawnTime = System.DateTime.Now.ToString();
                mobData.Lv = this.GetLvMob(mobData.MobSO.Type);
                MobManager.instance.InstanateMob(mobData);
            }
            catch (System.Exception e)
            {
                Debug.LogWarning(e);
            }
        }
        protected void GenerateGreaterMob()
        {
            try
            {
                Debug.LogWarning("GenerateGreaterMob");
                this.TimeSpawnGreaterMob = System.DateTime.Now;
                if (MobManager.instance.GreaterMobs.Count >= MobConfigs.LimitTopGreaterMob)
                {
                    PlayerPrefs.SetString(UserData.UserData.Instance.data.UserId + ":" + KeyTimeSpawnGreaterMob, System.DateTime.Now.ToString());
                    return;
                }
                PlayerPrefs.SetString(UserData.UserData.Instance.data.UserId + ":" + KeyTimeSpawnGreaterMob, System.DateTime.Now.ToString());
                string mobIndex = MobManager.fixedIndexGreaterMobs[Random.Range(0, MobManager.fixedIndexGreaterMobs.Length)];
                Coordinates coordinates = MobManager.instance.GetRandomRange(GameMaster.instance.player.Coordinates, MobManager.instance.DisVisible);
                MobData mobData = new MobData(coordinates, mobIndex);
                mobData.SpawnTime = System.DateTime.Now.ToString();
                mobData.Lv = this.GetLvMob(mobData.MobSO.Type);
                MobManager.instance.InstanateMob(mobData);
            }
            catch (System.Exception e)
            {
                Debug.LogWarning(e);
            }
        }
        public int GetLvMob(MobTypeCode mobTypeCode)
        {
            try
            {
                if (mobTypeCode == MobTypeCode.RegularMobs)
                {
                    int buffLv = 0;
                    int rand = Random.Range(0, 100);
                    if (rand <= 50) buffLv = 0;
                    else if (rand <= 75) buffLv = 1;
                    else if (rand <= 85) buffLv = 2;
                    else if (rand <= 90) buffLv = 3;
                    int lv = UserData.UserData.Instance.characterData.Level + buffLv;
                    if (lv < 1) lv = 1;
                    if (lv > 65) lv = 65;
                    return lv;
                }
                else if (mobTypeCode == MobTypeCode.GreaterMobs)
                {
                    return Random.Range(10, 65);
                }
            }
            catch (System.Exception) { }
            return 1;
        }

        public void InitEvent(){
            // switch (GeoPointManager.Instance.EventType)
            // {
            //     case LandEvent.EventType.Hunter:
            //         GenerateMobHunter();
            //         break;
            // }
        }

        public void GenerateMobHunter()
        {
            for (int i = 0; i < Random.Range(15, 20); i++)
            {
                try
                {
                    string mobIndex = MobManager.fixedIndexRegularMobs[Random.Range(0, MobManager.fixedIndexRegularMobs.Length)];
                    Coordinates coordinates = MobManager.instance.GetRandomRange(GameMaster.instance.player.Coordinates, MobManager.instance.DisVisible);
                    MobData mobData = new MobData(coordinates, mobIndex);
                    mobData.SpawnTime = System.DateTime.Now.ToString();
                    mobData.Lv = 20;
                    MobManager.instance.InstanateMob(mobData, true);
                }
                catch (System.Exception e)
                {
                    Debug.LogWarning(e);
                }
            }

            for (int i = 0; i < Random.Range(5, 8); i++)
            {
                try
                {
                    string mobIndex = MobManager.fixedIndexGreaterMobs[Random.Range(0, MobManager.fixedIndexGreaterMobs.Length)];
                    Coordinates coordinates = MobManager.instance.GetRandomRange(GameMaster.instance.player.Coordinates, MobManager.instance.DisVisible);
                    MobData mobData = new MobData(coordinates, mobIndex);
                    mobData.SpawnTime = System.DateTime.Now.ToString();
                    mobData.Lv = 30;
                    MobManager.instance.InstanateMob(mobData, true);
                }
                catch (System.Exception e)
                {
                    Debug.LogWarning(e);
                }
            }

        }
    }
}
