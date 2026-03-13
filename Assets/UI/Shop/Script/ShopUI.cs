using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NTFunctions_old;
using TMPro;

namespace GOA.Shop{
    using UserData;
    using Item;
    using NTPackage.Functions;
    using Rubik.Minigame.Restaurant;
    using Rubik.Myrk.Shop;
    using Spine.Unity;
    using Spine;
    using Rubik.CharacterGear;
    using Rubik.CharacterPlayer;
    using Rubik.Combat;
    using Rubik.Myrk.BattleTeam;
    using System.Linq;

    public class ShopUI : PopupUI
    {
        public TextMeshProUGUI TextNumberCoin;
        public TextMeshProUGUI TextNumberGin;
        public TextMeshProUGUI TextNumberKey;
        public TextMeshProUGUI textNameGear,textInfoGear,textStrenght;
        public InAppBoxUI InAppBoxUI;
        public Transform Content;
        public ShopItem ItemPrefab;
        public List<TabInven> lsTabsButton;
        public List<ShopItem> ItemList = new List<ShopItem>();
        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            try
            {
                TextNumberCoin.text = UserData.Instance.data.Coin.ToString();
                TextNumberGin.text = UserData.Instance.data.Gin.ToString();
                TextNumberKey.text = UserData.Instance.Inventory.PortalKey.ToString();
            }
            catch (System.Exception){}
        }

        public void OnUI(){
           // if(!this.CanShow()) return;
            //TabsShop(0);
            OnButtonTabs_Onclick(1);
            
           // SetCloths("",1);
        
            
            //NTFunction.ClearChild(this.Content);
            //foreach (KeyValuePair<string, GOA.Item.ItemInfoData> item in ItemAsset.instance.ItemDataDictionary2.Dictionary)
            //{
            //    if(!item.Value.Shop) continue;
            //    InAppBoxUI inAppBoxUI = Instantiate(this.InAppBoxUI);
            //    inAppBoxUI.transform.SetParent(this.Content);
            //    NTFunctions_old.NTFunction.ResetPosition(inAppBoxUI.transform);
            //    inAppBoxUI.SetData(item.Value);
            //}
            this.Show();
         


        }

        public void OnButtonTabs_Onclick(int index)
        {
            Debug.Log("Tab click ");
            foreach(TabInven tab in lsTabsButton)
            {
                tab.TabOn();
            }
            lsTabsButton[index].TabOn(true);
            currentIndex = index;
            TabsShop(index);
        }
        int currentIndex = 0;
        //this.ItemList = new List<ShopItem>();
        public void TabsShop(int index)
        {

          
           // ObjectPoolingManager.Instance.PushChildObjectIntoPooling(this.Content);
            
            Rubik.Myrk.Shop.ShopData tempShop;
            tempShop = ShopManager.Instance.IapShopData;
           
         //   BuyCloth.gameObject.SetActive(true);
          
            Debug.Log("Shop count : " + tempShop.ShopList.Length);
            int count = 0;
            //foreach (ItemShop itemShop in tempShop.ShopList)
            //{
            //    var temp = CharacterGearManager.Instance.GetGearDataByIndex(itemShop.OfferCharacterGearRand[0].Index);
            //    if (temp.Type== ItemList[count].typeGear)
            //    {
            //        ItemList[count].SetData(itemShop);
            //        count++;
            //    }

            //    //ShopItem item = ObjectPoolingManager.Instance.PullObjectFromPooling<ShopItem>(ObjectPoolingConfig.ShopItem);
            //    //if (item == null) item = Instantiate(this.ItemPrefab);
            //    // item.transform.SetParent(Content);
            //    //NTFunction.ResetPosition(item.transform);
            //   // ItemList[count].SetData(itemShop);
            //    //this.ItemList.Add(item);
            //    //if (item.typeGear == (Rubik.CharacterGear.CharacterGearType)index)
            //    //{
            //    //    item.gameObject.SetActive(true);
            //    //}
            //    //else
            //    //{
            //    //    item.gameObject.SetActive(false);
            //    //}

            //}
           
            foreach (ItemShop itemShop in tempShop.ShopList)
            {
                if (count >= ItemList.Count)
                {
                    ShopItem itemGo = Instantiate(ItemPrefab);
                    itemGo.transform.SetParent(Content,false);
                    ItemList.Add(itemGo);
                }
               
                ItemList[count].SetData(itemShop);
                if (ItemList[count].typeGear == (Rubik.CharacterGear.CharacterGearType)index)
                {
                    ItemList[count].gameObject.SetActive(true);
                    //itemShop.gearIndex = (Rubik.CharacterGear.CharacterGearType)index;
                    ItemList[count].tickGo.SetActive(CharacterGearManager.Instance.CheckGearInListIsUsed(itemShop.OfferCharacterGearRarity[0].Index));
                    //itemShop.gameObject.SetActive(!CharacterGearManager.Instance.CheckGearInList(itemShop.gearIndex));
                }
                else
                {
                    ItemList[count].gameObject.SetActive(false);
                }
                //if (CharacterGearManager.Instance.CheckGearInList(ItemList[count].itemShop.OfferCharacterGearRand[0].Index))
                //    ItemList[count].priceGo.SetActive(false);
                //else
                //    ItemList[count].priceGo.SetActive(true);

                count++;
            }
           
        }
        ShopItem currentShop;
        public void OnButtonOffUI(string id , ShopItem itemShop)
        {
            currentID = id;
            currentShop = itemShop;
          
           
            ShowInfo(id);
            foreach(ShopItem item in ItemList)
            {
                item.OffUI();
            }
        }
        public override void OffUI()
        {
            base.OffUI();
            //ObjectPoolingManager.Instance.PushChildObjectIntoPooling(this.Content);
           

        }
        public void ShowInfo(string id)
        {
           // var tempShop = ShopManager.Instance.GearShopData.ShopList[Random.Range(0,10)];
            textNameGear.text = id;
            textInfoGear.text = id;
            //var temp = CharacterGearManager.Instance.GetGearDataByIndex((CharacterGearIndex)id);
          //  textStrenght.text = temp.Hp.ToString();
        }
      
        
        string currentID;
        public void Buy()
        {
            Debug.Log(" Buy : " + currentID);
        }
        public void Equip()
        {
            Debug.Log(" Buy : " + currentID);
            string idGear = CharacterGearManager.Instance.GetIdByIndex(currentShop.itemShop.OfferCharacterGearRarity[0].Index);
            List<string> lsGears = BattleTeamManager.Instance.GetCharacterGearBattle().Select(gear => gear._id).ToList();
            CharacterGear gear = CharacterGearManager.Instance.GetCharacterGearByID(idGear);
            lsGears[(int)gear.GearData.Type] = idGear;
            // CharacterGearManager.Instance.ChangeGear(BattleTeamManager.Instance.GetCharacterPlayer()._id, lsGears.ToArray(), () =>
            // {
            //     TabsShop(currentIndex);
              
            // });
        }
        public void Info()
        {
            string id = CharacterGearManager.Instance.GetIdByIndex(currentShop.itemShop.OfferCharacterGearRarity[0].Index);
            var gear = CharacterGearManager.Instance.GetCharacterGearByID(id);
            NTPackage.UI.PopupManager.Instance.OnUI(NTPackage.UI.PopupCode.GearInfo_UI, gear, (popupUI) =>
            {
                GearInfo_UI gearInfoUI = popupUI as GearInfo_UI;
                gearInfoUI.SetData(gear._id, -1,null, () =>
                {
                    

                });
            });
        }

    }
}
