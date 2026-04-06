using DamageNumbersPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace Rubik.AR
{
    public class TapToSpawnDmg : MonoBehaviour
    {
        [Header("Spawn Settings")]
        [SerializeField] private DamageNumberMesh dmgPrefab;      // Assign the "dmg" prefab (effect, particle, text, etc.)
        [SerializeField] private float spawnOffsetY = 0.5f; // How high above the object to spawn
        [SerializeField] private bool destroyAfterSpawn = false;  // Destroy this object after tapping?

        public Camera arCamera;

        void Update()
        {
            if(Touch()) return;
            if(TouchMouse()) return;
            return;
        }

        public bool Touch()
        {
            if (Input.touchCount == 0) return false;
            if (arCamera == null)
            {
                Debug.LogError("arCamera is not assigned!");
                return false;
            }

            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                // Perform a raycast from the touch position
                Ray ray = arCamera.ScreenPointToRay(touch.position);
                RaycastHit hit;

                // Check if the ray hits this object (requires a Collider on this object)
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject.TryGetComponent(out CharVR charVR))
                    {
                        OnObjectTapped(charVR);
                        return true;
                    }
                }
            }
            return false;
        }

        public bool TouchMouse()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = arCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject.TryGetComponent(out CharVR charVR))
                    {
                        OnObjectTapped(charVR);
                        return true;
                    }
                }
            }
            return false;
        }

        private void OnObjectTapped(CharVR charVR)
        {
            Debug.Log("OnObjectTapped: " + charVR.name);
            // Spawn the dmg prefab
            if (dmgPrefab != null)
            {
                Vector3 spawnPos = charVR.HitPoint.position;
                Debug.Log("SpawnPos: " + spawnPos);
                DamageNumber dmg = dmgPrefab.Spawn(spawnPos, 1, charVR.HitPoint);
                charVR.TakeDamage(1);

                Debug.Log("DmgPos: " + dmg.transform.position);
            }
            else
            {
                Debug.LogError("dmgPrefab is not assigned!");
            }

            // Optionally destroy the tapped object
            if (destroyAfterSpawn)
            {
                // Remove the ARAnchor before destroying to avoid errors
                ARAnchor anchor = GetComponent<ARAnchor>();
                if (anchor != null) Destroy(anchor);
                Destroy(gameObject);
            }
        }
    }
}