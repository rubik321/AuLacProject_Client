using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using UnityEditor;

namespace GOA.WorldMap
{
    [CreateAssetMenu(fileName = "NewMobSO", menuName = "ScriptableObject/MobSO/New MobSO", order = 0)]
    public class MobSO : ScriptableObject
    {
        public bool FunnyCheck;
        public string Index;
        public MobTypeCode Type;
        public string Title;
        public GameObject Model;
        public Sprite Avatar;
        public BaseCharacterDataSO[] lsEnemies;

    #if UNITY_EDITOR
        public void Parse(JSONNode data){
            this.Index = data["Index"];
            this.Title = data["Title"];
            this.Model = this.LoadModel(data["ModelName"]);
            this.Avatar = this.LoadAvatar(data["IconName"]);
            this.Type = MobTypeCodeParser.FromString(data["Type"]);
        }

        public GameObject LoadModel(string name){
                string path = "Assets/Models/LOD/"+name+"/fbx/"+name+".prefab";
                Debug.LogWarning(path);
                return AssetDatabase.LoadAssetAtPath<GameObject>(path);
            }

            public Sprite LoadAvatar(string name){
                string path = "Assets/UI/Mob_Ava/"+name;
                return AssetDatabase.LoadAssetAtPath<Sprite>(path);
            }
    #endif
    }
}
