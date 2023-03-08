using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameStartCountdownUI : MonoBehaviour
{
    private const string NUMBER_POPUP = "NumberPopup";

    [SerializeField] private TextMeshProUGUI _countdownText;
    [SerializeField] private string _startText = "START"!;

    private Animator _animator;
    private int _previousCountdownNumber;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        KitchenGameManager.Instance.OnStateChanged += KitchenGameManager_OnStateChanged;

        Hide();
    }

    private void KitchenGameManager_OnStateChanged(object sender, System.EventArgs e)
    {
        if (KitchenGameManager.Instance.IsCountdownToStartActive)
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
        int countdownNumber = Mathf.RoundToInt(time);
        _countdownText.text = countdownNumber.ToString();

        if (_previousCountdownNumber != countdownNumber)
        {
            _previousCountdownNumber = countdownNumber;
            _animator.SetTrigger(NUMBER_POPUP);
            if (gameObject.activeInHierarchy)
                SoundsEffects.Instance.PlayCountdownSound();
            if (countdownNumber == 0)
                _countdownText.text = _startText;
        }
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
