using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class GamePauseUI : MonoBehaviour
{
    [Space]
    [Header("Pause Buttons")]
    [Space]
    [SerializeField] private Button _resumeButton;
    [SerializeField] private Button _mainMenusButton;
    [SerializeField] private Button _optionsButton;

    private void Awake()
    {
        AddButtonsOnClick();
    }

    private void Start()
    {
        KitchenGameStateMachine.Instance.OnLocalGamePaused += KitchenGameStateMachine_OnLocalGamePaused;
        KitchenGameStateMachine.Instance.OnLocalGameUnPaused += KitchenGameStateMachine_OnLocalGameUnPaused;

        Hide();
    }

    private void AddButtonsOnClick()
    {
        _resumeButton.onClick.AddListener(() =>
        {
            KitchenGameStateMachine.Instance.TogglePauseGame();
        });

        _mainMenusButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.Shutdown();
            Loader.Load(Loader.Scene.MainMenuScene);
        });

        _optionsButton.onClick.AddListener(() =>
        {
            Hide();
            OptionsUI.Instance.Show(Show);
        });
    }

    private void KitchenGameStateMachine_OnLocalGameUnPaused(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void KitchenGameStateMachine_OnLocalGamePaused(object sender, System.EventArgs e)
    {
        Show();
    }

    private void Show()
    {
        gameObject.SetActive(true);

        _resumeButton.Select();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

}
