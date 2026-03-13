using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace Rubik.Common
{
    
    public static class ImportUtilities
    {
#if UNITY_EDITOR
        public const TextureFormat PNG_TEXTURE_FORMAT = TextureFormat.RGBA32;
        public const TextureFormat JPG_TEXTURE_FORMAT = TextureFormat.RGB24;
#else

#if UNITY_ANDROID
        public const TextureFormat PNG_TEXTURE_FORMAT = TextureFormat.ETC2_RGBA8;
        public const TextureFormat JPG_TEXTURE_FORMAT = TextureFormat.ETC2_RGB;
#elif UNITY_IOS
        public const TextureFormat PNG_TEXTURE_FORMAT = TextureFormat.PVRTC_RGBA4;
        public const TextureFormat JPG_TEXTURE_FORMAT = TextureFormat.PVRTC_RGB4;
#else
        public const TextureFormat PNG_TEXTURE_FORMAT = TextureFormat.RGBA32;
        public const TextureFormat JPG_TEXTURE_FORMAT = TextureFormat.RGB24;
#endif

#endif
        public const float DefaultMipmapBias = -0.5f;
        public const bool UseMipMaps = false;
    }

    public static class ImageHelper
    {
        public static UnityEngine.Texture2D CreateTexture(string imgPath, TextureFormat textureFormat)
        {
            FileInfo fileInfo = new FileInfo(imgPath);
            if (fileInfo.Exists)
            {
                UnityEngine.Texture2D texture = null;
                byte[] data;
                data = File.ReadAllBytes(fileInfo.FullName);
                texture = new UnityEngine.Texture2D(2, 2, textureFormat, ImportUtilities.UseMipMaps);
                texture.LoadImage(data);
                texture.name = fileInfo.Name;
                return texture;
            }
            else
            {
                Debug.Log("File not exits: " + imgPath);
            }

            return null;
        }

        public static UnityEngine.Texture2D CreateTexture(string imgPath)
        {
            return CreateTexture(imgPath, ImportUtilities.PNG_TEXTURE_FORMAT);
        }

        public static Sprite CreateSprite(string imgPath)
        {
            UnityEngine.Texture2D texture2D = CreateTexture(imgPath);
            if (texture2D == null)
                return null;

            Rect rect = new Rect(0, 0, texture2D.width, texture2D.height);
            Vector2 pivot = new Vector2(0.5f, 0.5f);
            return Sprite.Create(texture2D, rect, pivot, 100);
        }
        
        public static Sprite CreateSprite(string imgPath, Vector2 pivot, float pixelsPerUnit)
        {
            UnityEngine.Texture2D texture2D = CreateTexture(imgPath);
            if (texture2D == null)
                return null;

            Rect rect = new Rect(0, 0, texture2D.width, texture2D.height);
            return Sprite.Create(texture2D, rect, pivot, pixelsPerUnit);
        }

        public static Sprite ToSprite(this UnityEngine.Texture2D texture2D)
        {
            Rect rect = new Rect(0, 0, texture2D.width, texture2D.height);
            Vector2 pivot = new Vector2(0.5f, 0.5f);
            return Sprite.Create(texture2D, rect, pivot, 100);
        }

        public static Sprite ToSprite(this UnityEngine.Texture2D texture2D, Rect rect)
        {
            Vector2 pivot = new Vector2(0.5f, 0.5f);
            return Sprite.Create(texture2D, rect, pivot, 100);
        }
        
        public static Sprite ToSprite(this UnityEngine.Texture2D texture2D, Vector4 border)
        {
            Rect rect = new Rect(0, 0, texture2D.width, texture2D.height);
            Vector2 pivot = new Vector2(0.5f, 0.5f);
            return Sprite.Create(texture2D, rect, pivot, 100, 1, SpriteMeshType.FullRect, border);
        }
        
        public static Sprite ToSprite(this UnityEngine.Texture2D texture2D, Rect rect, Vector2 pivot)
        {
            return Sprite.Create(texture2D, rect, pivot, 100);
        }

        public static UnityEngine.Texture2D ToTexture (this Sprite s, TextureFormat textureFormat = TextureFormat.RGBA32, bool mipmaps = false) {
            var spriteTexture = s.texture;
            var r = s.textureRect;
            var newTexture = new UnityEngine.Texture2D((int)r.width, (int)r.height, textureFormat, mipmaps);
            CopyTexture(spriteTexture, r, newTexture);
            return newTexture;
        }

        static UnityEngine.Texture2D GetClone (this UnityEngine.Texture2D t, TextureFormat textureFormat = TextureFormat.RGBA32, bool mipmaps = false) {
            var newTexture = new UnityEngine.Texture2D((int)t.width, (int)t.height, textureFormat, mipmaps);
            CopyTexture(t, new Rect(0, 0, t.width, t.height), newTexture);
            return newTexture;
        }

        static void CopyTexture (UnityEngine.Texture2D source, Rect sourceRect, UnityEngine.Texture2D destination) {
            Color[] pixelBuffer = source.GetPixels((int)sourceRect.x, (int)sourceRect.y, (int)sourceRect.width, (int)sourceRect.height);
            destination.SetPixels(pixelBuffer);
            destination.Apply();
            
            // if (SystemInfo.copyTextureSupport == UnityEngine.Rendering.CopyTextureSupport.None) {
            //     // GetPixels fallback for old devices.
            //     Color[] pixelBuffer = source.GetPixels((int)sourceRect.x, (int)sourceRect.y, (int)sourceRect.width, (int)sourceRect.height);
            //     destination.SetPixels(pixelBuffer);
            //     destination.Apply();
            // } else {
            //     Graphics.CopyTexture(source, 0, 0, (int)sourceRect.x, (int)sourceRect.y, (int)sourceRect.width, (int)sourceRect.height, destination, 0, 0, 0, 0);
            // }
        }
        
        
        
        
        public static async Task<Texture2D> CreateTextureAsync(string imgPath, TextureFormat textureFormat)
        {
            FileInfo fileInfo = new FileInfo(imgPath);
            
            byte[] imageBytes;
            using (FileStream fileStream = new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true))
            {
                imageBytes = new byte[fileStream.Length];
                await fileStream.ReadAsync(imageBytes, 0, (int) fileStream.Length);
            }

            try
            {
                Texture2D texture = null;
                texture = new Texture2D(2, 2, textureFormat, ImportUtilities.UseMipMaps);
                texture.LoadImage(imageBytes);
                texture.name = fileInfo.Name;
                return texture;
            }
            catch (Exception e)
            {
                return null;
            }
            
        }
        
        public static async Task<Texture2D> CreateTextureAsync(string imgPath)
        {
            FileInfo fileInfo = new FileInfo(imgPath);
            
            byte[] imageBytes;
            using (FileStream fileStream = new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true))
            {
                imageBytes = new byte[fileStream.Length];
                await fileStream.ReadAsync(imageBytes, 0, (int) fileStream.Length);
            }

            try
            {
                Texture2D texture = null;
                texture = new Texture2D(2, 2, ImportUtilities.PNG_TEXTURE_FORMAT, ImportUtilities.UseMipMaps);
                texture.LoadImage(imageBytes);
                texture.name = fileInfo.Name;
                return texture;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        
        
        public static async Task<Sprite> CreateSpriteAsync(string imgPath)
        {
            FileInfo fileInfo = new FileInfo(imgPath);
            
            byte[] imageBytes;
            using (FileStream fileStream = new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true))
            {
                imageBytes = new byte[fileStream.Length];
                await fileStream.ReadAsync(imageBytes, 0, (int) fileStream.Length);
            }

            try
            {
                Texture2D texture = null;
                texture = new Texture2D(2, 2, ImportUtilities.PNG_TEXTURE_FORMAT, ImportUtilities.UseMipMaps);
                texture.LoadImage(imageBytes);
                texture.name = fileInfo.Name;
                
                Rect rect = new Rect(0, 0, texture.width, texture.height);
                Vector2 pivot = new Vector2(0.5f, 0.5f);
                return Sprite.Create(texture, rect, pivot, 100);
            }
            catch (Exception e)
            {
                return null;
            }
        }
        
        public static async Task<Sprite> CreateSpriteAsync(string imgPath, Vector2 pivot, float pixelsPerUnit)
        {
            FileInfo fileInfo = new FileInfo(imgPath);
            
            byte[] imageBytes;
            using (FileStream fileStream = new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true))
            {
                imageBytes = new byte[fileStream.Length];
                await fileStream.ReadAsync(imageBytes, 0, (int) fileStream.Length);
            }

            try
            {
                Texture2D texture = null;
                texture = new Texture2D(2, 2, ImportUtilities.PNG_TEXTURE_FORMAT, ImportUtilities.UseMipMaps);
                texture.LoadImage(imageBytes);
                texture.name = fileInfo.Name;
                
                Rect rect = new Rect(0, 0, texture.width, texture.height);
                return Sprite.Create(texture, rect, pivot, pixelsPerUnit);
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
