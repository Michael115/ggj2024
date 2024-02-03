using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFovBreath : MonoBehaviour
{
    Camera cam;
    float origFov;
    public float breathSpeed = 1.0f;
    public float breathAmp = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        origFov = cam.fieldOfView;
    }

    // Update is called once per frame
    void Update()
    {
        cam.fieldOfView = origFov + breathAmp * Mathf.Sin(Time.time * breathSpeed);
    }
}
