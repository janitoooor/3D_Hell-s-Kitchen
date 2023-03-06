using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Recipe/RecipeListSO")]
public class RecipeListSO : ScriptableObject
{
    [SerializeField] private List<RecipeSO> _recipeSOList;
    public List<RecipeSO> RecipeSOList { get => _recipeSOList; }
}
