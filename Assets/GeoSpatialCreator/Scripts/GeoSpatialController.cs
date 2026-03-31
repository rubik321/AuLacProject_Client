using System;
using System.Collections;
using System.Collections.Generic;
using Google.XR.ARCoreExtensions;
using UnityEngine.UI;
using UnityEngine;
#if UNITY_ANDROID
using UnityEngine.Android;
#endif
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.EventSystems;
using TMPro;

namespace EasyARGeoSpatialAnchors
{

    public class GeoSpatialController : MonoBehaviour
    {
        [Header("Core Features")]
        [SerializeField]
        private Text InfoText;

        [SerializeField]
        private AREarthManager earthManager;

        [SerializeField]
        private ARAnchorManager anchorManager;

        [SerializeField]
        private ARRaycastManager raycastManager;

        [SerializeField]
        private ARCoreExtensions arcoreExtensions;

        private bool waitingForLocationService = false;
        private bool _shouldResolvingHistory = false;

        private Coroutine locationServiceLauncher;

        public GameObject GeospatialPrefab;

        private string saveData = null;

        private GuidePanelController guidePanelController;

        private GeospatialAnchorHistoryCollection _historyCollection = null;

        private List<GameObject> _anchorObjects = new List<GameObject>();

        private AnchorType _anchorType = AnchorType.Geospatial;

        public Text numberOfAnchorsSavedText;

        private const int _storageLimit = 20;
        private const double _orientationYawAccuracyThreshold = 25;
        private const double _headingAccuracyThreshold = 25;
        private const double _horizontalAccuracyThreshold = 20;

        private bool showTaptoScreen = false;
        [HideInInspector]
        public bool showNotStable = false;

        private void OnEnable()
        {
            StartCoroutine(StartLocationService());
            LoadGeospatialAnchorHistory();
            _shouldResolvingHistory = _historyCollection.Collection.Count > 0;
            Debug.Log("number of history collection" + _historyCollection.Collection.Count);
        }

        private void Start()
        {
            if (FindObjectOfType<GuidePanelController>())
            {
                guidePanelController = FindObjectOfType<GuidePanelController>();
            }
            else
            {
                Debug.Log("No guide panel found");
            }
        }

        private IEnumerator StartLocationService()
        {
            waitingForLocationService = true;

#if UNITY_ANDROID
            if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
            {
                Permission.RequestUserPermission(Permission.FineLocation);
                yield return new WaitForSeconds(3.0f);
            }
#endif

            if (!Input.location.isEnabledByUser)
            {
                Debug.Log("Location service is disabled by the user.");
                waitingForLocationService = false;
                yield break;
            }

            Input.location.Start();

            while (Input.location.status == LocationServiceStatus.Initializing)
            {
                yield return null;
            }

            waitingForLocationService = false;
            if (Input.location.status != LocationServiceStatus.Running)
            {
                Debug.LogWarningFormat(
                    "Location service ended with {0} status.", Input.location.status);
                Input.location.Stop();
            }

        }

        private void LoadGeospatialAnchorHistory()
        {
            if (PlayerPrefs.HasKey(saveData))
            {
                _historyCollection = JsonUtility.FromJson<GeospatialAnchorHistoryCollection>(
                                   PlayerPrefs.GetString(saveData));
                numberOfAnchorsSavedText.text = "Number of Anchors Saved : " + _historyCollection.Collection.Count;
            }
            else
            {
                _historyCollection = new GeospatialAnchorHistoryCollection();
                numberOfAnchorsSavedText.text = "Number of Anchors Saved : 0";
            }
        }

        void Update()
        {
            var earthTrackingState = earthManager.EarthTrackingState;
            var pose = earthTrackingState == TrackingState.Tracking ?
                    earthManager.CameraGeospatialPose : new GeospatialPose();
            if (Input.touchCount > 0)
            {

            }


            if (earthTrackingState != TrackingState.Tracking ||
                   pose.OrientationYawAccuracy > _orientationYawAccuracyThreshold ||
                   pose.HorizontalAccuracy > _horizontalAccuracyThreshold)
            {

                if (Input.touchCount > 0 && guidePanelController && !showNotStable && (Input.GetTouch(0).phase == TouchPhase.Began))
                {
                    showNotStable = true;
                    guidePanelController.ShowGuidedText("Keep horizontal & yaw \n accuracy below 20...", 1);
                }
            }
            else
            {
                if (guidePanelController && !showTaptoScreen)
                {
                    guidePanelController.ShowGuidedText("Tap to place anchor...", 0);
                    showTaptoScreen = true;
                }

                if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began
                       && !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                {
                    // Set anchor on screen tap.
                    PlaceAnchorByScreenTap(Input.GetTouch(0).position);
                }

            }

            ResolveHistory(pose);

            if (earthTrackingState == TrackingState.Tracking)
            {
                InfoText.text = string.Format(
                "Latitude/Longitude: {1}°, {2}°{0}" +
                "Horizontal Accuracy: {3}m{0}" +
                "Altitude: {4}m{0}" +
                "Vertical Accuracy: {5}m{0}" +
                "Eun Rotation: {6}{0}" +
                "Orientation Yaw Accuracy: {7}°",
                Environment.NewLine,
                pose.Latitude.ToString("F6"),
                pose.Longitude.ToString("F6"),
                pose.HorizontalAccuracy.ToString("F6"),
                pose.Altitude.ToString("F2"),
                pose.VerticalAccuracy.ToString("F2"),
                pose.EunRotation.ToString("F1"),
                pose.OrientationYawAccuracy.ToString("F1"));
            }
            else
            {
                InfoText.text = "GEOSPATIAL POSE: not tracking";
            }
        }

        private void ResolveHistory(GeospatialPose pose)
        {
            if (!_shouldResolvingHistory)
            {
                return;
            }

            if (pose.OrientationYawAccuracy < _orientationYawAccuracyThreshold &&
                   pose.HorizontalAccuracy < _horizontalAccuracyThreshold)
            {
                _shouldResolvingHistory = false;


                if (guidePanelController)
                {
                    guidePanelController.ShowGuidedText("Loading Achors...", 2);
                }

                foreach (var history in _historyCollection.Collection)
                {
                    PlaceGeospatialAnchor(history);
                }
            }

        }


        private void PlaceAnchorByScreenTap(Vector2 position)
        {
            List<ARRaycastHit> planeHitResults = new List<ARRaycastHit>();
            raycastManager.Raycast(
                position, planeHitResults, TrackableType.Planes | TrackableType.FeaturePoint);

            if (planeHitResults.Count > 0)
            {
                GeospatialAnchorHistory history = CreateHistory(planeHitResults[0].pose,
                        _anchorType);

                var anchor = PlaceGeospatialAnchor(history, true);
                if (anchor != null)
                {
                    _historyCollection.Collection.Add(history);
                    SaveGeospatialAnchorHistory();
                    numberOfAnchorsSavedText.text = "Number of Anchors Saved : " + _historyCollection.Collection.Count;
                }

            }

        }

        private void SaveGeospatialAnchorHistory()
        {
            _historyCollection.Collection.Sort((left, right) =>
                   right.CreatedTime.CompareTo(left.CreatedTime));

            // Remove the earliest data if the capacity exceeds storage limit.
            if (_historyCollection.Collection.Count > _storageLimit)
            {
                _historyCollection.Collection.RemoveRange(
                    _storageLimit, _historyCollection.Collection.Count - _storageLimit);
            }

            PlayerPrefs.SetString(saveData, JsonUtility.ToJson(_historyCollection));
            PlayerPrefs.Save();
        }

        private ARGeospatialAnchor PlaceGeospatialAnchor(GeospatialAnchorHistory history, bool tapped = false)
        {
            Quaternion eunRotation = CreateRotation(history);
            ARGeospatialAnchor anchor = null;

            anchor = anchorManager.AddAnchor(
                history.Latitude, history.Longitude, history.Altitude, eunRotation);

            if (anchor != null)
            {
                GameObject anchorGO = Instantiate(GeospatialPrefab, anchor.transform);
                Transform canvas = anchorGO.transform.Find("Canvas");

                if (tapped)
                {
                    DateTime dt = DateTime.Now;

                    canvas.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Created on \n" + dt.ToString();
                    anchorGO.GetComponent<Renderer>().material.color = new Color(238 / 255f, 26 / 255f, 85 / 255f, 0.9f);
                }
                else
                {
                    canvas.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Created on \n" + history.CreatedTime.ToString();
                    anchorGO.GetComponent<Renderer>().material.color = new Color(0f, 204 / 255f, 124 / 255f, 0.9f);
                }

                anchorGO.transform.parent = anchor.gameObject.transform;
                _anchorObjects.Add(anchor.gameObject);

            }

            return anchor;
        }

        private GeospatialAnchorHistory CreateHistory(Pose pose, AnchorType anchorType)
        {
            GeospatialPose geospatialPose = earthManager.Convert(pose);

            GeospatialAnchorHistory history = new GeospatialAnchorHistory(
                geospatialPose.Latitude, geospatialPose.Longitude, geospatialPose.Altitude,
                anchorType, geospatialPose.EunRotation);
            return history;
        }

        private Quaternion CreateRotation(GeospatialAnchorHistory history)
        {
            Quaternion eunRotation = history.EunRotation;
            if (eunRotation == Quaternion.identity)
            {
                // This history is from a previous app version and EunRotation was not used.
                eunRotation = Quaternion.AngleAxis(180f - (float)history.Heading, Vector3.up);
            }

            return eunRotation;
        }

        public void ClearAllAnchorsSaved()
        {
            foreach (var anchor in _anchorObjects)
            {
                Destroy(anchor);
            }

            _anchorObjects.Clear();
            _historyCollection.Collection.Clear();
            numberOfAnchorsSavedText.text = "Number of Anchors Saved : 0";
            SaveGeospatialAnchorHistory();
        }
    }
}