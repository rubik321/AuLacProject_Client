using Rubik.UserDataPlayer;

namespace Rubik.RewardData
{
    using Rubik.CardPlayer;
    using Rubik.CharacterCloth;
    using Rubik.CharacterGear;
    using Rubik.ItemPlayer;
    
    [System.Serializable]
    public class RewardData
    {
        public string Title;
        public ItemData[] Items;
        public CharacterGear[] GearItems;
        public CardPlayer[] Card;
    }
}