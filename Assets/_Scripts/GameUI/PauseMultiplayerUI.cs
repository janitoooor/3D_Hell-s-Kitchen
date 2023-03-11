using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMultiplayerUI : MonoBehaviour
{
    private void Start()
    {
        KitchenGameStateMachine.Instance.OnMultiplayerGamePaused += KitchenGameStateMachine_OnMultiplayerGamePaused;
        KitchenGameStateMachine.Instance.OnMultiplayerGameUnPaused += KitchenGameStateMachine_OnMultiplayerGameUnPaused;

        Hide();
    }

    private void KitchenGameStateMachine_OnMultiplayerGamePaused(object sender, System.EventArgs e)
    {
        Show();
    }

    private void KitchenGameStateMachine_OnMultiplayerGameUnPaused(object sender, System.EventArgs e)
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
