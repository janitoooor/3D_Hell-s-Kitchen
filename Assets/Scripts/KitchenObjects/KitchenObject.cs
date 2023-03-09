using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class KitchenObject : NetworkBehaviour
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

        //transform.parent = kitchenObjectParent.GetKitchenObjectFollowTransform();
        //transform.localPosition = Vector3.zero;
    }

    public virtual void DestroySelf()
    {
        _kitchenObjectParent.ClearKitchenObject();
        Destroy(gameObject);
    }

    public bool TryGetPlate(out PlateKitchenObject plateKitchenObject)
    {
        if (this is PlateKitchenObject)
        {
            plateKitchenObject = this as PlateKitchenObject;
            return true;
        }
        else
        {
            plateKitchenObject = null;
            return false;
        }
    }

    public static void SpawnKitchenObject(KitchenObjectSO kitchenObjectSO, IKitchenObjectParent kitchenObjectParent)
    {
        KitchenGameMultiplayer.Instance.SpawnKitchenObject(kitchenObjectSO, kitchenObjectParent);
    }
}

