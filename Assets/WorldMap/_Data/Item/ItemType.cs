using System;

namespace GOA.Item{
    public class ItemTypeCodeParser
    {
        public static ItemTypeCode FromString(string name)
        {
            try
            {
                name = name.Trim();
                return (ItemTypeCode)Enum.Parse(typeof(ItemTypeCode), name.Replace(" ",""));
            }
            catch (System.Exception)
            {
                return ItemTypeCode.Null;
            }
            
        }
    }

    public enum CurrencyType{
        Null = 0,
        Gold = 1,
        Gin = 2,
        RealMoney = 3,
    }

    public enum ItemTypeCode
    {
        Null = 0,
        Potion = 1,
        Material = 2,
        KeyItem = 3,
        Bundle = 4,

    }
}
