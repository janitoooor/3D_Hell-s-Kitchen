using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectUI : MonoBehaviour
{
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private Button _readyButton;
    [Space]
    [SerializeField] private TextMeshProUGUI _lobbyNameText;
    [SerializeField] private TextMeshProUGUI _lobbyCodeText;
    [SerializeField] private TextMeshProUGUI _readyButtonText;
    [Space]
    [SerializeField] string _readyTextSinglePlayer = "Start";

    private readonly string _lobbyName = "Lobby Name: ";
    private readonly string _lobbyCode = "Lobby Code: ";

    private Lobby _lobby;

    private void Awake()
    {
        _mainMenuButton.onClick.AddListener(() =>
        {
            KitchenGameLobby.Instance.LeaveLobby();
            NetworkManager.Singleton.Shutdown();
            Loader.Load(Loader.Scene.MainMenuScene);
        });

        _readyButton.onClick.AddListener(() =>
        {
            CharacterSelectReady.Instance.SetPlayerReady();
        });
    }

    private void Start()
    {
        if (KitchenGameLobby.IsSinglePlayer)
        {
            _lobbyNameText.gameObject.SetActive(false);
            _lobbyCodeText.gameObject.SetActive(false);
            _readyButtonText.text = _readyTextSinglePlayer;
            return;
        }

        _lobby = KitchenGameLobby.Instance.JoinedLobby;

        _lobbyNameText.text = _lobbyName + _lobby.Name;
        _lobbyCodeText.text = _lobbyCode + _lobby.LobbyCode;
    }
}
