using UnityEngine;
using DG.Tweening;
using Unity.Cinemachine;
using System.Collections.Generic;

public class AttackShake : MonoBehaviour 
{
    public static AttackShake instance;
    [Header("Camera Shake Settings")]
    public Camera mainCamera;
    public float cameraShakeDuration = 0.5f;
    public float cameraShakeStrength = 0.1f;
    public int cameraShakeVibrato = 10;
    public float cameraShakeRandomness = 90f; // Giá trị từ 0 đến 180
    public bool cameraShakeFadeOut = true;

    [Header("Object Shake Settings")]
    public Transform[] objectsToShake;
    public float objectShakeDuration = 0.3f;
    public float objectShakeStrength = 0.2f;
    public int objectShakeVibrato = 10;
    public float objectShakeRandomness = 90f; // Giá trị từ 0 đến 180
    public bool objectShakeFadeOut = true;

    [Header("Cinamachine")]
    public float intensity = 0.12f;
    public float shakeTime = 0.5f;
    public List<CinemachineCamera> lsCams;
    void Start()
    {
        instance = this;
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    public void TriggerAttackShake()
    {
        ShakeCamera();
        ShakeObjects();
    }

    void ShakeCamera()
    {
        if (mainCamera != null)
        {
            mainCamera.DOShakePosition(cameraShakeDuration, cameraShakeStrength, cameraShakeVibrato, cameraShakeRandomness, cameraShakeFadeOut);
        }
        else
        {
            Debug.LogWarning("Main Camera chưa được gán cho HeroAttackShake script.");
        }
    }

    void ShakeObjects()
    {
        if (objectsToShake != null && objectsToShake.Length > 0)
        {
            foreach (Transform objTransform in objectsToShake)
            {
                if (objTransform != null)
                {
                    // Rung vị trí của từng object
                   
                    objTransform.DOShakePosition(objectShakeDuration,
                                                new Vector3(objectShakeStrength, objectShakeStrength, 0),
                                                objectShakeVibrato,
                                                objectShakeRandomness, // Tham số này kiểm soát độ ngẫu nhiên của hướng
                                                objectShakeFadeOut);
                }
            }
        }
    }
    CinemachineCamera cam = new CinemachineCamera();
    public void ShakeCineCamera(float time =1f)
    {
       
        foreach(CinemachineCamera c in lsCams)
        {
            if (c.gameObject.activeSelf)
            {
                cam = c;
                break;
            }

        }
        var cine = cam.GetComponent<CinemachineBasicMultiChannelPerlin>();
        cine.AmplitudeGain = intensity;
        shakeTime = time;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TriggerAttackShake();
        }
        if (shakeTime > 0)
        {
            shakeTime -= Time.deltaTime;
            if (shakeTime <= 0)
            {
                var cine = cam.GetComponent<CinemachineBasicMultiChannelPerlin>();
                cine.AmplitudeGain = 0;
            }
        }
    }
}