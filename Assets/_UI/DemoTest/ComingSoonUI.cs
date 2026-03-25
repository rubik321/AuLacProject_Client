using UnityEngine;
using MNP;
using MNP.MNPopup;
using Rubik.Common.AudioHelper;
public class ComingSoonUI : MNPopup
{
    public override void Show()
    {
        AudioCtrl.Instance.Play(AudioName.UI_Button_Default);
        base.Show();
    }
    public override void Hide()
    {
        AudioCtrl.Instance.Play(AudioName.UI_Button_Default);
        base.Hide();
    }
}
