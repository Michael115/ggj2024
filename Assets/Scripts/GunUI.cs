using UnityEngine;

public class GunUI : MonoBehaviour
{
    private Gun[] _allGuns;

    private void Awake()
    {
        _allGuns = GetComponentsInChildren<Gun>(true);
    }

    public void SetGun(string gunName)
    {
        foreach (var gun in _allGuns)
        {
            gun.gameObject.SetActive(gun.name == gunName);
        }
    }
}