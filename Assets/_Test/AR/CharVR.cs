using UnityEngine;

namespace Rubik.AR
{
    public class CharVR : MonoBehaviour
    {
        public float health = 10;
        public Transform HitPoint;
        public PlaceOnFirstPlane placeOnFirstPlane;

        public void TakeDamage(float damage){
            health -= damage;
            if(health <= 0){
                placeOnFirstPlane.DestroyObject(this);
            }
        }
    }
}