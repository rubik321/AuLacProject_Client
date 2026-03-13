namespace Rubik.Myrk.Shop
{
    [System.Serializable]
    public class ShopData
    {
        public ItemShop[] ShopList;
    }

    [System.Serializable]
    public class ShopHistory
    {
        public string Index;
        public int BuyTime;
    }
}