using DG.Tweening;
using Lean.Localization;
using NTPackage.Functions;
using NTPackage.UI;
using Rubik.Combat;
using Rubik.Common.AudioHelper;
using Rubik.ItemPlayer;
using Rubik.Myrk.Clan;
using Rubik.UI;
using Rubik.UserDataPlayer;
using Sirenix.OdinInspector;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rubik.CardPlayer
{
    public class SummonUI : PopupUI
    {
        public Text textSummon;
        public SkeletonAnimation ske;
        public GameObject heroPos, rewardPanel, itemPanel, skipButton, effectPanel, textEffect,summonAds;
        public ParticleSystem effectSummon, effectStart, starEffect;
        public GameObject[] tabsOn, updateUsers;
        public Image summon1, summon10;
        public Sprite[] summonSprs;
        public ItemDataUI[] itemRewards;
        public ItemDataUI itemReward;
        public RewardSummon rewards;
        public Coroutine coroutineCountDown, coroutineText;
        public TextMeshProUGUI txtMess, txtSummonx1, txtSummonx10,txtSummonAdsCount, txtSummonx1Ads;
        int summonType = 0;
        [SerializeField] AspectRatioFitter cover;
        [Button]
        public override void OnUI(object data = null, bool isDefaultSound = true)
        {
            base.OnUI(data, isDefaultSound);
            cover.aspectRatio = (float)Screen.width / Screen.height;
            OnTabs(0);
            textEffect.SetActive(false);
            itemReward.SetActive(false);
            heroPos.SetActive(false);
            Invoke("StartText", 1);

            txtSummonx1.text = LeanLocalization.GetTranslationText("summon", "Summon") + " x1";
            txtSummonx10.text = LeanLocalization.GetTranslationText("summon", "Summon") + " x10";
            txtSummonx1Ads.text = LeanLocalization.GetTranslationText("summon", "Summon") + " x1";
            if(CardPlayerManager.Instance.GetAmountAdvSummon(SummonType.Normal).remain>0)
                txtSummonAdsCount.text = CardPlayerManager.Instance.GetAmountAdvSummon(SummonType.Normal).remain+"/"+ CardPlayerManager.Instance.GetAmountAdvSummon(SummonType.Normal).cap;
            else
                txtSummonAdsCount.text =  "0/" + CardPlayerManager.Instance.GetAmountAdvSummon(SummonType.Normal).cap;


        }
        public void StartText()
        {
            textEffect.SetActive(true);
            ShowText(LeanLocalization.GetTranslationText("summon_start", "Let's start summoning your monster."));
        }
        public override void UpdateData(object data)
        {
            base.UpdateData(data);
            // textSummon.text = ItemDataManager.Instance.GetItem(ItemType.SummonToken).Amount.ToString();
        }

        IEnumerator StartSummon(RewardSummon cards)
        {
            AudioCtrl.Instance.Play(AudioName.UI_Button_Summon_1);
            if (CardPlayerManager.Instance.GetAmountAdvSummon(SummonType.Normal).remain > 0)
                txtSummonAdsCount.text = CardPlayerManager.Instance.GetAmountAdvSummon(SummonType.Normal).remain + "/" + CardPlayerManager.Instance.GetAmountAdvSummon(SummonType.Normal).cap;
            else
                txtSummonAdsCount.text = "0/" + CardPlayerManager.Instance.GetAmountAdvSummon(SummonType.Normal).cap;
            rewards = cards;
            effectPanel.SetActive(true);
            effectStart.Play();
            foreach (ItemDataUI card in itemRewards)
            {

                card.gameObject.SetActive(false);
            }
            int index = 0;
            foreach (CardPlayer card in cards.Cards)
            {

                itemRewards[index].gameObject.SetActive(true);
                itemRewards[index].SetData(card);
                index++;

            }
            foreach (Rubik.ItemPlayer.ItemData card in cards.Shards)
            {

                itemRewards[index].gameObject.SetActive(true);
                itemRewards[index].SetData(card);
                index++;

            }
            yield return new WaitForSeconds(1);
            AudioCtrl.Instance.Play(AudioName.UI_Button_Summon_1);
            effectSummon.Play();
            yield return new WaitForSeconds(1);
            effectStart.Stop();
            effectSummon.Stop();
            
            if (cards.Cards != null && cards.Cards.Length > 0)
            {
                foreach (CardPlayer card in cards.Cards)
                {
                    AudioCtrl.Instance.Play(AudioName.UI_Button_Summon_2);
                    ShowText(LeanLocalization.GetTranslationText("summon_reward_monster"));
                    heroPos.SetActive(true);
                    //var model = CardPlayerManager.Instance.GetCharacterByIndex((int)card.Index);
                    starEffect.gameObject.SetActive(true);
                    starEffect.Play();
                    //ske.skeletonDataAsset = model.skeAsset;
                    var data = CardPlayerManager.Instance.GetCharacterByIndex((int)card.Index);
                    ske.skeletonDataAsset = data.skeAsset;
                    ske.skeletonDataAsset.GetSkeletonData(true);
                    ske.Initialize(true);

                    AudioCtrl.Instance.Play(AudioName.UI_Button_Summon_2);
                    if (data.baseData.skinIndex > 0)
                        ske.Skeleton.SetSkin(data.baseData.skinIndex.ToString());
                    //CardPlayerManager.Instance.SetSkeletonAnimationData(ske, (int)card.Index);
                    ske.transform.localScale = Vector3.zero;
                    yield return new WaitForSeconds(.2f);
                    //ske.transform.DOMoveY(transform.position.y + 1.5f, 1).SetEase(Ease.OutBack);
                    ske.transform.DOScale(30 * Vector3.one, 1).SetEase(Ease.OutBack);
                    yield return new WaitForSeconds(1.5f);

                }
            }
                

            if (cards.Shards!=null&&cards.Shards.Length > 0)
            {
                itemReward.SetActive(true);
                starEffect.gameObject.SetActive(true);
                rewardPanel.SetActive(true);
                heroPos.SetActive(false);
                foreach (Rubik.ItemPlayer.ItemData card in cards.Shards)
                {
                    AudioCtrl.Instance.Play(AudioName.UI_Button_Summon_2);
                    ShowText(LeanLocalization.GetTranslationText("summon_reward_card"));
                    itemReward.SetData(card);
                    itemReward.transform.position = starEffect.transform.position;
                    itemReward.transform.localScale = Vector3.zero;
                    starEffect.Play();
                    yield return new WaitForSeconds(.2f);
                    itemReward.transform.DOMoveY(transform.position.y + 1.5f, 1).SetEase(Ease.OutBack);
                    itemReward.transform.DOScale(3 * Vector3.one, 1).SetEase(Ease.OutBack);
                    yield return new WaitForSeconds(1f);
                    index++;

                }

            }
            ShowText(LeanLocalization.GetTranslationText("summon_revceid_reward"));
            effectPanel.SetActive(false);
            rewardPanel.SetActive(true);
            itemReward.gameObject.SetActive(false);
            skipButton.gameObject.SetActive(false);
            heroPos.SetActive(false);
            itemPanel.SetActive(true);

        }
        public void OnClaim()
        {
            AudioCtrl.Instance.Play(AudioName.Claim_Sound);
            ShowText(LeanLocalization.GetTranslationText("summon_start"));
            rewardPanel.SetActive(false);
            itemPanel.SetActive(false);
            effectPanel.SetActive(false);
            //itemReward.gameObject.SetActive(true);
            skipButton.gameObject.SetActive(true);
            AudioCtrl.Instance.Play(AudioName.UI_Button_Default);
        }
        public void OnSkip()
        {
            AudioCtrl.Instance.Play(AudioName.UI_Button_Default);
            rewardPanel.SetActive(true);
            itemPanel.SetActive(true);
            starEffect.gameObject.SetActive(false);
            itemReward.gameObject.SetActive(false);
            skipButton.gameObject.SetActive(false);
            if (coroutineCountDown != null)
            {
                StopCoroutine(coroutineCountDown);
            }
            AudioCtrl.Instance.Play(AudioName.UI_Button_Default);
        }
        public void InfoButton()
        {
            AudioCtrl.Instance.Play(AudioName.UI_Button_Default);
            CardSummonData cardSummonData = CardPlayerManager.Instance.GetCardSummonDataBySummonType((SummonType)summonType);  
            if (cardSummonData == null) return;
            PopupManager.Instance.OnUI(PopupCode.RateItemDataUI, null, (popup) =>
            {
                RateItemDataUI rateItemDataUI = popup as RateItemDataUI;

                string title = LeanLocalization.GetTranslationText("summon", "Summon");
                switch (summonType)
                {
                    case 0:
                        title = LeanLocalization.GetTranslationText("summon_basic", "Basic Summon");
                        break;
                    case 1:
                        title = LeanLocalization.GetTranslationText("summon_pre", "Premium Summon");
                        break;
                    case 2:
                        title = LeanLocalization.GetTranslationText("summon_ultra", "Ultra Summon");
                        break;
                }

                List<ListItemRate> listItemRates = new List<ListItemRate>();
                List<string> titles = new List<string>();
                // Card
                string c_title = LeanLocalization.GetTranslationText("monster", "Monster");
                ListItemRate listCard = new ListItemRate();
                listCard.Rate = cardSummonData.CardRate;
                c_title+= " (" + NTFunction.FormatLowerNumber(listCard.Rate * 100) + "%)";
                listCard.CardPlayerIndexes = cardSummonData.ListIndex;
                listItemRates.Add(listCard);
                titles.Add(c_title);

                // Shard
                foreach (ListItemRate shardRate in cardSummonData.ShardRate)
                {
                    string s_title = LeanLocalization.GetTranslationText("shard_monster", "Monster shard");
                    ListItemRate listShard = new ListItemRate();
                    listShard.Types = shardRate.Types;
                    listShard.Amount = shardRate.Amount;
                    s_title += " x" + shardRate.Amount;
                    s_title += " (" + NTFunction.FormatLowerNumber(shardRate.Rate * 100) + "%)";
                    listShard.Rate = shardRate.Rate;
                    Debug.LogError(listShard.Types.Length);
                    listItemRates.Add(listShard);
                    titles.Add(s_title);
                }
                rateItemDataUI.SetData(listItemRates, titles, title);
                AudioCtrl.Instance.Play(AudioName.UI_Button_Default);
            });
        }
        public void OnButtonSummon()
        {
           
            AudioCtrl.Instance.Play(AudioName.UI_Button_Summon_1);
            if (UserDataManager.Instance.IsCapSlotInventoryBag())
            {
                UserDataManager.Instance.ShowNotificationCapSlotInventoryBag();
                return;
            }
            switch (summonType)
            {
                case 0:
                    if (ItemDataManager.Instance.GetItem(ItemType.NormalSummonCardTicket).Amount < 1)
                    {
                        ShowText(LeanLocalization.GetTranslationText("not_enought_ticket_basic"));
                        //HUDCanvas.Instance.ShowNotification("Not enough Summon Ticket ! ", "Message", null);
                        return;
                    }
                    break;

                case 1:
                    if (ItemDataManager.Instance.GetItem(ItemType.PremiumSummonCardTicket).Amount < 1)
                    {
                        ShowText(LeanLocalization.GetTranslationText("not_enought_ticket_pre"));
                        //HUDCanvas.Instance.ShowNotification("Not enough Summon Ticket ! ", "Message", null);
                        return;
                    }
                    break;
                case 2:
                    if (ItemDataManager.Instance.GetItem(ItemType.UltraSummonCardTicket).Amount < 1)
                    {
                        ShowText(LeanLocalization.GetTranslationText("not_enought_ticket_ult"));
                        // HUDCanvas.Instance.ShowNotification("Not enough Summon Ticket ! ", "Message", null);
                        return;
                    }

                    break;
            }
            CardPlayerManager.Instance.SummonCard((SummonType)summonType, 1, (cards) =>
            {
                StopAllCoroutines();
                ShowText(LeanLocalization.GetTranslationText("summon_start_summon_des"));
                StartCoroutine(StartSummon(cards));
                //this.UpdateData(null);
            });
        }

        public void OnButtonSummonX10()
        {
            AudioCtrl.Instance.Play(AudioName.UI_Button_Default);
            if (UserDataManager.Instance.IsCapSlotInventoryBag())
            {
                UserDataManager.Instance.ShowNotificationCapSlotInventoryBag();
                return;
            }
            switch (summonType)
            {
                case 0:
                    if (ItemDataManager.Instance.GetItem(ItemType.NormalSummonCardTicket).Amount < 10)
                    {
                        ShowText(LeanLocalization.GetTranslationText("not_enought_ticket_basic"));
                        return;
                    }
                    break;

                case 1:
                    if (ItemDataManager.Instance.GetItem(ItemType.PremiumSummonCardTicket).Amount < 10)
                    {
                        ShowText(LeanLocalization.GetTranslationText("not_enought_ticket_pre"));
                        return;
                    }
                    break;
                case 2:
                    if (ItemDataManager.Instance.GetItem(ItemType.UltraSummonCardTicket).Amount < 10)
                    {
                        ShowText(LeanLocalization.GetTranslationText("not_enought_ticket_ult"));
                        return;
                    }

                    break;
            }
            CardPlayerManager.Instance.SummonCard((SummonType)summonType, 10, (cards) =>
            {
                coroutineCountDown = StartCoroutine(StartSummon(cards));
                //this.UpdateData(null);
            });
        }
        public void WatchAdsSummon()
        {

            AudioCtrl.Instance.Play(AudioName.UI_Button_Default);
            if (UserDataManager.Instance.IsCapSlotInventoryBag())
            {
                UserDataManager.Instance.ShowNotificationCapSlotInventoryBag();
                return;
            }
            if (CardPlayerManager.Instance.GetAmountAdvSummon(SummonType.Normal).remain > 0)
            {
                LevelPlayAds.Instance.OnShowReward(() =>
                {
                    AppsFlyerManager.TrackingAds("Summon");
                    CardPlayerManager.Instance.SummonCardAdv(SummonType.Normal, 1, (cards) =>
                    {
                        coroutineCountDown = StartCoroutine(StartSummon(cards));
                        //this.UpdateData(null);
                    });
                });
            }
            else
            {
                HUDCanvas.Instance.ShowNotification("Not enough Summon Ticket ! ", "Message", null);
            }
           
        }
        public void OnTabs(int index)
        {
            AudioCtrl.Instance.Play(AudioName.UI_Button_Default);
            summonType = index;
            for (int i = 0; i < tabsOn.Length; i++)
            {
                tabsOn[i].SetActive(false);
            }
            tabsOn[index].SetActive(true);
            summon1.sprite = summonSprs[index];
            summon10.sprite = summonSprs[index];
            switch (index)
            {
                case 0:
                    ShowText(LeanLocalization.GetTranslationText("summon_basic_des", "Let's start summoning your monster."));
                    if (ItemDataManager.Instance.GetItem(ItemType.NormalSummonCardTicket).Amount >= 1)
                        summon1.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
                    else
                        summon1.GetComponentInChildren<TextMeshProUGUI>().color = Color.red;
                    if (ItemDataManager.Instance.GetItem(ItemType.NormalSummonCardTicket).Amount >= 10)
                        summon10.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
                    else
                        summon10.GetComponentInChildren<TextMeshProUGUI>().color = Color.red;
                  
                    
                        summonAds.SetActive(LevelPlayAds.Instance.IsCanShowAds());
                    break;
                case 1:
                    ShowText(LeanLocalization.GetTranslationText("summon_pre_des", "Let's start summoning your monster."));
                    if (ItemDataManager.Instance.GetItem(ItemType.PremiumSummonCardTicket).Amount >= 1)
                        summon1.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
                    else
                        summon1.GetComponentInChildren<TextMeshProUGUI>().color = Color.red;
                    if (ItemDataManager.Instance.GetItem(ItemType.PremiumSummonCardTicket).Amount >= 10)
                        summon10.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
                    else
                        summon10.GetComponentInChildren<TextMeshProUGUI>().color = Color.red;
                   
                    summonAds.SetActive(false);
                    break;
                case 2:
                    ShowText(LeanLocalization.GetTranslationText("summon_ultra_des", "Let's start summoning your monster."));
                    if (ItemDataManager.Instance.GetItem(ItemType.UltraSummonCardTicket).Amount >= 1)
                        summon1.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
                    else
                        summon1.GetComponentInChildren<TextMeshProUGUI>().color = Color.red;
                    if (ItemDataManager.Instance.GetItem(ItemType.UltraSummonCardTicket).Amount >= 10)
                        summon10.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
                    else
                        summon10.GetComponentInChildren<TextMeshProUGUI>().color = Color.red;
                 
                    summonAds.SetActive(false);
                    break;
            }

        }

        float letterDelay = 0.02f; // Thời gian trễ giữa các chữ cái


        public void ShowText(string fullText)
        {
            if (coroutineText != null)
                StopCoroutine(coroutineText);
            coroutineText = StartCoroutine(TypeText(fullText));
        }

        private IEnumerator TypeText(string textToType)
        {
            txtMess.text = "";
            foreach (char letter in textToType)
            {
                txtMess.text += letter;
                yield return new WaitForSeconds(letterDelay);
            }
        }
    }
}