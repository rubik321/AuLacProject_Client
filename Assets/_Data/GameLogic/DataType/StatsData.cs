namespace Rubik.DataType
{
    [System.Serializable]
    public class GearStats{
        public long HeroAtk = 0;
        public long TeamHp = 0;
        public long TeamSpeed = 0;
        
        public void AddStats(GearStats stats){
            this.HeroAtk += stats.HeroAtk;
            this.TeamHp += stats.TeamHp;
            this.TeamSpeed += stats.TeamSpeed;
        }

        public bool IsZero(){
            return this.HeroAtk == 0 && this.TeamHp == 0 && this.TeamSpeed == 0;
        }
    }
}
