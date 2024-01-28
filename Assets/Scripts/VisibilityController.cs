using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class VisibilityController : MonoBehaviour
{
    [SerializeField] private Material[] seeThroughMaterials;
    private readonly List<MeshRenderer> _meshRenderers = new();

    private static readonly int SizeID = Shader.PropertyToID("_Size");

    private void Start()
    {
        foreach (var material in seeThroughMaterials)
        {
            material.SetFloat(SizeID, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Inside"))
        {
            if (_meshRenderers.Count > 0) return;

            foreach (var material in seeThroughMaterials)
            {
                material.SetFloat(SizeID, 1);
            }

            var floor = other.transform.parent;
            var nextFloor = floor.parent.GetChild(floor.GetSiblingIndex() + 1);
            foreach (var meshRenderer in nextFloor.GetComponentsInChildren<MeshRenderer>())
            {
                _meshRenderers.Add(meshRenderer);
                meshRenderer.enabled = false;
            }
        }
        // We can't easily use OnTriggerExit as we have multiple colliders within a single building
        else if (other.CompareTag("Outside"))
        {
            foreach (var material in seeThroughMaterials)
            {
                material.SetFloat(SizeID, 0);
            }
            foreach (var meshRenderer in _meshRenderers)
            {
                meshRenderer.enabled = true;
            }

            _meshRenderers.Clear();
        }
    }
}