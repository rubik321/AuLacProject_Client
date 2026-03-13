using System;
using UnityEditor;
using UnityEngine;

namespace Rubik.Common
{
#if UNITY_EDITOR
    public static class LoadAssetHelper
    {
        public static TextAsset LoadWordSyncText(string cmsPath, string filename)
        {
            try
            {
                var a = AssetDatabase.LoadAssetAtPath<TextAsset>($"{cmsPath}/word/_sync/{filename}.txt");
                if (a == null)
                {
                    Debug.Log("Load word sync text fail: " + filename);
                }

                return a;
            }
            catch (Exception e)
            {
                Debug.Log("Load word sync text error: " + filename);
                return null;
            }
        }

        public static TextAsset LoadSentenceSyncText(string cmsPath, string filename)
        {
            try
            {
                var a = AssetDatabase.LoadAssetAtPath<TextAsset>($"{cmsPath}/sentence/_sync/{filename}.txt");
                if (a == null)
                {
                    Debug.Log("Load sentence sync text fail: " + filename);
                }

                return a;
            }
            catch (Exception e)
            {
                Debug.Log("Load sentence sync text error: " + filename);
                return null;
            }
        }
        


        public static Sprite LoadPicture(string cmsPath, string filename)
        {
            try
            {
                var a = AssetDatabase.LoadAssetAtPath<Sprite>($"{cmsPath}/picture/{filename}.png");
                if (a == null)
                {
                    Debug.Log("Load picture fail: " + filename);
                }

                return a;
            }
            catch (Exception e)
            {
                Debug.Log("Load picture error: " + filename);
                return null;
            }
        }

        public static Sprite LoadImage(string cmsPath, string filename)
        {
            try
            {
                var a = AssetDatabase.LoadAssetAtPath<Sprite>($"{cmsPath}/image/{filename}.jpg");
                if (a == null)
                {
                    Debug.Log("Load image fail: " + filename);
                }

                return a;
            }
            catch (Exception e)
            {
                Debug.Log("Load image error: " + filename);
                return null;
            }
        }


        public static Sprite LoadPhoto(string cmsPath, string filename)
        {
            try
            {
                var a = AssetDatabase.LoadAssetAtPath<Sprite>($"{cmsPath}/photo/{filename}.jpg");
                if (a == null)
                {
                    Debug.Log("Load photo fail: " + filename);
                }

                return a;
            }
            catch (Exception e)
            {
                Debug.Log("Load photo error: " + filename);
                return null;
            }
        }

        public static AudioClip LoadSound(string cmsPath, string filename)
        {
            try
            {
                var a = AssetDatabase.LoadAssetAtPath<AudioClip>($"{cmsPath}/sound/{filename}.mp3");
                if (a == null)
                {
                    Debug.Log("Load sound fail: " + filename);
                }

                return a;
            }
            catch (Exception e)
            {
                Debug.Log("Load sound error: " + filename);
                return null;
            }
        }

        public static AudioClip LoadWordAudio(string cmsPath, string filename)
        {
            try
            {
                var a = AssetDatabase.LoadAssetAtPath<AudioClip>($"{cmsPath}/word/{filename}.mp3");
                if (a == null)
                {
                    Debug.Log("Load word audio fail: " + filename);
                }

                return a;
            }
            catch (Exception e)
            {
                Debug.Log("Load word audio error: " + filename);
                return null;
            }
        }

        public static AudioClip LoadSentenceAudio(string cmsPath, string filename)
        {
            try
            {
                var a = AssetDatabase.LoadAssetAtPath<AudioClip>($"{cmsPath}/sentence/{filename}.mp3");
                if (a == null)
                {
                    Debug.Log("Load sentence audio fail: " + filename);
                }

                return a;
            }
            catch (Exception e)
            {
                Debug.Log("Load sentence audio error: " + filename);
                return null;
            }
        }


        #region Book

        public static Sprite LoadBookImage(string cmsPath, string book, string page)
        {
            try
            {
                var a = AssetDatabase.LoadAssetAtPath<Sprite>($"{cmsPath}/res/{book}/{page}.png");
                if (a == null)
                {
                    Debug.Log($"Load picture fail: {book} - {page}");
                }

                return a;
            }
            catch (Exception e)
            {
                Debug.Log($"Load picture error: {book} - {page}");
                return null;
            }
        }
        
        public static Sprite LoadBookThumb(string cmsPath, string book)
        {
            try
            {
                var a = AssetDatabase.LoadAssetAtPath<Sprite>($"{cmsPath}/res/{book}/thumb.jpg");
                if (a == null)
                {
                    Debug.Log($"Load thumb fail: {book}");
                }

                return a;
            }
            catch (Exception e)
            {
                Debug.Log($"Load thumb error: {book}");
                return null;
            }
        }
        
        public static AudioClip LoadBookVoice(string cmsPath, string book, string page)
        {
            try
            {
                var a = AssetDatabase.LoadAssetAtPath<AudioClip>($"{cmsPath}/res/{book}/{page}.mp3");
                if (a == null)
                {
                    Debug.Log("Load voice fail: " + book + " at page: " + page);
                }

                return a;
            }
            catch (Exception e)
            {
                Debug.Log("Load voice error: " + book + " at page: " + page);
                return null;
            }
        }
        
        
        public static TextAsset LoadBookSyncText(string cmsPath, string book, string page)
        {
            try
            {
                var a = AssetDatabase.LoadAssetAtPath<TextAsset>($"{cmsPath}/res/{book}/{page}.txt");
                if (a == null)
                {
                    Debug.Log("Load sync text fail: " + book + " at page: " + page);
                }

                return a;
            }
            catch (Exception e)
            {
                Debug.Log("Load sync text error: "  + book + " at page: " + page);
                return null;
            }
        }

        #endregion

    }
#endif

}
