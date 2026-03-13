using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Rubik.Combat
{
    public class CharacterHealthBar : MonoBehaviour
    {
        [SerializeField] Image healthBar;
        [SerializeField] Image healthBarGreen;
        [SerializeField] Image manaBar;
        Camera mainCamera;

        private void Start()
        {
             mainCamera = Camera.main;
        }

        private void Update()
        {
            transform.LookAt(new Vector3(transform.position.x, mainCamera.transform.position.y, mainCamera.transform.position.z));
        }

        public void UpdateHealth(float percent)
        {
            healthBar.fillAmount = percent;
            // 0.2 alpha = 0, 0.8 alpha = 1
            healthBarGreen.color = new Color(1, 1, 1, Mathf.Clamp01((5 * percent - 1) / 3));
        }

        public void UpdateMana(float percent)
        {
            if (manaBar != null)
            {
                manaBar.fillAmount = percent;
            }
        }
    }
}
