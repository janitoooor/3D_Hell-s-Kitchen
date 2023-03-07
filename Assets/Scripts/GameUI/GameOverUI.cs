using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _recipesDeliveredText;

    private void Start()
    {
        KitchenGameManager.Instance.OnStateChanged += KitchenGameManager_OnStateChanged;

        Hide();
    }

    private void KitchenGameManager_OnStateChanged(object sender, System.EventArgs e)
    {
        if (KitchenGameManager.Instance.IsGameOver)
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
