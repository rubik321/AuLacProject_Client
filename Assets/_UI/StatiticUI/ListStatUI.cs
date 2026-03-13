
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rubik.UI.Statitic
{
    public class ListStatUI : MonoBehaviour
    {
        public List<StatUI> StatUIList;

        public void SetData(long atk, long def, long spd, long hp){
            StatData statDataATK = new StatData() { TypeStat = TypeStat.ATK, Value = atk };
            StatData statDataDEF = new StatData() { TypeStat = TypeStat.DEF, Value = def };
            StatData statDataSPD = new StatData() { TypeStat = TypeStat.SPD, Value = spd };
            StatData statDataHP = new StatData() { TypeStat = TypeStat.HP, Value = hp };
            this.StatUIList[0].SetData(statDataATK);
            this.StatUIList[1].SetData(statDataDEF);
            this.StatUIList[2].SetData(statDataSPD);
            this.StatUIList[3].SetData(statDataHP);
        }

        public void ShowValue(long atk, long def, long spd, long hp){
            this.StatUIList[0].ShowValue(atk);
            this.StatUIList[1].ShowValue(def);
            this.StatUIList[2].ShowValue(spd);
            this.StatUIList[3].ShowValue(hp);
        }

        public void HideValue(){
            this.StatUIList[0].HideValue();
            this.StatUIList[1].HideValue();
            this.StatUIList[2].HideValue();
            this.StatUIList[3].HideValue();
        }
    }
}