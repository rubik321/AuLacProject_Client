using Lean.Localization;
using NTFunctions_old;
using Rubik.Manager;
using Rubik.Myrk.Clan;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DonateClanUI : MonoBehaviour
{
    public TextMeshProUGUI timeLeft;
    public long countDown;
    public Coroutine coroutineCountDown;
    public Image[] lsDonateImg;
    public Sprite donateOn, donateOff;

    private void OnEnable()
    {
        this.countDown = ServerManager.Instance.GetNextTimeNewDay();
        if (this.coroutineCountDown != null)
        {
            StopCoroutine(this.coroutineCountDown);
        }
        this.coroutineCountDown = StartCoroutine(this.CotimeLeft());
        
    }
    public void SetActiveButton()
    {
        int index = 0;
        timeLeft.transform.parent.gameObject.SetActive(false);
        foreach (var Image in lsDonateImg)
        {
           if(ClanManager.Instance.IsDonated((ClanDonateType)index))
            {
                timeLeft.transform.parent.gameObject.SetActive(true);
                lsDonateImg[index].sprite =   donateOff;
            }
            else
            {
              
                lsDonateImg[index].sprite = donateOn;
            }
           index++;
        }
    }
    IEnumerator CotimeLeft()
    {
        while (true)
        {
            this.countDown = ServerManager.Instance.GetNextTimeNewDay();
            string temp = LeanLocalization.GetTranslationText("time_left", "Time left ") + ": <color=#BD7E92>" + NTFunction.FormatTimeHour(this.countDown) + "</color>";
            timeLeft.text = temp;
            yield return new WaitForSeconds(1);
        }
    }
}
