using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUI : MonoBehaviour
{
    [Header("Keys Keyboard Text")]
    [Space]
    [SerializeField] private TextMeshProUGUI _keyMoveUpText;
    [SerializeField] private TextMeshProUGUI _keyMoveDownText;
    [SerializeField] private TextMeshProUGUI _keyMoveLeftText;
    [SerializeField] private TextMeshProUGUI _keyMoveRightText;
    [SerializeField] private TextMeshProUGUI _keyInteractText;
    [SerializeField] private TextMeshProUGUI _keyInteractAlternateText;
    [SerializeField] private TextMeshProUGUI _keyPauseText;
    [Space]
    [Header("Keys Gamepad Text")]
    [Space]
    [SerializeField] private TextMeshProUGUI _keyGamepadInteractText;
    [SerializeField] private TextMeshProUGUI _keyGamepadInteractAlternateText;
    [SerializeField] private TextMeshProUGUI _keyGamepadPauseText;

    private void Start()
    {
        GameInput.Instance.OnBindingRebind += GameInput_OnBindingRebind;
        KitchenGameStateMachine.Instance.OnLocalPlayerReadyChanged += KitchenGameStateMachine_OnLocalPlayerReadyChanged;
        UpdateVisual();
        Show();
    }

    private void KitchenGameStateMachine_OnLocalPlayerReadyChanged(object sender, System.EventArgs e)
    {
        if (KitchenGameStateMachine.Instance.IsLocalPlayerReady)
            Hide();
    }

    private void GameInput_OnBindingRebind(object sender, System.EventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        _keyMoveUpText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Up);
        _keyMoveDownText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Down);
        _keyMoveLeftText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Left);
        _keyMoveRightText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Right);
        _keyInteractText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact);
        _keyInteractAlternateText.text = GameInput.Instance.GetBindingText(GameInput.Binding.InteractAlternate);
        _keyPauseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Pause);

        _keyGamepadInteractText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Gamepad_Interact);
        _keyGamepadInteractAlternateText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Gamepad_InteractAlternate);
        _keyGamepadPauseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Gamepad_Pause);
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject?.SetActive(false);
    }
}
