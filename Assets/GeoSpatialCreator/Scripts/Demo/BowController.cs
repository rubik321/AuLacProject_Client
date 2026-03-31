using UnityEngine;

namespace EasyARGeoSpatialAnchors
{

    public class BowController : MonoBehaviour
    {
        public float pullForceMultiplier = 1;
        public float moveSpeed = 10f;

        private Camera mainCamera;
        public float distanceFromCamera = 1f;
        public float bowAngle = 70f;

        public GameObject arrowPrefab;
        private GameObject currentArrow;
        private Vector2 startTouchPosition;
        private Vector2 currentTouchPosition;
        private Transform arrowStartPoint;
        private bool isAiming = false;

        private Transform startPoint; // Starting point
        private Transform endPoint; // Ending point
        private Animator anim;
        private AudioSource audioSource;

        private void Start()
        {
            mainCamera = FindObjectOfType<Camera>();
            startPoint = transform.Find("startPoint");
            endPoint = transform.Find("endPoint");
            arrowStartPoint = transform.Find("arrowStartPoint");
            anim = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();

        }

        void Update()
        {

            if (Input.touchCount > 0 && FindObjectOfType<BallonHuntController>().shootingStart && !FindObjectOfType<BallonHuntController>().resultPanel.activeSelf)
            {
                Touch touch = Input.GetTouch(0);
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        StartAiming(touch.position);
                        break;
                    case TouchPhase.Moved:
                        UpdateAiming(touch.position);
                        break;
                    case TouchPhase.Ended:
                        ReleaseArrow();
                        break;
                }
            }
        }

        void StartAiming(Vector2 touchPosition)
        {
            Vector3 direction = endPoint.localPosition - startPoint.localPosition;
            currentArrow = Instantiate(arrowPrefab);
            currentArrow.transform.parent = gameObject.transform;
            currentArrow.transform.position = arrowStartPoint.transform.position;
            currentArrow.transform.localRotation = Quaternion.LookRotation(-direction);

            startTouchPosition = touchPosition;
            isAiming = true;
        }

        void UpdateAiming(Vector2 touchPosition)
        {
            if (!isAiming) return;

            currentTouchPosition = touchPosition;

            Vector2 dragVector = currentTouchPosition - startTouchPosition;
            float pullDistance = (dragVector.magnitude) / 5000;
            Vector3 direction = (endPoint.localPosition - startPoint.localPosition).normalized;
            currentArrow.transform.localPosition = arrowStartPoint.localPosition + direction * pullDistance;
            anim.Play("PullBack", 0, pullDistance / 0.4f);
            // Here you can add visual feedback for the bow string being pulled
        }

        void ReleaseArrow()
        {
            if (!isAiming) return;
            anim.Play("Release");
            audioSource.Play();
            Vector3 direction = endPoint.position - startPoint.position;
            Vector2 dragVector = currentTouchPosition - startTouchPosition;
            float pullDistance = dragVector.magnitude / 5;
            currentArrow.GetComponent<Rigidbody>().useGravity = true;
            currentArrow.GetComponent<Rigidbody>().AddForce(-direction * pullDistance * pullForceMultiplier, ForceMode.Impulse);
            currentArrow = null;
            isAiming = false;
        }
    }
}