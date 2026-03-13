using NTPackage.UI;
using Rubik.CardPlayer;
using UnityEngine;

namespace Rubik.CardPlayer
{
public class BtnMonsterStoryUI : NTButtonEffect
{
    public CardPlayerIndex Index;
    public int Star;
    public int Level;

    public void _OnClick(){
        CardPlayerManager.Instance.ShowPopupStory(this.Index, this.Star, this.Level);
        }
    }
}
