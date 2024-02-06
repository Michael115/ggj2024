using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIImageFadeIn : MonoBehaviour
{
    // Start is called before the first frame update

    private Image uiImage;
    public float fadeInTime;

    void Start()
    {
        uiImage = GetComponent<Image>();
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        var elapsedTime = 0.0f;
        while(elapsedTime < fadeInTime)
        {
            elapsedTime += Time.fixedUnscaledDeltaTime;
            uiImage.color = new Color(1f, 1f, 1f, elapsedTime / fadeInTime);
            yield return new WaitForSecondsRealtime(Time.fixedUnscaledDeltaTime);
        }
    }

}
