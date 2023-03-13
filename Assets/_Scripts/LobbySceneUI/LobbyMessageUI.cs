using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class LobbyMessageUI : MonoBehaviour
{
    [Space]
    [Multiline][SerializeField] private string _failedToConnectLobbyString = "Failed to connect!";
    [Multiline][SerializeField] private string _waitingToCreateLobbyString = "Creating Lobby...";
    [Multiline][SerializeField] private string _joingStrartingLobbyString = "Joining Lobby...";
    [Multiline][SerializeField] private string _failedToJoinLobbyString = "Failed to join Lobby!";
    [Multiline][SerializeField] private string _failedToQuickJoinLobbyString = "Could not find a Lobby to Quick Join!";
    [Space]
    [SerializeField] private TextMeshProUGUI _messageText;
    [SerializeField] private Button _closeButton;

    private void Awake()
    {
        _closeButton.onClick.AddListener(() =>
        {
            Hide();
        });
    }

    private void Start()
    {
        KitchenGameMultiplayer.Instance.OnFailedToJoingGame += KitchenGameMultiplayer_OnFailedToJoingGame;
        KitchenGameLobby.Instance.OnCreateLobbyStarted += KitchenGameLobby_OnCreateLobbyStarted;
        KitchenGameLobby.Instance.OnCreateLobbyFailed += KitchenGameLobby_OnCreateLobbyFailed;
        KitchenGameLobby.Instance.OnJoinStarted += KitchenGameLobby_OnJoinStarted;
        KitchenGameLobby.Instance.OnJoinFailed += KitchenGameLobby_OnJoinFailed;
        KitchenGameLobby.Instance.OnQuickJoinFailed += KitchenGameLobby_OnQuickJoinFailed;

        Hide();
    }

    private void KitchenGameLobby_OnQuickJoinFailed(object sender, System.EventArgs e)
    {
        ShowMessage(_failedToQuickJoinLobbyString);
    }

    private void KitchenGameLobby_OnJoinFailed(object sender, System.EventArgs e)
    {
        ShowMessage(_failedToJoinLobbyString);
    }

    private void KitchenGameLobby_OnJoinStarted(object sender, System.EventArgs e)
    {
        ShowMessage(_joingStrartingLobbyString);
    }

    private void KitchenGameLobby_OnCreateLobbyFailed(object sender, System.EventArgs e)
    {
        ShowMessage(_failedToConnectLobbyString);
    }

    private void KitchenGameLobby_OnCreateLobbyStarted(object sender, System.EventArgs e)
    {
        ShowMessage(_waitingToCreateLobbyString);
    }

    private void ShowMessage(string message)
    {
        Show();
        _messageText.text = message;
    }

    private void OnDestroy()
    {
        KitchenGameMultiplayer.Instance.OnFailedToJoingGame -= KitchenGameMultiplayer_OnFailedToJoingGame;
    }
    private void KitchenGameMultiplayer_OnFailedToJoingGame(object sender, System.EventArgs e)
    {
        if (NetworkManager.Singleton.DisconnectReason == "")
            ShowMessage(_failedToConnectLobbyString);
        else
            ShowMessage(_failedToConnectLobbyString);
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
