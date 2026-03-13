using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace NTPackage.Functions
{
    public class NTFunction : NTBehaviour
    {
        public virtual void CheckNull(object variable)
        {
            if (variable == null)
            {
                Debug.LogWarning("Cant load: " + variable.ToString());
            }
        }

        public static void ClearChild(Transform trans)
        {
            int count = trans.childCount;
            for (int i = count - 1; i >= 0; i--)
            {
                Transform child = trans.GetChild(i);
                child.gameObject.SetActive(false);
                DestroyImmediate(child.gameObject);
            }
        }

        public static void SetActiveFalse(Transform trans)
        {
            foreach (Transform transChild in trans)
            {
                transChild.gameObject.SetActive(false);
            }
        }

        public static void ResetPosition(Transform trans)
        {
            trans.localPosition = new Vector3(0, 0, 0);
            trans.localRotation = Quaternion.identity;
            trans.localScale = new Vector3(1, 1, 1);
        }

        #region String
        public static string CollapString(string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength) + "...";
        }

        public static string FormatNumber(double num)
        {
            string[] suffix = { "", "K", "M", "B", "T", "Q" };
            int index = 0;
            while (num >= 1000.0f && index < suffix.Length - 1)
            {
                num /= 1000.0f;
                index++;
            }
            return $"{num:G3}{suffix[index]}";
        }

        public static string FormatNumberWithComa(long num)
        {
            return num.ToString("N0");
        }

        [NTButton]
        public static string FormatLowerNumber(float num)
        {
            if (num >= 1000) return num.ToString("F0");
            if (num >= 100)
            {
                if (((int)(num * 10 + 0.1f) % 10) > 0)
                {

                    return num.ToString("F1");
                }
                else
                {
                    return num.ToString("F0");
                }
            }
            if (num >= 10)
            {
                if (((int)(num * 100 + 0.1f) % 10) > 0)
                {
                    return num.ToString("F2");
                }
                else if (((int)(num * 10 + 0.1f) % 10) > 0)
                {
                    return num.ToString("F1");
                }
                else
                {
                    return num.ToString("F0");
                }
            }
            if (num >= 1)
            {
                if (((int)(num * 1000 + 0.1f) % 10) > 0)
                {
                    return num.ToString("F3");
                }
                else if (((int)(num * 100 + 0.1f) % 10) > 0)
                {
                    return num.ToString("F2");
                }
                else if (((int)(num * 10 + 0.1f) % 10) > 0)
                {
                    return num.ToString("F1");
                }
                else
                {
                    return num.ToString("F0");
                }
            }
            else
            {
                if (((int)(num * 10000 + 0.1f) % 10) > 0)
                {
                    return num.ToString("F4");
                }
                else if (((int)(num * 1000 + 0.1f) % 10) > 0)
                {
                    return num.ToString("F3");
                }
                else if (((int)(num * 100 + 0.1f) % 10) > 0)
                {
                    return num.ToString("F2");
                }
                else if (((int)(num * 10 + 0.1f) % 10) > 0)
                {
                    return num.ToString("F1");
                }
                else
                {
                    return "<0.0001";
                }
            }

        }

        // 999 -> 999, 9999 -> 9.99K, 99999 -> 99.9K, 999999 -> 999K, 9999999 -> 9.99M
        public static string FormatHigherNumber(double num){
            string result = "";
            string suffix = "";
            if(num < 1000){
                result = num.ToString("F0");
                suffix = "";
            }   
            else if(num < 10000){
                result = ( Math.Floor(num / 10) / 100 ).ToString("F2");
                suffix = "K";
            }else if(num < 100000){
                result = ( Math.Floor(num / 100) / 10 ).ToString("F1");
                suffix = "K";
            }else if(num < 1000000){
                result = ( Math.Floor(num / 1000) ).ToString("F0");
                suffix = "K";
            }else if(num < 10000000){
                result = ( Math.Floor(num / 10000) / 100 ).ToString("F2");
                suffix = "M";
            }else if(num < 100000000){
                result = ( Math.Floor(num / 100000) / 10 ).ToString("F1");
                suffix = "M";
            }else if(num < 1000000000){
                result = ( Math.Floor(num / 1000000) ).ToString("F0");
                suffix = "M";
            }else if(num < 10000000000){
                result = ( Math.Floor(num / 10000000) / 100 ).ToString("F2");
                suffix = "B";
            }else if(num < 100000000000){
                result = ( Math.Floor(num / 100000000) / 10 ).ToString("F1");
                suffix = "B";
            }else if(num < 1000000000000){
                result = ( Math.Floor(num / 1000000000) ).ToString("F0");
                suffix = "B";
            }else if(num < 10000000000000){
                result = ( Math.Floor(num / 10000000000) / 100 ).ToString("F2");
                suffix = "T";
            }else if(num < 100000000000000){
                result = ( Math.Floor(num / 100000000000) / 10 ).ToString("F1");
                suffix = "T";
            }else{
                result = ( Math.Floor(num / 1000000000000) ).ToString("F0");
                suffix = "T";
            }
            if (result.Contains("."))
            {
                result = result.TrimEnd('0').TrimEnd('.');
            }
            return result + suffix;
        }
        //696969 to grey
        public static Color StringHexToColor(string color)
        {
            if(color.StartsWith("#")){
                color = color.Substring(1);
            }
            float red = System.Convert.ToInt32(color.Substring(0, 2), 16) / 255f;
            float green = System.Convert.ToInt32(color.Substring(2, 2), 16) / 255f;
            float blue = System.Convert.ToInt32(color.Substring(4, 2), 16) / 255f;
            return new Color(red, green, blue);
        }
        #endregion

        //Math
        public static float NextFibonacci(float lv, float start, float raise)
        {
            float result = start + raise * lv * (lv + 1) / 2;
            return result;
        }

        #region Time
        //20240612
        public static int GetCurrentDateNumber()
        {
            DateTime date = DateTime.UtcNow;
            int number = date.Year * 10000;
            number += date.Month * 100;
            number += date.Day;
            return number;
        }

        public static string GetCurrentFomatDate()
        {
            DateTime date = DateTime.Now;
            int dd = date.Day;
            int mm = date.Month;
            int yy = date.Year;
            return dd + "/" + mm + "/" + yy;
        }

        // 00:00:00
        public static string FormatTimeHour(long time)
        {
            if(time <= 0) return "00:00:00";
            int second = (int)time;
            int minus = second / 60;
            int hour = minus / 60;
            minus -= hour * 60;
            second -= (minus * 60 + hour * 3600);
            string sec;
            if (second < 10)
            {
                sec = "0" + second.ToString();
            }
            else
            {
                sec = second.ToString();
            }
            string min;
            if (minus < 10)
            {
                min = "0" + minus.ToString();
            }
            else
            {
                min = minus.ToString();
            }
            return hour + ":" + min + ":" + sec;
        }

        // 00:00
        public static string FormatTimeMinus(double time)
        {
            int second = (int)time;
            int minus = second / 60;
            second = second - minus * 60;
            string sec;
            if (second < 10)
            {
                sec = "0" + second.ToString();
            }
            else
            {
                sec = second.ToString();
            }
            string min;
            if (minus < 10)
            {
                min = "0" + minus.ToString();
            }
            else
            {
                min = minus.ToString();
            }
            return min + ":" + sec;
        }

        // 40m 68s
        public static string FormatTimeMinus_1(double time)
        {
            int second = (int)time;
            int minus = second / 60;
            second = second - minus * 60;
            string sec;
            if (second < 10)
            {
                sec = "0" + second.ToString();
            }
            else
            {
                sec = second.ToString();
            }
            string min;
            if (minus < 10)
            {
                min = "0" + minus.ToString();
            }
            else
            {
                min = minus.ToString();
            }
            return min + "m " + ": " + sec + "s";
        }

        // 40h 68m
        public static string FormatTimeHour_1(double time)
        {
            int second = (int)time;
            int minus = second / 60;
            int hour = minus / 60;
            minus -= hour * 60;
            second -= (minus * 60 + hour * 3600);
            string sec;
            if (second < 10)
            {
                sec = "0" + second.ToString();
            }
            else
            {
                sec = second.ToString();
            }
            string min;
            if (minus < 10)
            {
                min = "0" + minus.ToString();
            }
            else
            {
                min = minus.ToString();
            }
            return hour + "h " + min + "m";
        }

        // 3d 2h
        public static string Format_Time(long time, int number = 2)
        {
            List<string> list = new List<string>();
            int totalSeconds = (int)time;
            int seconds = totalSeconds % 60;
            int minutes = (totalSeconds / 60) % 60;
            int hours = (totalSeconds / 3600) % 24;
            int days = totalSeconds / (3600 * 24);

            if (days > 0)
            {
                list.Add(days +" "+ Lean.Localization.LeanLocalization.GetTranslationText("time_d", "Day"));
            }
            if (hours > 0)
            {
                list.Add(hours + " " + Lean.Localization.LeanLocalization.GetTranslationText("time_h", "Hour"));
            }
            if (minutes > 0)
            {
                list.Add(minutes + " " + Lean.Localization.LeanLocalization.GetTranslationText("time_m", "Minute"));
            }
            if (seconds >= 0 || list.Count == 0)
            {
                list.Add(seconds + " " + Lean.Localization.LeanLocalization.GetTranslationText("time_s", "Seconds"));
            }

            string result = "";
            int count = Mathf.Min(number, list.Count);

            for (int i = 0; i < count; i++)
            {
                result += list[i];
                if(number > 1) result += ":";
                number--;
            }

            return result.TrimEnd();
        }

        public static DateTime UnixTimestampToDateTime(long unixTimestamp)
        {
            // Unix timestamp is seconds past epoch
            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(unixTimestamp);
            DateTime dateTime = dateTimeOffset.UtcDateTime;
            return dateTime;
        }

        //1702539405
        public static long GetUtcTimestamp()
        {
            DateTimeOffset nowUtcOffset = DateTimeOffset.UtcNow;
            long timestamp = nowUtcOffset.ToUnixTimeSeconds();
            return timestamp;
        }

        //19723 (%7= 4 is monday)
        public static long GetTotalDay(long timeStamp)
        {
            return (long)(timeStamp / (24 * 60 * 60));
        }

        // 19723 - 4 = Monday is next week
        public static long GetTotalWeek(long timeStamp)
        {
            return (long)(GetTotalDay(timeStamp) - 4) / 7;
        }

        public static long GetTotalMonth(long timeStamp)
        {
            return (long)(GetTotalDay(timeStamp) / 30);
        }

        public static (long Year, long Day, long Hour, long Minus, long Second) TimeStampToSpecific(long timeStamp)
        {
            long years = (long)(timeStamp / (365 * 24 * 60 * 60));
            long days = (long)(timeStamp / (24 * 60 * 60)) - years * 365;
            long hours = (long)(timeStamp / (60 * 60)) - (years * 365 + days) * 24;
            long minus = (long)(timeStamp / 60) - ((years * 365 + days) * 24 + hours) * 60;
            long seconds = timeStamp - (((years * 365 + days) * 24 + hours) * 60 + minus) * 60;
            return (years, days, hours, minus, seconds);
        }
        #endregion

        public static string GenerateId()
        {
            var timestamp = DateTime.UtcNow.Ticks / 10000L; // get the current timestamp in milliseconds
            var guidBytes = Guid.NewGuid().ToByteArray(); // get a new guid as a byte array
            var objectIdBytes = new byte[12]; // create a new byte array to hold the final ObjectId

            // Copy the timestamp bytes into the objectIdBytes array in big-endian order
            Array.Copy(BitConverter.GetBytes(timestamp), 0, objectIdBytes, 0, 4);
            // Copy the guid bytes into the objectIdBytes array in little-endian order
            Array.Copy(guidBytes, 0, objectIdBytes, 4, 8);

            // Convert the objectIdBytes array to a string in hexadecimal format
            var objectIdString = BitConverter.ToString(objectIdBytes).Replace("-", "").ToLower();

            return objectIdString;
        }

        public static IEnumerator WaitSecond(float time, Action callback)
        {
            yield return new WaitForSeconds(time);
            callback.Invoke();
        }

        #region Enum
        public static T ParseEnumFromString<T>(string name)
        {
            try
            {
                return (T)Enum.Parse(typeof(T), name);
            }
            catch (System.Exception)
            {
                return default;
            }
        }

        // Get max value of enum
        public static int GetMaxValueOfEnum<T>()
        {
            return Enum.GetValues(typeof(T)).Length;
        }
        #endregion

        // Funtion change Bytes to KB, MB, GB, TB
        public static string BytesToReadable(long bytes)
        {
            string[] Suffix = { "B", "KB", "MB", "GB", "TB" };
            int i = 0;
            double dblSByte = bytes;
            if (bytes > 1024)
                for (i = 0; (int)(bytes / 1024) > 0; i++, bytes /= 1024)
                    dblSByte = bytes / 1024.0;
            return String.Format("{0:0.##} {1}", dblSByte, Suffix[i]);
        }

        [NTButton]
        public void GoldScale(float income, float mul)
        {
            NTLog.LogMessage(income * mul * 1.618f + "");
        }

        #region Image

        public static string ConvertTextureToBase64(Texture2D texture)
        {
            byte[] imageBytes = texture.EncodeToPNG();  // Convert to PNG format
            return System.Convert.ToBase64String(imageBytes); // Convert to Base64 string
        }

        public static Texture2D LoadTextureFromBase64(string base64String)
        {
            byte[] imageBytes = System.Convert.FromBase64String(base64String);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(imageBytes);
            return texture;
        }

        public static Texture2D CropTexture(Texture2D source, int targetWidth, int targetHeight)
        {
            // Resize but keep 
            Texture2D result = new Texture2D(targetWidth, targetHeight, source.format, false);

            for (int y = 0; y < targetHeight; y++)
            {
                for (int x = 0; x < targetWidth; x++)
                {
                    float xRatio = (float)x / (targetWidth - 1);
                    float yRatio = (float)y / (targetHeight - 1);
                    Color newColor = source.GetPixelBilinear(xRatio, yRatio);
                    result.SetPixel(x, y, newColor);
                }
            }

            result.Apply();
            return result;
        }

        public static Texture2D ResizeTexture(Texture2D source, int targetWidth, int targetHeight)
        {
            // Calculate new dimensions while maintaining aspect ratio
            float aspectRatio = (float)source.width / source.height;
            int newWidth = targetWidth;
            int newHeight = Mathf.RoundToInt(targetWidth / aspectRatio);

            if (newHeight > targetHeight)
            {
                newHeight = targetHeight;
                newWidth = Mathf.RoundToInt(targetHeight * aspectRatio);
            }

            // Create a blank texture with transparency (RGBA32 supports alpha)
            Texture2D result = new Texture2D(targetWidth, targetHeight, TextureFormat.RGBA32, false);

            // Fill with transparent background (or change Color.clear to Color.white for a white background)
            Color[] fillPixels = new Color[targetWidth * targetHeight];
            for (int i = 0; i < fillPixels.Length; i++)
            {
                fillPixels[i] = Color.clear; // Transparent background
            }
            result.SetPixels(fillPixels);

            // Scale the original image to fit inside the new size
            Texture2D scaled = ScaleTexture(source, newWidth, newHeight);

            // Calculate position to center the image
            int startX = (targetWidth - newWidth) / 2;
            int startY = (targetHeight - newHeight) / 2;

            // Copy scaled pixels into the result texture
            for (int y = 0; y < newHeight; y++)
            {
                for (int x = 0; x < newWidth; x++)
                {
                    result.SetPixel(startX + x, startY + y, scaled.GetPixel(x, y));
                }
            }

            result.Apply();
            return result;
        }

        // Helper function: Scales the texture using bilinear filtering
        public static Texture2D ScaleTexture(Texture2D source, int newWidth, int newHeight)
        {
            Texture2D result = new Texture2D(newWidth, newHeight, source.format, false);
            for (int y = 0; y < newHeight; y++)
            {
                for (int x = 0; x < newWidth; x++)
                {
                    float xRatio = (float)x / newWidth;
                    float yRatio = (float)y / newHeight;
                    result.SetPixel(x, y, source.GetPixelBilinear(xRatio, yRatio));
                }
            }
            result.Apply();
            return result;
        }

        #region Object
        public static T Clone<T>(T obj)
        {
            string json = JsonUtility.ToJson(obj);
            return JsonUtility.FromJson<T>(json);
        }
        #endregion


        #endregion
    }
}
