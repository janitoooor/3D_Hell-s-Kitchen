using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    public static OptionsUI Instance { get; private set; }

    [Space]
    [SerializeField] private Transform _pressToRebindTransform;
    [Space]
    [Header("Options Buttons")]
    [Space]
    [SerializeField] private Button _soundEffectsButton;
    [SerializeField] private Button _musicButton;
    [SerializeField] private Button _closeButton;
    [Space]
    [Header("Options Buttons Text")]
    [Space]
    [SerializeField] private TextMeshProUGUI _soundEffectsText;
    [SerializeField] private TextMeshProUGUI _musicText;
    [Space]
    [Header("Controlls Buttons")]
    [Space]
    [SerializeField] private Button _moveUpButton;
    [SerializeField] private Button _moveDownButton;
    [SerializeField] private Button _moveLeftButton;
    [SerializeField] private Button _moveRightButton;
    [SerializeField] private Button _interactButton;
    [SerializeField] private Button _interactAlternateButton;
    [SerializeField] private Button _pauseButton;
    [SerializeField] private Button _gamepadInteractButton;
    [SerializeField] private Button _gamepadInteractAlternateButton;
    [SerializeField] private Button _gamepadPauseButton;
    [Space]
    [Header("Controlls Buttons Text")]
    [Space]
    [SerializeField] private TextMeshProUGUI _moveUpButtonText;
    [SerializeField] private TextMeshProUGUI _moveDownButtonText;
    [SerializeField] private TextMeshProUGUI _moveLeftButtonText;
    [SerializeField] private TextMeshProUGUI _moveRightButtonText;
    [SerializeField] private TextMeshProUGUI _interactButtonText;
    [SerializeField] private TextMeshProUGUI _interactAlternateButtonText;
    [SerializeField] private TextMeshProUGUI _pauseButtonText;
    [SerializeField] private TextMeshProUGUI _gamepadInteractButtonText;
    [SerializeField] private TextMeshProUGUI _gamepadInteractAlternateButtonText;
    [SerializeField] private TextMeshProUGUI _gamepadPauseButtonText;

    private Action OnCloseButtonAction;

    private readonly string _soundString = "Sounds Effects: ";
    private readonly string _musicString = "Music: ";

    private void Awake()
    {
        Instance = this;

        AddButtonsOnClick();
    }

    private void Start()
    {
        KitchenGameManager.Instance.OnGameUnPaused += KitchenGameManager_OnGameUnPaused;
        UpdateVisual();
        HidePressToRebindKey();
        Hide();
    }

    private void KitchenGameManager_OnGameUnPaused(object sender, EventArgs e)
    {
        Hide();
    }

    public void Show(Action onCloseButtonAction)
    {
        OnCloseButtonAction = onCloseButtonAction;
        gameObject.SetActive(true);
        _soundEffectsButton.Select();
    }

    private void AddButtonsOnClick()
    {
        _soundEffectsButton.onClick.AddListener(() =>
        {
            SoundsEffects.Instance.ChangeValue();
            UpdateVisual();
        });

        _musicButton.onClick.AddListener(() =>
        {
            BackgroundMusic.Instance.ChangeValue();
            UpdateVisual();
        });

        _closeButton.onClick.AddListener(() =>
        {
            Hide();
            OnCloseButtonAction();
        });

        _moveUpButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Move_Up); });
        _moveDownButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Move_Down); });
        _moveLeftButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Move_Left); });
        _moveRightButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Move_Right); });
        _interactButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Interact); });
        _interactAlternateButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.InteractAlternate); });
        _pauseButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Pause); });
        _gamepadInteractButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Gamepad_Interact); });
        _gamepadInteractAlternateButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Gamepad_InteractAlternate); });
        _gamepadPauseButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Gamepad_Pause); });
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void ShowPressToRebindKey()
    {
        _pressToRebindTransform.gameObject.SetActive(true);
    }

    private void HidePressToRebindKey()
    {
        _pressToRebindTransform.gameObject.SetActive(false);
    }

    private void UpdateVisual()
    {
        float multiplyValue = 10f;
        _soundEffectsText.text = _soundString + Mathf.Round(SoundsEffects.Instance.Volume * multiplyValue);
        _musicText.text = _musicString + Mathf.Round(BackgroundMusic.Instance.Volume * multiplyValue);

        _moveUpButtonText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Up);
        _moveDownButtonText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Down);
        _moveLeftButtonText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Left);
        _moveRightButtonText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Right);
        _interactButtonText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact);
        _interactAlternateButtonText.text = GameInput.Instance.GetBindingText(GameInput.Binding.InteractAlternate);
        _pauseButtonText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Pause);
        _gamepadInteractButtonText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Gamepad_Interact);
        _gamepadInteractAlternateButtonText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Gamepad_InteractAlternate);
        _gamepadPauseButtonText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Gamepad_Pause);
    }

    private void RebindBinding(GameInput.Binding binding)
    {
        ShowPressToRebindKey();
        GameInput.Instance.RebindBunding(binding, () =>
        {
            HidePressToRebindKey();
            UpdateVisual();
        });
    }
}
