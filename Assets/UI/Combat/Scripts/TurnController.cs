using System.Collections;
using System.Collections.Generic;
using Rubik.Battle;
using Rubik.BattleEngine;
using Rubik.UI;
using UnityEngine;

public class TurnController : MonoBehaviour
{
    public List<TurnElement> lsTurns;
    public Transform content;
    public GameObject stateGo;
    public List<GameObject> lsStateGo = new List<GameObject>();
    public Transform panelhero, panelEnemy;
    public NTPackage.NTDictionary<string, TurnElement> lsElement = new NTPackage.NTDictionary<string, TurnElement>();
    public void Start()
    {
        
    }
    public void SetEffect(string index, BuffDataShort[] buffs)
    {
       
        lsElement.Get(index).TurnEffectBuff(buffs);
    }
    public void SetTurn(string index) {
        //Debug.Log(index);
        foreach(TurnElement turn in lsElement.ToList())
        {
            turn.SetHightLight(false);
        }
        lsElement.Get(index).SetHightLight(true);
    }
    public void OffElement(string index)
    {
       // lsElement[index].gameObject.SetActive(false);
    }
    public void SetState(Dictionary<string, HeroController> lsHeroTurns)
    {
        for (int i = 0; i < lsTurns.Count; i++)
        {

            lsTurns[i].gameObject.SetActive(false);
        }
        int indexHero = 0,indexEnemy = 5;
        foreach (string key in lsHeroTurns.Keys)
        {
            //Debug.Log("count : " + index + "   ");
            int index = 0;
            if (lsHeroTurns[key].isEnemy)
                index = indexEnemy;
            else
                index = indexHero;
            lsElement.Add(key, lsTurns[index]);
            lsTurns[index].ID = key;
            lsTurns[index].SetCharacterAva(lsHeroTurns[key].heroIndex); //lsHeroTurns[key].avaSpr;
            lsTurns[index].SetSide(lsHeroTurns[key].isEnemy);
            lsTurns[index].SetStar(lsHeroTurns[key].star);
            lsTurns[index].SetLv(lsHeroTurns[key].lv);
           // lsTurns[index].icon. ;
            lsTurns[index].gameObject.SetActive(true);
            //if (lsHeroTurns[key].isEnemy)
            //{
            //    lsTurns[index].transform.SetParent(panelEnemy);
            //}
            //else
            //{
            //    lsTurns[index].transform.SetParent(panelhero);
            //}
            if (lsHeroTurns[key].isEnemy)
                 indexEnemy++;
            else
               indexHero++;
        }
    }
}
