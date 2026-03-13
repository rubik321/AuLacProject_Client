
namespace Rubik.UserDataPlayer
{
    using CardPlayer;
    using CharacterPlayer;
    using Rubik.Myrk.BattleTeam;

    [System.Serializable]
    public class UserDataShort
    {
        public string UserID;
        public string DisplayName;
        public int Avatar;
        public int AvatarBorder;
        public string AvatarCustom;
        public int Level;
        public int Exp;
        public long LastLogin;
        public long LastTimeGet;
        public Role Role;
        public BattleShortTeam BattleTeam;
    }
}