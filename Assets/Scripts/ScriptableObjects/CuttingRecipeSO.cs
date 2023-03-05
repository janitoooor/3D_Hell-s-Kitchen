using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class CuttingRecipeSO : ScriptableObject
{
    [SerializeField] private KitchenObjectSO _input;
    [SerializeField] private KitchenObjectSO _output;
    [SerializeField] private int _cuttingProgressMax;

    public KitchenObjectSO Input { get => _input; }
    public KitchenObjectSO Output { get => _output; }
    public int CuttingProgressMax { get => _cuttingProgressMax; }
}
