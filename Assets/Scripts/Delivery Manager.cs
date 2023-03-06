using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    public static DeliveryManager Instance { get; private set; }

    [SerializeField] private int _waitingRecipesAmountMax = 4;
    [SerializeField] private float _timeSpawn = 4f;
    [Space]
    [SerializeField] private RecipeListSO _recipeListSO;
    private List<RecipeSO> _waitingRecipeSOList;

    private void Awake()
    {
        Instance = this;
        _waitingRecipeSOList = new();
    }

    private void Start()
    {
        StartCoroutine(SpawnRecipeTimer());
    }

    public void DeliveryRecipe(PlateKitchenObject plateKitchenObject)
    {
        for (int i = 0; i < _waitingRecipeSOList.Count; i++)
        {
            RecipeSO waitingRecipeSO = _waitingRecipeSOList[i];

            if (waitingRecipeSO.KitchenObjectSOList.Count == plateKitchenObject.PlateKitchenObjectSOList.Count)
            {
                bool plateContentsMatchesRecipe = true;
                foreach (KitchenObjectSO recipeKitchenObjectSO in waitingRecipeSO.KitchenObjectSOList)
                {
                    bool ingridientFound = false;
                    foreach (KitchenObjectSO plateKitchenObjectSO in plateKitchenObject.PlateKitchenObjectSOList)
                    {
                        if (plateKitchenObjectSO == recipeKitchenObjectSO)
                        {
                            ingridientFound = true;
                            break;
                        }
                    }
                    if (!ingridientFound)
                    {
                        plateContentsMatchesRecipe = false;
                    }
                }

                if (plateContentsMatchesRecipe)
                {
                    print("Delivered Correct!");
                    _waitingRecipeSOList.RemoveAt(i);
                    return;
                }
            }
        }

        print("not correct recipe was delivered!");
    }

    private IEnumerator SpawnRecipeTimer()
    {
        yield return new WaitForSeconds(_timeSpawn);

        if (_waitingRecipesAmountMax > _waitingRecipeSOList.Count)
        {
            RecipeSO waitingRecipeSO = _recipeListSO.RecipeSOList[Random.Range(0, _recipeListSO.RecipeSOList.Count)];
            _waitingRecipeSOList.Add(waitingRecipeSO);
            print(waitingRecipeSO.RecipeName);
        }

        StartCoroutine(SpawnRecipeTimer());
    }
}
