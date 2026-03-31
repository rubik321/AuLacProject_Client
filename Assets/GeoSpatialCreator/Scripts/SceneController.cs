using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EasyARGeoSpatialAnchors
{
    public class SceneController : MonoBehaviour
    {
        public void LoadGeosSpatialWithEditor()
        {
            SceneManager.LoadScene(1);
        }

        public void LoadGeosSpatialRunTime()
        {
            SceneManager.LoadScene(2);
        }

        public void LoadMenuScene()
        {
            SceneManager.LoadScene(0);
        }

        public void LoadDemoScene()
        {
            SceneManager.LoadScene(3);
        }
    }
}