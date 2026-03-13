using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using DG.Tweening;
using Spine.Unity;
using Rubik.Config;

public enum TypeCharacter
{
    Player,
    Enemy
}
public class CardCharacter : MonoBehaviour
{
    public TypeCharacter type;
   
    //public TextMeshPro strenghtTxt;
   // public SpriteRenderer ava,typeAttack,RareBox,avaBox,effectIcon;
    public bool isPress;
    public bool isLock = false;
    public int indexSlot;
    public UnityEngine.Events.UnityAction callback;
    public SkeletonAnimation[] lsEffects;

    public int str;
    private Vector2 originalScale;
    [SerializeField] SkeletonAnimation anim;
    void Start()
    {

        anim = GetComponentInChildren<SkeletonAnimation>();
        anim.AnimationState.SetAnimation(0, AnimationConfigs.IDLE, true);
        originalScale = transform.localScale;
        // card.cardType = CardType.Melee;
    }
   
    
   
    public void SetAvaToCard()
    {
        
    }
    public void SetOff()
    {
        isLock = true;
      
    }
    public void SetOn(bool isResetState = false)
    {
        isLock = false;
       
    }
    public void SetAnimation(string animName,Vector2 pos,bool isLoop = false,UnityAction callback = null)
    {
       
        var entry =  anim.AnimationState.SetAnimation(0, animName, isLoop);
        if (callback != null)
        {
            if (animName == AnimationConfigs.SKILL_ATTACK)
            {
                lsEffects[0].GetComponent<MeshRenderer>().sortingLayerName = "Zfx";
              //  EffectController.Instance.SpawnSkeletonEffect(lsEffects[0], pos);
            }
            entry.Complete += delegate {
                callback();
            };
        }
      
    }
    public void SetAnimation(string animName, bool isLoop = false, UnityAction callback = null)
    {

        var entry = anim.AnimationState.SetAnimation(0, animName, isLoop);
        if (callback != null)
        {
           
            entry.Complete += delegate {
                callback();
            };
        }

    }
    public void TakeDame(int dame)
    {
        str -= dame;
        //strenghtTxt.text = str.ToString();
        if (str <= 0)
        {
            gameObject.SetActive(false);
        }
    }
    public void TakeDame(int dame,UnityAction callback = null)
    {
        str -= dame;
       // AssetLoader.Instance.dameNumber.Spawn(transform.position, -dame,Color.red);
        if(str < 0)
        {
            str = 0;
        }
       // strenghtTxt.text = str.ToString();
        
        if (str <= 0)
        {
            callback();
            Invoke("CharacterDeath", 1);
        }
    }
    void CharacterDeath()
    {
       // EffectController.Instance.SpawnEffect(card.CardType,card.EffectType, transform.position, true);
       // GameController.Instance.ShakeCamera();
        gameObject.SetActive(false);
        //if (GameController.Instance.CheckPlayerLose())
        //{
        //    //playerTurnTxt.text = ("Enemy win");
        //    StaticData.state = GameState.End;
        //    UIController.Instance.ShowDefeat();
        //}
    }
   
     public void SetLayerMax()
    {
        anim.GetComponent<MeshRenderer>().sortingLayerName = "Zfx";
        //typeAttack.sortingLayerName = "MovingCard";
        //RareBox.sortingLayerName = "MovingCard";
        //avaBox.sortingLayerName = "MovingCard";
        //GetComponent<SpriteRenderer>().sortingLayerName = "MovingCard";
    }
    public void SetLayerMin()
    {
        anim.GetComponent<MeshRenderer>().sortingLayerName = "Character";
        //typeAttack.sortingLayerName = "Card";
        //RareBox.sortingLayerName = "Card";
        //avaBox.sortingLayerName = "Card";
        //GetComponent<SpriteRenderer>().sortingLayerName = "Card";
    }
    public void AddSkills()
    {
        //Health health = new Health();
        //health.ThisCard = this;
       // health.HealingThisCard();
    }
    public void PlaySkills()
    {

    }
}
