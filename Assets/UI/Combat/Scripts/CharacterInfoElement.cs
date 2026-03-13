using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

namespace Rubik.UI
{
    public class CharacterInfoElement : MonoBehaviour
    {
        [ReadOnly] public string instanceId;
        [SerializeField] Image healthBar;
        [SerializeField] Image healthBarGreen;
        [SerializeField] Image avatar;
        [SerializeField] GameObject boost;
        [SerializeField] GameObject frameCurrentTurn;
        [SerializeField] GameObject fillCurrentTurn;
        [SerializeField] GameObject fillNextTurn;

        public void SetupData(string characterId, string instanceId)
        {
            this.avatar.sprite = SpriteHelper.Instance.GetSprite(characterId);
            this.instanceId = instanceId;
            frameCurrentTurn.SetActive(false);
            fillCurrentTurn.SetActive(false);
            fillNextTurn.SetActive(false);
        }

        public void UpdateHealthBar(float percent)
        {
            // 0.2 alpha = 0, 0.8 alpha = 1
            healthBarGreen.color = new Color(1, 1, 1, Mathf.Clamp01((5 * percent - 1) / 3));
            healthBar.fillAmount = percent;
        }

        public void SetBoostActive(bool active)
        {
            boost.SetActive(active);
        }

        public void SetTurnActive(bool active)
        {
            fillCurrentTurn.SetActive(active);
            frameCurrentTurn.SetActive(active);
        }

        public void SetNextTurnActive(bool active)
        {
            fillNextTurn.SetActive(active);
        }
    }
}
