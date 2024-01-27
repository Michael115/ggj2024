using UnityEngine;

public class CircleSync : MonoBehaviour
{
    [SerializeField] private Material material;
    [SerializeField] private new Camera camera;
    [SerializeField] private LayerMask mask;

    private static readonly int SizeID = Shader.PropertyToID("_Size");
    private float _size;

    private void Start()
    {
        _size = Mathf.Max(1, material.GetFloat(SizeID));
    }

    private void Update()
    {
        var position = transform.position;
        var direction = camera.transform.position - position;
        var ray = new Ray(position, direction);
        if (Physics.Raycast(ray, 1000, mask))
        {
            material.SetFloat(SizeID, _size);
        }
        else
        {
            material.SetFloat(SizeID, 0);
        }
    }
}