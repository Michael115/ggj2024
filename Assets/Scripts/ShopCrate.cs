using System;
using UnityEngine;


public class ShopCrate : MonoBehaviour
{
    public BoxCollider interactArea;
    private InputSystemReader _inputReader;
    public bool playerCanInteract;

    public Transform showGunPosition;
    
    public Light openLight;
    public Animator animator;
    public bool boxOpen = false;

    public GameObject guns;
    
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
        
        print("Player can interact");
        
        if (!boxOpen)
        {
            print("Play anim");
            animator.Play("Open");
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
        print("Player at crate");
        if (other.CompareTag("Player"))
        {
            playerCanInteract = true;
        }
    }
}
