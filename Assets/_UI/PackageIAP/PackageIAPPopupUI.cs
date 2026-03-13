using System;
using System.Collections;
using System.Collections.Generic;
using NTPackage.Functions;
using NTPackage.UI;
using UnityEngine;

namespace Rubik.Myrk.PackageIAP
{
    public class PackageIAPPopupAnim
    {
        public static string OnUI = "OnUI";
        public static string OffUI = "OffUI";
    }

    public class PackageIAPPopupUI : PopupUI
    {
        public PackageIAP PackageIAP;
        public Transform Holder;
        public Action OnPurchase;

        public Animator Animator;

        public override void OnUI(object data = null, bool isDefaultSound = true)
        {
            this.PackageIAP = data as PackageIAP;
            this.UpdateData();
            base.OnUI(data, isDefaultSound);
            if(this.Animator == null) return;
            this.Animator.Play(PackageIAPPopupAnim.OnUI);
        }

        public override void OffUI()
        {
            StartCoroutine(this.OffAnim());
        }

        public override void UpdateData(object data = null)
        {
            base.UpdateData(data);
            ObjectPoolingManager.Instance.PushChildObjectIntoPooling(this.Holder);
            PackageIAPData packageIAPData = PackageIAPManager.Instance.GetPackageIAPData(this.PackageIAP.Index);
            if (packageIAPData == null)
            {
                NTLog.LogError("PackageIAPData is null");
                this.OffUI();
                return;
            }
            PackageIAPUI packageIAPUIPrefab = PackageIAPManager.Instance.GetPackageIAPUI(packageIAPData.Index);
            if (packageIAPUIPrefab == null)
            {
                NTLog.LogError("PackageIAPUIPrefab is null");
                this.OffUI();
                return;
            }

            PackageIAPUI packageIAPUI = ObjectPoolingManager.Instance.InstantiateObject<PackageIAPUI>("PackageIAPUI" + packageIAPData.Index, packageIAPUIPrefab.transform);
            packageIAPUI.SetData(this.PackageIAP, () =>
            {
                this.OnPurchase?.Invoke();
                this.OffUI();
            }, () => this.OffUI());
            packageIAPUI.transform.SetParent(this.Holder);
            NTFunction.ResetPosition(packageIAPUI.transform);
            packageIAPUI.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
            this.Animator = packageIAPUI.Animator;
        }

        public void SetData(Action onPurchase)
        {
            this.OnPurchase = onPurchase;
        }

        public IEnumerator OffAnim()
        {
            if (this.Animator == null)
            {
                base.OffUI();
                yield break;
            }
            this.Animator.Play(PackageIAPPopupAnim.OffUI);
            AnimationClip[] clips = this.Animator.runtimeAnimatorController.animationClips;
            foreach (AnimationClip clip in clips)
            {
                if (clip.name == PackageIAPPopupAnim.OffUI)
                {
                    yield return new WaitForSeconds(clip.length);
                    break;
                }
            }
            base.OffUI();
        }
    }
}