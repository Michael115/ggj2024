using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UITextPop : MonoBehaviour
{
    // Start is called before the first frame update
    private TextMeshProUGUI uiText;
    public float popSize = 1.5f;
    public float popTime = 2.0f;
    private float popTimeRemaining = 0f;

    void Start()
    {
        uiText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        popTimeRemaining = Mathf.Max(popTimeRemaining - Time.deltaTime, 0.0f);
        var timeProp = popTimeRemaining / popTime;
        transform.localScale = timeProp * (popSize * Vector3.one) + (1f - timeProp) * Vector3.one;
    }

    public void PopText()
    {
        popTimeRemaining = popTime;
    }
}
