using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EasyARGeoSpatialAnchors
{
    public class Arrow : MonoBehaviour
    {

        void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Balloon"))
            {
                collision.gameObject.GetComponent<AudioSource>().Play();
                Destroy(gameObject);
                Transform particle = collision.gameObject.transform.Find("particle");
                particle.parent = null;
                Destroy(collision.gameObject);
                particle.GetComponent<ParticleSystem>().Play();

                if (FindObjectOfType<BallonHuntController>().numberOfBalloonsLeft > 0)
                {
                    FindObjectOfType<BallonHuntController>().numberOfBalloonsLeft -= 1;
                }
            }
        }
    }
}