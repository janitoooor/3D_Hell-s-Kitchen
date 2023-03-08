using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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

    public bool IsFried { get => _state == State.Fried; }

    private State _state;
    private float _fryingTimer;
    private float _burningTimer;
    private FryingRecipeSO _fryingRecipeSO;
    private BurningRecipeSO _burningRecipeSO;

    private void Start()
    {
        _state = State.Idle;
    }

    private void Update()
    {
        if (HasKitchenObject())
        {
            switch (_state)
            {
                case State.Idle:
                    break;
                case State.Frying:
                    _fryingTimer += Time.deltaTime;

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        ProgressNormalized = _fryingTimer / _fryingRecipeSO.FryingTimerMax
                    });

                    if (_fryingTimer > _fryingRecipeSO.FryingTimerMax)
                    {
                        _fryingTimer = 0;
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(_fryingRecipeSO.Output, this);
                        _burningRecipeSO = GetBurningRecipeSOWithInput(GetKitchenObject().KitchenObjectSO);
                        _state = State.Fried;

                        OnStateChanged?.Invoke(this, new OnStateChangeEventArgs
                        {
                            State = _state
                        });
                    }
                    break;
                case State.Fried:
                    _burningTimer += Time.deltaTime;

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        ProgressNormalized = _burningTimer / _burningRecipeSO.BurningTimerMax
                    });

                    if (_burningTimer > _burningRecipeSO.BurningTimerMax)
                    {
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(_burningRecipeSO.Output, this);
                        _state = State.Burned;

                        OnStateChanged?.Invoke(this, new OnStateChangeEventArgs
                        {
                            State = _state
                        });

                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                        {
                            ProgressNormalized = 0
                        });
                    }
                    break;
                case State.Burned:
                    break;
            }
        }
    }

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            if (player.HasKitchenObject() && HasRecipeWithInput(player.GetKitchenObject().KitchenObjectSO))
            {
                player.GetKitchenObject().SetKitchenObjectParent(this);

                _fryingRecipeSO = GetFryingRecipeSOWithInput(GetKitchenObject().KitchenObjectSO);
                _state = State.Frying;
                _fryingTimer = 0;
                _burningTimer = 0;

                OnStateChanged?.Invoke(this, new OnStateChangeEventArgs
                {
                    State = _state
                });

                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    ProgressNormalized = _fryingTimer / _fryingRecipeSO.FryingTimerMax
                });
            }
        }
        else
        {
            if (!player.HasKitchenObject())
            {
                GetKitchenObject().SetKitchenObjectParent(player);

                _state = State.Idle;
                OnStateChanged?.Invoke(this, new OnStateChangeEventArgs
                {
                    State = _state
                });

                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    ProgressNormalized = 0
                });
            }
            else if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
            {
                if (plateKitchenObject.TryAddIngredient(GetKitchenObject().KitchenObjectSO))
                    GetKitchenObject().DestroySelf();

                _state = State.Idle;
                OnStateChanged?.Invoke(this, new OnStateChangeEventArgs
                {
                    State = _state
                });

                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    ProgressNormalized = 0
                });
            }
        }
    }

    private bool HasRecipeWithInput(KitchenObjectSO kitchenObjectSO)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(kitchenObjectSO);
        return fryingRecipeSO != null;
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
        if (fryingRecipeSO != null)
            return fryingRecipeSO.Output;
        else
            return null;
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
