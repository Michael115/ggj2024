using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class StartInstructions : MonoBehaviour
{
    public float torchTurnOnDelay;
    public float musicInstructionDelay;
    public float textScrollDelay;
    public float fadeTime;
    // Start is called before the first frame update
    public TextMeshProUGUI startUI;
    public AudioSource sceneMusic;
    public AudioSource torchOnSound;
    public Light torchLight;

    void Start()
    {
        startUI.text = "";
        torchLight.enabled = false;
        StartCoroutine(InstructStart());
    }

    IEnumerator InstructStart()
    {
        yield return new WaitForSecondsRealtime(torchTurnOnDelay);
        torchOnSound.Play();
        torchLight.enabled = true;
        yield return new WaitForSecondsRealtime(musicInstructionDelay);


        sceneMusic.Play();


        var full_phrase = "Do Not Get Tickled".Split(' ').ToList();
        startUI.text = full_phrase[0];

        yield return null;

        for(var i = 1; i < full_phrase.Count; i++)
        {
            yield return new WaitForSecondsRealtime(textScrollDelay);
            startUI.text = String.Join(' ', full_phrase.GetRange(0, i + 1));
        }


        var elapsedTime = 0.0f;
        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.fixedDeltaTime;
            startUI.alpha = 1.0f - elapsedTime / fadeTime;
            yield return new WaitForFixedUpdate();
        }

        // Destroy itself?
        gameObject.SetActive(false);

    }
}
