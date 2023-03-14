using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    [Header("Buttons")]
    [Space]
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private Button _createLobbyButton;
    [SerializeField] private Button _quickJoinButton;
    [SerializeField] private Button _joinCodeButton;
    [Space]
    [Header("Input Fields")]
    [Space]
    [SerializeField] private TMP_InputField _joinCodeInputField;
    [SerializeField] private TMP_InputField _playerNameInputField;
    [Space]
    [Header("Lobby")]
    [Space]
    [SerializeField] private LobbyCreateUI _lobbyCreateUI;
    [Space]
    [Header("Lobbies container")]
    [Space]
    [SerializeField] private Transform _lobbyContainer;
    [SerializeField] private Transform _lobbyTemplate;

    private void Awake()
    {
        _mainMenuButton.onClick.AddListener(() =>
        {
            KitchenGameLobby.Instance.LeaveLobby();
            Loader.Load(Loader.Scene.MainMenuScene);
        });

        _createLobbyButton.onClick.AddListener(() =>
        {
            _lobbyCreateUI.Show();
        });

        _quickJoinButton.onClick.AddListener(() =>
        {
            KitchenGameLobby.Instance.QuickJoing();
        });

        _joinCodeButton.onClick.AddListener(() =>
        {
            KitchenGameLobby.Instance.JoinWithCode(_joinCodeInputField.text);
        });

        _lobbyTemplate.gameObject.SetActive(false);
    }

    private void Start()
    {
        _playerNameInputField.text = KitchenGameMultiplayer.Instance.PlayerName;
        _playerNameInputField.onValueChanged.AddListener((string newText) =>
        {
            KitchenGameMultiplayer.Instance.SetPlayerName(newText);
        });

        KitchenGameLobby.Instance.OnLobbyListChanged += KitchenGameLobby_OnLobbyListChanged;
        UpdateLobbyList(new List<Lobby>());
    }

    private void OnDestroy()
    {
        KitchenGameLobby.Instance.OnLobbyListChanged -= KitchenGameLobby_OnLobbyListChanged;
    }

    private void KitchenGameLobby_OnLobbyListChanged(object sender, KitchenGameLobby.OnLobbyListChangedEventArgs e)
    {
        UpdateLobbyList(e.LobbyList);
    }

    private void UpdateLobbyList(List<Lobby> lobbyList)
    {
        foreach (Transform child in _lobbyContainer)
        {
            if (child == _lobbyTemplate) continue;
            Destroy(child.gameObject);
        }

        foreach (Lobby lobby in lobbyList)
        {
            Transform lobbyTransform = Instantiate(_lobbyTemplate, _lobbyContainer);
            lobbyTransform.gameObject.SetActive(true);
            lobbyTransform.GetComponent<LobbyListSingleUI>().SetLobby(lobby);
        }
    }
}
