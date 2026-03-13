using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOA.Shop{
    [System.Serializable]
    public class ShopData
    {
        public string Index;
        public string IconName;
        public Sprite Icon;
        public float Cost;
        public string TitleLocal;
        public string DetailLocal;
    }
}