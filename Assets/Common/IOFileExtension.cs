using System;
using System.IO;
using UnityEngine;

namespace Rubik.Common
{
    public static class IOFileExtension
    {

        public static void CopyFileFromStreamingAsset(string source, string destination)
        {
            //Combine source path
            string path = Path.Combine(Application.streamingAssetsPath, source);
            FileInfo sourceInfo = new FileInfo(path);
            FileInfo fileInfo = new FileInfo(destination);
            //Create directory if not exists
            if (!fileInfo.Directory.Exists)
            {
                fileInfo.Directory.Create();
            }

            //Check extention of the file
            if (fileInfo.Extension != sourceInfo.Extension)
            {
                string fixPath =
                    $"{destination.Substring(0, destination.Length - fileInfo.Extension.Length)}{sourceInfo.Extension}";
                fileInfo = new FileInfo(fixPath);
            }
#if UNITY_ANDROID && !UNITY_EDITOR
            var loadDb = new WWW(path); 
            while (!loadDb.isDone) // CAREFUL here, for safety reasons you shouldn't let this while loop unattended, place a timer and error check
            {}  
            File.WriteAllBytes(fileInfo.FullName, loadDb.bytes);
#else
            File.Copy(path, fileInfo.FullName, true);
#endif
        }
        
        public static void CopyDirectoryFromStreamingAssets(string source, string destination)
        {
            Debug.Log("Copy resources" + "ANDROID DEBUG: CopyDirectoryFromStreamingAssets");
            string path = Path.Combine(Application.streamingAssetsPath, source);
            if (!Directory.Exists(path))
            {
                Debug.Log("Copy resources" + "ANDROID DEBUG: Directory not exists: " + path);
                return;
            }

            foreach (string file in Directory.GetFiles(path, "*", SearchOption.AllDirectories))
            {
                Debug.Log("Copy resources"+ "ANDROID DEBUG: " + file);
                if (!file.EndsWith(".meta", StringComparison.OrdinalIgnoreCase) &&
                    !file.EndsWith(".DS_Store", StringComparison.OrdinalIgnoreCase))
                {
                    string pathFixed = FixUriAndroid(file);
                    string fileName = pathFixed.Substring(path.Length + 1);
                    string fullDestination = Path.Combine(destination, fileName);
                    CopyFileFromStreamingAsset(pathFixed, fullDestination);
                }
            }
        }

        private static string FixUriAndroid(string path)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            if (path.StartsWith("/", StringComparison.OrdinalIgnoreCase))
            {
                return path.Substring(1);
            }
#endif
            return path;
        }
        
        public static string ToFolderName(string title, char spaceReplacement = '_')
        {
            string output = title.Trim();
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            bool can_add_space = false;
            for (int i = 0; i < output.Length; i++)
            {
                char c = output[i];
                if (char.IsLetterOrDigit(c) || char.IsWhiteSpace(c))
                {
                    bool space = char.IsWhiteSpace(c);
                    if (space)
                    {
                        if (can_add_space)
                        {
                            sb.Append(spaceReplacement);
                            can_add_space = false;
                        }
                    }
                    else
                    {
                        sb.Append(c);
                        can_add_space = true;
                    }
                }
            }

            output = sb.ToString();
            return output;
        }
    }
}
