using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : NetworkBehaviour, IKitchenObjectParent
{
    public static event EventHandler OnAnyPlayerSpawned;
    public static event EventHandler OnAnyPickedSomething;

    public static void ResetStaticData()
    {
        OnAnyPlayerSpawned = null;
        OnAnyPickedSomething = null;
    }

    public static Player LocalInstance { get; private set; }

    public event EventHandler OnPickSomething;
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter SelectedCounter;
    }

    [SerializeField] private Transform _kitchenObjectHoldPoint;
    [Space]
    [SerializeField] private LayerMask _collisionsLayerMask;
    [SerializeField] private LayerMask _countersLayerMask;
    [Space]
    [SerializeField] private float _rotateSpeed = 10f;
    [SerializeField] private float _speed = 7f;
    [SerializeField] private float _playerRadius = 0.7f;
    [SerializeField] private float _raycastDistance = 2f;
    [Space]
    [SerializeField] private List<Vector3> _spawnPositionList;
    [Space]
    [SerializeField] private PlayerVisual _playerVisual;

    private BaseCounter _selectedCounter;
    private KitchenObject _kitchenObject;

    private bool _isWalking;
    public bool IsWalking { get => _isWalking; }
    private void Start()
    {
        GameInput.Instance.OnInteractAction += GameInput_OnInteractAction;
        GameInput.Instance.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;

        PlayerData playerData = KitchenGameMultiplayer.Instance.GetPlayerDataFromClientId(OwnerClientId);
        _playerVisual.SetPlayerColor(KitchenGameMultiplayer.Instance.GetPlayerColor(playerData.ColorId));
    }

    private void Update()
    {
        if (!IsOwner)
            return;

        HandleMovement();
        HandleInteractions();
    }

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
            LocalInstance = this;

        transform.position = _spawnPositionList[KitchenGameMultiplayer.Instance.GetPlayerDataIndexFromClientId(OwnerClientId)];
        transform.rotation = new Quaternion(0f, -180, 0f, 0);
        OnAnyPlayerSpawned?.Invoke(this, EventArgs.Empty);

        if (IsServer)
            NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectCallback;
    }

    private void NetworkManager_OnClientDisconnectCallback(ulong clientId)
    {
        if (clientId == OwnerClientId && HasKitchenObject())
        {
            KitchenObject.DestroyKitchenObject(GetKitchenObject());
        }

    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return _kitchenObjectHoldPoint;
    }

    public bool HasKitchenObject()
    {
        return _kitchenObject != null;
    }

    public NetworkObject GetNetworkObject()
    {
        return NetworkObject;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        _kitchenObject = kitchenObject;

        if (kitchenObject != null)
        {
            OnPickSomething?.Invoke(this, EventArgs.Empty);
            OnAnyPickedSomething?.Invoke(this, EventArgs.Empty);
        }
    }

    public void ClearKitchenObject()
    {
        _kitchenObject = null;
    }

    public KitchenObject GetKitchenObject()
    {
        return _kitchenObject;
    }

    private void GameInput_OnInteractAlternateAction(object sender, EventArgs e)
    {
        if (!KitchenGameStateMachine.Instance.IsGamePlaying)
            return;

        if (_selectedCounter != null)
            _selectedCounter.InteractAlternate(this);
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        if (!KitchenGameStateMachine.Instance.IsGamePlaying)
            return;

        if (_selectedCounter != null)
            _selectedCounter.Interact(this);
    }

    private void HandleInteractions()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit raycastHit, _raycastDistance,
            _countersLayerMask))
        {
            if (raycastHit.transform.TryGetComponent(out BaseCounter selectedCounter))
            {
                if (selectedCounter != _selectedCounter)
                    SelectedCounter(selectedCounter);
            }
            else
            {
                SelectedCounter(null);
            }
        }
        else
        {
            SelectedCounter(null);
        }
    }

    private void HandleMovement()
    {
        Vector2 inputVector = GameInput.Instance.GetMovementVectorNormalize();
        Vector3 moveDirection = new(inputVector.x, 0, inputVector.y);
        float moveDistance = _speed * Time.deltaTime;
        float deadZoneValue = 0.5f;

        if (PlayerCanMove(moveDirection, moveDistance))
        {
            _isWalking = moveDirection != Vector3.zero;
            transform.position += moveDistance * moveDirection;

        }
        else
        {
            Vector3 moveX = new Vector3(moveDirection.x, 0, 0).normalized;
            _isWalking = moveX != Vector3.zero;
            if ((moveDirection.x < -deadZoneValue || moveDirection.x > deadZoneValue) && PlayerCanMove(moveX, moveDistance))
            {
                transform.position += moveDistance * moveX;
            }
            else
            {
                Vector3 moveZ = new Vector3(0, 0, moveDirection.z).normalized;
                if (_isWalking)
                    _isWalking = moveZ != Vector3.zero;
                if ((moveDirection.z < -deadZoneValue || moveDirection.z > deadZoneValue) && PlayerCanMove(moveZ, moveDistance))
                    transform.position += moveDistance * moveZ;
                else
                    _isWalking = false;
            }
        }

        if (moveDirection != transform.forward)
            transform.forward = Vector3.Slerp(transform.forward, moveDirection, _rotateSpeed * Time.deltaTime);
    }

    private bool PlayerCanMove(Vector3 moveDirection, float moveDistance)
    {
        return !Physics.BoxCast(transform.position, Vector3.one * _playerRadius, moveDirection, Quaternion.identity,
            moveDistance, _collisionsLayerMask);
    }

    private void SelectedCounter(BaseCounter selectedCounter)
    {
        _selectedCounter = selectedCounter;

        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
            SelectedCounter = selectedCounter
        });
    }

}
