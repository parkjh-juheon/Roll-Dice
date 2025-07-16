using System;
using Unity.Cinemachine;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;

    private CinemachineCamera vcam;
    private CinemachineBasicMultiChannelPerlin noise;

    [Header("셰이크 설정")]
    public float shakeDuration = 0.3f;
    public float shakeAmplitude = 2.0f;
    public float shakeFrequency = 2.0f;

    private float shakeTimer;
    private Vector3 mainCamOriginalPosition; // 메인 카메라 위치 저장용
    private Transform mainCamTransform;      // 메인 카메라 Transform 참조

    void Awake()
    {
        Instance = this;
        vcam = GetComponent<CinemachineCamera>();
        if (vcam != null)
        {
            noise = vcam.GetComponentInChildren<CinemachineBasicMultiChannelPerlin>();
        }
        if (noise == null)
        {
            Debug.LogError("CinemachineBasicMultiChannelPerlin 컴포넌트를 찾을 수 없습니다.");
        }

        // 메인 카메라 Transform 찾기
        if (Camera.main != null)
        {
            mainCamTransform = Camera.main.transform;
            mainCamOriginalPosition = mainCamTransform.position;
        }
        else
        {
            Debug.LogError("Main Camera를 찾을 수 없습니다.");
        }
    }

    void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;

            if (shakeTimer <= 0f)
            {
                StopShake();
            }
        }
    }
    public void StopShake()
    {
        if (noise == null) return;
        noise.AmplitudeGain = 0f;
        noise.FrequencyGain = 0f;

        // 메인 카메라 위치 복구
        if (mainCamTransform != null)
            mainCamTransform.position = mainCamOriginalPosition;
    }

    internal void ShakeCamera()
    {
        if (noise == null) return;

        // 흔들기 전 메인 카메라 위치 저장
        if (mainCamTransform != null)
            mainCamOriginalPosition = mainCamTransform.position;

        noise.AmplitudeGain = shakeAmplitude;
        noise.FrequencyGain = shakeFrequency;
        shakeTimer = shakeDuration;
    }

}
