using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    public static event EventHandler OnAnyPlateDestroyed;
    public static void ResetStaticData()
    {
        OnAnyPlateDestroyed = null;
    }

    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;
    public class OnIngredientAddedEventArgs : EventArgs
    {
        public KitchenObjectSO KitchenObjectSO;
    }

    [SerializeField] private List<KitchenObjectSO> _validKitchenObjectSOList;

    private List<KitchenObjectSO> _kitchenObjectSOList;

    public List<KitchenObjectSO> PlateKitchenObjectSOList { get => _kitchenObjectSOList; }

    protected override void Awake()
    {
        base.Awake();
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
            AddIngredientServerRpc(KitchenGameMultiplayer.Instance.GetKitchenObjectSOIndex(kitchenObjectSO));
            return true;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void AddIngredientServerRpc(int kitchenObjectSOIndex)
    {
        AddIngredientClientRpc(kitchenObjectSOIndex);
    }

    [ClientRpc]
    private void AddIngredientClientRpc(int kitchenObjectSOIndex)
    {
        KitchenObjectSO kitchenObjectSO = KitchenGameMultiplayer.Instance.GetKitchenObjectSOFromIndex(kitchenObjectSOIndex);
        _kitchenObjectSOList.Add(kitchenObjectSO);

        OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs
        {
            KitchenObjectSO = kitchenObjectSO,
        });
    }

    public override void DestroySelf()
    {
        OnAnyPlateDestroyed?.Invoke(this, EventArgs.Empty);
        base.DestroySelf();
    }
}
