using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AmmoDisplay : MonoBehaviour
{
    // Start is called before the first frame update

    private PlayerController player;
    private TextMeshProUGUI uiText;
    private Vector3 originalPos;

    public float shakeAmpMax;
    public float shakeSpeedMax;

    public float lowAmmoThreshold;

    public Color fullAmmoColor;
    public Color lowAmmoColor;
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        uiText = GetComponent<TextMeshProUGUI>();
        originalPos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        var equippedGun = player.GetPlayerGun();

        float ammo_prop = equippedGun.GetCurrentAmmo() / (float) equippedGun.maxAmmo;
        uiText.text = equippedGun.GetCurrentAmmo() + "/" + equippedGun.maxAmmo;

        if (ammo_prop < lowAmmoThreshold)
        {
            uiText.color = lowAmmoColor;
            transform.localPosition = originalPos + shakeAmpMax * Mathf.Sin(Time.time * shakeSpeedMax) * Vector3.left;
        }
        else
        {
            uiText.color = fullAmmoColor;
            transform.localPosition = originalPos;
        }
        

    }
}
