using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoShared;
using GOA.WorldMap;
using NTFunctions_old;
using SimpleJSON;
using GOA.Config;
using GOA.UserData;

namespace GOA.WorldMap
{
    [System.Serializable]
    public class MobManagerData{
        public List<MobData> MobDatas = new List<MobData>();
    }

    public class MobManager : LoadBehaviour
    {
        [SerializeField]
        private double disVisible = 10;
        public double DisVisible{
            get => 400;
        }
        public double DisRegion{
            get => 800;
        }
        [SerializeField]
        private float disFollow = 4;
        public double DisFollow{
            get => this.disFollow * NTConst.distanceUnit;
        }

        public Mob mobSample;

        public TextAsset dataMob;

        public Transform holder;
        public Transform Holder{
            get {
                if(this.holder == null) this.LoadHolder();
                return this.holder;
            }
        }
        public List<Mob> RegularMobs;
        public List<Mob> GreaterMobs;

        public MobSpawn MobSpawn;

        public ObjectPooling objectPooling;
        public bool IsLoadFromStore = false;
        public MobManagerData MobManagerData = new MobManagerData();
        public const string KeyMobData = "WorldMapMobData";

        public static MobManager instance;
        protected override void Awake()
        {
            base.Awake();
            if (MobManager.instance != null) Debug.LogError("Only 1 MobManager allow");
            MobManager.instance = this;
        }

        public override void LoadComponents()
        {
            base.LoadComponents();
            this.LoadHolder();
            this.LoadMobs();
            this.LoadObjectPooling();
            this.LoadMobSample();
        }

        protected void LoadHolder(){
            if(holder != null) return;
            this.holder = transform.Find("Holder");
        }
        protected void LoadMobs(){
            this.RegularMobs.Clear();
            foreach (Transform item in this.Holder)
            {
                if(item.TryGetComponent<Mob>(out Mob mob)){
                    this.RegularMobs.Add(mob);
                }
            }
        }

        protected void LoadObjectPooling(){
            if(objectPooling != null) return;
            this.objectPooling = transform.GetComponent<ObjectPooling>();
        }

        protected void LoadMobSample(){
            if(mobSample != null) return;
            this.mobSample = transform.Find("Collections").Find("MobSample").GetComponent<Mob>();
        }

        //Function
        public float delayUpdateData = 0.5f;
        [SerializeField]
        private float countTimeUpdateData = 0f;

        [ContextMenu("DeleteMobData")]
        public void DeleteMobData(){
            PlayerPrefs.DeleteKey(UserData.UserData.Instance.data.UserId+":"+KeyMobData);
        }

        public void Init()
        {
            return;
            if(this.IsLoadFromStore) return;
            this.MobManagerData = new MobManagerData();
            this.IsLoadFromStore = true;
            try
            {
                // Debug.Log(PlayerPrefs.GetString(KeyMobData));
                MobManagerData mobManagerData = JsonUtility.FromJson<MobManagerData>(PlayerPrefs.GetString(UserData.UserData.Instance.data.UserId+":"+KeyMobData));
                foreach(MobData item in mobManagerData.MobDatas)
                {
                    if(System.DateTime.Now.Subtract(System.DateTime.Parse(item.SpawnTime)).TotalDays >= 1) continue;
                    if(UserData.UserData.Instance.DataInCombat.IsMobClean){
                        if(item.Index.Equals(UserData.UserData.Instance.DataInCombat.MobDataPrevious.Index)
                            && item.coordinates.latitude == UserData.UserData.Instance.DataInCombat.MobDataPrevious.coordinates.latitude
                            && item.coordinates.longitude == UserData.UserData.Instance.DataInCombat.MobDataPrevious.coordinates.longitude
                        ) continue;
                    }
                    this.InstanateMob(item);
                }
                UserData.UserData.Instance.DataInCombat = new DataInCombat();
            }
            catch (System.Exception e)
            {
                Debug.LogWarning(e);
            }
            this.MobSpawn.countSpawn = MobManagerData.MobDatas.Count;
            this.MobSpawn.Init();
            this.UpdateData();
        }

        public bool IsUpdateMob = false;
        public void UpdateData(){
            if(!IsUpdateMob){
                // Thread thread = new Thread(UpdateMob);
                // thread.Start();
                UpdateMob();
            }
        }

        public void UpdateMob(){
            this.IsUpdateMob = true;
            for (int i = this.RegularMobs.Count -1; i >= 0; i--){
                if(this.RegularMobs[i].isDestroy) this.DestroyMob(this.RegularMobs[i]);
                else{
                    this.RegularMobs[i].UpdateData();
                }
            }
            foreach (Mob item in this.GreaterMobs)
            {
                item.UpdateData();
            }
            for (int i = this.GreaterMobs.Count -1; i >= 0; i--){
                if(this.GreaterMobs[i].isDestroy) this.DestroyMob(this.GreaterMobs[i]);
                else{
                    this.GreaterMobs[i].UpdateData();
                }
            }
            this.IsUpdateMob = false;
        }

        public void InstanateMob(MobData mobData, bool unlimit = false){
            mobData.MobSO = null;
            Mob newMob = null;
            if(mobData.MobSO.Type == MobTypeCode.RegularMobs && !unlimit){
                if(this.RegularMobs.Count >= Config.MobConfigs.LimitTopRegularMob) return;
            }
            if(mobData.MobSO.Type == MobTypeCode.GreaterMobs && !unlimit){
                if(this.GreaterMobs.Count >= Config.MobConfigs.LimitTopGreaterMob) return;
            }
            Transform transMob = this.objectPooling.GetObjectFromPooling(mobData.Index);
            if(transMob == null){
                newMob = Instantiate<Mob>(this.mobSample);
            }else{
                newMob = transMob.GetComponent<Mob>();
            }
            newMob.transform.position = new Vector3(0, -50, 0);
            if(mobData.MobSO.Type == MobTypeCode.RegularMobs){
                this.RegularMobs.Add(newMob);
            }else if(mobData.MobSO.Type == MobTypeCode.GreaterMobs){
                this.GreaterMobs.Add(newMob);
            }
            newMob.transform.SetParent(this.Holder);
            newMob.transform.localRotation = Quaternion.Euler(0,Random.Range(0,360),0);
            newMob.ParserFromData(mobData);
            newMob.transform.name = mobData.Index;
            newMob.gameObject.SetActive(true);
            try
            {
                this.MobManagerData.MobDatas.Add(mobData);
                if(this.IsLoadFromStore)
                    PlayerPrefs.SetString(UserData.UserData.Instance.data.UserId+":"+KeyMobData, JsonUtility.ToJson(this.MobManagerData));
            }
            catch (System.Exception e)
            {
                Debug.LogWarning(e);
            }
        }
        
        public void DestroyMob(Mob mob){
            if(mob.mobData.MobSO.Type == MobTypeCode.RegularMobs){
                this.RegularMobs.Remove(mob);
            }else if(mob.mobData.MobSO.Type == MobTypeCode.GreaterMobs){
                this.GreaterMobs.Remove(mob);
            }
            mob.transform.localPosition = Vector3.zero;
            objectPooling.PushObjectIntoPooling(mob.transform);
            try
            {
                this.MobManagerData.MobDatas.Remove(mob.mobData);
                if(this.IsLoadFromStore)
                    PlayerPrefs.SetString(UserData.UserData.Instance.data.UserId+":"+KeyMobData, JsonUtility.ToJson(this.MobManagerData.MobDatas));
            }
            catch (System.Exception e)
            {
                Debug.LogWarning(e);
            }
        }

        public Coordinates GetRandomCoordinatesInRange(Coordinates center, double rangeMetter){
            Vector2 centerVec = center.convertCoordinateToVector2D();
            float range = (float) rangeMetter;
            Vector2 randomPos = centerVec + Random.insideUnitCircle * range;
            Vector3 pos = new Vector3(randomPos.x, 0, randomPos.y);
            return Coordinates.convertVectorToCoordinates(pos);
        }

        public Coordinates GetRandomRange(Coordinates center, double rangeMetter){
            // Set center position of circle
            float range = (float) rangeMetter;
            Vector3 centerPosition = center.convertCoordinateToVector();

            // Generate random position in circle
            float angle = Random.Range(0, 360);
            float radius = Random.Range(50, range);
            Vector3 randomPosition = centerPosition + Quaternion.Euler(0, angle, 0) * Vector3.forward * radius;

            // Add vertical displacement
            return Coordinates.convertVectorToCoordinates(randomPosition);
        }

        public MobSO GetMobScriptableObjectByIndex(string index){
            return MobAssets.instance.GetMobScriptableObjectByIndex(index);
            // return Resources.Load<MobSO>("MobSO/"+index);
        }
        public static string[] fixedIndexRegularMobs={
            "M010001",
            "M010002",
            "M010003",
            "M010004",
            "M010005",
            "M010006"
            
        };
    
        public static string[] fixedIndexGreaterMobs={
            "M020001",
            "M020002",
            "M020003",
            "M020004",
            "M020005",
            "M020006",
            "M020007",
            "M020008",
            "M020009",
            "M020010",
            "M020011",
            "M020012",
            "M020013",
            "M020014",
            "M020015",
            "M020016",
            "M020017",
            "M020018",
            "M020019",
            "M020020"
        };

        public static string[] fixedIndexReaperPortal = {
            "M040001",
            "M040002",
            "M040003",
            "M040004",
            "M040005",
            "M040006",
        };

        public static MobInfo Calculator(MobInfo mobInfo, MobStatsScaleByLevel mobStatsScaleByLevel, int lv){
            MobInfo newMobInfo = (MobInfo) mobInfo.Clone();
            newMobInfo.Lv = lv;
            int scale = lv - 1;
            newMobInfo.HP += mobStatsScaleByLevel.HPRate * scale;
            newMobInfo.MP += mobStatsScaleByLevel.MPRate * scale;
            newMobInfo.Strength += mobStatsScaleByLevel.StrRate * scale;
            newMobInfo.Vitality += mobStatsScaleByLevel.VitRate * scale;
            newMobInfo.Mind += mobStatsScaleByLevel.MndRate * scale;
            newMobInfo.Spirit += mobStatsScaleByLevel.Spirit * scale;
            newMobInfo.Dexterity += mobStatsScaleByLevel.DexRate * scale;
            newMobInfo.Speed += mobStatsScaleByLevel.Speed * scale;
            newMobInfo.HitRate += mobStatsScaleByLevel.HitRate * scale;
            newMobInfo.Evade += mobStatsScaleByLevel.Evade * scale;
            newMobInfo.CritRate += mobStatsScaleByLevel.CritRate * scale;
            newMobInfo.CritDmg += mobStatsScaleByLevel.CritDmg * scale;
            return newMobInfo;
        }
    }
}
