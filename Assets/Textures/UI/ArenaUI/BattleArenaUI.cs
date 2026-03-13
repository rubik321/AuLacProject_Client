using Lean.Localization;

using NTPackage.Functions;
using NTPackage.UI;
using Rubik.Common.AudioHelper;
using Rubik.ItemPlayer;
using Rubik.Manager;
using Rubik.Myrk.Arena;
using Rubik.Myrk.BattleTeam;
using Rubik.Myrk.Clan;
using Rubik.UI;
using Rubik.UserDataPlayer;
using Rubik.UserProfile;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BattleArenaUI : MonoBehaviour
{
    public BattleMemberitem[] lsMembers;
    public AvatarPlayerUI avatarPlayer;
    public TextMeshProUGUI namePlayerTxt, pointTxt, powerTxt,rankTxt, timeLeft,resetGemTxt;
    public GameObject resetGem, resetFree, resetAdv;
    public Image rankIcon;
    public long countDown;
    public Sprite[] lsIcons;
    Coroutine coroutineCountDown;
    private void OnEnable()
    {
        SetData();
        SetTime();
    }
    void SetTime()
    {
        this.countDown = ArenaManager.Instance.GetFreeReset();
        resetGemTxt.text = ArenaManager.Instance.ArenaData.ResetCost.Amount.ToString();
        if (countDown < 1)
        {
            timeLeft.transform.parent.gameObject.SetActive(false);
            resetGem.SetActive(false);
            resetFree.SetActive(true);
            resetAdv.SetActive(false);

        }
        else
        {
            timeLeft.transform.parent.gameObject.SetActive(true);
            resetGem.SetActive(true);
            resetFree.SetActive(false);
            resetAdv.SetActive(false);
        }
        if (this.coroutineCountDown != null)
        {
            StopCoroutine(this.coroutineCountDown);
        }
        this.coroutineCountDown = StartCoroutine(this.CotimeLeft());
    }
    void SetData()
    {
        var ArenaResponse = ArenaManager.Instance.ArenaResponse;
        int index = 0;
        foreach (var member in ArenaResponse.Opponents)
        {
            lsMembers[index].SetUp(member);
            index++;
        }
        this.avatarPlayer.SetData(UserProfileManager.Instance.AvatarPlayer.Current, UserProfileManager.Instance.AvatarBorderPlayer.Current);
        namePlayerTxt.text = UserDataManager.Instance.UserData.DisplayName;
        pointTxt.text = ArenaResponse.Score.ToString();
        powerTxt.text = BattleTeamManager.Instance.GetPower(BattleTeamManager.Instance.GetBattleTeamDataSelected().ToShortTeam()).ToString();
        rankIcon.sprite = lsIcons[GetIndexRank(ArenaManager.Instance.GetRankingTypeByScore(ArenaResponse.Score).ToString())];
        rankTxt.text = ArenaManager.Instance.GetRankingTypeByScore(ArenaManager.Instance.ArenaResponse.Score).ToString();
    }
    public void ResetOpponet(bool isGem = false)
    {
        if (isGem&& ItemDataManager.Instance.GetItem(ItemType.Gem).Amount < 10)
        {
            ItemDataManager.Instance.ShowDontEnoughItem(ItemType.Gem);
            return;
        }
        if (isGem)
        {
            PopupManager.Instance.OnUI(PopupCode.MessageOptionPanel, null, (popupUI) =>
            {
                MessageOptionPanel messageOptionPanel = popupUI as MessageOptionPanel;
                messageOptionPanel.SetData(Lean.Localization.LeanLocalization.GetTranslationText("clan_message"), Lean.Localization.LeanLocalization.GetTranslationText("arena_mess_reset"));

                messageOptionPanel.SetActionConfirm(() =>
                {
                    popupUI.OffUI();
                    ArenaManager.Instance.ResetOpponent(false, true, () => {
                        AudioCtrl.Instance.Play(AudioName.UI_Button_Default);
                        SetData();
                        SetTime();
                    });
                }, Lean.Localization.LeanLocalization.GetTranslationText("btn_confirm", "Confirm"));
                messageOptionPanel.SetActionReject(() =>
                {
                    popupUI.OffUI();
                }, Lean.Localization.LeanLocalization.GetTranslationText("btn_cancel", "Cancel"));
                //messageOptionPanel.SetTimeConfirm();
            });
        }
        else
        {
            ArenaManager.Instance.ResetOpponent(false, isGem, () => {
                AudioCtrl.Instance.Play(AudioName.UI_Button_Default);
                SetData();
                SetTime();
            });
        }
       
    }
    IEnumerator CotimeLeft()
    {
        while (true)
        {
            this.countDown = ArenaManager.Instance.GetFreeReset();
            timeLeft.text = LeanLocalization.GetTranslationText("time_left", "Time left ") + ": <color=#BD7E92>" + NTFunction.FormatTimeMinus(this.countDown) + "</color>";
            yield return new WaitForSeconds(1);
            if (countDown < 1)
            {
                SetTime();
                yield break;
            }

        }
    }
    int GetIndexRank(string rank)
    {
        if (rank.Contains("Bronze"))
        {
            rankTxt.color = Color.brown;
            return 0;
        }
        if (rank.Contains("Silver"))
        {
            rankTxt.color = Color.white;
            return 1;
        }
        if (rank.Contains("Gold"))
        {
            rankTxt.color = Color.yellow;
            return 2;
        }
        if (rank.Contains("Platinum"))
        {
            rankTxt.color = Color.pink;
            return 3;
        }
        return 0;
    }
}
