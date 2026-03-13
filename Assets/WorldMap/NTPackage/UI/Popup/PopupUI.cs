using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

namespace NTFunctions_old
{   
    public enum KindPopup{
        none = 0,
        oneStep = 1,
        twoStep = 2,
        threeStep = 3,
    }

    public class PopupUI : LoadBehaviour
    {
        public PopupCode popupCode = PopupCode.Unknown;
        public float lvUI;
        public Transform transPanel;

        public KindPopup show = KindPopup.none;
        public KindPopup hide = KindPopup.none;

        public override void LoadComponents()
        {
            base.LoadComponents();
            this.LoadTransPanel();
            this.popupCode = PopupCodeParser.FromString(transform.name);
            this.lvUI = new PopupLv().GetValue(transform.name);
        }

        protected void LoadTransPanel(){
            if(this.transPanel != null) return;
            try
            {
                this.transPanel = transform.Find("Panel");
            }
            catch (System.Exception)
            {
                Debug.LogWarning("Can't LoadImageItem");
            }
            
        }
        //Function

        public virtual void  OffUI(){
            Debug.LogWarning("Off");
            this.Hide();
        }

        public virtual void  UpdateData(){
        }

        protected virtual bool CanShow(){
            return this.lvUI > UIManager.instance.GetCurrentLvUI();
        }

        public virtual void  Show(){
            this.UpdateData();
            this.ShowSound();
            switch (show)
            {
                case KindPopup.oneStep:
                    this.ShowOneDG();
                    break;
                case KindPopup.twoStep:
                    this.ShowDoubleDG();
                    break;
                case KindPopup.threeStep:
                    this.ShowTrippleDG();
                    break;     
                default:
                    this.ShowNone();
                    break;
            }
        }

        protected virtual void ShowSound(){
       //     Rubik.Common.AudioHelper.AudioCtrl.instance.Play(Rubik.Common.AudioHelper.AudioName.UI_Popup_Panel_Default);
        }

        public virtual void ShowNone(){
            transform.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
            this.transPanel.localScale = new Vector3(1,1,1);
            this.transPanel.gameObject.SetActive(true);
        }
        public virtual void ShowOneDG(){
            transform.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
            this.transPanel.localScale = new Vector3(0,0,0);
            this.transPanel.gameObject.SetActive(true);
            this.transPanel.DOScale(new Vector3(1f,1f,1f), 0.3f);
        }

        public virtual void ShowDoubleDG(){
            transform.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
            this.transPanel.localScale = new Vector3(0,0,0);
            this.transPanel.gameObject.SetActive(true);
            this.transPanel.DOScale(new Vector3(1.1f,1.1f,1.1f), 0.2f).OnComplete(()=>{
                this.transPanel.DOScale(new Vector3(1f,1f,1f), 0.1f);
            });
        }

        public virtual void ShowTrippleDG(){
            transform.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
            this.transPanel.localScale = new Vector3(0,0,0);
            this.transPanel.gameObject.SetActive(true);
            this.transPanel.DOScale(new Vector3(1.1f,1.1f,1.1f), 0.2f).OnComplete(()=>{
                this.transPanel.DOScale(new Vector3(0.9f,0.9f,0.9f), 0.06f).OnComplete(()=>{
                    this.transPanel.DOScale(new Vector3(1.1f,1.1f,1.1f), 0.04f);
                });
            });
        }

        public virtual void  Hide(){
            try
            {
                this.HideSound();   
            }
            catch (System.Exception){}
            switch (hide)
            {
                case KindPopup.oneStep:
                    this.HideOneDG();
                    break;
                case KindPopup.twoStep:
                    this.HideDoubleDG();
                    break;
                default:
                    this.HideNone();
                    break;
            }
        }

        protected virtual void HideSound(){
            Rubik.Common.AudioHelper.AudioCtrl.Instance.Play(Rubik.Common.AudioHelper.AudioName.UI_Button_Back_Exit);
        }

        public virtual void HideNone(){
            this.transPanel.gameObject.SetActive(false);
            transform.GetComponent<RectTransform>().localScale = new Vector3(0,0,0);
            this.transPanel.localScale = new Vector3(0,0,0);
        }

        public virtual void HideOneDG(){
            this.transPanel.DOScale(new Vector3(0f,0f,0f), 0.25f).OnComplete(()=>{
                this.transPanel.gameObject.SetActive(false);
                transform.GetComponent<RectTransform>().localScale = new Vector3(0,0,0);
            });
        }

        public virtual void HideDoubleDG(){
            this.transPanel.localScale = new Vector3(0,0,0);
            this.transPanel.gameObject.SetActive(true);
            this.transPanel.DOScale(new Vector3(1.1f,1.1f,1.1f), 0.075f).OnComplete(()=>{
                this.transPanel.DOScale(new Vector3(0f,0f,0f), 0.15f).OnComplete(()=>{
                    this.transPanel.gameObject.SetActive(false);
                    transform.GetComponent<RectTransform>().localScale = new Vector3(0,0,0);
                });
            });
        }

        public bool IsShow(){
            return this.transPanel.gameObject.activeSelf;
        }

        public void Home(){
            UIManager.instance.OffAllUI();
        }
    }
}
