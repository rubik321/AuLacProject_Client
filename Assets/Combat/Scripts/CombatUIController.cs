using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOA.Config;
using Pixelplacement;

namespace Rubik.UI
{
    public class CombatUIController : StateMachine
    {
        public static CombatUIController Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        public void btnBack_Onclick()
        {
            //UnityEngine.SceneManagement.SceneManager.LoadScene(Configs.WorldMap_Screen);
            try
            {
                bl_SceneLoaderManager.LoadScene(Configs.WorldMap_Screen);
                Rubik.Common.AudioHelper.AudioCtrl.Instance.Play(Rubik.Common.AudioHelper.AudioName.Test);
            }
            catch (System.Exception e)
            {
                NTPackage_old.Functions.NTLog.LogError(e.ToString(), gameObject);
            }
        }
    }
}
