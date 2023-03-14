using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button _playMultiplayerButton;
    [SerializeField] private Button _playSinglePlayerButton;
    [SerializeField] private Button _quitButton;

    private void Awake()
    {
        AddButtonsOnClick();

        Time.timeScale = 1f;
    }

    private void AddButtonsOnClick()
    {
        _playMultiplayerButton.onClick.AddListener(() =>
        {
            KitchenGameLobby.IsSinglePlayer = false;
            Loader.Load(Loader.Scene.LobbyScene);
        });

        _playSinglePlayerButton.onClick.AddListener(() =>
        {
            KitchenGameLobby.IsSinglePlayer = true;
            Loader.Load(Loader.Scene.LobbyScene);
        });

        _quitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }
}
