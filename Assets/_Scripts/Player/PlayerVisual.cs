using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    [SerializeField] private MeshRenderer _headMeshRender;
    [SerializeField] private MeshRenderer _bodyMeshRender;

    private Material _material;

    private void Awake()
    {
        _material = new Material(_headMeshRender.material);
        _headMeshRender.material = _material;
        _bodyMeshRender.material = _material;
    }
    public void SetPlayerColor(Color color)
    {
        _material.color = color;
    }
}
