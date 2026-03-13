using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using CodeHelper;
using Pixelplacement;
using DG.Tweening;
using GOA.Config;
using NTPackage_old.EventDispatcher;
using GOA.UserData;
using GOA.WorldMap;

namespace Rubik.UI
{
    public class EndCombatUI : State
    {
        [SerializeField] GameObject popup;
        [SerializeField] GameObject bgEnd, bgVictory;
        [SerializeField] GameObject rewardElementExp;
        [SerializeField] Transform btnClaim,btnStage;
        [SerializeField] GameObject rewardElementPrefab;
        List<GameObject> rewardElements = new List<GameObject>();
        [SerializeField] TextMeshProUGUI stageTxt;
        private void Awake()
        {
            popup.SetActive(false);
        }

        private void OnEnable()
        {
            //Show(Random.Range(50, 100), new List<(string id, int amount)>()
            //        {
            //            ("potion_health", Random.Range(1, 3)),
            //            ("potion_mana", Random.Range(1, 3)),
            //            ("potion_magic", Random.Range(1, 3))
            //        });
        }

        public void Show(int exp, List<(string id, int amount, int rarity)> rewards = null)
        {
            bool isVictory = Rubik.Combat.CombatManager.Instance.GetAliveEnemies().Count == 0;
            popup.SetActive(true);
            bgVictory.SetActive(isVictory);
            bgEnd.SetActive(!isVictory);
            gameObject.SetActive(true);
            if(isVictory&&UserData.Instance.DataInCombat.TypeCombat == MobTypeCode.Dungeon)
            {
                btnClaim.gameObject.SetActive(false);
                btnStage.gameObject.SetActive(true);
            }
            else
            {
                btnClaim.gameObject.SetActive(true);
                btnStage.gameObject.SetActive(false);
            }
            if (exp > 0)
            {
                rewardElementExp.GetComponentInChildren<TMP_Text>().text = "+" + exp;
                rewardElementExp.transform.localScale = Vector3.zero;
                rewardElementExp.transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.OutBack).SetDelay(0.5f);
            }
            else
            {
                rewardElementExp.gameObject.SetActive(false);
            }
            if (rewards == null)
                rewards = new List<(string id, int amount, int rarity)>();
            for (int i = 0; i < rewards.Count; i++)
            {
                if (i >= rewardElements.Count)
                {
                    rewardElements.Add(Instantiate(rewardElementPrefab, rewardElementExp.transform.parent));
                } 

                rewardElements[i].transform.Find("Icon").GetComponent<Image>().sprite = SpriteHelper.Instance.GetSprite(rewards[i].id);
                rewardElements[i].transform.Find("Rarity").gameObject.SetActive(true);
                switch (rewards[i].rarity)
                {
                    case 0:
                        rewardElements[i].transform.Find("Rarity").GetComponent<Image>().color = Color.white;
                        break;
                    case 1:
                        rewardElements[i].transform.Find("Rarity").GetComponent<Image>().color = Color.green;
                        break;
                    case 2:
                        rewardElements[i].transform.Find("Rarity").GetComponent<Image>().color = Color.blue;
                        break;
                    case 3:
                        rewardElements[i].transform.Find("Rarity").GetComponent<Image>().color = Color.magenta;
                        break;
                    case 4:
                        rewardElements[i].transform.Find("Rarity").GetComponent<Image>().color = Color.yellow;
                        break;
                    default :
                        rewardElements[i].transform.Find("Rarity").gameObject.SetActive(false);
                        break;
                }
                rewardElements[i].GetComponentInChildren<TMP_Text>().text =  rewards[i].amount.ToString();
                rewardElements[i].transform.localScale = Vector3.zero;
                rewardElements[i].transform.DOScale(Vector3.one, 0.25f).SetDelay(0.5f * (i + 2)).SetEase(Ease.OutBack);
            }
            btnClaim.transform.localScale = Vector3.zero;
            btnClaim.transform.DOScale(Vector3.one, 0.25f).SetDelay(0.5f * (rewards.Count + 2)).SetEase(Ease.OutBack);
            EventListenerManager.instance.PostEvent(EventCode.EndCombat);
        }

        public void OnClick()
        {
            //UnityEngine.SceneManagement.SceneManager.LoadScene(Configs.WorldMap_Screen);
            Rubik.Common.AudioHelper.AudioCtrl.Instance.Play(Rubik.Common.AudioHelper.AudioName.UI_Button_Default);
            try
            {
                if(StaticData.GameMode == GameMode.Adventure)
                    bl_SceneLoaderManager.LoadScene(Configs.Adventure_Screen);
                else
                    bl_SceneLoaderManager.LoadScene(Configs.WorldMap_Screen);
                Rubik.Common.AudioHelper.AudioCtrl.Instance.Play(Rubik.Common.AudioHelper.AudioName.Test);
            }
            catch (System.Exception e)
            {
                NTPackage_old.Functions.NTLog.LogError(e.ToString(), gameObject);
            }
        }

        public void NextStage_OnClick()
        {
            DataInCombat data = new DataInCombat();
            data.TypeCombat = MobTypeCode.Dungeon;
            data.mobsInCombat = new List<MobInfo>();
            int count;
            if (UserData.Instance.DungeonInfo.Stage == 0)
            {
                count = 1;
            }
            else if(UserData.Instance.DungeonInfo.Stage % 3==0)
            {
                count = 3;
            }
            else
            {
                count = UserData.Instance.DungeonInfo.Stage % 3;
            }
            int lv;
            if (UserData.Instance.DungeonInfo.Stage == 0)
            {
                lv = 1;
            }
            else
            {
                lv = (UserData.Instance.DungeonInfo.Stage - 1) / 3 + 2;
            }
            for (int i = 0; i < count; i++)
            {
                var index = MobManager.fixedIndexRegularMobs[UnityEngine.Random.Range(0, MobManager.fixedIndexRegularMobs.Length)];
                var mobInfo = UserData.Instance.DictionaryMobInfo[index];
                mobInfo.Lv = lv;
                mobInfo.CurHp = mobInfo.HP;
                data.mobsInCombat.Add(mobInfo);
            }
            UserData.Instance.DataInCombat = data;
            try
            {
                if (StaticData.GameMode == GameMode.Adventure)
                    bl_SceneLoaderManager.LoadScene(Configs.Adventure_Screen);
                else
                    bl_SceneLoaderManager.LoadScene(Configs.WorldMap_Screen);
                Rubik.Common.AudioHelper.AudioCtrl.Instance.Play(Rubik.Common.AudioHelper.AudioName.Test);
         
            }
            catch (System.Exception e)
            {
                NTPackage_old.Functions.NTLog.LogError(e.ToString(), gameObject);
            }
            
        }
    }
}
