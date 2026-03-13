using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;
using UnityEngine.UI;
using TMPro;
using Rubik.UserDataPlayer;
using Rubik.Format;
using Rubik.UserProfile;
using Rubik.BattleEngine;

public class UIGamePlayBattle : MonoBehaviour
{
    public static Rect safeArea;
    public static UIGamePlayBattle Instance;
    [SerializeField] GameObject panelButton, panelStart,x2Hightlight;
    public StateMachine uiMain;
    public TurnController turnControl;
    public Sprite[] lsSkillSprs;
    public int[] cost;
    public Image imgSkill;
    public Text txtSkillCost;
    public TextMeshProUGUI TextName, TextNameEnemy, TextLevel, TextExp,txtTurn, TextLevelEnemy;
    public AvatarPlayerUI AvatarPlayerUI,AvatarEnemyUI;
    public GameObject botGo, topGo,enemyGo;
    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        this.TextName.text = UserDataManager.Instance.UserData.DisplayName;
        (long exp, long expNext, int level) = UserDataManager.Instance.GetPlayerLevel();
        this.TextLevel.text = (level+1).ToString();
        this.TextExp.text = $"{FormatData.GetFriendlyShortNumber(exp)}/{FormatData.GetFriendlyShortNumber(expNext)}";
        this.AvatarPlayerUI.SetData(UserProfileManager.Instance.AvatarPlayer.Current, UserProfileManager.Instance.AvatarBorderPlayer.Current, UserDataManager.Instance.UserData.DisplayName, 1);
        switch (BattleEngineController.Instance.BattleType)
        {
           
            case BattleType.Arena:
                botGo.SetActive(false);
                topGo.SetActive(true);
                enemyGo.SetActive(true);
                TextNameEnemy.text = BattleEngineController.Instance.enemyData.UserData.DisplayName;
                this.TextLevelEnemy.text = (BattleEngineController.Instance.enemyData.UserData.Level + 1).ToString();
                this.AvatarEnemyUI.SetData(BattleEngineController.Instance.enemyData.UserData.Avatar, BattleEngineController.Instance.enemyData.UserData.AvatarBorder, BattleEngineController.Instance.enemyData.UserData.DisplayName, 1);
                break;
            default:
                botGo.SetActive(true);
                topGo.SetActive(false);
                enemyGo.SetActive(false);
                break;
        }
    }

    public void ShowPanelItem(bool isShow)
    {
      uiMain.ChangeState("PanelItem");
    }
    public void ShowPanelButton()
    {
        uiMain.ChangeState("PanelButton");
    }
    public void ExitPanelButton()
    {
        uiMain.Exit();
    }
    public void ShowPanelSkills(bool isShow)
    {
       
            uiMain.ChangeState("PanelSkills");
        
        
    }
    public void ShowStateTxt(string txt)
    {
       // turnControl.SetState(txt);
    }
    public void ShowSkill(int index)
    {
        txtSkillCost.text = "(" + cost[index].ToString()+")";
        imgSkill.sprite = lsSkillSprs[index];
        GameController.Instance.SkillCost = cost[index];
    }
    public void UpdatePlayerData(object data = null)
    {
        this.TextName.text = UserDataManager.Instance.UserData.DisplayName;
        (long exp, long expNext, int level) = UserDataManager.Instance.GetPlayerLevel();
        this.TextLevel.text = level.ToString();
        this.TextExp.text = $"{FormatData.GetFriendlyShortNumber(exp)}/{FormatData.GetFriendlyShortNumber(expNext)}";
      
    }
}
