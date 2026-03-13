  using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NTFunctions_old;
using TMPro;

namespace GOA.UIMenu{
    using UserData;

    [System.Serializable]
    public class ClassesData{
        public bool isLevelup;
        public ClassName className;
        public float HP;
        public float MP;
        public float Str;
        public float Vit;
        public float Mind;
        public float Spirit;
        public float Dex;
        public float Speed;
        public float HIT;
        public float EVA;
        public float CRI;
        public float CRD;
        public ClassesData(){}
        public ClassesData(bool isLvUp ,float hp, float mp, float str, float vit, float mnd, float spi, float dex, float spd, float hit, float eva, float cri, float crd){
            this.HP = hp;
            this.MP = mp;
            this.Str = str;
            this.Vit = vit;
            this.Mind = mnd;
            this.Spirit = spi;
            this.Dex = dex;
            this.Speed = spd;
            this.HIT = hit;
            this.EVA = eva;
            this.CRI = cri;
            this.CRD = crd;
            this.isLevelup = isLvUp;
        }
    }
    public enum ClassName{
        mage,
        warrior
    }
    public class ClassesUI : PopupUI
    {
        public static ClassesData[] classesDatas =
        {
            new ClassesData(false,10,10,2,3,2,3,2,3,2,3,2,3),
            new ClassesData(false,20,5,3,2,3,2,3,2,3,2,3,2),
        };
        public ClassesData classesData;
        public int index = 0;

        public ItemStatUI[] itemStatUIs;

        public TextMeshProUGUI textClass;
        // public TextMeshProUGUI textClass;
        public TextMeshProUGUI textDescript;

        public override void LoadComponents()
        {
            base.LoadComponents();
            this.lvUI = new PopupLv().GetValue(transform.name);
        }

        [ContextMenu("OnUI")]
        public void OnUI(){
            if(!this.CanShow()) return;
            this.Show();
            
            this.UpdateData();
        }

        public override void UpdateData(){

            this.classesData = UserData.Instance.LevelUpData;
            this.itemStatUIs[0].text.text = "HP";
            this.itemStatUIs[0].SetData(string.Format("{0} (+{1})", UserData.Instance.characterData.HP, classesData.HP));
            this.itemStatUIs[1].text.text = "MP";
            this.itemStatUIs[1].SetData(string.Format("{0} (+{1})", UserData.Instance.characterData.MP, classesData.MP));
            this.itemStatUIs[2].text.text = "STR";
            this.itemStatUIs[2].SetData(string.Format("{0} (+{1})", UserData.Instance.characterData.Str, classesData.Str));
            this.itemStatUIs[3].text.text = "VIT";
            this.itemStatUIs[3].SetData(string.Format("{0} (+{1})", UserData.Instance.characterData.Vit, classesData.Vit));
            this.itemStatUIs[4].text.text = "MND";
            this.itemStatUIs[4].SetData(string.Format("{0} (+{1})", UserData.Instance.characterData.Mind, classesData.Mind));
            this.itemStatUIs[5].text.text = "SPI";
            this.itemStatUIs[5].SetData(string.Format("{0} (+{1})", UserData.Instance.characterData.Spirit, classesData.Spirit));
            this.itemStatUIs[6].text.text = "DEX";
            this.itemStatUIs[6].SetData(string.Format("{0} (+{1})", UserData.Instance.characterData.Dex, classesData.Dex));
            this.itemStatUIs[7].text.text = "SPD";
            this.itemStatUIs[7].SetData(string.Format("{0} (+{1})",UserData.Instance.characterData.Speed, classesData.Speed) );
            if (itemStatUIs[8] != null)
            {
                this.itemStatUIs[8].text.text = "HIT";
                this.itemStatUIs[8].SetData(classesData.HIT);
            }
            if (itemStatUIs[8] != null)
            {
                this.itemStatUIs[9].text.text = "EVA";
                this.itemStatUIs[9].SetData(classesData.EVA);
            }
            if (itemStatUIs[8] != null)
            {
                this.itemStatUIs[10].text.text = "CRI";
                this.itemStatUIs[10].SetData(classesData.CRI);
            }
            if (itemStatUIs[8] != null)
            {
                this.itemStatUIs[11].text.text = "CRD";
                this.itemStatUIs[11].SetData(classesData.CRD);
            }


            string strClass;
            if (UserData.Instance.characterData.CharType ==0){
                strClass = "WARRIOR";
                this.textClass.text = "<color=#ff9e2c>"+strClass+"</color>";
            }
            else if (UserData.Instance.characterData.CharType == 1)
            {
                strClass = "THIEF";
                this.textClass.text = "<color=#00ffba>"+strClass+"</color>";
            }
            else
            {
                strClass = "MAGE";
                this.textClass.text = "<color=#00ffba>" + strClass + "</color>";
            }
            if(this.textDescript!=null)
            this.textDescript.text = Lean.Localization.LeanLocalization.GetTranslationText("class_descript_"+this.classesData.className, "This is Mage");
        }

        public void NextClass(){
            this.index++;
            if(this.index > classesDatas.Length - 1) this.index = 0;
            this.classesData = classesDatas[index];
            this.UpdateData();
        }

        public void BackClass(){
            this.index--;
            if(this.index < 0) this.index = classesDatas.Length - 1;
            this.classesData = classesDatas[index];
            this.UpdateData();
        }
    }
}
