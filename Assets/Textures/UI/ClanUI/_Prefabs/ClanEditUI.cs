using Lean.Localization;
using Rubik.Myrk.Clan;
using Rubik.UserDataPlayer;
using System.Diagnostics;
using TMPro;
using UnityEngine;
public class ClanEditUI : MonoBehaviour
{
    public TextMeshProUGUI inputTxt;
    public TextMeshProUGUI titleTxt;
    public void ShowEdit(int index)
    {
        gameObject.SetActive(true);
        switch (index)
        {
            case 0:
                titleTxt.text = LeanLocalization.GetTranslationText("clan_noti_clan_21"); 
                break;
            case 1:
                titleTxt.text = LeanLocalization.GetTranslationText("clan_noti_clan_15"); 
                break;

        }
        this.index = index;
    }
    int index = 0;
    public void ConfirmEdit()
    {
        switch (index)
        {
            case 0:
                StartCoroutine(ClanManager.Instance.IEEditAnnounce(UserDataManager.Instance.GetUserID(), ClanManager.Instance.PlayerClan._id, inputTxt.text, () => {
                    GetComponentInParent<ClanUI>().ShowMyClan();
                    gameObject.SetActive(false);
                }));

                break;
            case 1:
                StartCoroutine(ClanManager.Instance.IEEditSlogan(UserDataManager.Instance.GetUserID(), ClanManager.Instance.PlayerClan._id, inputTxt.text, () => {
                    GetComponentInParent<ClanUI>().ShowMyClan();
                    gameObject.SetActive(false);
                }));

                break;

        }

       
    }
}
