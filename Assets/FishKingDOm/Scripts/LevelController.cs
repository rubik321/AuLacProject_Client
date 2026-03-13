using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;
using System;
[Serializable]
public class LevelConfigs
{
    public float speedGame;
    public float timeBetWeenTurn;
    public float timeBetWeenCharacterAttack ;
    public float timeLoadNewWave;
    public float speedRange =0.2f;

}
[Serializable]
public class CharacterModel
{
    public int ID;
    public int level;
}
[Serializable]
public class WaveData
{
    public List<CharacterModel> lsCharacters = new List<CharacterModel>();
}
[Serializable]
public class LevelData
{
    public List<WaveData> lsWaves = new List<WaveData>();
}
public class LevelController : MonoBehaviour
{
    [SerializeField] TextAsset levetAsset,levelTowerAsset,levelBossAsset;
    public List<LevelData> lsLevel =  new List<LevelData>();
    public LevelData lv,lvTower,lvBoss;
    public LevelConfigs levelConfig;
    public BaseCharacterDataSO[] Enemies;

    public static LevelController Instance;
    public List<int> lsHeroIndex = new List<int>();
    // Start is called before the first frame update
    void Awake()
    {
        // lsLevel.Add(lv);
        Instance = this;
         lv = JsonUtility.FromJson<LevelData>(levetAsset.text);
        
    }
    public BaseCharacterDataSO GetEnemyByID(int id)
    {
        foreach(BaseCharacterDataSO enemy in Enemies)
        {
            if(enemy.baseData.Index == id)
            {
                return enemy;
            }
        }
        return null;
    }
    
}
