using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using NTPackage.Functions;
using Lean.Localization;

namespace Rubik.Tool
{
    [System.Serializable]
    public class Data
    {
        public string FileID;
        // 1zzvNsn8X5mzSelcTA616ELDnDQMJOPiY
        public string LocalPath;
        // Assets\_UI\Localization\Language\Vietnamese.txt

    }

    public class UpdateLanguageFile : NTBehaviour
    {
        public List<Data> DataList;
        public List<LeanLanguageCSV> LeanLanguageCSVList;

        [NTButton]
        public void UpdateFile()
        {
            foreach (Data item in this.DataList)
            {
                string fileUrl = "https://drive.google.com/uc?export=download&id=" + item.FileID;
                StartCoroutine(ReadTextFileFromUrl(fileUrl, item.LocalPath));
            }
        }

        IEnumerator ReadTextFileFromUrl(string fileUrl, string localPath)
        {
            // Create a UnityWebRequest to fetch the file
            UnityWebRequest request = UnityWebRequest.Get(fileUrl);

            // Wait for the request to complete
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                // Success! Get the file content as a string
                string fileContent = request.downloadHandler.text;
                // Check file exist
                if (!File.Exists(localPath))
                {
                    NTLog.LogError("File not exist: " + localPath);
                }else{
                    // You can now process the fileContent as needed
                    File.WriteAllText(localPath, fileContent);
                    Debug.Log(localPath + " update success");
                }
            }
            else
            {
                // Handle errors
                Debug.LogError("Error reading file: " + request.error);
            }
        }

        [NTButton]
        public void UpdateLeanLanguageCSV()
        {
            foreach (LeanLanguageCSV item in this.LeanLanguageCSVList)
            {
                item.LoadFromSource();
            }
        }
    }
}
