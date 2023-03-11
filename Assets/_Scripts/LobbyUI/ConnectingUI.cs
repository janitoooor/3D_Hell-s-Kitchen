using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectingUI : MonoBehaviour
{
    private void Start()
    {
        KitchenGameMultiplayer.Instance.OnFailedToJoingGame += KitchenGameMultiplayer_OnFailedToJoingGame;
        KitchenGameMultiplayer.Instance.OnTryingToJoingGame += KitchenGameMultiplayer_OnTryingToJoingGame;

        Hide();
    }

    private void OnDestroy()
    {
        KitchenGameMultiplayer.Instance.OnFailedToJoingGame -= KitchenGameMultiplayer_OnFailedToJoingGame;
        KitchenGameMultiplayer.Instance.OnTryingToJoingGame -= KitchenGameMultiplayer_OnTryingToJoingGame;
    }

    private void KitchenGameMultiplayer_OnTryingToJoingGame(object sender, System.EventArgs e)
    {
        Show();
    }

    private void KitchenGameMultiplayer_OnFailedToJoingGame(object sender, System.EventArgs e)
    {
        Hide();
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
