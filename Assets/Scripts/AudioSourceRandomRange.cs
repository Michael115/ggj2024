using System.Linq;
using UnityEngine;

public class AudioSourceRandomRange : MonoBehaviour
{
    public AudioSource audioSource;
    
    // Want it so that there are distinct sets of clips that should only play together, so at the start select 1 set of clips, then select randomly from that selection
    public AudioClip[] audio1;
    public AudioClip[] audio2;
    public AudioClip[] audio3;

    private AudioClip[][] _clipSelector;
    private AudioClip[] _chosenClips;
    
    public float secondsBetweenSounds = 3f;
    private float _nextSoundTime = 0.0f;

    void Start()
    {
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
        if (Time.time >= _nextSoundTime)
        {
            // play a random clip for our chosen set
            var clip = _chosenClips[Random.Range(0, _chosenClips.Length)];
            audioSource.clip = clip;
            print("Playing clip");

            // Randomize pitch also
            audioSource.pitch = Random.Range(0.8f, 1.2f);
            audioSource.PlayOneShot(clip);
            _nextSoundTime = Time.time + secondsBetweenSounds;
        }
    }

}