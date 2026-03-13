using System;
using System.Collections;
using System.Collections.Generic;
using NTPackage;
using NTPackage.EventDispatcher;
using NTPackage.Functions;
using NTPackage.UI;
using Rubik.CardPlayer;
using Rubik.Config;
using Rubik.DataCenter;
using Rubik.Manager;
using Rubik.UI;
using Rubik.UserDataPlayer;
using SimpleJSON;
using UnityEngine;

namespace Rubik.ItemPlayer
{
    using Lean.Localization;
    using Rubik.AddressablesLoader;
    using Rubik.Banner;
    using Rubik.Myrk.Arena;
    using Rubik.Myrk.BattlePass;
    using Rubik.Myrk.Portal;
    using Rubik.Myrk.Shop;
    using Rubik.Server;
    using Rubik.UserProfile;

    public class ItemDataCf
    {
        public static string UseItem = "/api/2D_GPS/item_data/use_item";
        public static string SellItem = "/api/2D_GPS/item_data/sell_item";
        public static string ExchangeItem = "/api/2D_GPS/item_data/exchange_item";
        public static string ExchangeItemAdv = "/api/2D_GPS/item_data/exchange_adv_item";

        public static List<ItemType> ItemBag = new List<ItemType>(){
            ItemType.Adv,
            ItemType.Gem,
            ItemType.Coin,
            ItemType.GearStone,
            ItemType.Fruit,

            // Summon Ticket
            ItemType.NormalSummonCardTicket,
            ItemType.PremiumSummonCardTicket,
            ItemType.UltraSummonCardTicket,

            // Chest
            ItemType.WoodChest,
            ItemType.BronzeChest,
            ItemType.SilverChest,
            ItemType.GoldChest,

            // Shard Card Player
            ItemType.ShardCard_0,
            ItemType.ShardCard_1,
            ItemType.ShardCard_2,
            ItemType.ShardCard_3,
            ItemType.ShardCard_4,
            ItemType.ShardCard_5,
            ItemType.ShardCard_6,
            ItemType.ShardCard_7,
            ItemType.ShardCard_8,
            ItemType.ShardCard_9,

            // Shard Creep
            ItemType.ShardCrep_0,
            ItemType.ShardCrep_1,
            ItemType.ShardCrep_2,
            ItemType.ShardCrep_3,
            ItemType.ShardCrep_4,
            ItemType.ShardCrep_5,
            ItemType.ShardCrep_6,
            ItemType.ShardCrep_7,
            ItemType.ShardCrep_8,
            ItemType.ShardCrep_9,
        };

        public static string HexColorRed = "931041";
        public static string HexColorGreen = "109331";
        public static string HexColorBrown = "7F5B29";
    }

    public class ItemDataManager : NTBehaviour
    {
        #region Player Data
        public NTDictionary<string, ItemData> UserItems;
        public List<UserExchange> ExchangeDaily;
        #endregion

        #region Game Data
        public NTDictionary<string, ItemDataInfo> ItemDataInfoDic;
        public List<ExchangeItem> ItemExchangeData;
        public List<ExchangeItemAdv> ItemExchangeAdvData;
        #endregion

        #region Resource
        [SerializeField] private ListSpriteAddressable ItemIconAddressable;
        [SerializeField] private NTDictionary<string, Sprite> ItemIconDic;
        public Sprite DefaultIcon;
        #endregion

        public static ItemDataManager Instance;
        protected override void Awake()
        {
            base.Awake();
            if (ItemDataManager.Instance != null)
            {
                NTLog.LogError("Only 1 Instance allow");
                return;
            }
            ItemDataManager.Instance = this;
        }

        #region Function

        public IEnumerator LoadData()
        {
            int amount = 0;
            AddressablesLoader.Instance.LoadAssetsFromLocal(AddressablesAssetsConfig.ItemIconAddressableSprite, (result) =>
            {
                this.ItemIconAddressable = result.GetComponent<ListSpriteAddressable>();
                this.ItemIconAddressable.transform.SetParent(transform);
                this.ItemIconDic = new NTDictionary<string, Sprite>();
                foreach (var item in this.ItemIconAddressable.ListSprite)
                {
                    this.ItemIconDic.Add(item.name, item);
                }
                amount++;
            });
            yield return new WaitUntil(() => amount >= 1);

            this.UserItems = new NTDictionary<string, ItemData>();
            this.ItemDataInfoDic = new NTDictionary<string, ItemDataInfo>();
            JSONNode jsonNode = JSON.Parse(DataCenterManager.Instance.GetData(DataName.ItemDataInfo));
            foreach (JSONNode item in jsonNode.AsArray)
            {
                ItemDataInfo itemDataInfo = JsonUtility.FromJson<ItemDataInfo>(item.ToString());
                itemDataInfo.Icon = this.ItemIconDic.Get(itemDataInfo.ImagePath);
                if (itemDataInfo.Icon == null) NTLog.LogError("ItemDataInfo Icon not found: " + itemDataInfo.ImagePath, gameObject);
                this.ItemDataInfoDic.Add(itemDataInfo.Type.ToString(), itemDataInfo);
            }

            jsonNode = JSON.Parse(DataCenterManager.Instance.GetData(DataName.ItemExchangeData));
            this.ItemExchangeData = new List<ExchangeItem>();
            foreach (JSONNode item in jsonNode.AsArray)
            {
                ExchangeItem itemExchangeData = JsonUtility.FromJson<ExchangeItem>(item.ToString());
                this.ItemExchangeData.Add(itemExchangeData);
            }

            jsonNode = JSON.Parse(DataCenterManager.Instance.GetData(DataName.ItemExchangeAdvData));
            this.ItemExchangeAdvData = new List<ExchangeItemAdv>();
            foreach (JSONNode item in jsonNode.AsArray)
            {
                ExchangeItemAdv itemExchangeAdvData = JsonUtility.FromJson<ExchangeItemAdv>(item.ToString());
                this.ItemExchangeAdvData.Add(itemExchangeAdvData);
            }

            yield return null;
        }

        public void Logout()
        {
            this.UserItems = new NTDictionary<string, ItemData>();
            this.ExchangeDaily = new List<UserExchange>();
        }

        public void UpdateData(ItemData[] itemData)
        {
            if (itemData == null || itemData.Length == 0) return;
            foreach (ItemData item in itemData)
            {
                this.UserItems.Add(item.Type.ToString(), item);
            }
            EventListenerManager.instance.PostEvent(EventCode.UpdateItemData);
            this.CheckCapSlotInventoryBag();
        }

        public void UpdateExchangeDaily(UserExchange[] userExchanges)
        {
            if (userExchanges == null || userExchanges.Length == 0) return;
            this.ExchangeDaily = new List<UserExchange>(userExchanges);
        }

        public void ShowInfo(ItemData itemData, Action<ItemDataDetailUI> action = null)
        {
            PopupManager.Instance.OnUI(PopupCode.ItemDataDetailUI, itemData, popupUI =>
            {
                action?.Invoke((ItemDataDetailUI)popupUI);
            });
        }

        public void ShowToolTip(ItemType itemType)
        {
            string title = "<size=150%>" + this.GetItemTextSprite(itemType) + " " + ItemDataManager.Instance.GetItemName(itemType);
            string description = ItemDataManager.Instance.GetItemDescription(itemType);
            HUDCanvas.Instance.ShowToolTip(title, description);
        }

        public void ShowDontEnoughItem(ItemType itemType)
        {
            string str = Lean.Localization.LeanLocalization.GetTranslationText("dont_enough_item", "You have insufficient {0}!");
            string itemName = ItemDataManager.Instance.GetItemName(itemType) + this.GetItemTextSprite(itemType);
            string replace = Lean.Localization.LeanLocalization.GetTranslationText("item_ insufficient_" + (int)itemType);
            if (replace != null && replace != "")
            {
                str = replace;
                itemName = this.GetItemTextSprite(itemType);
            }
            HUDCanvas.Instance.ShowNotification(String.Format(str, itemName), Lean.Localization.LeanLocalization.GetTranslationText("dont_enough_item_title", "Insufficient Resources"));
        }

        public void OnClickAdd(ItemType itemType)
        {
            switch (itemType)
            {
                case ItemType.Coin:
                    this.OnShopResource();
                    break;
                case ItemType.Gem:
                    this.OnShopResource();
                    break;
                case ItemType.Energy:
                    this.ExchangeItem(itemType);
                    break;
                case ItemType.BattlePassPoint:
                    PopupManager.Instance.OnUI(PopupCode.QuestUI);
                    break;
                default:
                    break;
            }
        }

        public void ExchangeItem(ItemType itemType)
        {
            PopupManager.Instance.OnUI(PopupCode.ExchangeItemUI, null, (popup) =>
            {
                ExchangeItemUI exchangeItemUI = popup as ExchangeItemUI;
                exchangeItemUI.SetData(itemType);
            });
        }

        public void OnShopResource()
        {
            if (UserProfileManager.Instance.IsFunctionLocked(LockFunctionType.Market))
            {
                PopupManager.Instance.OnUI(PopupCode.MessagePanel, null, (PopupUI popupUI) =>
                {
                    popupUI.GetComponent<MessagePanel>().SetData(LeanLocalization.GetTranslationText("lock_title", "Feature Locked"), string.Format(LeanLocalization.GetTranslationText("unlock_feature", "Feature unlocks at Level {0}"), UserProfileManager.Instance.GetLevelUnlockFunction(LockFunctionType.Market)));
                });
                return;
            }
            PopupManager.Instance.OnUI(PopupCode.IAPShopUI, null, (popup) =>
            {
                IAPShopUI iapShopUI = popup as IAPShopUI;
                iapShopUI.MoveToTab(1);
            });
        }

        public void CheckCapSlotInventoryBag()
        {
            if (UserDataManager.Instance.IsCapSlotInventoryBag())
            {
                BannerManager.Instance.AddTopBanner(BannerTopConfig.WarningCapSlotInventoryBag, Lean.Localization.LeanLocalization.GetTranslationText("warning_cap_slot_inventory_bag", "You have reached the maximum number of items in your inventory. Please expand your inventory to continue."));
            }
            else
            {
                BannerManager.Instance.RemoveTopBanner(BannerTopConfig.WarningCapSlotInventoryBag);
            }
        }

        #endregion

        #region API
        public IEnumerator UseItem(ItemType itemType, int amount, Action done = null)
        {
            JSONNode jdata = new JSONObject();
            jdata["userID"] = UserDataManager.Instance.GetUserID();
            jdata["itemType"] = (int)itemType;
            jdata["amount"] = amount;

            yield return APIManager.Instance.PostDataUrl(jdata.ToString(), URL_Config.BASE_API_URL + ItemDataCf.UseItem, (data) =>
            {
                APIResponseData apiResponseData = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                if (apiResponseData.Status == 0) return;
                done?.Invoke();
            });
        }
        public void SellItem(ItemType itemType, int amount, Action done = null)
        {
            StartCoroutine(CoSellItem(itemType, amount, done));
        }
        public IEnumerator CoSellItem(ItemType itemType, int amount, Action done = null)
        {
            JSONNode jdata = new JSONObject();
            jdata["userID"] = UserDataManager.Instance.GetUserID();
            jdata["itemType"] = (int)itemType;
            jdata["amount"] = amount;
            Debug.Log("Sell item : " + jdata.ToString());
            yield return APIManager.Instance.PostDataUrl(jdata.ToString(), URL_Config.BASE_API_URL + ItemDataCf.SellItem, (data) =>
            {
                APIResponseData apiResponseData = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                if (apiResponseData.Status == 0) return;
                done?.Invoke();
            });
        }

        public void ExchangeItem(ItemType itemType, int amount, Action done = null)
        {
            StartCoroutine(IEExchangeItem(itemType, amount, done));
        }

        public IEnumerator IEExchangeItem(ItemType itemType, int amount, Action done = null)
        {
            JSONNode jdata = new JSONObject();
            jdata["userID"] = UserDataManager.Instance.GetUserID();
            jdata["itemType"] = (int)itemType;
            jdata["amount"] = amount;
            yield return APIManager.Instance.PostDataUrl(jdata.ToString(), URL_Config.BASE_API_URL + ItemDataCf.ExchangeItem, (data) =>
            {
                APIResponseData apiResponseData = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                if (apiResponseData.Status == 0) return;
                done?.Invoke();
            });
        }

        public IEnumerator IEExchangeItemAdv(ItemType itemType, Action done = null)
        {
            JSONNode jdata = new JSONObject();
            jdata["userID"] = UserDataManager.Instance.GetUserID();
            jdata["itemType"] = (int)itemType;
            yield return APIManager.Instance.PostDataUrl(jdata.ToString(), URL_Config.BASE_API_URL + ItemDataCf.ExchangeItemAdv, (data) =>
            {
                APIResponseData apiResponseData = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                if (apiResponseData.Status == 0) return;
                done?.Invoke();
            });
        }

        #endregion

        #region Get
        public ItemData GetItem(ItemType itemType)
        {
            switch (itemType)
            {
                case ItemType.Energy:
                    (int energy, int energyMax, int energyRecover) = UserDataManager.Instance.GetUserEnergyItem();
                    ItemData itemData = new ItemData();
                    itemData.Type = itemType;
                    itemData.Amount = energy;
                    return itemData;
                case ItemType.ArenaTicket:
                    (int ticket, int maxTicket, int recover) = ArenaManager.Instance.GetUserArenaTicketItem();
                    return new ItemData(itemType, ticket);
                case ItemType.BattlePassPoint:
                    return new ItemData(itemType, BattlePassManager.Instance.BattlePass.BattlePassPoint);
                case ItemType.KeyPortal:
                    return new ItemData(itemType, PortalWorldMapManager.Instance.GetKeyPortalAmount());
                default:
                    ItemData item = this.UserItems.Get(itemType.ToString());
                    if (item == null)
                    {
                        NTLog.LogError("itemData not found: " + itemType.ToString());
                        return null;
                    }
                    return item;
            }
        }

        public List<ItemData> GetAllItem()
        {
            List<ItemData> listItem = new List<ItemData>();
            foreach (ItemData item in this.UserItems.ToList())
            {
                if (item.Amount > 0 && this.IsItem(item.Type))
                {
                    listItem.Add(item);
                }
            }
            return listItem;
        }

        public List<ItemData> GetItemBag()
        {
            List<ItemData> listItem = new List<ItemData>();
            foreach (ItemType itemType in ItemDataCf.ItemBag)
            {
                ItemData item = this.GetItem(itemType);
                if (item != null && item.Amount > 0)
                {
                    listItem.Add(item);
                }
            }
            return listItem;
        }
        public bool CheckItemFullSlot(ItemType itemType)
        {
            if (GetItem(itemType) == null && GetItemBag().Count >= UserDataManager.Instance.GetCurrentSlotInventoryBag())
            {
                return true;
            }
            return false;

        }
        public Sprite GetIcon(ItemType itemType)
        {
            ItemDataInfo itemDataInfo = this.ItemDataInfoDic.Get(itemType.ToString());
            if (itemDataInfo == null)
            {
                Debug.LogError("itemDataInfo not found: " + itemType.ToString());
                return this.DefaultIcon;
            }
            if (itemDataInfo.Icon == null)
            {
                itemDataInfo.Icon = this.ItemIconDic.Get(itemDataInfo.ImagePath);
            }
            if (itemDataInfo.Icon == null)
            {
                Debug.LogError("sprite not found: " + itemDataInfo.ImagePath);
                return this.DefaultIcon;
            }
            return itemDataInfo.Icon;
        }

        public ItemDataInfo GetItemDataInfo(ItemType itemType)
        {
            return this.ItemDataInfoDic.Get(itemType.ToString());
        }

        public string GetItemName(ItemType itemType)
        {
            return Lean.Localization.LeanLocalization.GetTranslationText("item_name_" + (int)itemType, this.ItemDataInfoDic.Get(itemType.ToString()).ImagePath);
        }

        public string GetItemDescription(ItemType itemType)
        {
            return Lean.Localization.LeanLocalization.GetTranslationText("item_des_" + (int)itemType, this.ItemDataInfoDic.Get(itemType.ToString()).ImagePath);
        }

        public bool IsItem(ItemType itemType)
        {
            return true;
        }

        public bool CanSellItem(ItemType itemType)
        {
            return this.ItemDataInfoDic.Get(itemType.ToString()).IsSell;
        }

        public ItemData GetPriceSell(ItemType itemType)
        {
            ItemDataInfo itemDataInfo = this.ItemDataInfoDic.Get(itemType.ToString());
            if (itemDataInfo == null)
            {
                Debug.LogError("itemDataInfo not found: " + itemType.ToString());
                return null;
            }
            return itemDataInfo.Price;
        }

        public string GetItemTextSprite(ItemType itemType)
        {
            ItemDataInfo itemDataInfo = this.ItemDataInfoDic.Get(itemType.ToString());
            if (itemDataInfo == null)
            {
                NTLog.LogError("itemDataInfo not found: " + itemType.ToString());
                return "";
            }
            return "<sprite name=\"" + itemDataInfo.ImagePath + "\">";
        }

        public bool CanCombineItem(ItemType itemType)
        {
            ItemDataInfo itemDataInfo = ItemDataManager.Instance.GetItemDataInfo(itemType);
            if (itemDataInfo == null) return false;
            return itemDataInfo.IsCombine;
        }

        public ItemData[] GetCombineRequire(ItemType itemType)
        {
            ItemDataInfo itemDataInfo = ItemDataManager.Instance.GetItemDataInfo(itemType);
            if (itemDataInfo == null) return new ItemData[0];
            return itemDataInfo.CombineRequire;
        }

        public CardPlayerIndex GetCombineResult(ItemType itemType)
        {
            ItemDataInfo itemDataInfo = ItemDataManager.Instance.GetItemDataInfo(itemType);
            if (itemDataInfo == null) return CardPlayerIndex.Card_0;
            return itemDataInfo.CombineResult;
        }

        public bool CanMergeItem(ItemType itemType)
        {
            ItemDataInfo itemDataInfo = ItemDataManager.Instance.GetItemDataInfo(itemType);
            if (itemDataInfo == null) return false;
            return itemDataInfo.IsMerge;
        }

        public ItemData[] GetMergeRequire(ItemType itemType)
        {
            ItemDataInfo itemDataInfo = ItemDataManager.Instance.GetItemDataInfo(itemType);
            if (itemDataInfo == null) return new ItemData[0];
            return itemDataInfo.MergeRequire;
        }

        public ItemData GetMergeResult(ItemType itemType)
        {
            ItemDataInfo itemDataInfo = ItemDataManager.Instance.GetItemDataInfo(itemType);
            if (itemDataInfo == null) return null;
            return itemDataInfo.MergeResult;
        }

        public bool IsAdd(ItemType itemType)
        {
            switch (itemType)
            {
                case ItemType.Energy:
                    return true;
                case ItemType.Coin:
                    return true;
                case ItemType.Gem:
                    return true;
                case ItemType.BattlePassPoint:
                    return true;
                default:
                    return false;
            }
        }

        public ExchangeItem GetExchangeItem(ItemType itemType)
        {
            return this.ItemExchangeData.Find(item => item.Type == itemType);
        }

        public long GetAmountExchange(ItemType itemType)
        {
            ExchangeItem exchangeItem = this.ItemExchangeData.Find(item => item.Type == itemType);
            if (exchangeItem == null)
            {
                NTLog.LogError("GetAmountExchange not found: " + itemType.ToString());
                return 0;
            }
            if (itemType == ItemType.Energy)
            {
                long max_exchange = UserDataManager.Instance.GetEnergyMax();
                if (max_exchange < exchangeItem.ExchangeResult.Amount) max_exchange = exchangeItem.ExchangeResult.Amount;
                return max_exchange;
            }

            return exchangeItem.ExchangeRequire.Amount;
        }

        public long GetCapDailyExchange(ItemType itemType)
        {
            ExchangeItem exchangeItem = this.ItemExchangeData.Find(item => item.Type == itemType);
            if (exchangeItem == null)
            {
                NTLog.LogError("GetCapDailyExchange not found: " + itemType.ToString());
                return 0;
            }
            return exchangeItem.Cap;
        }

        public bool IsCapExchange(ItemType itemType)
        {
            ExchangeItem exchangeItem = this.ItemExchangeData.Find(item => item.Type == itemType);
            if (exchangeItem == null)
            {
                NTLog.LogError("IsCapExchange not found: " + itemType.ToString());
                return true;
            }
            long remainDailyExchange = this.GetRemainDailyExchange(itemType);
            if (remainDailyExchange > 0)
            {
                return false;
            }
            return true;
        }

        public bool IsCapExchangeAdv(ItemType itemType)
        {
            ExchangeItemAdv exchangeItemAdv = this.ItemExchangeAdvData.Find(item => item.Type == itemType);
            if (exchangeItemAdv == null)
            {
                NTLog.LogError("IsCapExchangeAdv not found: " + itemType.ToString());
                return true;
            }

            long remainDailyExchangeAdv = this.GetRemainDailyExchangeAdv(itemType);
            if (remainDailyExchangeAdv > 0)
            {
                return false;
            }
            return true;
        }

        public long GetRemainDailyExchange(ItemType itemType)
        {
            long cap = this.GetCapDailyExchange(itemType);
            ExchangeItem exchangeItem = this.ItemExchangeData.Find(item => item.Type == itemType);
            if (exchangeItem == null)
            {
                NTLog.LogError("GetRemainDailyExchange not found: " + itemType.ToString());
                return 0;
            }
            if (cap == 0) return 0;
            UserExchange userExchange = this.ExchangeDaily.Find(item => item._id == exchangeItem._id);
            if (userExchange == null)
            {
                return cap;
            }
            return cap - userExchange.Amount < 0 ? 0 : cap - userExchange.Amount;
        }

        public ExchangeItemAdv GetExchangeItemAdv(ItemType itemType)
        {
            return this.ItemExchangeAdvData.Find(item => item.Type == itemType);
        }

        public long GetRemainDailyExchangeAdv(ItemType itemType)
        {
            ExchangeItemAdv exchangeItemAdv = this.GetExchangeItemAdv(itemType);
            if (exchangeItemAdv == null)
            {
                NTLog.LogError("GetRemainDailyExchangeAdv not found: " + itemType.ToString());
                return 0;
            }
            long cap = exchangeItemAdv.Cap;
            UserExchange userExchange = this.ExchangeDaily.Find(item => item._id == exchangeItemAdv._id);
            if (userExchange == null)
            {
                return cap;
            }
            return cap - userExchange.Amount < 0 ? 0 : cap - userExchange.Amount;
        }

        public bool IsExchangeAdv(ItemType itemType)
        {
            ExchangeItemAdv exchangeItemAdv = this.GetExchangeItemAdv(itemType);
            if (exchangeItemAdv == null)
            {
                NTLog.LogError("IsExchangeAdv not found: " + itemType.ToString());
                return false;
            }
            return true;
        }

        #endregion
    }


}