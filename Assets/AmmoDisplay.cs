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
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        uiText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        var equippedGun = player.GetPlayerGun();
        uiText.text = equippedGun.GetCurrentAmmo() + "/" + equippedGun.maxAmmo;

    }
}
