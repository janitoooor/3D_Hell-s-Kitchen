using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO _kitchenObjectSO;

    private IKitchenObjectParent _kitchenObjectParent;
    public IKitchenObjectParent KitchenObjectParent { get => _kitchenObjectParent; }

    public KitchenObjectSO KitchenObjectSO { get => _kitchenObjectSO; }

    public void SetKitchenObjectParent(IKitchenObjectParent kitchenObjectParent)
    {
        _kitchenObjectParent?.ClearKitchenObject();

        _kitchenObjectParent = kitchenObjectParent;
        if (_kitchenObjectParent.HasKitchenObject())
            Debug.LogError("KitchenObjectParent already has a KitchenObject!");

        _kitchenObjectParent.SetKitchenObject(this);
        transform.parent = kitchenObjectParent.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }
}

