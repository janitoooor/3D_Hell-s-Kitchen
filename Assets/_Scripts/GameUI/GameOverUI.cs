using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _recipesDeliveredText;
    [SerializeField] private Button _buttonPlayAgain;

    private void Awake()
    {
        _buttonPlayAgain.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.Shutdown();
            Loader.Load(Loader.Scene.MainMenuScene);
        });
    }

    private void Start()
    {
        KitchenGameStateMachine.Instance.OnStateChanged += KitchenGameStateMachine_OnStateChanged;

        Hide();
    }

    private void KitchenGameStateMachine_OnStateChanged(object sender, System.EventArgs e)
    {
        if (KitchenGameStateMachine.Instance.IsGameOver)
            Show();
        else
            Hide();
    }

    private void UpdateDeliveredText()
    {
        _recipesDeliveredText.text = DeliveryManager.Instance.SuccessfulRecipesAmount.ToString();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void Show()
    {
        gameObject.SetActive(true);
        UpdateDeliveredText();
    }
}
