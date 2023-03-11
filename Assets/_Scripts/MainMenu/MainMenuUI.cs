using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _quitButton;

    private void Awake()
    {
        AddButtonsOnClick();

        Time.timeScale = 1f;
    }

    private void AddButtonsOnClick()
    {
        _playButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.LobbyScene);
        });

        _quitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }
}
