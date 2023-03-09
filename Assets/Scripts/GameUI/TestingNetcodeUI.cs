using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class TestingNetcodeUI : MonoBehaviour
{
    [SerializeField] private Button _startHostButton;
    [SerializeField] private Button _startClientButton;

    private void Awake()
    {
        _startHostButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
            print("Start Host");
            Hide();
        });

        _startClientButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
            print("Start Client");
            Hide();
        });
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
