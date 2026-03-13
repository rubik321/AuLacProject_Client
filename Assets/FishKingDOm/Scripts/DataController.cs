using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace XenoWars
{
    public class DataController : MonoBehaviour
    {
        private static DataController instance;
        
        private DataController()
        {
            instance = this;
        }
        public static DataController getInstance()
        {
            if (instance == null)
            {
                instance = new DataController();
            }
            return instance;
        }
        
    }
}
