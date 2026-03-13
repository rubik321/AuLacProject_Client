using System.Collections;
using NTPackage.UI;
using TMPro;
using UnityEngine;

namespace Rubik.Myrk.Guide
{
    public class GuideAnim
    {
        public const string OnUI = "OnUI";
        public const string OffUI = "OffUI";
    }


    public class GuideUI : PopupUI
    {
        public TextMeshProUGUI GuideTitle;
        public TextMeshProUGUI GuideDesc;
        public Animator Animator;

        public override void OnUI(object data = null, bool isDefaultSound = true)
        {
            base.OnUI(data, isDefaultSound);
            this.Animator.Play(GuideAnim.OnUI);
        }

        public override void ScriptOffUI()
        {
            if (this.ScreenDim != null) this.ScreenDim.gameObject.SetActive(false);
            this.Animator.Play(GuideAnim.OffUI);
            StartCoroutine(this.OffGuideAnim());
        }

        public void SetData(string title, string desc)
        {
            this.GuideTitle.text = title;
            this.GuideDesc.text = desc;
        }

        public IEnumerator OffGuideAnim()
        {
            AnimationClip[] clips = this.Animator.runtimeAnimatorController.animationClips;
            foreach (AnimationClip clip in clips)
            {
                if (clip.name == GuideAnim.OffUI)
                {
                    yield return new WaitForSeconds(clip.length);
                    break;
                }
            }
            base.ScriptOffUI();
        }

    }
}