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

namespace EasyARGeoSpatialAnchors
{

    public class BallonHuntController : MonoBehaviour
    {
        private GeoSpatialController geoSpatialController;

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

        private GameObject arCamera;

        private UIManager uIManager;


        private bool waitingForLocationService = false;
        private bool _shouldResolvingHistory = false;
        private bool placingEnabled = false;

        [HideInInspector]
        public bool shootingStart = false;

        private Coroutine locationServiceLauncher;

        public GameObject geospatialPrefab;
        public GameObject bow;
        public Sprite[] guideSprites;
        public Texture[] balloonTextures;

        private const string saveBallonData = "savedBalloons";

        private GeospatialAnchorHistoryCollection _historyCollection = null;

        private List<GameObject> _anchorObjects = new List<GameObject>();

        private AnchorType _anchorType = AnchorType.Geospatial;

        public Text numberOfAnchorsSavedText;
        public Text ballonsCount;
        [HideInInspector]
        public int numberOfBalloonsLeft;
        public Text time;
        public GameObject resultPanel;
        public GameObject gamePanel;
        public Text result;

        private float timeRemaining = 15f; // Initial time in seconds
        private bool timerIsRunning = false;

        public Text guideText;

        public Slider accuracyLevel;
        private Image accuracyLevelColor;

        private bool showAccuracyLevelWarning = false;

        private const int _storageLimit = 20;
        private const double _orientationYawAccuracyThreshold = 25;
        private const double _headingAccuracyThreshold = 25;
        private const double _horizontalAccuracyThreshold = 20;

        private void Awake()
        {
            uIManager = FindObjectOfType<UIManager>();
            arCamera = FindObjectOfType<ARCameraManager>().gameObject;
        }

        private void OnEnable()
        {
            StartCoroutine(StartLocationService());
            LoadGeospatialAnchorHistory();
            _shouldResolvingHistory = _historyCollection.Collection.Count > 0;
        }

        private void OnDisable()
        {
            StopCoroutine(StartLocationService());
        }

        public void LoadBalloons()
        {
            LoadGeospatialAnchorHistory();
            shootingStart = true;
            placingEnabled = false;
            uIManager.buttonPanel.SetActive(false);

            if (_shouldResolvingHistory)
            {
                ShowGuidedText("Tap, drag and release finger to hunt balloons...", guideSprites[3]);
                SetupGame();
            }
            else
            {
                ShowGuidedText("Place balloons before hunt...", guideSprites[0]);
            }
        }

        private void SetupGame()
        {
            numberOfBalloonsLeft = _historyCollection.Collection.Count;
            ballonsCount.text = "Balloons Left : " + numberOfBalloonsLeft + "/" + _historyCollection.Collection.Count;
            gamePanel.SetActive(true);
            resultPanel.SetActive(false);
            if (uIManager.infoPanel.activeSelf)
            {
                uIManager.infoPanel.SetActive(false);
                uIManager.infoPanelEnabled = false;
            }
            RemoveAllPreviousBalloons();
            ResolveHistory();
            StartTimer();
            bow.SetActive(true);

        }

        private void RemoveAllPreviousBalloons()
        {
            GameObject[] balloons = GameObject.FindGameObjectsWithTag("Balloon");
            foreach (GameObject balloon in balloons)
            {
                Destroy(balloon);
            }
        }

        public void StartTimer()
        {
            if (!timerIsRunning)
            {
                timerIsRunning = true;
                timeRemaining = numberOfBalloonsLeft * 10;
                StartCoroutine(RunTimer());
            }
        }

        private IEnumerator RunTimer()
        {
            while (timeRemaining > 0)
            {

                if (time != null)
                {
                    time.text = "Time : " + timeRemaining.ToString("F0") + "s";
                }

                ballonsCount.text = "Balloons Left : " + numberOfBalloonsLeft + "/" + _historyCollection.Collection.Count;

                if (numberOfBalloonsLeft == 0)
                {
                    // If balloon count is zero, end the timer
                    ShowResult("You Won!");
                    break;
                }

                yield return new WaitForSeconds(1f);

                timeRemaining--;

                if (timeRemaining <= 0)
                {
                    timeRemaining = 0;
                    TimerEnded();
                }


            }


        }

        private void ShowResult(string text)
        {
            time.text = "Time : 0s";
            timerIsRunning = false;
            StopCoroutine(RunTimer());
            resultPanel.SetActive(true);
            result.text = text;
        }

        private void TimerEnded()
        {
            ShowResult("You Lost");
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

        public void StartPlacingBallons()
        {
            shootingStart = false;
            var pose = earthManager.CameraGeospatialPose;

            if (pose.HorizontalAccuracy > _horizontalAccuracyThreshold || pose.OrientationYawAccuracy > _orientationYawAccuracyThreshold)
            {
                ShowGuidedText("Accuracy level is low. Keep accuracy level green...", guideSprites[1]);
            }
            else
            {
                ShowGuidedText("Tap screen to place balloons...", guideSprites[0]);
            }


            if (_historyCollection.Collection.Count < _storageLimit)
            {
                placingEnabled = true;
            }
            else
            {
                placingEnabled = false;
                ShowGuidedText("Stotage limit exeeds. Clear balloons before place.", guideSprites[2]);
            }

        }

        public void ShowGuidedText(string text, Sprite sprite)
        {
            guideText.text = text;
            uIManager.guidePanel.transform.Find("image").GetComponent<Image>().sprite = sprite;
            StartCoroutine(ShowPanel());
        }

        IEnumerator ShowPanel()
        {
            uIManager.guidePanel.SetActive(true);
            yield return new WaitForSeconds(3);
            uIManager.guidePanel.SetActive(false);
        }

        private void LoadGeospatialAnchorHistory()
        {
            if (PlayerPrefs.HasKey(saveBallonData))
            {
                _historyCollection = JsonUtility.FromJson<GeospatialAnchorHistoryCollection>(
                                   PlayerPrefs.GetString(saveBallonData));
                numberOfAnchorsSavedText.text = "Number of Balloon Saved : " + _historyCollection.Collection.Count;
            }
            else
            {
                _historyCollection = new GeospatialAnchorHistoryCollection();
                numberOfAnchorsSavedText.text = "Number of Balloon Saved : 0";
            }
        }

        void Update()
        {
            var earthTrackingState = earthManager.EarthTrackingState;
            var pose = earthTrackingState == TrackingState.Tracking ?
                    earthManager.CameraGeospatialPose : new GeospatialPose();

            if (earthTrackingState != TrackingState.Tracking ||
                   pose.OrientationYawAccuracy > _orientationYawAccuracyThreshold ||
                   pose.HorizontalAccuracy > _horizontalAccuracyThreshold)
            {

                if (showAccuracyLevelWarning == false)
                {
                    ShowGuidedText("Accuracy level is low. Keep accuracy level green...", guideSprites[1]);
                    showAccuracyLevelWarning = true;
                }

            }
            else
            {
                if (placingEnabled && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began
                       && !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                {
                    // Set anchor on screen tap.
                    PlaceAnchorByScreenTap(Input.GetTouch(0).position);

                }

            }



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

                accuracyLevel.value = (float)(100f - pose.HorizontalAccuracy);

                if (accuracyLevel.value < 50)
                {
                    accuracyLevelColor.color = Color.red;
                }
                else if (accuracyLevel.value > 80 && pose.OrientationYawAccuracy < _orientationYawAccuracyThreshold)
                {
                    accuracyLevelColor.color = Color.green;
                }
                else
                {
                    accuracyLevelColor.color = Color.yellow;
                }
            }
            else
            {
                InfoText.text = "GEOSPATIAL POSE: not tracking";
            }
        }

        private void ResolveHistory()
        {
            if (!_shouldResolvingHistory)
            {
                return;
            }

            foreach (var history in _historyCollection.Collection)
            {
                PlaceGeospatialAnchor(history);
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

                var anchor = PlaceGeospatialAnchor(history);
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

            PlayerPrefs.SetString(saveBallonData, JsonUtility.ToJson(_historyCollection));
            PlayerPrefs.Save();
        }

        private ARGeospatialAnchor PlaceGeospatialAnchor(GeospatialAnchorHistory history)
        {
            Quaternion eunRotation = CreateRotation(history);
            ARGeospatialAnchor anchor = null;

            anchor = anchorManager.AddAnchor(
                history.Latitude, history.Longitude, history.Altitude, eunRotation);

            if (anchor != null)
            {
                GameObject balloon = Instantiate(geospatialPrefab, anchor.transform);
                int randomNum = UnityEngine.Random.Range(0, 4);
                balloon.transform.Find("Balloon").GetComponent<Renderer>().material.SetTexture("_BaseMap", balloonTextures[randomNum]);

                balloon.transform.parent = anchor.gameObject.transform;
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

        private void Start()
        {
            geoSpatialController = FindObjectOfType<GeoSpatialController>();
            accuracyLevelColor = accuracyLevel.transform.GetChild(1).GetChild(0).GetComponent<Image>();

        }
    }

}