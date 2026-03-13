using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;
using Rubik.Combat;
using GOA.WorldMap;
using GOA.UserData;
using GOA.ChangeName;
using Spine.Unity;
using Spine;
using Rubik.CharacterGear;

public enum MixSkin
{
    eye2,
}
public class CharacterUIController : MonoBehaviour
{
    public static CharacterUIController Instance;
    // Start is called before the first frame update
    public GameObject[] lsBottomOn, lsBottomOff;
    public GameObject bottomField,orbsObj,gearObj;
    public List<GearItem> lsHeroButton;
    StateMachine stateMachine;
    GOA.WorldMap.CameraManager camManager;
    [SerializeField] GearDetail_UI gearDetail,characterDetail;
    public CharacterGearUI charGearUI;
    public CharacterGearItemDetailUI gearItemDetaiUI;
    public GameObject CameraUI,inventoryItem,bgGo;
    public InventoryItemUI inventoryInfo;
    public ChangeNameUI changeNameUI;
    [SerializeField] GameObject CharacterPlayer;
    public SkeletonAnimation skeletonAnimation;
    public int orbIndex;
    void Awake()
    {
        Instance = this;
        stateMachine = GetComponent<StateMachine>();
        camManager = FindObjectOfType<GOA.WorldMap.CameraManager>();
        foreach (CompanionData comp in UserData.Instance.companions.Companions)
        {
            if (comp.Equiped)
            {
                UserData.Instance.SetCompanion(UserData.Instance.GetCompanionData(comp.CompCode));
            }

        }
       
       
    }
    private void OnEnable()
    {
        for(int i = 0; i < UserData.Instance.lsHeros.Count; i++)
        {
            if (UserData.Instance.lsHeros[i])
            {
                MixSkinBoby(i);
            }
        }
    }
    public void MixSkin()
    {
        AssetLoader.Instance.MixSkin(skeletonAnimation);
        //var skeleton = skeletonAnimation.Skeleton;
        //var skeletonData = skeleton.Data;
        //var mixAndMatchSkin = new Skin("Skin 1");
        //mixAndMatchSkin.AddSkin(skeletonData.FindSkin(Rubik._2DGPS.UserData.UserDataManager.instance.UserData.SkinData.Weapon));
        //mixAndMatchSkin.AddSkin(skeletonData.FindSkin(Rubik._2DGPS.UserData.UserDataManager.instance.UserData.SkinData.Eye));
        //mixAndMatchSkin.AddSkin(skeletonData.FindSkin(Rubik._2DGPS.UserData.UserDataManager.instance.UserData.SkinData.Hair));
        //mixAndMatchSkin.AddSkin(skeletonData.FindSkin(Rubik._2DGPS.UserData.UserDataManager.instance.UserData.SkinData.Scar));
        //mixAndMatchSkin.AddSkin(skeletonData.FindSkin(Rubik._2DGPS.UserData.UserDataManager.instance.UserData.SkinData.Suit));
        //skeleton.SetSkin(mixAndMatchSkin);
        //skeleton.SetSlotsToSetupPose();
    }
    public void MixSkin(int index)
    {
        //if(index == 1)
        {
            Rubik._2DGPS.UserData.UserDataManager.instance.UserData.SkinData.Hair = "Hair/hair " + index;
        }
        AssetLoader.Instance.MixSkin(skeletonAnimation);
        
    }
    public void MixSkinBoby(int index)
    {
        Rubik._2DGPS.UserData.UserDataManager.instance.UserData.SkinData.Suit = "Suit/suit " + (index+1);
        //switch (index)
        //{
        //    case 0:
        //        Rubik._2DGPS.UserData.UserDataManager.instance.UserData.SkinData.Suit = "Suit/suit " + index;
        //        break;
        //    case 1:
        //        Rubik._2DGPS.UserData.UserDataManager.instance.UserData.SkinData.Suit = "Hair/hair " + index;
        //        break;
        //    case 2:
        //        Rubik._2DGPS.UserData.UserDataManager.instance.UserData.SkinData.Suit = "Hair/hair " + index;
        //        break;
        //}
        AssetLoader.Instance.MixSkin(skeletonAnimation);

    }
    public void MixSkinBobyInHero(int index)
    {
        Rubik._2DGPS.UserData.UserDataManager.instance.UserData.SkinData.Suit = "Suit/suit " + (index);
        AssetLoader.Instance.MixSkin(skeletonAnimation);
        for (int i = 0; i < lsHeroButton.Count; i++)
        {
            lsHeroButton[i].rareHightlightImg.gameObject.SetActive(false);
        }
        lsHeroButton[index - 1].rareHightlightImg.gameObject.SetActive(true);
    }
    public void SwitchPanel(int index)
    {
        Rubik.Common.AudioHelper.AudioCtrl.Instance.Play(Rubik.Common.AudioHelper.AudioName.UI_Button_Default);
        if (index < 5)
        {
            for (int i = 0; i < lsBottomOff.Length; i++)
            {
                lsBottomOff[i].SetActive(true);
                lsBottomOn[i].SetActive(false);
            }
            lsBottomOff[index].SetActive(false);
            lsBottomOn[index].SetActive(true);
        }
        
        switch (index)
        {
            case 0:
                stateMachine.ChangeState("Character_Gear");
               // charGearUI.ButtonInventory_OnClick();
                break;
            case 1:
                stateMachine.ChangeState("Character_Gear");
               
                break;
            case 2:
                 stateMachine.ChangeState("Character_Gear");
                CharacterPlayer.SetActive(true);
              
                break;
            case 3:
                
                stateMachine.ChangeState("Character_Companion");
                break;
            case 5:
                stateMachine.ChangeState("Adventure");
                break;
        }
        //bottomField.SetActive(true);
        bgGo.SetActive(true);
        //stateMachine.EnterState(bottomField);
    }
    public void ChangeBottomField()
    {
        Rubik.Common.AudioHelper.AudioCtrl.Instance.Play(Rubik.Common.AudioHelper.AudioName.UI_Button_Back_Exit);
        for (int i = 0; i < lsBottomOff.Length; i++)
        {
            lsBottomOff[i].SetActive(true);
            lsBottomOn[i].SetActive(false);
        }
        CameraUI.SetActive(false);
        bgGo.SetActive(false);
        stateMachine.ChangeState("BottomField");
        bottomField.SetActive(false);
        GetComponentInParent<NTFunctions_old.UIManager>().MainCanvas.SetActive(true);
    }
    public void Companion_plus()
    {
        stateMachine.ChangeState("Companions");
        //bottomField.SetActive(true);
    } 
    public void Character_CompanionEquip()
    {
        stateMachine.ChangeState("Character_CompanionEquip");
       // bottomField.SetActive(true);
    }
    public void Character_ChangeOrbs()
    {
        stateMachine.ChangeState("Skill_Orbs_Detail");
        gearObj.SetActive(true);
       // bottomField.SetActive(true);
    }
    public void Character_OrbsPick()
    {
        //stateMachine.ChangeState("PickOrb");
        //gearObj.SetActive(true);
        //charGearUI.characterGear.gameObject.SetActive(false);
        //bottomField.SetActive(true);
    } 
    public void Character_Inventory()
    {
        stateMachine.ChangeState("Inventory_ItemInfo");
        gearObj.SetActive(true);
       // bottomField.SetActive(true);
      
    }
    public void Character_Inventory_Close()
    {
        SwitchPanel(0);
        gearObj.SetActive(true);
      //  bottomField.SetActive(true);

    }
    public void Character_Obrs()
    {
        SwitchPanel(1);
    }
    //public void Character_ItemDetail(int index)
    //{
    //    gearItemDetaiUI.gearIndex = index;
    //    stateMachine.ChangeState("Items_Detail");
    //    gearObj.SetActive(true);
    //    bottomField.SetActive(true);
    //}
    public void Character_GearDetail(int index)
    {
        
        Rubik.Common.AudioHelper.AudioCtrl.Instance.Play(Rubik.Common.AudioHelper.AudioName.UI_Popup_Panel_Open_Default);
        if (index != -1)
        {
            stateMachine.ChangeState("Gear_detail");
            gearDetail.SetUpAllGears(index);
        }
        else
        {
            stateMachine.ChangeState("Hero");
            characterDetail.SetUpCharacter();
        }
        gearObj.SetActive(true);
        CharacterPlayer.SetActive(false);
    }
    public void Character_Gears(int index)
    {
        SwitchPanel(index);
       // charGearUI.characterGear.gameObject.SetActive(true);
    }
   
}
