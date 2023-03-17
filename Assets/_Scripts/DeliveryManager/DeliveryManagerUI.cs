using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManagerUI : MonoBehaviour
{
    [SerializeField] private int _amountCreateTemplate = 4;
    [Space]
    [SerializeField] private Transform _container;
    [SerializeField] private Transform _recipeTemplate;

    private List<DeliveryManagerSingleUI> _recipeSingleUI;

    private void Awake()
    {
        _recipeSingleUI = new();

        for (int i = 0; i < _amountCreateTemplate; i++)
            InstantiateTemplate();
    }

    private void Start()
    {
        DeliveryManager.Instance.OnRecipeCompleted += DeliveryManager_OnRecipeCompleted;
        DeliveryManager.Instance.OnRecipeSpawned += DeliveryManager_OnRecipeSpawned;

        UpdateVisual();
    }

    private void DeliveryManager_OnRecipeSpawned(object sender, System.EventArgs e)
    {
        UpdateVisual();
    }

    private void DeliveryManager_OnRecipeCompleted(object sender, System.EventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        foreach (Transform child in _container.transform)
            child.gameObject.SetActive(false);

        for (int i = 0; i < DeliveryManager.Instance.WaitingRecipeSOList.Count; i++)
        {
            _recipeSingleUI[i].gameObject.SetActive(true);
            _recipeSingleUI[i].SetRecipeSO(DeliveryManager.Instance.WaitingRecipeSOList[i]);
        }
    }

    private void InstantiateTemplate()
    {
        Transform recipeTransform = Instantiate(_recipeTemplate, _container);
        recipeTransform.gameObject.SetActive(true);
        _recipeSingleUI.Add(recipeTransform.GetComponent<DeliveryManagerSingleUI>());
    }
}
