using NTPackage.UI;
using Rubik.ItemPlayer;
using Rubik.Myrk.Arena;
using Rubik.Myrk.BattleTeam;
using Rubik.UserDataPlayer;
using Rubik.UserProfile;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArenaResultItem : MonoBehaviour
{
    public AvatarPlayerUI avatarPlayer;
    public TextMeshProUGUI namePlayerTxt, pointTxt, powerTxt,rankTxt;
    public Sprite winSpr, normalSpr;
    public OpponentItem atkItem, defItem;
   public void SetUp(ArenaHistory history)
    {
        atkItem.SetUp(history.Attacker,history.BonusAttacker);
        defItem.SetUp(history.Defender,history.BonusDefender);
        if (history.Attacker.UserID == UserDataManager.Instance.GetUserID()&& history.Attacker.IsWin ||
            history.Defender.UserID == UserDataManager.Instance.GetUserID() && history.Defender.IsWin)
        {
            
            GetComponent<Image>().sprite = winSpr;
        }
        else
        {
          
            GetComponent<Image>().sprite = normalSpr;
        }
    }
    
}
