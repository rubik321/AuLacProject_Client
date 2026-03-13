using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System;
using GOA.Item;
using GOA.UIProfile;
using GOA.LandEvent;

namespace GOA.UserData
{
    [System.Serializable]
    public class Data{
        public string UserName, UserId, DisplayName;
        public float Coin, Gin, Level, DailyIncome;

        public int WoodChest_FK;
        public int BronzeChest_FK;
        public int GoldChest_FK;
        public int PlatinumChest_FK;
        public int DiamondChest_FK;
        public int ScoreChest_FK;
        public int GoldBar_FK;
        public int PromotionGem_FK;
        public int Iron_FK;
        public int SummonToken_FK;

        public string[] CardTeam_FK;

        public float AmountShopBuilding, AmountBlacksmithBuilding, AmountDungeonBuilding, AmountCastleBuilding;

        public Inventory inventory = new Inventory();
        public Data(string userName, string userId, float level){
            this.UserName = userName;
            this.UserId = userId;
            this.Level = level;
        }

        public void AddCurrency(ItemCode itemCode, int amount)
        {
            this.AddPropValue(itemCode.ToString(), amount);
        }

        public int GetCurrency(ItemCode itemCode)
        {
            return GetPropValue(itemCode.ToString());
        }

        public void SetCurrency(ItemCode itemCode, int amount)
        {
            this.SetPropValue(itemCode.ToString(), amount);
        }

        public int GetPropValue(string name)
        {
            return (int)this.GetType().GetField(name).GetValue(this);
        }

        public void SetPropValue(string name, int amount)
        {
            this.GetType().GetField(name).SetValue(this, amount);
        }

        public void AddPropValue(string name, int amount)
        {
            this.SetPropValue(name, this.GetPropValue(name) + amount);
        }
    }

    [System.Serializable]
    public class DataInCombat{
        public bool IsMobClean = false;
        public GOA.WorldMap.MobTypeCode TypeCombat;
        public GOA.WorldMap.MobData MobDataPrevious;
        public string OutpostId = "";
        public string PortalId = "";
        public List<MobInfo> mobsInCombat = new List<MobInfo>();
    }

    [Serializable]
    public class Inventory
    {
        public int Potion;
        public int HiPotion;
        public int Ether;
        public int HiEther;
        public int Elixir;
        public int PoorLeather;

        public int PortalKey;
        public int NameTag;
        public int WoodenChest;
        public int BronzeChest;
        public int SilverChest;
        public int GoldChest;
        public int SaikiSeal;
        public int MysticMandate;

        public int ChieftainBlood;
        public int HardLeather;
        public int AbyssRoots;
        public int DragonFlower;
        public int ProtectorSoul;
        public int DestroyerSoul;
        public int VanguardSoul;
        public int TritinumAlloy;
        public int Ferrieliquid;
        public int MetalPlate;
        public int StonePile;
        public int SilicatePowder;

        public float GetPropValue(string name)
        {
            name = name.Replace(" ","");
           
            return (int) this.GetType().GetField(name).GetValue(this);
        }

        public void SetPropValue(string name, int amount)
        {
            this.GetType().GetField(name).SetValue(this, amount);
        }

        public void AddInventoryByID(string id,int number)
        {
            Debug.LogWarning(id);
            switch (id)
            {
                case "I010001":
                    Potion += number;
                    break;
                case "I010002":
                    HiPotion += number;
                    break;
                case "I010003":
                    Ether += number;
                    break;
                case "I010004":
                    HiEther += number;
                    break;
                case "I010005":
                    Elixir += number;
                    break;
                case "I020001":
                    PoorLeather += number;
                    break; 
                case "I550001":
                    PortalKey += number;
                    break;
                case "Potion":
                    Potion += number;
                    break;
                case "HiPotion":
                    HiPotion += number;
                    break;
                case "Ether":
                    Ether += number;
                    break;
                case "HiEther":
                    HiEther += number;
                    break;
                case "Elixir":
                    Elixir += number;
                    break;
                case "PoorLeather":
                    PoorLeather += number;
                    break;
                default:
                    break;
                
            }
        }
    
        public void AddInventoryByCode(ItemCode code, int value){
            if(code == ItemCode.Coin){
                UserData.Instance.data.Coin += value;
                return;
            }
            if(code == ItemCode.Gin){
                UserData.Instance.data.Gin += value;
                return;
            }
            this.SetPropValue(code.ToString(), GetInventoryByCode(code)+value);
        }
    
        public int GetInventoryByCode(ItemCode code){
            if(code == ItemCode.Coin) return (int)UserData.Instance.data.Coin;
            if(code ==  ItemCode.Gin) return (int)UserData.Instance.data.Gin;
            return (int) GetPropValue(code.ToString());
        }
    
        public void SetInventoryByCode(ItemCode code, int value){
            if(code == ItemCode.Coin){
                UserData.Instance.data.Coin = value;
                return;
            }
            if(code == ItemCode.Gin){
                UserData.Instance.data.Gin = value;
                return;
            }
            this.SetPropValue(code.ToString(), value);
        }
    }
    [Serializable]
    public class MobInfo : ICloneable{
        public string Index;
        public GOA.WorldMap.MobTypeCode Type;
        public int Lv = 1;
        public string Attack;
        public float Atk_Rate;
        public string SKill_1;
        public float SK_1_Rate;
        public string SKill_2;
        public float SK_2_Rate;
        public string SKill_3;
        public float SK_3_Rate;
        public string SKill_4;
        public float SK_4_Rate;
        public string SKill_5;
        public float SK_5_Rate;
        public float HP;
        public float CurHp;
        public float MP;
        public float Strength;
        public float Vitality;
        public float Dexterity;
        public float Mind;
        public float Spirit;
        public float HitRate;
        public float Evade;
        public float CritRate;
        public float CritDmg;
        public float Speed;
        
        public MobInfo(){
            this.CurHp = this.HP;
        }

        public List<string> skills = new List<string>();
        public List<float> skillsRate = new List<float>();
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    [Serializable]
    public class MobStatsScaleByLevel{
        public string Index;
        
        public float HPRate;	
        public float MPRate;	
        public float StrRate;	
        public float VitRate;	
        public float MndRate;	
        public float Spirit;
        public float DexRate;	
        public float Speed;	
        public float HitRate;	
        public float Evade;	
        public float CritRate;	
        public float CritDmg;
    }
    public enum GameMode
    {
        NORMAL,
        DUNGEON
    }
    public class UserData : MonoBehaviour
    {
        public static UserData Instance;
        public GameMode gameMode = GameMode.NORMAL;
        public int StageLevel;
        public Data data;
        public GearDatas gearData = new GearDatas();
        ListOrbsData orbData = new ListOrbsData();
        public CharacterData characterData;
        public List<OrbData> characterOrbData = new List<OrbData>();
         [SerializeField]CharacterData characterBaseData;
        public Inventory Inventory;
        // Only active skills
        public List<string> skills = new List<string>() ;
        public ListSkillData SkillDatas = new ListSkillData();
        // Quest Data 
        public ListQuestData questData = new ListQuestData();
        public GOA.UIMenu.ClassesData LevelUpData;
        public ListCompanionData companions = new ListCompanionData();
        public bool visDailyRewardShow ;
        public bool isDailyRewardShow,isNotifyShow = false;
        public BaseCharacterDataSO[] eneyDatas;
        public List<bool> lsHeros = new List<bool>() { true, false, false };
        public DataInCombat DataInCombat = new DataInCombat();
        public string[] Teams;
        public float movespeed
        {
            get
            {
                return PlayerPrefs.GetFloat("SpeedGame", 1); 
            }
            set
            {
                
                PlayerPrefs.SetFloat("SpeedGame", value);
            }
        }
        
        public DungeonInfo DungeonInfo;
        public MobInfo companion;

        public Dictionary<string, int> OutpostsOccupied = new Dictionary<string, int>();

        public TextAsset DataMob;
        public TextAsset DataMobStatsScaleByLevel;
        public Dictionary<string, MobInfo> DictionaryMobInfo = new Dictionary<string, MobInfo>();
        public Dictionary<string, MobStatsScaleByLevel> DictionaryMobStatsScaleByLevel = new Dictionary<string, MobStatsScaleByLevel>();
        public Dictionary<string, OrbData> DictionaryOrbData= new Dictionary<string, OrbData>();
        public Dictionary<string, CompanionData> DictionaryCompanionData = new Dictionary<string, CompanionData>();
        public Dictionary<string, bool> ListDailyQuest = new Dictionary<string, bool>();
        ListGearIDName lsIdToName = new ListGearIDName();
        public int dailyIncome;
        public string Token;
        public int CountLand = 0;
       public ListCompanionData lsCompData = new ListCompanionData();
        public Dictionary<string, string> lsGoIdToName = new Dictionary<string, string>();
        [SerializeField]
        TextAsset dataGearName,dataMobName;
        [SerializeField]
        private int _totalDameInPortal;
        public int TotalDameInPortal { set{
            this._totalDameInPortal = value;
        }
        get{
            return this._totalDameInPortal;
        } }

        public List<LandData> LandDatas;
        public BaseCharacterDataSO[] lsCharacters;
        public List<int> lsTeams;
        private void Awake()
        {
            Instance = this;
            characterData = new CharacterData();
            characterBaseData = new CharacterData();
            dataGearName = Resources.Load<TextAsset>("GearName");
            dataMobName = Resources.Load<TextAsset>("MobName");
            lsIdToName = JsonUtility.FromJson<ListGearIDName>(dataGearName.text);
            orbData = JsonUtility.FromJson<ListOrbsData>(Resources.Load<TextAsset>("OrbData").text);
            questData = JsonUtility.FromJson<ListQuestData>(Resources.Load<TextAsset>("QuestData").text);
            lsCompData = JsonUtility.FromJson<ListCompanionData>(Resources.Load<TextAsset>("CompanionData").text);
            // lsMobIdName = JsonUtility.FromJson<ListGearIDName>(dataMobName.text);
            foreach (GearIDName gear in lsIdToName.GearDataName)
            {
                try
                {
                    lsGoIdToName.TryAdd(gear.ID, gear.Name);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
            foreach(OrbData orb in orbData.OrbsData)
            {
                try
                {
                    DictionaryOrbData.TryAdd(orb.Index, orb);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
            foreach (CompanionData comp in lsCompData.Companions)
            {
                try
                {
                    DictionaryCompanionData.TryAdd(comp.CompCode, comp);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
            JSONNode dataMob = JSON.Parse(this.DataMob.text);
            for (int i = 0; i < dataMob.Count; i++)
            {
                MobInfo mobInfo = JsonUtility.FromJson<MobInfo>(dataMob[i].ToString());
                if (!string.IsNullOrEmpty(mobInfo.SKill_1)|| mobInfo.SKill_1!="0")
                {
                    mobInfo.skills.Add(mobInfo.SKill_1);
                    mobInfo.skillsRate.Add(mobInfo.SK_1_Rate);
                }
                if (!string.IsNullOrEmpty(mobInfo.SKill_2) || mobInfo.SKill_2 != "0")
                {
                    mobInfo.skills.Add(mobInfo.SKill_2);
                    mobInfo.skillsRate.Add(mobInfo.SK_2_Rate);
                }
                if (!string.IsNullOrEmpty(mobInfo.SKill_3) || mobInfo.SKill_3 != "0")
                {
                    mobInfo.skills.Add(mobInfo.SKill_3);
                    mobInfo.skillsRate.Add(mobInfo.SK_3_Rate);
                }
                if (!string.IsNullOrEmpty(mobInfo.SKill_4) || mobInfo.SKill_4 != "0")
                {
                    mobInfo.skills.Add(mobInfo.SKill_4);
                    mobInfo.skillsRate.Add(mobInfo.SK_4_Rate);
                }
                if (!string.IsNullOrEmpty(mobInfo.SKill_5) || mobInfo.SKill_5 != "0")
                {
                    mobInfo.skills.Add(mobInfo.SKill_5);
                    mobInfo.skillsRate.Add(mobInfo.SK_5_Rate);
                }

                this.DictionaryMobInfo[mobInfo.Index] = mobInfo;
                // Debug.LogWarning(JsonUtility.ToJson(mobInfo));
            }
            JSONNode dataMobStatsScaleByLevel = JSON.Parse(this.DataMobStatsScaleByLevel.text);
            for (int i = 0; i < dataMobStatsScaleByLevel.Count; i++)
            {
                MobStatsScaleByLevel mobStatsScaleByLevel = JsonUtility.FromJson<MobStatsScaleByLevel>(dataMobStatsScaleByLevel[i].ToString());
                this.DictionaryMobStatsScaleByLevel[mobStatsScaleByLevel.Index] = mobStatsScaleByLevel;
            }
        }
       
        public void GetUserData(JSONNode data){
            Debug.LogWarning("UserData"+data.ToString());
            if (data["Data"]["UserData"]["UserName"] != null)
                this.data.UserName = data["Data"]["UserData"]["UserName"];
            if (data["Data"]["UserData"]["UserID"] != null)
                this.data.UserId = data["Data"]["UserData"]["UserID"];
            if (data["Data"]["UserData"]["DisplayName"] != null)
                this.data.DisplayName = data["Data"]["UserData"]["DisplayName"];
            this.data.Coin = data["Data"]["UserData"]["Coin"];
            this.data.Gin = data["Data"]["UserData"]["Gin"];
            this.data.Level = data["Data"]["UserData"]["Level"];
            this.data.DailyIncome = data["Data"]["UserData"]["DailyIncome"];
            this.data.AmountShopBuilding = data["Data"]["UserData"]["AmountShopBuilding"];
            this.data.AmountBlacksmithBuilding = data["Data"]["UserData"]["AmountBlacksmithBuilding"];
            this.data.AmountDungeonBuilding = data["Data"]["UserData"]["AmountDungeonBuilding"];
            this.data.AmountCastleBuilding = data["Data"]["UserData"]["AmountCastleBuilding"];
            for(int i =0;i< questData.QuestData.Count; i++)
            {
                try
                {
                    ListDailyQuest.Add(questData.QuestData[i].Index, data["Data"]["DailyQuest"][questData.QuestData[i].Index]);
                }
                catch (System.Exception e)
                {
                    NTPackage_old.Functions.NTLog.LogError(e. ToString(), gameObject);
                }
            }
            bool newDay = data["Data"]["isNewDay"];
            isDailyRewardShow = newDay;
            if (newDay)
            {
                QuestManager.Instance.SetDailyQuest();
              
            }
            if (PlayerPrefs.GetInt("FirstLoginApp" + UserData.Instance.data.UserName, 0) == 0)
            {
                PlayerPrefs.SetInt("FirstLoginApp"+UserData.Instance.data.UserName, 1);
                QuestManager.Instance.SetDailyQuest();
            }
            dailyIncome = data["Data"]["DailyIncome"];
            Debug.LogWarning("token"+data["Data"]["token"].ToString());
            this.Token = data["Data"]["token"];
            APIManager.Instance.GetOutpostList();
            APIManager.Instance.ReloadCompanion();
            APIManager.Instance.GetLand();
            StartCoroutine(LandEventManager.instance.GetUserData());
        }
        public CompanionData GetCompanionData(string code)
        {
           
            if (DictionaryCompanionData.TryGetValue(code, out CompanionData comp))
                    return comp;
            Debug.LogError("Missing asset " + code);
            return null;
        }
        public void GetCharacterData(JSONNode data)
        {
            if (data["Data"]["CharacterData"]["_id"] != null)
            characterBaseData._id = data["Data"]["CharacterData"]["_id"];
            characterBaseData.Level = data["Data"]["CharacterData"]["Level"];
            characterData.CharType = data["Data"]["CharacterData"]["CharType"];
            characterBaseData.Exp = data["Data"]["CharacterData"]["Exp"];
            if (data["Data"]["CharacterData"]["CharacterID"] != null)
            characterBaseData.CharacterID = data["Data"]["CharacterData"]["CharacterID"];
            characterBaseData.HP = data["Data"]["CharacterData"]["HP"];
            characterBaseData.CurrentHP = data["Data"]["CharacterData"]["CurrentHP"];
            characterBaseData.CurrentMP = data["Data"]["CharacterData"]["CurrentMP"];
            characterBaseData.MP = data["Data"]["CharacterData"]["MP"];
            characterBaseData.Str = data["Data"]["CharacterData"]["Str"];
            characterBaseData.Vit = data["Data"]["CharacterData"]["Vit"];
            characterBaseData.Dex = data["Data"]["CharacterData"]["Dex"];
            characterBaseData.Mind = data["Data"]["CharacterData"]["Mind"];
            characterBaseData.Spirit = data["Data"]["CharacterData"]["Spirit"];
            characterBaseData.Speed = data["Data"]["CharacterData"]["Speed"];
            characterBaseData.HitRate = data["Data"]["CharacterData"]["HitRate"];
            characterBaseData.Evade = data["Data"]["CharacterData"]["Evade"];
            characterBaseData.CritRate = data["Data"]["CharacterData"]["CritRate"];
            characterBaseData.CritDame = data["Data"]["CharacterData"]["CritDame"];
            characterBaseData.Vis = data["Data"]["CharacterData"]["Vis"];
            characterBaseData.MaxEXP = data["Data"]["CharacterData"]["MaxEXP"];
            string jsondata = data["Data"];
            characterData._id = characterBaseData._id;
            characterData.CharacterID = characterBaseData.CharacterID;
            characterData._id = characterBaseData._id;
            GetCompanionsData(data["Data"].ToString());
            Debug.Log(data["Data"].ToString());
            ResetCharacter();
            characterData.CurrentHP = characterBaseData.CurrentHP;
            characterData.CurrentMP = characterBaseData.CurrentMP;
           // skills = new List<string>() { "SK_000110", "SK_00011", "SK_00012", "SK_00013", "SK_00014", "SK_00015", "SK_00016" };
            GetSkillsData(data["Data"].ToString());

        }
        public void GetSkillsData(string data)
        {
            SkillDatas = JsonUtility.FromJson<ListSkillData>(data);
            skills.Clear();
            GetStatsGearData();
           
        }
        public void GetCompanionsData(string data)
        {
            companions = JsonUtility.FromJson<ListCompanionData>(data);
            // APIManager.Instance.GetNFTGenesis();

        }
        public void SetCompanion(CompanionData data)
        {
            companion.Index = lsGoIdToName[data.CompCode];
            companion.Atk_Rate = (data.Attack);
            companion.SK_1_Rate = (data.Skill_1_Rate);
            companion.SK_2_Rate = (data.Skill_2_Rate );
            companion.Strength = data.STR * characterData.Str;
            companion.Mind = data.MND*characterData.Mind;
            companion.Speed = data.SPD * characterData.Speed;
            companion.HitRate = data.HIT;
            companion.CritRate = data.CRI;
            companion.CritDmg = data.CRD ;
            companion.skills = new List<string>();
            companion.HP = 100;
            companion.CurHp = 100;
           if (!string.IsNullOrEmpty(data.Skill_1))
                companion.skills.Add(data.Skill_1);
            if (!string.IsNullOrEmpty(data.Skill_2))
                companion.skills.Add(data.Skill_2);
            switch (data.CompType)
            {
                case 0:
                    companion.Type = WorldMap.MobTypeCode.RegularMobs;
                    break;
                case 1:
                    companion.Type = WorldMap.MobTypeCode.MobChieftainOutpost;
                    break;
                case 2:
                    companion.Type = WorldMap.MobTypeCode.MobReaperPortal;
                    break;
            }

            //MobInfo mobInfo = JsonUtility.FromJson<MobInfo>(dataMob[i].ToString());
            if (!string.IsNullOrEmpty(companion.SKill_1) || companion.SKill_1 != "0")
            {
                companion.skills.Add(companion.SKill_1);
                companion.skillsRate.Add(companion.SK_1_Rate);
            }
            if (!string.IsNullOrEmpty(companion.SKill_2) || companion.SKill_2 != "0")
            {
                companion.skills.Add(companion.SKill_2);
                companion.skillsRate.Add(companion.SK_2_Rate);
            }
            if (!string.IsNullOrEmpty(companion.SKill_3) || companion.SKill_3 != "0")
            {
                companion.skills.Add(companion.SKill_3);
                companion.skillsRate.Add(companion.SK_3_Rate);
            }
            if (!string.IsNullOrEmpty(companion.SKill_4) || companion.SKill_4 != "0")
            {
                companion.skills.Add(companion.SKill_4);
                companion.skillsRate.Add(companion.SK_4_Rate);
            }
            if (!string.IsNullOrEmpty(companion.SKill_5) || companion.SKill_5 != "0")
            {
                companion.skills.Add(companion.SKill_5);
                companion.skillsRate.Add(companion.SK_5_Rate);
            }

        }
        public void SetSkillPassive(StatSkill skill, float number)
        {
            
            
            switch (skill)
            {
                case StatSkill.MAX_HP:
                    UserData.Instance.characterData.HP += (int)Mathf.Round(UserData.Instance.characterData.HP * number);
                    break;
                case StatSkill.MAX_MP:
                    UserData.Instance.characterData.MP += (int)Mathf.Round(UserData.Instance.characterData.MP * number);
                    break;
                case StatSkill.STR:
                    UserData.Instance.characterData.Str += (int)Mathf.Round(UserData.Instance.characterData.Str * number);
                    break;
                case StatSkill.VIT:
                    UserData.Instance.characterData.Vit += (int)Mathf.Round(UserData.Instance.characterData.Vit * number);
                    characterData.HP = characterData.HP + (int)Mathf.Round(UserData.Instance.characterData.Vit * number) * 3;
                    break;
                case StatSkill.DEX:
                    UserData.Instance.characterData.Dex += (int)Mathf.Round(UserData.Instance.characterData.Dex * number);
                    break;
                case StatSkill.SPD:
                    UserData.Instance.characterData.Speed += (int)Mathf.Round(UserData.Instance.characterData.Speed * number);
                    break;
                case StatSkill.MND:
                    UserData.Instance.characterData.Mind += (int)Mathf.Round(UserData.Instance.characterData.Mind * number);
                    Debug.Log(number + " " + UserData.Instance.characterData.Mind);
                    break;
                case StatSkill.SPI:
                    //UserData.Instance.characterData.Spirit += (int)Mathf.Round(UserData.Instance.characterData.Spirit * number);
                    characterData.MP = characterData.MP;
                    break;
                case StatSkill.HIT:
                    UserData.Instance.characterData.HitRate += number;
                    break;
                case StatSkill.EVA:
                    UserData.Instance.characterData.Evade +=  number;
                    break;
                case StatSkill.CRI:
                    
                    UserData.Instance.characterData.CritRate +=  number;
                   // Debug.Log(number + " " + skill +" "+ characterData.CritRate);
                    break;
                case StatSkill.CRD:
                    UserData.Instance.characterData.CritDame += number;
                    break;
            }
            StartCoroutine(this.Sync_HP_MP());
        }
        public void GetInventoryData(JSONNode data){
            this.Inventory = JsonUtility.FromJson<Inventory>(data["Data"]["Inventory"].ToString());
        }

        public void GetStatsGearData()
        {
            //ResetCharacter();
            if (gearData != null && gearData.Data.GearData.Count > 0)
            {
                ResetCharacter();
                float perHP = 0;
                float perMP = 0;
                foreach (GearData data in gearData.Data.GearData)
                {
                    if (data.Equiped)
                    {
                        characterData.Str += data.STR;
                        characterData.Vit += data.VIT;
                        characterData.Dex += data.DEX;
                        characterData.Mind += data.MND;
                        //characterData.Spirit += data.SPI;
                        characterData.Speed += data.SPD;
                        characterData.HitRate += (data.HIT + 0.001f * data.SPD);
                        characterData.Evade += (float)Math.Round((double)data.EVA, 2) + 0.001f * data.SPD;
                        characterData.CritDame += (float)Math.Round((double)data.CRD, 2) + 0.001f * data.DEX;
                        characterData.CritRate += (float)Math.Round((double)data.CRI, 2) + 0.0005f * data.DEX;
                        characterData.HP += data.HP + data.VIT * 3;
                        characterData.MP += data.MP;
                        perHP += data.HP_Per;
                        perMP += data.MP_Per;
                        characterData.BaseDame += data.DMG;
                    }
                }
                characterData.HP = (int)(characterData.HP * (1 + perHP));
                characterData.MP = (int)(characterData.MP * (1 + perMP));
            }
            StartCoroutine(this.Sync_HP_MP());

            foreach (OrbData orb in SkillDatas.SkillData)
            {
                if (orb.Equiped)
                {

                    skills.Add(orb.OrbCode);
                    if (DictionaryOrbData[orb.OrbCode].SkillType == 0)
                    {
                        SetSkillPassive((StatSkill)DictionaryOrbData[orb.OrbCode].StatSkill, DictionaryOrbData[orb.OrbCode].Multiplier);
                    }
                }
            }
        }
        void ResetCharacter()
        {
            characterData.Str = characterBaseData.Str ;
            characterData.Vit = characterBaseData.Vit ;
            characterData.Dex = characterBaseData.Dex ;
            characterData.Mind = characterBaseData.Mind ;
            //characterData.Spirit = characterBaseData.Spirit ;
            characterData.Speed = characterBaseData.Speed ;
            characterData.HitRate = characterBaseData.HitRate+0.001f * characterData.Speed;
            characterData.Evade = characterBaseData.Evade + 0.001f * characterData.Speed;
            characterData.CritDame = characterBaseData.CritDame + 0.001f * characterData.Dex;
            characterData.CritRate = characterBaseData.CritRate + 0.0005f * characterData.Dex;
            characterData.HP = characterBaseData.HP + characterData.Vit * 3;
            characterData.MP = characterBaseData.MP ;
            characterData.Exp = characterBaseData.Exp;
            characterData.MaxEXP = characterBaseData.MaxEXP;
            characterData.BaseDame = characterBaseData.BaseDame;
            characterData.Level = characterBaseData.Level;
        }

        IEnumerator Sync_HP_MP(){
            yield return new WaitForSeconds(0.5f);
            if (characterData.HP < characterData.CurrentHP)
                characterData.CurrentHP = characterData.HP;
            if (characterData.MP < characterData.CurrentMP)
                characterData.CurrentMP = characterData.MP;
            if ( characterData.CurrentMP<0)
                characterData.CurrentMP =0;

        }
    }
  
}

