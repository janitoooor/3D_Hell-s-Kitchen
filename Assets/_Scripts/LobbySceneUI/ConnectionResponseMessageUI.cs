using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class ConnectionResponseMessageUI : MonoBehaviour
{
    [Space]
    [Multiline][SerializeField] private string _failedToConnectString = "Failed to connect";
    [Space]
    [SerializeField] private TextMeshProUGUI _connectionResponseMessageText;
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

        Hide();
    }

    private void OnDestroy()
    {
        KitchenGameMultiplayer.Instance.OnFailedToJoingGame -= KitchenGameMultiplayer_OnFailedToJoingGame;
    }
    private void KitchenGameMultiplayer_OnFailedToJoingGame(object sender, System.EventArgs e)
    {
        Show();

        _connectionResponseMessageText.text = NetworkManager.Singleton.DisconnectReason;

        if (_connectionResponseMessageText.text == "")
            _connectionResponseMessageText.text = _failedToConnectString;
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
