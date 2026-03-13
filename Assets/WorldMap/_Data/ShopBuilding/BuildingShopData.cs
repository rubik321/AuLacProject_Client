using System.Collections;
using System.Collections.Generic;
using GOA.Item;
using UnityEngine;

namespace GOA.ShopBuilding
{
    [System.Serializable]
    public class ItemGearShopData{
        public string Slot;
        public string Index;
        public int Lv;
        public CurrencyType Currency;
        public float Price;
        public bool Unlimited;
        public int Amount;
    }
    [System.Serializable]
    public class ItemShopData{
        public string Slot;
        public string Index;
        public ItemCode Code;
        public string Name;
        public CurrencyType Currency;
        public float Price;
        public bool Unlimited = false;
        public float Amount;
    }

    [System.Serializable]
    public class PlayerShopData{
        public string Id;
        public int version;
        public List<ItemShopData> ItemShopDatas;
        public List<ItemGearShopData> ItemGearShopDatas;
        public int TimeBuy;
        public int CoinInc;
    }
    [System.Serializable]
    public class NeutralShopData{
        public string Id;
        public int version;
        public List<ItemShopData> ItemShopDatas;
        public List<ItemGearShopData> ItemGearShopDatas;
        public int TimeBuy;
        public int CoinInc;
    }
    [System.Serializable]
    public class WanderingDealerData
    {
        public string Id = "";
        public int version = 0;
        public List<ItemShopData> ItemShopDatas;
    
    }
}