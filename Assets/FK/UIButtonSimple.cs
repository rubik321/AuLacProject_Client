using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// <ref>UIButtonSimple</ref> uses UnityUI and DOTween.
/// </summary>
public class UIButtonSimple : InteractiveUI
{
    public bool isUp = false;
    [SerializeField]
    public Image avatar;
    [SerializeField]
    private Sprite spritePressed;
    [SerializeField]
    private bool changeSprite = false;
    [SerializeField]
    private bool setSpriteNativeSize = false;
    [SerializeField]
    private bool changeColor = false;
    [SerializeField]
    private bool changeTransform = true;
    private Sprite spriteNormal;
    private float pressedY { get; set; }
    private Vector2 originalScale { get; set; }
    private Color avatarColor { get; set; }

    // Use this for initialization
    void Start()
    {
        if (avatar == null)
        {
            avatar = GetComponent<Image>();
        }
        spriteNormal = avatar.sprite;
        if (spritePressed == null)
        {
            spritePressed = spriteNormal;
        }
        onFingerDown.AddListener(OnFingerDown);
        onFingerUp.AddListener(OnFingerUp);
        originalScale = avatar.transform.localScale;
        //avatarColor = new Color(avatar.color.r, avatar.color.g, avatar.color.b, 1);
        draggableRoot = GetComponentInParent<ScrollRect>();
    }

    private void OnFingerDown()
    {
        //SoundController.Instance.PlaySingle(FXSound.Instance.Fx_Button1);
        avatar.rectTransform.DOKill(true);
        avatar.transform.DOKill(true);
        avatar.DOKill(true);
        pressedY = avatar.transform.localPosition.y;
        if (changeSprite)
        {
            avatar.sprite = spritePressed;
            if (setSpriteNativeSize)
            {
                avatar.SetNativeSize();
            }
        }
        //if (changeColor)
        //{
        //    avatar.DOColor(
        //        new Color(
        //            avatar.color.r * 0.8f,
        //            avatar.color.g * 0.8f,
        //            avatar.color.b * 0.8f, 1),
        //        0.1f).SetUpdate(true);
        //}
        if (changeTransform)
        {
            avatar.transform.DOLocalMoveY(-5, .1f).
                SetRelative().
                SetEase(Ease.Linear).
                SetUpdate(true);
            avatar.transform.DOScale(originalScale * 0.98f, .1f).
                SetEase(Ease.Linear).
                SetUpdate(true);
        }
        //else if(changeTransform && isUp){
        //    avatar.transform.DOLocalMoveY(5, .1f).
        //        SetRelative().
        //        SetEase(Ease.Linear).
        //        SetUpdate(true);
        //        avatar.transform.DOScale(originalScale * 0.98f, .1f).
        //        SetEase(Ease.Linear).
        //        SetUpdate(true);
        //}
    }

    private void OnFingerUp()
    {
       
        avatar.rectTransform.DOKill(true);
        avatar.transform.DOKill(true);
        avatar.DOKill(true);
        if (changeSprite)
        {
            avatar.sprite = spriteNormal;
            if (setSpriteNativeSize)
            {
                avatar.SetNativeSize();
            }
        }
        if (changeColor)
        {
            avatar.DOColor(avatarColor, 0.1f);
        }
        if (changeTransform)
        {
            avatar.transform.DOLocalMoveY(pressedY, .1f).
                SetEase(Ease.Linear).
                SetUpdate(true);
            avatar.transform.DOScale(originalScale, .1f).
                SetEase(Ease.Linear).
                SetUpdate(true);
        }
    }
   
}
