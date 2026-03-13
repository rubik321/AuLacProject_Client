using System.Collections.Generic;
using Rubik.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class GearItem : MonoBehaviour
{
    string[] TextColor = new string[] {  "#FFFFFF", "#06BB23", "#4587F8", "#A7129E", "#D6A365", "#FFFFFF" };
    public TextMeshProUGUI itemName;
    public GameObject chooseObj, gearTxt,sellButtonObj;
    public Button pressBtn;
    public Transform gearTextContent;
    public Image gearIcon,rareIgm,rareHightlightImg,titleImg;
    public Sprite equipSprite;
    public Sprite unequipSprite;
    List<GameObject> lsGearIteam;
   
    public StarIcon Star;
    public TextMeshProUGUI TextUpgradeLv;
    public int EquipSlot;
    GearData idGear;
    public bool IsEquipped;

    public void SetButtonEquip(bool isEquipped)
    {
        this.IsEquipped = isEquipped;
        if (isEquipped)
        {
            pressBtn.GetComponent<Image>().sprite = unequipSprite;
            pressBtn.GetComponentInChildren<TextMeshProUGUI>().text = Lean.Localization.LeanLocalization.GetTranslationText("unequipe");
            sellButtonObj.SetActive(false);
        }
        else
        {
            pressBtn.GetComponent<Image>().sprite = equipSprite;
            pressBtn.GetComponentInChildren<TextMeshProUGUI>().text = Lean.Localization.LeanLocalization.GetTranslationText("equip");
            sellButtonObj.SetActive(true);
        }
    }


    public void SetUpGearItem(GearData data,string indexOfGear, string nameOfGear,int equipSlot, List<BaseDataStat> baseStats)
    {
        if (lsGearIteam != null && lsGearIteam.Count > 0)
        {
            for (int i = 0; i < lsGearIteam.Count; i++)
            {
                Destroy(lsGearIteam[i].gameObject);
            }
            lsGearIteam.Clear();
        }
        else
        {
            lsGearIteam = new List<GameObject>();
        }
        itemName.text = nameOfGear+" +"+(data.UpgradeLv > 0 ? data.UpgradeLv : "") + " Lv."+data.Level;
        Color color1;
        ColorUtility.TryParseHtmlString(TextColor[data.Rarity],out color1);
        itemName.color = color1;
        //titleImg.sprite = SpriteHelper.Instance.lsRareTitleSprites[data.Rarity];
        gearIcon.sprite = SpriteHelper.Instance.GetIconGearSprite(indexOfGear);
        EquipSlot = equipSlot;
       // rareIgm.sprite = SpriteHelper.Instance.lsRareSprites[data.Rarity];
        //rareHightlightImg.sprite = SpriteHelper.Instance.lsRareHightlightSprites[data.Rarity];
        foreach (BaseDataStat go in baseStats)
        {
            if (go.StatKey == "SPI")
                continue;
            var gogear = Instantiate(gearTxt);
            gogear.transform.SetParent(gearTextContent, false);
            lsGearIteam.Add(gogear);
            if (go.StatKey == "MIND")
            {
                gogear.GetComponent<GearTextUI>().nameTxt.text = "INT";
            }
            else
                gogear.GetComponent<GearTextUI>().nameTxt.text = go.StatKey;
            if(go.StatKey == "CRI"|| go.StatKey == "CRD"|| go.StatKey == "HIT"|| go.StatKey == "EVA")
                gogear.GetComponent<GearTextUI>().valueTxt.text = (go.StatValue * 100) + "%";
            else
            {
                gogear.GetComponent<GearTextUI>().valueTxt.text = go.StatValue.ToString();
            }
        }
        idGear = data;
        this.Star.SetData(data.Star);
    }
    public void SellButton_Onclick()
    {
        int temp = (idGear.Rarity + 1) * 50;
        Rubik.UI.HUDCanvas.Instance.ShowNotification(string.Format("Sell <color=green>{0}</color> for <color=#DECF23>{1}</color> Gold", itemName.text, temp.ToString()), "Message", null, () => {
            gameObject.SetActive(false);
            APIManager.Instance.SellGear(idGear._id,() => {
                GOA.UserData.UserData.Instance.data.Coin += temp;
                
                });
        });
       
    }
    public void SetGearRare(int rare)
    {
        //titleImg.sprite = SpriteHelper.Instance.lsRareTitleSprites[rare];
        //rareIgm.sprite = SpriteHelper.Instance.lsRareSprites[rare];
       //areHightlightImg.sprite = SpriteHelper.Instance.lsRareHightlightSprites[rare];
        Color color1;
        ColorUtility.TryParseHtmlString(TextColor[rare], out color1);
        itemName.color = color1;
    }

    public void SetData(int star, int lvUg){
        this.Star.SetData(star);
        if(lvUg == 0) this.TextUpgradeLv.text ="";
        else this.TextUpgradeLv.text = "+"+lvUg;
    }
}
