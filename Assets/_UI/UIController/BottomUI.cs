using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class BottomUI : MonoBehaviour
{
    public Transform[] lsButtons;
    float Ypos;
    private void Awake()
    {
       
    }
    IEnumerator Start()
    {
        yield return new WaitForSeconds(.1f);
        Ypos = transform.localPosition.y;
        transform.localPosition = new Vector3(transform.localPosition.x, Ypos - 300);
        foreach (Transform item in lsButtons)
        {
            item.localScale = Vector3.zero;
        }
        yield return new WaitForSeconds(.8f);
        OnShow();
    }
    void OnShow()
    {
        transform.DOLocalMoveY(Ypos, 1).OnComplete(()=> {
            int index = 0;
            foreach(Transform item in lsButtons)
            {
                item.DOScale(Vector3.one, .5f)
                .SetDelay(0.15f * index)
                .SetEase(Ease.InOutBack);
                index++;
            }
            
        });
    }
}
