using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using Pixelplacement;
using Rubik.Battle;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine.UI;
namespace Rubik.Common
{
    public class Common : MonoBehaviour
    {
        public static void ResetContent(Transform content)
        {
            if(content == null || content.GetComponentInParent<ScrollRect>() == null) return;
            content.GetComponentInParent<ScrollRect>().vertical = false;

            DOVirtual.DelayedCall(1, () =>
            {
                content.GetComponentInParent<ScrollRect>().vertical = true;
                content.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            });
        }
        public static void ResetContentY(Transform content)
        {

            content.GetComponentInParent<ScrollRect>().vertical = false;

            DOVirtual.DelayedCall(1, () =>
            {
                content.GetComponentInParent<ScrollRect>().vertical = true;
                content.GetComponent<RectTransform>().anchoredPosition = new Vector2(content.GetComponent<RectTransform>().anchoredPosition.x,0);
            });
        }

        public static void MoveObjectToPosition(GameObject go, GameObject desPos, float speed, UnityAction callbaclk = null)
        {
            go.transform.DOKill();
            go.transform.DOMove(desPos.transform.position, speed)
               .SetEase(Ease.Linear)
               .OnComplete(() => {
                   callbaclk();
               });
        }
        public static void MoveObjectToPosition(GameObject go, Vector2 desPos, float speed, UnityAction callbaclk = null)
        {
            go.transform.DOKill();
            go.transform.DOMove(desPos, speed)
               .SetEase(Ease.Linear)
               .OnComplete(() => {
                   callbaclk();
               });
        }
        public static void MoveObjectToPositions(List<GameObject> go, List<CardCharacter> desPos, UnityAction callbaclk = null)
        {
            // for(int i = 0; i < desPos.Count; i++)
            //{
            //    LeanTween.move(go[i].gameObject, desPos[i].transform.position, .3f).setDelay(.1f)
            //  .setEase(LeanTweenType.linear)
            //  .setOnComplete(() => {
            //      callbaclk();
            //  });
            //}

        }
        public static GameObject SpawnCoin(Vector2 spawnPos, int number = 1)
        {
            GameObject go = ObjectPool.Spawn("CoinDrop", spawnPos);
            go.transform.DOMove(new Vector2(spawnPos.x + Random.Range(-.5f, .5f), spawnPos.y + Random.Range(-.5f, .5f)), .3f);
            go.transform.localScale = 0.1f * Vector2.one;
            return go;
        }

        public static HeroController SpawnCardToPosition(GameObject card, Vector3 startPos, Vector3 pos, Transform parent, int indexSlot, UnityAction callback = null, float timeDelaySpawn = 0)
        {
            GameObject go = ObjectPool.Spawn(card);
            //go.transform.position = startPos;
            go.transform.localScale = Vector3.one;
            // go.transform.eulerAngles = new Vector3(0, 0, 90);
            // go.GetComponent<SpriteRenderer>().sortingOrder = 100;
            go.transform.SetParent(parent);
            go.transform.localPosition = startPos;
            var carChar = go.GetComponent<HeroController>();
            //carChar.SetLayerMax();
            //MoveAndScale(pos, go, timeDelaySpawn);
            return go.GetComponent<HeroController>();
        }
        static void MoveAndScale(Vector3 desPos, GameObject go, float timeDelaySpawn)
        {
            //var state = StaticData.state;
            // StaticData.state = GameState.Start;
            float speed = .25f;
            //LeanTween.move(go.gameObject, desPos, speed).setDelay(timeDelaySpawn);
        }

        public static Sprite GetAvatar(string id)
        {
            return Resources.Load<Sprite>("IconCard/" + id);
        }
        public static IEnumerator CurrencyChange(Text textToDisplay, double orgNum, double desNum, float timePlay, bool DisplayZero)
        {
            float timeSinceStarted = 0f;
            while (true)
            {
                timeSinceStarted += Time.deltaTime;
                double fracJourney = (double)(timeSinceStarted / timePlay);
                if (fracJourney > 1)
                    fracJourney = 1;
                double curNum = (desNum - orgNum) * fracJourney + orgNum;
                textToDisplay.text = System.Math.Truncate(curNum).ToString();// GetFriendlyShortNumber(System.Math.Truncate(curNum));
                                                                             // If the object has arrived, stop the coroutine 
                if (curNum == desNum)
                {
                    if (desNum == 0 && !DisplayZero)
                        textToDisplay.text = "0";
                    textToDisplay.text = desNum.ToString();// GetFriendlyShortNumber(System.Math.Truncate(desNum));
                    yield break;
                }
                yield return null;
            }

        }
        public static string GetFriendlyShortNumber(double inputNumber)
        {
            string retval = "";
            if (inputNumber < 1000)
            {
                retval = inputNumber.ToString("0.##");
            }
            else if (inputNumber < 1000000)
            {
                retval = (inputNumber / 1000).ToString("0.##") + "K";
            }
            else if (inputNumber < 1000000000)
            {
                retval = (inputNumber / 1000000).ToString("0.##") + "M";
            }
            else
            {
                retval = (inputNumber / 1000000000).ToString("0.##") + "B";
            }
            return retval;
        }
        public static void MoveToPosition(Transform trans, float time, UnityAction callback = null, Spine.Unity.SkeletonAnimation anim = null)
        {
            Debug.Log("Scale : " + "ssssssss");
            if (anim != null)
            {
                anim.GetComponent<MeshRenderer>().sortingOrder = 30;
            }
            trans.DOScale(new Vector3(1.5f, 1.5f), .2f).SetEase(Ease.Linear).OnComplete(() =>
            {
                trans.DOScale(new Vector3(1.2f, 1.2f), 0.2f).SetEase(Ease.Linear).OnComplete(() =>
                {
                    trans.DOScale(new Vector3(2f, 2f), .3f).SetEase(Ease.Linear).OnComplete(() =>
                    {
                        trans.DOScale(new Vector3(1f, 1f), .3f);
                    });
                    //trans.GetComponent<Animator>().Play("Run");
                    //trans.DOMove(GamePlayController.Instance.targetPos.position, .6f).SetEase(Ease.Linear).OnComplete(() =>
                    //{
                    //    trans.gameObject.SetActive(false);
                    //    SoundController.Instance.PlaySingle(FXSound.Instance.FX_TargetClaim);
                    //    if (callback != null)
                    //        callback();
                    //    GamePlayController.Instance.CheckGameEnd();
                    //});

                });
            });
        }
        public static void DoScale(Transform go)
        {
            go.transform.localScale = Vector3.zero;
            go.DOScale(1, .3f);
        }
        public static async void IncreaseGold(List<GameObject> coinProp, Vector3 endPos, ParticleSystem coinEffect, UnityAction callback = null)
        {

            for (int i = 0; i < coinProp.Count; i++)
            {
                GameObject coin = coinProp[i];
                int temp = i;
                //Vector3 target = coin.transform.position + new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), 0);//generate a random pos
                //Pixelplacement.Tween.Position(coin.transform, target, 0.5f + i * 0.06f, 0, Pixelplacement.Tween.EaseOutStrong);
                Pixelplacement.Tween.Position(coin.transform, endPos, 0.1f + i * 0.06f + 0.1f, i * 0.06f, Pixelplacement.Tween.EaseInStrong, Pixelplacement.Tween.LoopType.None, null, () =>
                {
                    if (callback != null && temp == 0)
                        callback();
                    coin.Recycle();
                    if (coinEffect != null)
                        coinEffect.Play();
                });
            }
            float timeStamp = Time.time;
            while (Time.time - timeStamp < 1)
                await System.Threading.Tasks.Task.Yield();

            //DataController.Instance.gameData.gold += totalGold;
            //DataController.Instance.SaveData();
        }
        public static Color GetColerByIndex(int index, List<string> lsStringColor)
        {
            Color myColor = new Color();
            ColorUtility.TryParseHtmlString(lsStringColor[index], out myColor);
            return myColor;
        }

    }

}
