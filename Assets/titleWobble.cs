using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class titleWobble : MonoBehaviour
{

    Vector3 originalScale;
    Vector3 originalPos;
    public float scaleAmp = 0.1f;
    public float scaleSpeed = 1.0f;

    public float wobbleAmp = 0.1f;
    public float wobbleSpeed = 1.0f;

    Text textComp;

    // Start is called before the first frame update
    void Start()
    {
        originalScale = transform.localScale;
        originalPos = transform.localPosition;
        textComp = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = originalScale + new Vector3(1.0f, 1.0f, 1.0f) * scaleAmp * Mathf.Sin(scaleSpeed * Time.time);
        transform.localPosition = originalPos + new Vector3(wobbleAmp * Mathf.Cos(wobbleSpeed * Time.time), 0.0f, 0.0f); 
    }
}
