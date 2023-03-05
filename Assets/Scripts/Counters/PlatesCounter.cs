using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlatesCounter : BaseCounter
{
    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateRemoved;

    [SerializeField] private KitchenObjectSO _plateKitchenObjectSO;
    [Space]
    [SerializeField] private float _spawnPlateTimerMax = 4;
    [SerializeField] private int _platesSpawnedAmountMax = 2;


    private int _platesSpawnedAmount;
    private float _spawnPlateTimer;
    private void Update()
    {
        _spawnPlateTimer += Time.deltaTime;
        if (_spawnPlateTimer > _spawnPlateTimerMax)
        {
            _spawnPlateTimer = 0;

            if (_platesSpawnedAmount < _platesSpawnedAmountMax)
            {
                _platesSpawnedAmount++;
                OnPlateSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject() && _platesSpawnedAmount > 0)
        {
            _platesSpawnedAmount--;
            KitchenObject.SpawnKitchenObject(_plateKitchenObjectSO, player);

            OnPlateRemoved?.Invoke(this, EventArgs.Empty);
        }
    }
}
