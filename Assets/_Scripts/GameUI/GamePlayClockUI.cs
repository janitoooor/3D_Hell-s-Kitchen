using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayClockUI : MonoBehaviour
{
    [SerializeField] private Image _timerImage;

    private float _timer;
    private float _timerMax;

    private void Start()
    {
        _timerMax = KitchenGameStateMachine.Instance.GamePlayingTimer;
        _timer = _timerMax;
    }

    private void Update()
    {
        if (KitchenGameStateMachine.Instance.IsGamePlaying)
            ChangeImageFill();
    }

    public void ChangeImageFill()
    {
        _timer -= Time.deltaTime;
        if (_timer >= 0)
            _timerImage.fillAmount = 1 - _timer / _timerMax;
    }
}
