using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BaseCounter : NetworkBehaviour, IKitchenObjectParent
{
    public static event EventHandler OnAnyObjectPlacedHere;
    public static void ResetStaticData()
    {
        OnAnyObjectPlacedHere = null;
    }

    [SerializeField] private Transform _counterTopPoint;

    private KitchenObject _kitchenObject;

    public virtual void Interact(Player player)
    {
        Debug.LogError("BaseCounter.Interact();");
    }

    public virtual void InteractAlternate(Player player)
    {
        //Debug.LogError("BaseCounter.InteractAlternate();");
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        _kitchenObject = kitchenObject;

        if (_kitchenObject != null)
            OnAnyObjectPlacedHere?.Invoke(this, EventArgs.Empty);
    }

    public void ClearKitchenObject()
    {
        _kitchenObject = null;
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return _counterTopPoint;
    }

    public bool HasKitchenObject()
    {
        return _kitchenObject != null;
    }

    public KitchenObject GetKitchenObject()
    {
        return _kitchenObject;
    }

    public NetworkObject GetNetworkObject()
    {
        return NetworkObject;
    }
}
