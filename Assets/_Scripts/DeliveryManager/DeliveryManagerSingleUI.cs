using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryManagerSingleUI : MonoBehaviour
{
    [SerializeField] private int _amountCreatedTemplates = 5;
    [Space]
    [SerializeField] private TextMeshProUGUI _recipeNameText;
    [SerializeField] private Transform _iconContainer;
    [SerializeField] private Transform _iconTemplate;

    private List<Image> _createsTemplates;

    private void Awake()
    {
        _createsTemplates = new();

        for (int i = 0; i < _amountCreatedTemplates; i++)
            InstantiateTemplates();
    }

    public void SetRecipeSO(RecipeSO recipeSO)
    {
        _recipeNameText.text = recipeSO.RecipeName;

        foreach (Transform child in _iconContainer)
            child.gameObject.SetActive(false);

        for (int i = 0; i < recipeSO.KitchenObjectSOList.Count; i++)
        {
            _createsTemplates[i].gameObject.SetActive(true);
            _createsTemplates[i].sprite = recipeSO.KitchenObjectSOList[i].Sprite;
        }
    }

    private void InstantiateTemplates()
    {
        Transform iconTransform = Instantiate(_iconTemplate, _iconContainer);
        iconTransform.gameObject.SetActive(false);
        _createsTemplates.Add(iconTransform.GetComponent<Image>());
    }
}
