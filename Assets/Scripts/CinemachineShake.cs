using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineShake : MonoBehaviour {

    public CinemachineVirtualCamera cinemachineCamera;
    public CinemachineBasicMultiChannelPerlin virtualCameraNoise;
    public ShakeScriptable defaultProfile;
    private NoiseSettings originalNoise;

    private float originalAmplitude;
    private float originalFrequency;
    private Coroutine shakeCoroutine;

    public static CinemachineShake instance;
    private void Awake() {
        if (!instance) { instance = this; }
    }

    private void Start() {
        virtualCameraNoise = cinemachineCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private IEnumerator Shake(ShakeScriptable profile) {
        float timer = profile.shakeDuration;

        originalNoise = virtualCameraNoise.m_NoiseProfile;
        originalAmplitude = virtualCameraNoise.m_AmplitudeGain;
        originalFrequency = virtualCameraNoise.m_FrequencyGain;

        virtualCameraNoise.m_NoiseProfile = profile.noiseType;
        while (timer > 0) {
            virtualCameraNoise.m_AmplitudeGain = profile.amplitude.Evaluate(timer / profile.shakeDuration);
            virtualCameraNoise.m_FrequencyGain = profile.frequency.Evaluate(timer / profile.shakeDuration);


            timer -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        virtualCameraNoise.m_NoiseProfile = originalNoise;
        virtualCameraNoise.m_AmplitudeGain = originalAmplitude;
        virtualCameraNoise.m_FrequencyGain = originalFrequency;
    }

    public void StartShake(ShakeScriptable profile = null) {

        if (profile == null) { profile = defaultProfile; }

        if (shakeCoroutine != null) { StopCoroutine(shakeCoroutine); }
        shakeCoroutine = StartCoroutine(Shake(profile));

    }

    [ContextMenu("TestShake")]
    public void TestShake() {
        StartShake();
    }

}
