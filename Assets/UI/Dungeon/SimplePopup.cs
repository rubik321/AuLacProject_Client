using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

public enum AnimationPopupType
{
    OnTopDown,
    OnFade
}
public class SimplePopup : MonoBehaviour
{
	
    [SerializeField]
    protected CanvasGroup content;
    [SerializeField]
    private Image cover;
    [SerializeField]
    public TextMeshProUGUI txtTitle;
    [SerializeField]
    private GameObject gridBtn;
    [SerializeField] bool isUp;
    /// Is this pop-up closed when player hits Back button on device?
    public bool IsClosedOnEsc;
    public SimplePopup ParentPopup;
    private bool parentCloseOnEsc { get; set; }
    private Action onShown { get; set; }
    private Action onHidden { get; set; }
    protected float contentStartY { get; set; }
    private float yPos { get; set; }
    public bool IsActive { get; set; }
    public AnimationPopupType animType = AnimationPopupType.OnTopDown;
    protected virtual void Start()
    {
        contentStartY = content.transform.localPosition.y;
       
    }

    /// <summary>
    /// Show the pop-up. This method should be called at the end, AFTER all assignments to this pop-up (especially adding button callbacks) have been made.
    /// </summary>
    /// <param name="content">String of content</param>
    /// <param name="isYesNo">Is this consisting of 2 buttons?</param>
    /// <param name="isStatus">Is this pop-up a status display for confirmation only?</param>
    /// <param name="isError">Is this a warning/error popping?</param>
    public virtual void ShowUp( AnimationPopupType type = AnimationPopupType.OnTopDown)
    {
        if (IsActive)
            return;
        if (isUp)
        {
            yPos = 1678 + contentStartY;
        }
        else
        {
            yPos = contentStartY- 1678;
        }
       // SoundController.Instance.PlaySingle(FXSound.Instance.FX_Pop_up);
        IsActive = true;
        this.content.blocksRaycasts = true;
        animType = type;
       
        ShowUpAnimations(type);
      
    }
    public void ShowPopupWithDelayTime(AnimationPopupType type, float timedelay = 0.3f)
    {
        ShowUpAnimations(type, timedelay);
    }
    public void HidePopupWithDelayTime(AnimationPopupType type, float timedelay = 0.3f)
    {
        HideAnimations(type, timedelay);
    }
    private void ShowUpAnimations(AnimationPopupType type,float timedelay = 0.3f)
    {
        switch (type)
        {
            case AnimationPopupType.OnTopDown:
                cover.DOKill(false);
                content.DOKill(false);
                content.transform.DOKill(false);
                cover.gameObject.SetActive(true);
               // cover.DOFade(0.75f, 0.1f).SetUpdate(true);
                content.transform.localPosition = new Vector3(0, yPos);

                content.gameObject.SetActive(true);
                content.DOFade(1, 0.01f).SetUpdate(true);
                content.transform.DOLocalMoveY(contentStartY, timedelay).SetEase(Ease.OutBack).OnComplete(() =>
                {
                    IsActive = true;
                   // Global.Instance.IsClosedOnEsc = IsClosedOnEsc = true;
                    OnCompletedButton();
                    if (onShown != null)
                    {
                        onShown();

                    }
                }).SetUpdate(true);
                break;
            case AnimationPopupType.OnFade:
                cover.DOKill(false);
                content.DOKill(false);
                content.transform.DOKill(false);
                cover.gameObject.SetActive(true);
                //cover.DOFade(0.75f, 0.1f).SetUpdate(true);
                content.transform.localScale = new Vector3(0, 0);
                content.gameObject.SetActive(true);
                content.DOFade(1, 0.01f).SetUpdate(true);
                content.transform.DOScale(new Vector3(1,1), timedelay).SetEase(Ease.OutBack).OnComplete(() =>
                {
                    IsActive = true;
                    //Global.Instance.IsClosedOnEsc = IsClosedOnEsc = true;
                    OnCompletedButton();
                    if (onShown != null)
                    {
                        onShown();

                    }
                   
                }).SetUpdate(true);
                break;
        }
    }
        
    public virtual void OnCompletedButton()
    {

    }
    public virtual void OnCloseButton()
    {
        Hide();

       //SoundController.Instance.PlaySingle(FXSound.Instance.Fx_Button1);
    }

    public virtual void OnNoButton()
    {
        //Hide();
        //SoundController.Instance.PlaySingle(FXSound.Instance.Fx_Button1);
    }

    public virtual void OnYesButton()
    {
        Hide();
       // SoundController.Instance.PlaySingle(FXSound.Instance.Fx_Button1);
    }

    public virtual void Hide()
    {
       // SoundController.Instance.PlaySingle(UISfxController.Instance.Sfx_Button);
        HideAnimations(animType);
        //IsActive = false;
       // cover.gameObject.SetActive(false);
        //content.blocksRaycasts = false;
        //content.gameObject.SetActive(false);
       // if (!Global.Instance.isEditorMode)
          //  SoundController.Instance.PlaySingle(UISfxController.Instance.SfxButtonSound);
    }
    bool isClick = false;
    private void HideAnimations(AnimationPopupType type, float timedelay = 0.3f)
    {
        if (isClick)
            return ;
        isClick = true;
        switch (type)
        {
            case AnimationPopupType.OnTopDown:
                //Global.Instance.IsClosedOnEsc = IsClosedOnEsc = false;
                cover.DOKill(false);
                content.DOKill(false);
                content.transform.DOKill(false);
               // cover.DOFade(0, 0.3f).SetUpdate(true);
                //content.DOFade(0, 0.3f).SetDelay(0.2f).SetUpdate(true);

                if (onHidden != null)
                {
                    Debug.Log("Hiden ");
                    onHidden();
                    onHidden = null;
                }
                content.transform.DOLocalMoveY(yPos, timedelay).SetEase(Ease.InBack).OnComplete(() =>
                {
                    IsActive = false;
                    cover.gameObject.SetActive(false);
                    content.blocksRaycasts = false;
                    content.gameObject.SetActive(false);
                    isClick = false;
                    if (onHidden != null)
                    {
                        Debug.Log("Hiden ");
                        onHidden();
                        onHidden = null;
                    }

                }).SetUpdate(true);
                break;
            case AnimationPopupType.OnFade:
                //Global.Instance.IsClosedOnEsc = IsClosedOnEsc = false;
                cover.DOKill(false);
                content.DOKill(false);
                content.transform.DOKill(false);
                //cover.DOFade(0, 0.3f).SetUpdate(true);
                //content.DOFade(0, 0.1f).SetDelay(0.2f).SetUpdate(true);

                //if (onHidden != null)
                //{
                //    onHidden();
                //    onHidden = null;
                //}
                content.transform.DOScale(new Vector3(0, 0), timedelay).SetEase(Ease.InBack).OnComplete(() =>
                {
                    IsActive = false;
                    cover.gameObject.SetActive(false);
                    content.blocksRaycasts = false;
                    content.gameObject.SetActive(false);
                    isClick = false;
                    if (onHidden != null)
                    {
                        onHidden();
                        onHidden = null;
                    }

                }).SetUpdate(true);
                break;
        }
       
    }

    public void SetOnShown(Action onShown)
    {
        this.onShown = null;
        this.onShown += onShown;
    }

    public void SetOnHidden(Action onHidden)
    {
        this.onHidden = null;
        this.onHidden += onHidden;
    }
    [Button]
    public void HidePopup()
    {
        cover.gameObject.SetActive(false);
        content.gameObject.SetActive(false);
    }
    [Button]
    public void ShowPopup()
    {
        cover.gameObject.SetActive(true);
        content.gameObject.SetActive(true);
    }
    private void Update()
    {
        if (IsActive
            && IsClosedOnEsc
            && Input.GetKeyDown(KeyCode.Escape))
        {
			
            OnCloseButton();
        }
    }
}
