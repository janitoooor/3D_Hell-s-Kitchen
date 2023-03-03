using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public event EventHandler OnInteractAction;
    private PlayerInputActions _playerInputActions;

    private void Awake()
    {
        _playerInputActions = new();
        _playerInputActions.Player.Enable();

        _playerInputActions.Player.Interact.performed += Interact_performed;
    }

    private void Interact_performed(InputAction.CallbackContext obj)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalize()
    {
        Vector2 inputVector = _playerInputActions.Player.Move.ReadValue<Vector2>();

        return inputVector.normalized;
    }
}
