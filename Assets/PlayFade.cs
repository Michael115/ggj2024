using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class PlayFade : MonoBehaviour
{
    public GameObject fadePanel;
    private AudioSource playSound;
    // Start is called before the first frame update
    void Start()
    {
        playSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        print("Game Starting...");
        var button = GetComponent<Button>();
        button.interactable = false;

        playSound.Play();
        fadePanel.SetActive(true);
    }
}
