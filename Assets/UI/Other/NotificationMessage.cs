using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NotificationMessage : MonoBehaviour
{
    public TextMeshProUGUI txtContent, txtTitle;
    public Button yesButton, noButton, okbutton;
    [SerializeField] GameObject rewardObj, messObj;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void ShowMessage(string str, bool isYesNo = false, UnityEngine.Events.UnityAction callback = null)
    {
        gameObject.SetActive(true);
        rewardObj.SetActive(false);
        messObj.SetActive(true);
        txtContent.text = str;
        txtTitle.text = "Message";
        if (!isYesNo)
        {
            yesButton.gameObject.SetActive(false);
            noButton.gameObject.SetActive(false);
            okbutton.gameObject.SetActive(true);
            okbutton.onClick.RemoveAllListeners();
            okbutton.onClick.AddListener(() => {
                HideMessage();
            });
        }
        else
        {
            yesButton.gameObject.SetActive(true);
            noButton.gameObject.SetActive(true);
            noButton.onClick.RemoveAllListeners();
            yesButton.onClick.RemoveAllListeners();
            okbutton.gameObject.SetActive(false);
            noButton.onClick.AddListener(() => {
                HideMessage();
            });
            yesButton.onClick.AddListener(() => {
                callback();
                HideMessage();

            });
        }
    }
    public void HideMessage()
    {
        Rubik.Common.AudioHelper.AudioCtrl.Instance.Play(Rubik.Common.AudioHelper.AudioName.UI_Button_Default);
        gameObject.SetActive(false);
    }
}
