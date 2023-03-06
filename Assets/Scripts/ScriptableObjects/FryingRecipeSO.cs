using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/CountersRecipe/FryingRecipeSO")]
public class FryingRecipeSO : ScriptableObject
{
    [SerializeField] private KitchenObjectSO _input;
    [SerializeField] private KitchenObjectSO _output;
    [SerializeField] private float _fryingTimerMax;

    public KitchenObjectSO Input { get => _input; }
    public KitchenObjectSO Output { get => _output; }
    public float FryingTimerMax { get => _fryingTimerMax; }
}
