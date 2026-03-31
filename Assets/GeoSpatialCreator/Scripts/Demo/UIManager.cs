using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace EasyARGeoSpatialAnchors
{

    public class UIManager : MonoBehaviour
    {
        public GameObject infoPanel;
        public GameObject guidePanel;
        public GameObject buttonPanel;
        public GameObject gamePanel;
        [HideInInspector]
        public bool infoPanelEnabled = false;

        private void Start()
        {
            infoPanel.SetActive(false);
            guidePanel.SetActive(false);
        }

        public void GotoMainMenu()
        {
            if (FindObjectOfType<BallonHuntController>().shootingStart)
            {
                buttonPanel.SetActive(true);
                infoPanel.SetActive(false);
                gamePanel.SetActive(false);
                FindObjectOfType<BallonHuntController>().bow.SetActive(false);
                RemoveAllArrows();
                FindObjectOfType<BallonHuntController>().shootingStart = false;
            }
            else
            {
                SceneManager.LoadScene(0);
            }

        }

        private void RemoveAllArrows()
        {
            foreach (Arrow arrow in FindObjectsOfType<Arrow>())
            {
                Destroy(arrow.gameObject);
            }
        }

        public void SwitchInfoPanel()
        {
            infoPanel.SetActive(infoPanelEnabled ? false : true);
            infoPanelEnabled = infoPanelEnabled ? false : true;
        }
    }
}