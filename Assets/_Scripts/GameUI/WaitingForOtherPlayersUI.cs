using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingForOtherPlayersUI : MonoBehaviour
{
    private void Start()
    {
        KitchenGameStateMachine.Instance.OnLocalPlayerReadyChanged += KitchenGameStateMachine_OnLocalPlayerReadyChanged;
        KitchenGameStateMachine.Instance.OnStateChanged += KitchenGameStatesMachine_OnStateChanged;
        Hide();
    }

    private void KitchenGameStatesMachine_OnStateChanged(object sender, System.EventArgs e)
    {
        if (KitchenGameStateMachine.Instance.IsCountdownToStartActive)
            Hide();
    }

    private void KitchenGameStateMachine_OnLocalPlayerReadyChanged(object sender, System.EventArgs e)
    {
        if (KitchenGameStateMachine.Instance.IsLocalPlayerReady)
            Show();
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
