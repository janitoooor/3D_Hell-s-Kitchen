using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(menuName = "ScriptableObjects/KitchenObjects/KitchenObjectListSO")]
public class KitchenObjectListSO : ScriptableObject
{
    [SerializeField] private List<KitchenObjectSO> _kitchenObjectSOList;
    public List<KitchenObjectSO> KitchenObjectSOList { get => _kitchenObjectSOList; }
}
