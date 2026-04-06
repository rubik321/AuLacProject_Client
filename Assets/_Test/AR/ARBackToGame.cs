using Rubik.Config;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Rubik.AR
{
    public class ARBackToGame : MonoBehaviour
    {
        public void BackToGame()
        {
            SceneController.Instance.LoadScene(SceneConfig.WorldMap_Screen);
        }
    }
}
