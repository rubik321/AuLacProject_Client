using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Spine.Unity;
using Rubik.Combat;
using DG.Tweening;
using NTPackage.UI;
using Rubik.ItemPlayer;
using System.Linq;
using Sirenix.OdinInspector;
using Spine.Unity;
using TMPro;
using DG.Tweening;
namespace Rubik.CardPlayer
{
    public class ShardUI : PopupUI
    {
        public Transform monsterPos;
        public GameObject combineShard, mergeShard,claimButton,nameGo,arrowGo;
        public SkeletonAnimation charAnim;
        public ItemDataBarUI shard;
        public UserItemDataBarUI shardBar;
        public TextMeshProUGUI nameMonster;
        public Image origin;
        public ItemDataUI itemData1, itemData2;
        public TextMeshProUGUI numberTxt;
        CardPlayerIndex monsterIndex;
        [SerializeField]ItemPlayer.ItemData itemData;
        [SerializeField] ParticleSystem effectStart,effectSummon,waterEffect;
        public override void OnUI(object data = null, bool isDefaultSound = true)
        {
            base.OnUI(data, isDefaultSound);
            itemData = (Rubik.ItemPlayer.ItemData)data;
            SetCombine();
            //SetMerge();
        }
        void SetCombine()
        {
            claimButton.SetActive(false);
            combineShard.gameObject.SetActive(true);
            mergeShard.gameObject.SetActive(false);
            charAnim.gameObject.SetActive(true);
            combineShard.GetComponent<Button>().interactable = ItemDataManager.Instance.CanCombineItem(itemData.Type);
            SpawnMonster();
            
        }
        public override void UpdateData(object data = null)
        {
            base.UpdateData(data);

        }
        public override void ScriptOffUI()
        {
            base.ScriptOffUI();
        }
        public void SpawnMonster()
        {
            monsterIndex = ItemDataManager.Instance.GetCombineResult(itemData.Type);
            shard.SetData(ItemDataManager.Instance.GetCombineRequire(itemData.Type)[0]);
            shardBar.SetData(itemData.Type);
          
            charAnim.skeletonDataAsset = CardPlayerManager.Instance.GetCharacterByIndex((int)monsterIndex).skeAsset;
            charAnim.transform.localScale = 200*CardPlayerManager.Instance.GetCharacterByIndex((int)monsterIndex).baseData.ScaleData;
            charAnim.Initialize(true);
            nameMonster.text = CardPlayerManager.Instance.GetCardName(monsterIndex);
            origin.sprite = CardPlayerManager.Instance.GetOriginSpriteCircle(CardPlayerManager.Instance.GetCardPlayerDataByIndex(monsterIndex).Origin);
        }
        Vector3 scaleMosnter;
        public void OnButtonCombine()
        {
            StartCoroutine(CardPlayerManager.Instance.IECombineCardShard(itemData.Type, (mosters)=> {
                CardPlayer card = mosters[0];
                scaleMosnter = charAnim.transform.localScale;
                charAnim.transform.localScale = Vector3.zero;
                combineShard.gameObject.SetActive(false);
                nameGo.SetActive(false);
                shardBar.SetData(itemData.Type);
                StartCoroutine(StartSummon());
            }));
        }
        IEnumerator StartSummon()
        {
            waterEffect.Stop();
            effectStart.Play();
            yield return new WaitForSeconds(1.5f);
            effectStart.Stop();
            yield return new WaitForSeconds(.5f);
            effectSummon.Play();
            yield return new WaitForSeconds(2f);
            effectSummon.Stop();
            charAnim.transform.DOScale(scaleMosnter, 0.5f);
            yield return new WaitForSeconds(1f);
            nameGo.SetActive(true);
            claimButton.SetActive(true);
            waterEffect.Play();
        }
        public void SetMerge()
        {
            claimButton.SetActive(false);
            combineShard.gameObject.SetActive(false);
            mergeShard.gameObject.SetActive(true);
            charAnim.gameObject.SetActive(false);
            itemData1.SetData(ItemDataManager.Instance.GetMergeRequire(itemData.Type)[0]);
            itemData2.SetData(ItemDataManager.Instance.GetMergeResult(itemData.Type));
            floatYoffset = itemData1.transform.localPosition;
            itemData1.transform.DOLocalMoveY(floatYoffset.y + 30, 0.5f).SetLoops(-1, LoopType.Yoyo);
            itemData2.transform.DOLocalMoveY(floatYoffset.y + 30, 0.5f).SetLoops(-1, LoopType.Yoyo);
            shardBar.SetData(itemData.Type);
            //combineShard.GetComponent<Button>().interactable = ItemDataManager.Instance.CanCombineItem(itemData.Type);
        }
        Vector2 floatYoffset;
        public void Plus()
        {

        }
        public void OnButtonMerge()
        {
            StartCoroutine(CardPlayerManager.Instance.IEMergeCardShard(itemData.Type, (items) => {
                // ItemData card = items[0];
                mergeShard.gameObject.SetActive(false);
                StartCoroutine(Startmerge());
            }));
        }
        IEnumerator Startmerge()
        {
            itemData2.gameObject.SetActive(false);

            arrowGo.gameObject.SetActive(false);
            itemData1.transform.DOKill();
            itemData1.transform.DOLocalMove(Vector2.zero,0.5f);
            itemData1.transform.DOScale(itemData1.transform.localScale * 1.5f, 0.5f);
            yield return new WaitForSeconds(1f);
            effectStart.Play();
            itemData1.gameObject.SetActive(false);
            yield return new WaitForSeconds(1.5f);
            effectStart.Stop();
            itemData2.transform.localScale = Vector2.zero;
            itemData2.gameObject.SetActive(true);
            itemData2.transform.DOScale(itemData2.transform.localScale * 1.5f, 0.5f);
            yield return new WaitForSeconds(1f);
            nameGo.SetActive(true);
            claimButton.SetActive(true);
            waterEffect.Play();
        }
    }
}