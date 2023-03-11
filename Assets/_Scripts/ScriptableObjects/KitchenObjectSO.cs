using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/KitchenObjects/KitchenObjectSO")]
public class KitchenObjectSO : ScriptableObject
{
    [SerializeField] private Transform _prefab;
    [SerializeField] private Sprite _sprite;
    [SerializeField] private string _objectName;

    public Transform Prefab { get => _prefab; }
    public Sprite Sprite { get => _sprite; }
    public string ObjectName { get => _objectName; }
}
