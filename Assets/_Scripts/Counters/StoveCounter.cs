using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Netcode;

public class StoveCounter : BaseCounter, IHasProgress
{
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    public event EventHandler<OnStateChangeEventArgs> OnStateChanged;
    public class OnStateChangeEventArgs : EventArgs
    {
        public State State;
    }

    public enum State
    {
        Idle,
        Frying,
        Fried,
        Burned,
    }

    [SerializeField] private BurningRecipeSO[] _burningRecipeSOArray;
    [SerializeField] private FryingRecipeSO[] _fryingRecipeSOArray;

    public bool IsFried { get => _state.Value == State.Fried; }

    private readonly NetworkVariable<State> _state = new(State.Idle);
    private readonly NetworkVariable<float> _fryingTimer = new(0f);
    private readonly NetworkVariable<float> _burningTimer = new(0f);

    private FryingRecipeSO _fryingRecipeSO;
    private BurningRecipeSO _burningRecipeSO;

    private void Start()
    {
        _state.Value = State.Idle;
    }

    private void Update()
    {
        if (!IsServer)
            return;

        if (HasKitchenObject())
        {
            switch (_state.Value)
            {
                case State.Idle:
                    break;
                case State.Frying:
                    StartStateFrying();
                    break;
                case State.Fried:
                    StartStateFried();
                    break;
                case State.Burned:
                    break;
            }
        }
    }

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
            GetKitchenObjectAndStartCook(player);
        else if (_state.Value == State.Fried || _state.Value == State.Burned)
            GivePlayerKitchenObject(player);
    }

    public override void OnNetworkSpawn()
    {
        _fryingTimer.OnValueChanged += FryingTimer_OnValueChanged;
        _burningTimer.OnValueChanged += BurningTimer_OnValueChanged;
        _state.OnValueChanged += State_OnValueChanged;
    }

    private void FryingTimer_OnValueChanged(float previousValue, float newValue)
    {
        float fryingTimerMax = _fryingRecipeSO != null ? _fryingRecipeSO.FryingTimerMax : 1f;

        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
        {
            ProgressNormalized = _fryingTimer.Value / fryingTimerMax
        });
    }

    private void BurningTimer_OnValueChanged(float previousValue, float newValue)
    {
        float burningTimerMax = _burningRecipeSO != null ? _burningRecipeSO.BurningTimerMax : 1f;

        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
        {
            ProgressNormalized = _burningTimer.Value / burningTimerMax
        });
    }

    private void State_OnValueChanged(State previousValue, State newValue)
    {
        OnStateChanged?.Invoke(this, new OnStateChangeEventArgs
        {
            State = _state.Value
        });

        if (_state.Value == State.Burned || _state.Value == State.Idle)
        {
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {
                ProgressNormalized = 0
            });
        }
    }

    private void StartStateFrying()
    {
        _fryingTimer.Value += Time.deltaTime;

        if (_fryingTimer.Value > _fryingRecipeSO.FryingTimerMax)
        {
            _fryingTimer.Value = 0;

            KitchenObject.DestroyKitchenObject(GetKitchenObject());
            KitchenObject.SpawnKitchenObject(_fryingRecipeSO.Output, this);

            SetBurningRecipeSOClientRpc(
                KitchenGameMultiplayer.Instance.GetKitchenObjectSOIndex(GetKitchenObject().KitchenObjectSO)
                );
            _state.Value = State.Fried;
        }
    }

    private void StartStateFried()
    {
        _burningTimer.Value += Time.deltaTime;

        if (_burningTimer.Value > _burningRecipeSO.BurningTimerMax)
        {
            KitchenObject.DestroyKitchenObject(GetKitchenObject());
            KitchenObject.SpawnKitchenObject(_burningRecipeSO.Output, this);
            _state.Value = State.Burned;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void InteractLogicPlaceObjectOnCounterServerRpc(int kitchenObjectSOIndex)
    {
        _fryingTimer.Value = 0;
        _burningTimer.Value = 0;
        _state.Value = State.Frying;

        SetFryingRecipeSOClientRpc(kitchenObjectSOIndex);
    }

    [ClientRpc]
    private void SetBurningRecipeSOClientRpc(int kitchenObjectSOIndex)
    {
        KitchenObjectSO kitchenObjectSO = KitchenGameMultiplayer.Instance.GetKitchenObjectSOFromIndex(kitchenObjectSOIndex);
        _burningRecipeSO = GetBurningRecipeSOWithInput(kitchenObjectSO);
    }

    [ClientRpc]
    private void SetFryingRecipeSOClientRpc(int kitchenObjectSOIndex)
    {
        KitchenObjectSO kitchenObjectSO = KitchenGameMultiplayer.Instance.GetKitchenObjectSOFromIndex(kitchenObjectSOIndex);
        _fryingRecipeSO = GetFryingRecipeSOWithInput(kitchenObjectSO);
    }

    private void GetKitchenObjectAndStartCook(Player player)
    {
        if (player.HasKitchenObject() && HasRecipeWithInput(player.GetKitchenObject().KitchenObjectSO))
        {
            KitchenObject kitchenObject = player.GetKitchenObject();
            kitchenObject.SetKitchenObjectParent(this);

            InteractLogicPlaceObjectOnCounterServerRpc(
                KitchenGameMultiplayer.Instance.GetKitchenObjectSOIndex(kitchenObject.KitchenObjectSO)
            );
        }
    }

    private void GivePlayerKitchenObject(Player player)
    {
        if (!player.HasKitchenObject())
            GivePlayerKitchenObjectWithoutPlate(player);
        else if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
            GivePlayerKitchenObjectOnPlate(plateKitchenObject);
    }

    private void GivePlayerKitchenObjectWithoutPlate(Player player)
    {
        GetKitchenObject().SetKitchenObjectParent(player);

        SetStateIdleServerRpc();
    }

    private void GivePlayerKitchenObjectOnPlate(PlateKitchenObject plateKitchenObject)
    {
        if (plateKitchenObject.TryAddIngredient(GetKitchenObject().KitchenObjectSO))
            KitchenObject.DestroyKitchenObject(GetKitchenObject());

        SetStateIdleServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetStateIdleServerRpc()
    {
        _state.Value = State.Idle;
    }


    private bool HasRecipeWithInput(KitchenObjectSO kitchenObjectSO)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(kitchenObjectSO);
        return fryingRecipeSO != null;
    }

    private FryingRecipeSO GetFryingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (FryingRecipeSO fryingRecipeSO in _fryingRecipeSOArray)
        {
            if (fryingRecipeSO.Input == inputKitchenObjectSO)
                return fryingRecipeSO;
        }

        return null;
    }

    private BurningRecipeSO GetBurningRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (BurningRecipeSO burningRecipeSO in _burningRecipeSOArray)
        {
            if (burningRecipeSO.Input == inputKitchenObjectSO)
                return burningRecipeSO;
        }

        return null;
    }
}
