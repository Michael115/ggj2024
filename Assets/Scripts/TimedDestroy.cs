using UnityEngine;

public class TimedDestroy : MonoBehaviour
{
    public float duration = 5f;
    void Start()
    {
        Destroy(gameObject, duration);
    }
}