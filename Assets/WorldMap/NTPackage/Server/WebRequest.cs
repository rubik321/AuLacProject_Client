using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace NTPackage_old.Server{
    [System.Serializable]
    public class WebRequest
    {
        public string respone;
        public IEnumerator PostWebData(string url, string json)
        {
            var request = new UnityWebRequest(url, "POST");
            byte[] bodyRaw = new System.Text.UTF8Encoding(true).GetBytes(json);
            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogWarning("Something went wrong: " + request.error);
                respone = null;
            }
            else
            {
                respone = request.downloadHandler.text;
                Debug.Log("POST successful!: "+ request.downloadHandler.text);
            }
        
        }
    }
}
