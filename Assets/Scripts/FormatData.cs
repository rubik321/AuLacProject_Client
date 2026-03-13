using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rubik.Format
{
    public class FormatData : MonoBehaviour
    {
        public static string GetFriendlyShortNumber(double inputNumber)
        {
            string retval = "";
            if (inputNumber < 1000)
            {
                retval = inputNumber.ToString("0.#");
            }
            else if (inputNumber < 1000000)
            {
                retval = (Math.Floor(inputNumber / 10)/100).ToString("0.#") + "K";
            }
            else if (inputNumber < 1000000000)
            {
                retval = (Math.Floor(inputNumber / 10000)/100).ToString("0.#") + "M";
            }
            else
            {
                retval = (Math.Floor(inputNumber / 10000000)/100).ToString("0.#") + "B";
            }
            return retval;
        }

        public static string GetFriendlyShortNumberFromString(string strNumber){
            try
            {
                return GetFriendlyShortNumber(double.Parse(strNumber));
            }
            catch (System.Exception e)
            {
                Debug.LogWarning(e);
                return "####";
            }
        }
    }

}
