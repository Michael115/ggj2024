using System.Linq;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioSourceRandomRange : MonoBehaviour
{
    private AudioSource audioSource;
    
    // Want it so that there are distinct sets of clips that should only play together, so at the start select 1 set of clips, then select randomly from that selection
    public AudioClip[] audio1;
    public AudioClip[] audio2;
    public AudioClip[] audio3;

    private AudioClip[][] _clipSelector;
    private AudioClip[] _chosenClips;

    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();

        _clipSelector = new []
        {
            audio1,
            audio2,
            audio3
        };
        
        // Select the set of clips to choose from hereafter
        _chosenClips = _clipSelector[Random.Range(0, _clipSelector.Length)];
    }

    public void PlayRandom()
    {
        // play a random clip for our chosen set
        var clip = _chosenClips[Random.Range(0, _chosenClips.Length)];
        audioSource.clip = clip;
        audioSource.Play();
    }

}