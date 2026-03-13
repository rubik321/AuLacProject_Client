using System.Collections;
using System.Collections.Generic;
using Rubik.Battle;
using UnityEngine;
using Spine.Unity;

[CreateAssetMenu(fileName = "CharacterData", menuName = "ScriptableObjects/CharacterData", order = 1)]
public class BaseCharacterDataSO : ScriptableObject
{
    public SkeletonDataAsset skeAsset;
    public BaseCharacterData baseData;
    public List<EffectData> listEffect;
    public float delay = 0f;
}



