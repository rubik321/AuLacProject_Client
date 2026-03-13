using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class ItemGamePool
{
    public int numItem;
    public GameObject item;
}

public class GamePool : MonoBehaviour
{
    //public GameObject itemDiamond;
    //public GameObject itemBullet;
    public GameObject itemCoinFly;
    public GameObject effectAtkEnemy;
    public GameObject effectEnemyDie;
    public GameObject effectPlantDie;

    public GameObject effectMerge;
    public GameObject effectSpawn;
    public GameObject bulletCharacter;
    public GameObject bulletThrough;
    public GameObject bulletBossATK1;
    public GameObject bulletMulti;
    public GameObject bulletBuff;
    public GameObject bulletBuff_explode;

    public GameObject multiShoot_explode;
    public GameObject throughShoot_explode;
    public GameObject dizzy_explode;
    public GameObject bulletDizzy;
    public GameObject boomb;
    public GameObject itemCoin;
    public GameObject itemHpEnemy;
    public GameObject enemy;
    public GameObject character;
    public GameObject boss1;
    public GameObject boss2;
    public GameObject boss3;
    public GameObject boss4;
    public GameObject boss5;
    public GameObject effectDameBoss4;


    public ItemGamePool[] listItemPool;

    public static GamePool Instance;
    //private readonly Stack<GameObject> instances = new Stack<GameObject>();
    List<int> _PooledKeyList = new List<int>();
    Dictionary<int, List<GameObject>> _PooledGoDic = new Dictionary<int, List<GameObject>>();
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        CreatePoolObj();
    }
    // Update is called once per frame
    void Update()
    {

    }

    void CreatePoolObj()
    {
        for (int i = 0; i < listItemPool.Length; i++)
        {
            if (listItemPool[i].item != null)
            {
                for (int j = 0; j < listItemPool[i].numItem; j++)
                {
                    GameObject obj = GetGameObject(listItemPool[i].item, Vector3.zero, Quaternion.identity, true);
                    obj.SetActive(false);
                }
            }
        }
    }

    public GameObject GetGameObject(GameObject prefab, Vector3 position, Quaternion rotation, bool forceInstantiate = false)
    {
        if (prefab == null)
        {
            return null;
        }
        int key = prefab.GetInstanceID();

        if (_PooledKeyList.Contains(key) == false && _PooledGoDic.ContainsKey(key) == false)
        {
            _PooledKeyList.Add(key);
            _PooledGoDic.Add(key, new List<GameObject>());
        }

        List<GameObject> goList = _PooledGoDic[key];
        //List<GameObject> goList = new List<GameObject>();
        GameObject go = null;

        if (forceInstantiate == false)
        {
            for (int i = goList.Count - 1; i >= 0; i--)
            {
                go = goList[i];
                if (go == null)
                {
                    goList.Remove(go);
                    continue;
                }
                if (go.activeSelf == false)
                {
                    // Found free GameObject in object pool.
                    Transform goTransform = go.transform;
                    goTransform.position = position;
                    goTransform.rotation = rotation;
                    go.SetActive(true);
                    return go;
                }
            }
        }

        // Instantiate because there is no free GameObject in object pool.
        go = (GameObject)Instantiate(prefab, position, rotation);
        go.transform.parent = transform;
        goList.Add(go);

        return go;
    }




}
