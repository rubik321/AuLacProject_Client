using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NTFunctions_old
{
    public class NTList<T> : List<T>
    {
        public List<T> list;
        public NTList(List<T> list)
        {
            this.list = list;
        }
    }
    public class NTFunction : LoadBehaviour
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

        public static string FormatTimeHour(float time)
        {
            int second = (int)time;
            int minus = second / 60;
            int hour = minus / 60;
            return hour + ":" + (minus - hour * 60) + ":" + (second - minus * 60);
        }
        public static long GetUtcTimestamp()
        {
            DateTimeOffset nowUtcOffset = DateTimeOffset.UtcNow;
            long timestamp = nowUtcOffset.ToUnixTimeSeconds();
            return timestamp;
        }

        public static int GetCurrentDateNumber()
        {
            DateTime date = DateTime.Now;
            int number = date.Year * 10000;
            number += date.Month * 100;
            number += date.Day;
            Debug.Log(number);
            return number;
        }

        //Math
        public static float NextFibonacci(float lv, float start, float raise)
        {
            float result = start + raise * lv * (lv + 1) / 2;
            return result;
        }

        public static Color StringHexToColor(string color)
        {
            if (color.StartsWith("#"))
            {
                color = color.Substring(1);
            }
            float red = System.Convert.ToInt32(color.Substring(0, 2), 16) / 255f;
            float green = System.Convert.ToInt32(color.Substring(2, 2), 16) / 255f;
            float blue = System.Convert.ToInt32(color.Substring(4, 2), 16) / 255f;
            return new Color(red, green, blue);
        }

        //String
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

        public static string CollapString(string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength) + "...";
        }

    }
}
