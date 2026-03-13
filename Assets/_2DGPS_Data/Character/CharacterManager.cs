using System;
using System.Collections.Generic;
using System.Collections;
using NTPackage_old.Functions;
using SimpleJSON;
using Sirenix.OdinInspector;
using UnityEngine;
using NTFunctions_old;
using GOA.Config;

namespace Rubik._2DGPS.Character
{
    using Rubik._2DGPS.DataCenter;
    using UserData;

    public class CharacterManager : LoadBehaviour
    {
        public NTDictionary<Character> CharacterDic;

        public List<CharacterLvCost> CharacterLvCosts;

        public static CharacterManager instance;
        protected override void Awake()
        {
            base.Awake();
            if (CharacterManager.instance != null)
            {
                NTLog.LogWarning("Only 1 instance allow");
                return;
            }
            CharacterManager.instance = this;
        }

        public IEnumerator LoadData()
        {
            this.LoadCharacterLvCost();
            yield return null;
        }

        public void LoadCharacterLvCost()
        {
            this.CharacterLvCosts = new List<CharacterLvCost>();
            JSONNode jdata = JSONNode.Parse(DataCenterManager.instance.GetData(DataName.CharacterLvCost));
            foreach (JSONNode item in jdata)
            {
                CharacterLvCost characterLvCost = JsonUtility.FromJson<CharacterLvCost>(item.ToString());
                this.CharacterLvCosts.Add(characterLvCost);
            }
        }

        public IEnumerator Init()
        {
            if (UserDataManager.instance.GetUserID().Length == 0)
            {
                Debug.LogWarning("Can't get userID");
            }
            else
            {
                this.CharacterDic = new NTDictionary<Character>();
                yield return this.GetCharacters();
            }
        }

        public IEnumerator GetCharacters(Action<List<Character>> callback = null)
        {
            JSONNode jdata = new JSONObject();
            jdata["userID"] = UserDataManager.instance.UserData._id;
            yield return APIManager.Instance.PostDataUrl(jdata.ToString(), SeverConfigs.GPS2D_Server + SeverConfigs._2DGPS_API_Character_GetCharacters, (data) =>
                {
                    JSONNode jdata = JSONNode.Parse(data.downloadHandler.text);
                    APIManager.Instance.BaseAPIRespone(jdata);
                    List<Character> characters = new List<Character>();
                    foreach (JSONNode item in jdata["Data"]["Update_Characters"])
                    {
                        try
                        {
                            Character character = JsonUtility.FromJson<Character>(item.ToString());
                            characters.Add(character);
                        }
                        catch (System.Exception e)
                        {
                            NTLog.LogError(e.ToString(), gameObject);
                        }
                    }
                    callback?.Invoke(characters);
                }
            );
        }

        [Button]
        public void AddRandomCharacter(Action<List<Character>> callback = null)
        {
            JSONNode jdata = new JSONObject();
            jdata["userID"] = UserDataManager.instance.UserData._id;
            StartCoroutine(APIManager.Instance.PostDataUrl(jdata.ToString(), SeverConfigs.GPS2D_Server + SeverConfigs._2DGPS_API_Character_AddRandomCharacter, (data) =>
                {
                    JSONNode jdata = JSONNode.Parse(data.downloadHandler.text);
                    APIManager.Instance.BaseAPIRespone(jdata);
                    List<Character> characters = new List<Character>();
                    foreach (JSONNode item in jdata["Data"]["Update_Characters"])
                    {
                        try
                        {
                            Character character = JsonUtility.FromJson<Character>(item.ToString());
                            characters.Add(character);
                        }
                        catch (System.Exception e)
                        {
                            NTLog.LogError(e.ToString(), gameObject);
                        }
                    }
                    callback?.Invoke(characters);
                }
            ));
        }

        [Button]
        public void UpgradeLvCharacter(string characterID, Action<(List<Character>, bool)> callback = null)
        {
            JSONNode jdata = new JSONObject();
            jdata["characterID"] = characterID;
            StartCoroutine(APIManager.Instance.PostDataUrl(jdata.ToString(), SeverConfigs.GPS2D_Server + SeverConfigs._2DGPS_API_Character_UpgradeLvCharacter, (data) =>
                {
                    JSONNode jdata = JSONNode.Parse(data.downloadHandler.text);
                    APIManager.Instance.BaseAPIRespone(jdata);
                    List<Character> characters = new List<Character>();
                    foreach (JSONNode item in jdata["Data"]["Update_Characters"])
                    {
                        try
                        {
                            Character character = JsonUtility.FromJson<Character>(item.ToString());
                            characters.Add(character);
                        }
                        catch (System.Exception e)
                        {
                            NTLog.LogError(e.ToString(), gameObject);
                        }
                    }
                    callback?.Invoke((characters, jdata["Data"]["Success"]));
                }
            ));
        }

        [Button]
        public void EquipGear(string characterID, string gearID, Action<(List<Character>, bool)> callback = null)
        {
            JSONNode jdata = new JSONObject();
            jdata["characterID"] = characterID;
            jdata["gearID"] = gearID;
            StartCoroutine(APIManager.Instance.PostDataUrl(jdata.ToString(), SeverConfigs.GPS2D_Server + SeverConfigs._2DGPS_API_Character_EquipGear, (data) =>
                {
                    JSONNode jdata = JSONNode.Parse(data.downloadHandler.text);
                    APIManager.Instance.BaseAPIRespone(jdata);
                    List<Character> characters = new List<Character>();
                    foreach (JSONNode item in jdata["Data"]["Update_Characters"])
                    {
                        try
                        {
                            Character character = JsonUtility.FromJson<Character>(item.ToString());
                            characters.Add(character);
                        }
                        catch (System.Exception e)
                        {
                            NTLog.LogError(e.ToString(), gameObject);
                        }
                    }
                    callback?.Invoke((characters, jdata["Data"]["Success"]));
                }
            ));
        }
        [Button]
        public void UnequipGear(string characterID, string gearID, Action<(List<Character>, bool)> callback = null)
        {
            JSONNode jdata = new JSONObject();
            jdata["characterID"] = characterID;
            jdata["gearID"] = gearID;
            StartCoroutine(APIManager.Instance.PostDataUrl(jdata.ToString(), SeverConfigs.GPS2D_Server + SeverConfigs._2DGPS_API_Character_EquipGear, (data) =>
                {
                    JSONNode jdata = JSONNode.Parse(data.downloadHandler.text);
                    APIManager.Instance.BaseAPIRespone(jdata);
                    List<Character> characters = new List<Character>();
                    foreach (JSONNode item in jdata["Data"]["Update_Characters"])
                    {
                        try
                        {
                            Character character = JsonUtility.FromJson<Character>(item.ToString());
                            characters.Add(character);
                        }
                        catch (System.Exception e)
                        {
                            NTLog.LogError(e.ToString(), gameObject);
                        }
                    }
                    callback?.Invoke((characters, jdata["Data"]["Success"]));
                }
            ));
        }

        public void UpdateCharacters(JSONNode jdata)
        {
            foreach (JSONNode item in jdata)
            {
                Character character = JsonUtility.FromJson<Character>(item.ToString());
                if (this.CharacterDic.Get(character._id) == null) this.CharacterDic.Add(character._id, character);
                else this.CharacterDic.Get(character._id).UpdateCharacter(character);
            }
        }

    }
}