using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Netcode;

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

    private void Awake()
    {
        PlateKitchenObject.OnAnyPlateDestroyed += PlateKitchenObject_OnPlateDestroyed;
    }

    private void Update()
    {
        if (!IsServer)
            return;

        _spawnPlateTimer += Time.deltaTime;
        if (_spawnPlateTimer > _spawnPlateTimerMax)
        {
            _spawnPlateTimer = 0;

            if (KitchenGameStateMachine.Instance.IsGamePlaying && _platesSpawnedAmount < _platesSpawnedAmountMax)
                SpawnPlateServerRpc();
        }
    }

    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject() && _platesSpawnedAmount > 0)
        {
            KitchenObject.SpawnKitchenObject(_plateKitchenObjectSO, player);
            InteractLogicServerRpc();
        }
    }

    [ServerRpc]
    private void SpawnPlateServerRpc()
    {
        SpawnPlateClientRpc();
    }

    [ClientRpc]
    private void SpawnPlateClientRpc()
    {
        _platesSpawnedAmount++;
        OnPlateSpawned?.Invoke(this, EventArgs.Empty);
    }

    private void PlateKitchenObject_OnPlateDestroyed(object sender, EventArgs e)
    {
        _platesSpawnedAmount--;
    }

    [ServerRpc(RequireOwnership = false)]
    private void InteractLogicServerRpc()
    {
        InteractLogicClientRpc();
    }

    [ClientRpc]
    private void InteractLogicClientRpc()
    {
        OnPlateRemoved?.Invoke(this, EventArgs.Empty);
    }
}
