﻿using System.Collections;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Collider))]
public class ShopCrate : MonoBehaviour
{
    public Transform showGunPosition;
    public Light openLight;
    private Color _openLightColor;
    public Animator animator;

    private Gun[] _guns;
    public bool playerCanInteract = false;
    public bool boxOpen = false;
    public bool gunReady = false;

    public bool denied;

    public int cost = 800;
    public int maxCost = 4000;

    private PlayerController _player;
    private InputSystemReader _inputReader;
    private Gun _randomGun;

    public float stayOpenTime;

    public AudioSource slotMachineSound;
    public AudioSource gunParty;
    public AudioSource purchaseSound;

    [SerializeField] private GameObject openButton;

    private void Start()
    {
        _openLightColor = openLight.color;
        _player = GameObject.FindWithTag("Player").transform.root.GetComponent<PlayerController>();
        _guns = GetComponentsInChildren<Gun>(true);
    }

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
        if (denied) return;

        if (!boxOpen && _player.playerMoney < cost)
        {
            denied = true;
            StartCoroutine(RedLight());
            return;
        }

        if (boxOpen && gunReady)
        {
            // Select Gun
            _player.SetPlayerGun(_randomGun.name);
            _randomGun.gameObject.SetActive(false);
            gunReady = false;
            return;
        }

        if (!boxOpen)
        {
            _player.AddMoney(-cost);
            cost = Mathf.Min(cost * 2, maxCost);
            purchaseSound.Play();
            boxOpen = true;
            openButton.SetActive(false);
            animator.Play("Open");
            StartCoroutine(DelayShow());
            return;
        }
    }

    IEnumerator RedLight()
    {
        openLight.color = Color.red;
        yield return new WaitForSeconds(1f);
        openLight.color = _openLightColor;
        denied = false;
    }

    IEnumerator DelayShow()
    {
        yield return new WaitForSeconds(0.5f);
        _randomGun = _guns[Random.Range(0, _guns.Length - 1)];
        slotMachineSound.Play();
        for (int i = 10; i < 30; i++)
        {
            yield return new WaitForSeconds(i * 0.01f);
            foreach (var gun in _guns)
            {
                gun.gameObject.SetActive(false);
            }

            _randomGun = _guns.Where(w => w != _randomGun).ToList()[Random.Range(0, _guns.Length - 1)];
            _randomGun.gameObject.SetActive(true);
        }

        gunReady = true;
        slotMachineSound.Stop();
        gunParty.PlayDelayed(0.1f);

        yield return new WaitForSeconds(stayOpenTime);

        _randomGun.gameObject.SetActive(false);
        gunReady = false;
        animator.Play("Close");

        yield return new WaitForSeconds(1f);
        openLight.enabled = false;
        // Cooldown
        yield return new WaitForSeconds(1f);

        openLight.enabled = true;
        boxOpen = false;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerCanInteract = false;
            openButton.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerCanInteract = true;
            if (!boxOpen)
            {
                openButton.SetActive(true);
            }
        }
    }
}