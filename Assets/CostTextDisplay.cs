using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CostTextDisplay : MonoBehaviour
{
    TextMeshPro costText;
    private ShopCrate shopCrate;
    // Start is called before the first frame update
    void Start()
    {
        shopCrate = GetComponentInParent<ShopCrate>();
        costText = GetComponent<TextMeshPro>();
        costText.text = "(" + shopCrate.cost.ToString() + ")";
    }
}
