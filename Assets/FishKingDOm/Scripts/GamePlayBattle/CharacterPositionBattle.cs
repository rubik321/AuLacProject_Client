using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Rubik.Battle
{
    public class CharacterPositionBattle : MonoBehaviour
    {
        [SerializeField]public bool positionHeroSelf;
        [SerializeField] ParticleSystem spawnEffect;
        public HeroController heroInSlot;
        public Vector2 originPos;
        public void Start()
        {
            originPos = transform.position;
        }
        public void Begin(HeroController _hero)
        {
            heroInSlot = _hero;
            originPos = heroInSlot.transform.position;
        }
        public void Begin()
        {
            originPos = heroInSlot.transform.position;
        }
        public void ResetSlot()
        {
            heroInSlot = null;
        }
        public Vector2 position()
        {
            return originPos;
        }
        public Vector2 positionHit()
        {
            return positionHeroSelf ? position() + new Vector2(1, 0) : position() - new Vector2(1, 0);
        }
        public Vector2 positionHitRange()
        {
            return new Vector2(transform.position.x,transform.position.y+.4f);
        }
        public Transform positionHitRangeWithTranform()
        {

            return transform;
        }
        public void PlaySpwanEffect()
        {
            spawnEffect.Play();
        }
    }
}