using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour, IKitchenObjectParent
{
    public static Player Instance { get; private set; }

    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter SelectedCounter;
    }

    [SerializeField] private Transform _kitchenObjectHoldPoint;
    [Space]
    [SerializeField] private LayerMask _countersLayerMask;
    [SerializeField] private GameInput _gameInput;
    [Space]
    [SerializeField] private float _rotateSpeed = 10f;
    [SerializeField] private float _speed = 7f;
    [SerializeField] private float _playerRadius = 0.7f;
    [SerializeField] private float _playerHight = 2f;
    [SerializeField] private float _raycastDistance = 2f;

    private BaseCounter _selectedCounter;
    private KitchenObject _kitchenObject;

    private bool _isWalking;
    public bool IsWalking { get => _isWalking; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        _gameInput.OnInteractAction += GameInput_OnInteractAction;
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        if (_selectedCounter != null)
            _selectedCounter.Interact(this);
    }

    private void Update()
    {
        HandleMovement();
        HandleInteractions();
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
        Vector2 inputVector = _gameInput.GetMovementVectorNormalize();
        Vector3 moveDirection = new(inputVector.x, 0, inputVector.y);
        float moveDistance = _speed * Time.deltaTime;

        if (PlayerCanMove(moveDirection, moveDistance))
        {
            _isWalking = moveDirection != Vector3.zero;
            transform.position += moveDistance * moveDirection;
        }
        else
        {
            Vector3 moveX = new Vector3(moveDirection.x, 0, 0).normalized;
            _isWalking = moveX != Vector3.zero;
            if (PlayerCanMove(moveX, moveDistance))
            {
                transform.position += moveDistance * moveX;
            }
            else
            {
                Vector3 moveZ = new Vector3(0, 0, moveDirection.z).normalized;
                _isWalking = moveZ != Vector3.zero;
                if (PlayerCanMove(moveZ, moveDistance))
                    transform.position += moveDistance * moveZ;
            }
        }

        if (moveDirection != transform.forward)
            transform.forward = Vector3.Slerp(transform.forward, moveDirection, _rotateSpeed * Time.deltaTime);
    }

    private bool PlayerCanMove(Vector3 moveDirection, float moveDistance)
    {
        return !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * _playerHight,
            _playerRadius, moveDirection, moveDistance);
    }

    private void SelectedCounter(BaseCounter selectedCounter)
    {
        _selectedCounter = selectedCounter;

        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
            SelectedCounter = selectedCounter
        });
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return _kitchenObjectHoldPoint;
    }

    public bool HasKitchenObject()
    {
        return _kitchenObject != null;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        _kitchenObject = kitchenObject;
    }

    public void ClearKitchenObject()
    {
        _kitchenObject = null;
    }

    public KitchenObject GetKitchenObject()
    {
        return _kitchenObject;
    }
}
