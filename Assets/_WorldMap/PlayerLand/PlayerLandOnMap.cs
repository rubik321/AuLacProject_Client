using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rubik.PlayerLand
{
    public class PlayerLandOnMap : MonoBehaviour
    {
        public SpriteRenderer SR_Land;
        
        private void Start()
        {
            this.SR_Land.sprite = PlayerLandManager.Instance.GetPlayerSprite();
        }
    }
}