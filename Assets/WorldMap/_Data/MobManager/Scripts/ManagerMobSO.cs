using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using UnityEditor;

namespace GOA.WorldMap
{

    [CreateAssetMenu(fileName = "ManagerMobSO", menuName = "ScriptableObject/MobSO/Manager MobSO", order = 0)]
    public class ManagerMobSO : MonoBehaviour
    {
        public TextAsset dataMob;
        #if UNITY_EDITOR
            [ContextMenu("Load")]
            public void Load(){
                JSONNode data = JSON.Parse(this.dataMob.text);
                int count = data.Count;
                // count = 1;
                for (int i = 0; i < count; i++)
                {
                    string path = "Assets/WorldMap/_Data/MobManager/Resources/MobSO/"+data[i]["Index"]+".asset";
                    MobSO mobSO = AssetDatabase.LoadAssetAtPath<MobSO>(path);
                    mobSO = ScriptableObject.CreateInstance<MobSO>();
                    mobSO.Parse(data[i]);
                    AssetDatabase.CreateAsset(mobSO, path);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                    EditorUtility.FocusProjectWindow();
                    Selection.activeObject = mobSO;
                }
            }
        #endif
    }
}
