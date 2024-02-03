using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGameFade : MonoBehaviour
{
    // Start is called before the first frame update
    Image fadePanel;
    public float fadeTime = 5.0f;
    private float elapsedTime;

    void Start()
    {
        fadePanel = GetComponent<Image>();
        fadePanel.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
        elapsedTime = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        fadePanel.color = new Color(0.0f, 0.0f, 0.0f, Mathf.Sqrt(elapsedTime / fadeTime));

        
        if (elapsedTime > fadeTime)
        {
            SceneManager.LoadScene(1);
        }
    }
}
