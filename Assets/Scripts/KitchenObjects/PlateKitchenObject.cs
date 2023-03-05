using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;
    public class OnIngredientAddedEventArgs : EventArgs
    {
        public KitchenObjectSO KitchenObjectSO;
    }


    [SerializeField] private List<KitchenObjectSO> _validKitchenObjectSOList;

    private List<KitchenObjectSO> _kitchenObjectSOList;

    public List<KitchenObjectSO> PlateKitchenObjectSOList { get => _kitchenObjectSOList; }

    private void Awake()
    {
        _kitchenObjectSOList = new();
    }
    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO)
    {
        if (!_validKitchenObjectSOList.Contains(kitchenObjectSO))
            return false;

        if (_kitchenObjectSOList.Contains(kitchenObjectSO))
        {
            return false;
        }
        else
        {
            _kitchenObjectSOList.Add(kitchenObjectSO);

            OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs
            {
                KitchenObjectSO = kitchenObjectSO,
            });
            return true;
        }
    }
}
