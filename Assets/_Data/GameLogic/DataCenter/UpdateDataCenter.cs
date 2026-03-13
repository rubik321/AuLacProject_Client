using System.Collections;
using System.IO;
using NTPackage.Functions;
using UnityEngine;
using UnityEngine.Networking;

namespace Rubik.DataCenter
{
    public class UpdateDataCenter : NTBehaviour
    {
        public string LocalPath = "";

        [NTButton]
        public void UpdateFile()
        {
            string fileUrl = "http://15.235.180.137:7180/api/2D_GPS/data_center/get_data";
            StartCoroutine(ReadTextFileFromUrl(fileUrl, this.LocalPath));
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
                }
                else
                {
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
    }
}