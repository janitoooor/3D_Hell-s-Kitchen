using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class GameStartCountdownUI : MonoBehaviour
{
    private const string NUMBER_POPUP = "NumberPopup";

    [SerializeField] private TextMeshProUGUI _countdownText;
    [SerializeField] private string _startText = "START"!;

    private Animator _animator;
    private int _previousCountdownNumber;
    private float _timeToHideAfterStart = 0.9f;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        KitchenGameStateMachine.Instance.OnStateChanged += KitchenGameManager_OnStateChanged;
        KitchenGameStateMachine.Instance.OnCountdownToStartTimerChanged += OnCountdownToStartTimerChanged_OnCountdownToStartTimerChanged;

        Hide();
    }

    private void OnCountdownToStartTimerChanged_OnCountdownToStartTimerChanged(object sender, EventArgs e)
    {
        UpdateCountDownText(KitchenGameStateMachine.Instance.CountdownToStartTimer);
    }

    private void KitchenGameManager_OnStateChanged(object sender, EventArgs e)
    {
        if (KitchenGameStateMachine.Instance.IsCountdownToStartActive)
        {
            Show();
            _animator.SetTrigger(NUMBER_POPUP);
            SoundsEffects.Instance.PlayCountdownSound();
        }
        else
        {
            Hide();
        }
    }

    public void UpdateCountDownText(float time)
    {
        if (time < 0)
            return;

        int countdownNumber = Mathf.RoundToInt(time);
        _countdownText.text = countdownNumber.ToString();

        if (_previousCountdownNumber != countdownNumber)
        {
            _previousCountdownNumber = countdownNumber;
            _animator.SetTrigger(NUMBER_POPUP);
            SoundsEffects.Instance.PlayCountdownSound();

            if (countdownNumber == 0)
            {
                _countdownText.text = _startText;
                StartCoroutine(HideAfterStart());
                KitchenGameStateMachine.Instance.OnCountdownToStartTimerChanged -= OnCountdownToStartTimerChanged_OnCountdownToStartTimerChanged;
            }
        }
    }

    private IEnumerator HideAfterStart()
    {
        yield return new WaitForSeconds(_timeToHideAfterStart);
        Hide();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
}
