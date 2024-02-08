using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamShakeEffects : MonoBehaviour
{
    // Start is called before the first frame update
    CinemachineVirtualCamera vCam;
    CinemachineBasicMultiChannelPerlin vNoise;

    private float shakeAmp = 0.75f;
    private float shakeTimeLeft = 0.0f;

    void Start()
    {
        vCam = GetComponent<CinemachineVirtualCamera>();
        vNoise = vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        vNoise.m_AmplitudeGain = 0.0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        shakeTimeLeft = Mathf.Max(shakeTimeLeft - Time.fixedDeltaTime, 0.0f);
        if (shakeTimeLeft > 0.0f)
        {
            vNoise.m_AmplitudeGain = shakeAmp;
        }
        else
        {
            vNoise.m_AmplitudeGain = 0.0f;
        }
    }

    public void ShakeCamera()
    {
        shakeTimeLeft = 0.1f;
    }

}
