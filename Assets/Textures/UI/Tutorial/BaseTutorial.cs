using Lean.Localization;
using NTPackage.UI;
using Rubik.Combat;
using Rubik.UserProfile;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BaseTutorial : MonoBehaviour
{
    public int ID;

    public int indexTut;
    public TextMeshProUGUI uiText; // Gán Text UI trong Editor
    public float letterDelay = 0.01f; // Thời gian trễ giữa các chữ cái
    public string fullText;
    public Button btnNext,btnSkip;
    public float timeDelay = 1;
    public GameObject targetButton,currentButton,arrowGo;
    public bool isEnd = false;
    public TutorialType tutDoneIndex;
    public LockFunctionType locktype = LockFunctionType.None;
    private void OnEnable()
    {
        fullText = LeanLocalization.GetTranslationText(gameObject.name);
        if (btnSkip != null)
        {
            btnSkip.onClick.RemoveAllListeners();
            btnSkip.onClick.AddListener(OnSkip);
            btnSkip.gameObject.SetActive(false);
        }
        if (currentButton != null)
            currentButton.gameObject.SetActive(false);
        if(arrowGo!=null)
            arrowGo.gameObject.SetActive(false);

        ShowText(fullText);
    }
    public void ShowText(string fullText)
    {
        if(btnNext!=null)
            btnNext.gameObject.SetActive(false);
        StartCoroutine(TypeText(fullText));
    }

    private IEnumerator TypeText(string textToType)
    {
        uiText.text = "";
        //if (currentButton != null)
        //    currentButton.gameObject.SetActive(false);
        foreach (char letter in textToType)
        {
            uiText.text += letter;
            yield return new WaitForSeconds(letterDelay);
        }
        OnTypeEnd();
    }
    public void OnSkip()
    {
        StopAllCoroutines();
        uiText.text = fullText;
       
        OnTypeEnd();
    }
    void OnTypeEnd()
    {
        btnSkip.gameObject.SetActive(false);
        if (btnNext != null)
        {
            btnNext.onClick.RemoveAllListeners();
            btnNext.onClick.AddListener(() =>
            {
                gameObject.SetActive(false);
                if (!isEnd)
                {
                    var temp = PopupManager.Instance.GetPopupUI(PopupCode.TutorialUI).GetComponent<TutorialUI>();

                    if (tutDoneIndex == TutorialType.Tutorial5&&temp.isEndOftut())
                    {
                        UserProfileManager.Instance.DoneTutorial(tutDoneIndex, () =>
                        {
                            AppsFlyerManager.TrackingEvent(AppsflyerEvents.player_finish_tutorial, 1, 1);
                            temp.indexOftut = 0;
                            temp.CkeckNextTutorialIsShow();
                            if (temp.isCanNextTut)
                                PopupManager.Instance.OnUI(PopupCode.TutorialUI);
                        });
                    }
                   else if (temp.isCanNextTut)
                        PopupManager.Instance.OnUI(PopupCode.TutorialUI);
                    //else
                     //   PopupManager.Instance.OffAllPopupUI();
                }
                else
                {
                    PopupManager.Instance.OffAllPopupUI();
                    UserProfileManager.Instance.DoneTutorial(tutDoneIndex, () =>
                    {
                    });
                }
                   
            });
        }
        if (currentButton != null)
            currentButton.gameObject.SetActive(true);
        if (targetButton != null)
        {
            currentButton.transform.position = targetButton.transform.position;
            if (targetButton.GetComponent<NTButtonEffect>() != null)
            {
                currentButton.GetComponent<Button>().onClick.AddListener(() =>
                {
                    AssetLoader.Instance.IsTut = true;
                    targetButton.GetComponent<NTButtonEffect>().Onclick.Invoke();
                    gameObject.SetActive(false);

                    //PopupManager.Instance.OnUI(PopupCode.TutorialUI);

                });
            }
        }
        if (arrowGo != null)
            arrowGo.gameObject.SetActive(false);
        if (arrowGo != null)
            arrowGo.gameObject.SetActive(true);

        if (btnNext != null)
            btnNext.gameObject.SetActive(true);
    }
}
