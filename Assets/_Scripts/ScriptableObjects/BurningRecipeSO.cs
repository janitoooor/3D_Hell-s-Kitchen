using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/CountersRecipe/BurningRecipeSO")]
public class BurningRecipeSO : ScriptableObject
{
    [SerializeField] private KitchenObjectSO _input;
    [SerializeField] private KitchenObjectSO _output;
    [SerializeField] private float _burningTimerMax;

    public KitchenObjectSO Input { get => _input; }
    public KitchenObjectSO Output { get => _output; }
    public float BurningTimerMax { get => _burningTimerMax; }
}
