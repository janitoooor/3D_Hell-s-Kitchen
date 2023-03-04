using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{
    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            if (player.HasKitchenObject())
                player.GetKitchenObject().SetKitchenObjectParent(this);
        }
        else
        {
            if (!player.HasKitchenObject())
            {
                GetKitchenObject().SetKitchenObjectParent(player);
            }
            else if (player.GetKitchenObject() is PlateKitchenObject)
            {
                PlateKitchenObject plateKitchenObject = player.GetKitchenObject() as PlateKitchenObject;
                if (plateKitchenObject.TryAddIngridient(GetKitchenObject().KitchenObjectSO))
                    GetKitchenObject().DestroySelf();
            }
        }

    }
}
