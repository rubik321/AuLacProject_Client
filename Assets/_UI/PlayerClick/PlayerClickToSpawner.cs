using System;
using UnityEngine;
using UnityEngine.EventSystems;
using NTPackage.Functions;
using System.Collections;

namespace Rubik.Myrk.PlayerClick
{
    public class PlayerClickToSpawner : NTBehaviour
    {
        public RectTransform Holder;
    public Transform PlayerClickEffectPrefab;
    public Vector3 Offset;

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            this.Offset = Input.mousePosition;
        }

        // Only click allowed
        if (Input.GetMouseButtonUp(0))
        {
            // Get the local canvas position from screen click
            Vector2 pos = Input.mousePosition;
            if(Vector2.Distance(this.Offset, pos) > 10)
            {
                return;
            }
            transform.SetAsLastSibling();
            // RectTransformUtility.ScreenPointToLocalPointInRectangle(
            //     Holder, Input.mousePosition, null, out pos);

            Transform playerClickEffect = ObjectPoolingManager.Instance.InstantiateObject<Transform>(ObjectPoolingConfig.PlayerClickEffect, PlayerClickEffectPrefab);
            playerClickEffect.SetParent(Holder);
            playerClickEffect.transform.position = pos;
            StartCoroutine(PushPlayerClickEffectToPooling(playerClickEffect));
        }
    }
    public IEnumerator PushPlayerClickEffectToPooling(Transform playerClickEffect)
    {
        yield return new WaitForSeconds(1f);
        ObjectPoolingManager.Instance.PushObjectIntoPooling(playerClickEffect);
    }
    }

    
}
