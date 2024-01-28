using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ShopCrate : MonoBehaviour
{
    public Transform showGunPosition;
    public Light openLight;
    public Animator animator;

    public Transform[] guns;
    public bool playerCanInteract = false;
    public bool boxOpen = false;
    
    private InputSystemReader _inputReader;
    
    void OnEnable()
    {
        _inputReader ??= GameController.Instance.InputReader;
        if (_inputReader != null)
        {
            _inputReader.InteractEvent += OnInteract;
        }
    }
    void OnDisable()
    {
        _inputReader ??= GameController.Instance.InputReader;
        if (_inputReader != null)
        {
            _inputReader.InteractEvent -= OnInteract;
        }
    }
    
    private void OnInteract()
    {
        if (!playerCanInteract) return;

        if (boxOpen) return;
        
        animator.Play("Open");
        StartCoroutine(DelayShow(0.5f));
    }
    
    IEnumerator DelayShow(float delaySeconds)
    {
        yield return new WaitForSeconds(delaySeconds);
        
        foreach (var gun in guns)
        {
            gun.gameObject.SetActive(false);
        }
        
        var randomGun = guns[Random.Range(0, guns.Length)];
        randomGun.gameObject.SetActive(true);
    }
    
    private void OnTriggerLeave(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerCanInteract = false;
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerCanInteract = true;
        }
    }
}
