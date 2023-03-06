using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Recipe/RecipeSO")]
public class RecipeSO : ScriptableObject
{
    [SerializeField] private string _recipeName;
    [SerializeField] private List<KitchenObjectSO> _kitchenObjectSOList;
    public List<KitchenObjectSO> KitchenObjectSOList { get => _kitchenObjectSOList; }
    public string RecipeName { get => _recipeName; }
}
