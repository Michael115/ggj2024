using System.Collections;
using System.Linq;
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
    public bool gunReady = false;
    
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

        if (boxOpen)
        {
            
        }
        else
        {
            animator.Play("Open");
            StartCoroutine(DelayShow(0.5f));
        }
    }
    
    IEnumerator DelayShow(float delaySeconds)
    {  
        yield return new WaitForSeconds(0.5f);
        var randomGun = guns[Random.Range(0, guns.Length-1)];
        for (int i = 10; i < 30; i++)
        {
            yield return new WaitForSeconds(i*0.01f);
            foreach (var gun in guns)
            {
                gun.gameObject.SetActive(false);
            }
            randomGun = guns.Where(w=>w != randomGun).ToList()[Random.Range(0, guns.Length-1)];
            randomGun.gameObject.SetActive(true);
        }
    }
    
    IEnumerator DelayClose(float delaySeconds)
    {
        for (int i = 0; i < 20; i++)
        {
            yield return new WaitForSeconds(0.2f);
            foreach (var gun in guns)
            {
                gun.gameObject.SetActive(false);
            }
            var randomGun = guns[Random.Range(0, guns.Length)];
            randomGun.gameObject.SetActive(true);
        }
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
