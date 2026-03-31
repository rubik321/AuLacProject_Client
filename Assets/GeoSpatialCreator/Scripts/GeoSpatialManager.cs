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

namespace EasyARGeoSpatialAnchors
{

    public class GeoSpatialManager : MonoBehaviour
    {
        [Header("Core Features")]
        [SerializeField]
        private Text geospatialStatusText;

        [SerializeField]
        private GameObject guidePanel;

        [SerializeField]
        private GameObject objectToPlace;

        [SerializeField]
        private AREarthManager earthManager;

        [SerializeField]
        private ARCoreExtensions arcoreExtensions;

        private bool waitingForLocationService = false;
        private bool showGuidePanel = false;

        private Coroutine locationServiceLauncher;

        private void Awake()
        {
            // Enable geospatial sample to target 60fps camera capture frame rate
            // on supported devices.
            // Note, Application.targetFrameRate is ignored when QualitySettings.vSyncCount != 0.
            Application.targetFrameRate = 60;
        }

        private void Start()
        {
            StartCoroutine(ShowObject());
        }

        void Update()
        {
            if (earthManager == null)
            {
                return;
            }

            if (ARSession.state != ARSessionState.SessionInitializing &&
                   ARSession.state != ARSessionState.SessionTracking)
            {
                return;
            }

            // Check feature support and enable Geospatial API when it's supported.
            var featureSupport = earthManager.IsGeospatialModeSupported(GeospatialMode.Enabled);
            switch (featureSupport)
            {
                case FeatureSupported.Unknown:
                    break;
                case FeatureSupported.Unsupported:
                    if (!showGuidePanel)
                    {
                        ShowGuidedText("The Geospatial API is not supported by this device.");
                        showGuidePanel = true;
                    }

                    break;
                case FeatureSupported.Supported:
                    if (arcoreExtensions.ARCoreExtensionsConfig.GeospatialMode == GeospatialMode.Disabled)
                    {
                        arcoreExtensions.ARCoreExtensionsConfig.GeospatialMode =
                            GeospatialMode.Enabled;
                        arcoreExtensions.ARCoreExtensionsConfig.StreetscapeGeometryMode =
                            StreetscapeGeometryMode.Enabled;
                    }

                    if (!showGuidePanel)
                    {
                        ShowGuidedText("Receiving Anchor Data...");
                        showGuidePanel = true;
                    }


                    break;
            }

            var pose = earthManager.EarthState == EarthState.Enabled &&
                earthManager.EarthTrackingState == TrackingState.Tracking ?
                earthManager.CameraGeospatialPose : new GeospatialPose();
            var supported = earthManager.IsGeospatialModeSupported(GeospatialMode.Enabled);

            if (geospatialStatusText != null)
            {
                geospatialStatusText.text =
                    $"SessionState: {ARSession.state}\n" +
                    $"LocationServiceStatus: {Input.location.status}\n" +
                    $"FeatureSupported: {supported}\n" +
                    $"EarthState: {earthManager.EarthState}\n" +
                    $"EarthTrackingState: {earthManager.EarthTrackingState}\n" +
                    $"  LAT/LNG: {pose.Latitude:F6}, {pose.Longitude:F6}\n" +
                    $"  HorizontalAcc: {pose.HorizontalAccuracy:F6}\n" +
                    $"  ALT: {pose.Altitude:F2}\n" +
                    $"  VerticalAcc: {pose.VerticalAccuracy:F2}\n" +
                    $"  EunRotation: {pose.EunRotation:F2}\n" +
                    $"  OrientationYawAcc: {pose.OrientationYawAccuracy:F2}";
            }
        }

        private void OnEnable()
        {
            locationServiceLauncher = StartCoroutine(StartLocationService());
        }

        private void OnDisable()
        {
            StopCoroutine(locationServiceLauncher);
            locationServiceLauncher = null;
            Input.location.Stop();
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
                Debug.Log($"Location service ended with {0} status {Input.location.status}");
                Input.location.Stop();
            }
        }

        public void ShowGuidedText(string text)
        {
            guidePanel.transform.Find("Text").GetComponent<Text>().text = text;
            StartCoroutine(ShowPanel());
        }

        IEnumerator ShowPanel()
        {
            guidePanel.SetActive(true);
            yield return new WaitForSeconds(3);
            guidePanel.SetActive(false);
        }

        IEnumerator ShowObject()
        {
            objectToPlace.SetActive(false);
            yield return new WaitForSeconds(3);
            objectToPlace.SetActive(true);
        }
    }
}