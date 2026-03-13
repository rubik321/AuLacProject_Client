using GOA.Config;
using Pixelplacement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rubik.UI
{
    public class FleeUI : State
    {
        [SerializeField] GameObject popupMain;
        [SerializeField] GameObject popupSuccess;
        [SerializeField] GameObject popupFail;

        private void OnEnable()
        {
            Time.timeScale = 0;
            popupMain.SetActive(true);
            popupSuccess.SetActive(false);
            popupFail.SetActive(false);
        }

        public void OnClickNo()
        {
            Time.timeScale = 1;
            CombatUIController.Instance.ChangeState(0);
        }

        public void OnClickYes()
        {
            bool fleeSuccess = Random.Range(0, 100) < 80;

            popupMain.SetActive(false);
            popupSuccess.SetActive(fleeSuccess);
            popupFail.SetActive(!fleeSuccess);
        }

        public void OnClickContinue(bool fleeSuccess)
        {
            if (fleeSuccess)
            {
                Time.timeScale = 1;
                Combat.CombatManager.Instance.Flee();
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
            else
            {
                Time.timeScale = 1;
                CombatUIController.Instance.ChangeState(0);
            }
        }
    }
}
