using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DeliveryManager : NetworkBehaviour
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
        StartCoroutine(CheckIsServer());
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
                    DeliveryCorrectRecipeServerRpc(i);
                    return;
                }
            }
        }

        DeliveryIncorrectRecipeServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void DeliveryIncorrectRecipeServerRpc()
    {
        DeliveryIncorrectRecipeClientRpc();
    }

    [ClientRpc]
    private void DeliveryIncorrectRecipeClientRpc()
    {
        OnRecipeFailed?.Invoke(this, EventArgs.Empty);
    }

    [ServerRpc(RequireOwnership = false)]
    private void DeliveryCorrectRecipeServerRpc(int waitingRecipeSOListIndex)
    {
        DeliveryCorrectRecipeClientRpc(waitingRecipeSOListIndex);
    }

    [ClientRpc]
    private void DeliveryCorrectRecipeClientRpc(int waitingRecipeSOListIndex)
    {
        _successfulRecipesAmount++;
        _waitingRecipeSOList.RemoveAt(waitingRecipeSOListIndex);
        OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
        OnRecipeSuccess?.Invoke(this, EventArgs.Empty);
        StartCoroutine(SpawnRecipeTimer());
    }

    [ClientRpc]
    private void SpawnNewWaitingRecipeClientRpc(int indexWaitingRecipeSO)
    {
        RecipeSO waitingRecipeSO = _recipeListSO.RecipeSOList[indexWaitingRecipeSO];
        _waitingRecipeSOList.Add(waitingRecipeSO);
        OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
    }

    private IEnumerator SpawnRecipeTimer()
    {
        yield return new WaitForSeconds(_timeSpawn);

        if (_waitingRecipesAmountMax > _waitingRecipeSOList.Count)
        {
            if (KitchenGameManager.Instance.IsGamePlaying)
            {
                int indexWaitingRecipeSO = UnityEngine.Random.Range(0, _recipeListSO.RecipeSOList.Count);
                SpawnNewWaitingRecipeClientRpc(indexWaitingRecipeSO);
            }
            if (_waitingRecipesAmountMax > _waitingRecipeSOList.Count)
                StartCoroutine(SpawnRecipeTimer());
        }
    }

    private IEnumerator CheckIsServer()
    {
        yield return new WaitForSeconds(1);

        if (IsServer)
            StartCoroutine(SpawnRecipeTimer());
        else
            StartCoroutine(CheckIsServer());
    }
}
