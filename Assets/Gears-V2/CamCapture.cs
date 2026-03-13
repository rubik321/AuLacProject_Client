using System.Collections;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using UnityEngine;
using TMPro;
using NTPackage.Functions;
namespace Rubik.Axie
{
    public class CamCapture : NTBehaviour
    {
        public string NamePNG = "default";

        public TMP_InputField txtName;
        [NTButton]
        public void Capture()
        {
            Camera Cam = GetComponent<Camera>();

            RenderTexture currentRT = RenderTexture.active;
            RenderTexture.active = Cam.targetTexture;

            Cam.Render();

            Texture2D Image = new Texture2D(Cam.targetTexture.width, Cam.targetTexture.height);
            Image.ReadPixels(new Rect(0, 0, Cam.targetTexture.width, Cam.targetTexture.height), 0, 0);
            Image.Apply();
            RenderTexture.active = currentRT;

            var Bytes = Image.EncodeToPNG();
            Destroy(Image);

            File.WriteAllBytes(Application.dataPath + "/CamCapture/"+NamePNG+".png", Bytes);
        }

        public void CapturePic()
        {
            Camera Cam = GetComponent<Camera>();

            RenderTexture currentRT = RenderTexture.active;
            RenderTexture.active = Cam.targetTexture;

            Cam.Render();

            Texture2D Image = new Texture2D(Cam.targetTexture.width, Cam.targetTexture.height);
            Image.ReadPixels(new Rect(0, 0, Cam.targetTexture.width, Cam.targetTexture.height), 0, 0);
            Image.Apply();
            RenderTexture.active = currentRT;

            var Bytes = Image.EncodeToPNG();
            Destroy(Image);

            File.WriteAllBytes(Application.dataPath + "/CamCapture/" + txtName.text.Trim() + ".png", Bytes);
        }
    }
}

