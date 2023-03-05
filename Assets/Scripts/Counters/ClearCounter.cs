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
            else if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
            {
                if (plateKitchenObject.TryAddIngredient(GetKitchenObject().KitchenObjectSO))
                    GetKitchenObject().DestroySelf();
            }
            else if (GetKitchenObject().TryGetPlate(out plateKitchenObject))
            {
                if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().KitchenObjectSO))
                    player.GetKitchenObject().DestroySelf();
            }
        }
    }
}
