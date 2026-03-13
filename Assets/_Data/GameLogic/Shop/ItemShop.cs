using Rubik.CharacterCloth;
namespace Rubik.Myrk.Shop
{
    using Rubik.CharacterGear;
    using Rubik.ItemPlayer;
    using Rubik.UserDataPlayer;

    public enum LimitType
    {
        None,
        Daily,
        Weekly,
        Monthly,
    }

    [System.Serializable]
    public class ItemShop
    {
        public string Index;

        public ItemData[] OfferItems;
        public ChracterGearRarity[] OfferCharacterGearRarity;

        public ItemData[] PayItems;
        public string ProductId;
        public int Group;
        public LimitType LimitType = LimitType.None;
        public int Limit;

        public Role[] Role;
    }

}