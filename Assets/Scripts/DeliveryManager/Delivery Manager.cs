using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeCompleted;
    public event EventHandler OnRecipeSuccess;
    public event EventHandler OnRecipeFailed;

    public static DeliveryManager Instance { get; private set; }

    [SerializeField] private int _waitingRecipesAmountMax = 4;
    [SerializeField] private float _timeSpawn = 4f;
    [Space]
    [SerializeField] private RecipeListSO _recipeListSO;

    private List<RecipeSO> _waitingRecipeSOList;
    public List<RecipeSO> WaitingRecipeSOList { get => _waitingRecipeSOList; }

    private int _successfulRecipesAmount;
    public int SuccessfulRecipesAmount { get => _successfulRecipesAmount; }

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
                    _successfulRecipesAmount++;
                    _waitingRecipeSOList.RemoveAt(i);
                    OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
                    OnRecipeSuccess?.Invoke(this, EventArgs.Empty);
                    StartCoroutine(SpawnRecipeTimer());
                    return;
                }
            }
        }

        OnRecipeFailed?.Invoke(this, EventArgs.Empty);
    }

    private IEnumerator SpawnRecipeTimer()
    {
        yield return new WaitForSeconds(_timeSpawn);

        if (_waitingRecipesAmountMax > _waitingRecipeSOList.Count)
        {
            if (KitchenGameManager.Instance.IsGamePlaying)
            {
                RecipeSO waitingRecipeSO = _recipeListSO.RecipeSOList[UnityEngine.Random.Range(0, _recipeListSO.RecipeSOList.Count)];
                _waitingRecipeSOList.Add(waitingRecipeSO);
                OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
            }
            if (_waitingRecipesAmountMax > _waitingRecipeSOList.Count)
                StartCoroutine(SpawnRecipeTimer());
        }
    }
}
