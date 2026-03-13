using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace XenoWars.Battle
{
    public class ZfxGamePlayController : MonoBehaviour
    {
        [SerializeField] GameObject zfxItemPrefab;
        List<GameObject> listItemZfx;

        public void LoadZfx(Vector2 _position)
        {
            GameObject itemZfx = GetItemZfx(_position);
        }
        GameObject GetItemZfx(Vector2 _position)
        {
            GamePool.Instance.GetGameObject(zfxItemPrefab, _position, Quaternion.identity);
            return null;
        }
        GameObject CreateZfx(Vector2 _position)
        {
            GameObject item = Instantiate(zfxItemPrefab, transform);
            item.transform.position = _position;
            return item;
        }
        
    }
}