using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayClockUI : MonoBehaviour
{
    private const string WARNING = "Warning";

    [SerializeField] private float _timeToWarning = 30f;
    [Space]
    [SerializeField] private Image _timerImage;
    [SerializeField] private TextMeshProUGUI _timeText;
    [Space]
    [SerializeField] private Color _startColor;
    [SerializeField] private Color _midleColor;
    [SerializeField] private Color _endColor;
    [Space]
    [SerializeField] private Animator _timerTextAnimator;
    [SerializeField] private Animator _timerIconAnimator;

    private float _timer;
    private float _timerMax;
    private float _timerSound;

    public bool IsTimeToWarning { get => _timer <= _timeToWarning; }
    private float NormalizeValue { get => _timer / _timerMax; }

    private void Start()
    {
        _timerMax = KitchenGameStateMachine.Instance.GamePlayingTimer;
        _timer = _timerMax;
        _timeText.text = ConvertTime();
        _timerImage.color = _startColor;
    }

    private void Update()
    {
        if (KitchenGameStateMachine.Instance.IsGamePlaying)
            ÑhangeTimer();
    }

    public void ÑhangeTimer()
    {
        if (_timer < 0)
            return;

        _timer -= Time.deltaTime;
        _timerImage.fillAmount = NormalizeValue;
        _timeText.text = ConvertTime();

        if (IsTimeToWarning)
            StartWarning();
        else
            ChangeColor();
    }

    private void ChangeColor()
    {
        if (NormalizeValue >= 0.5f)
            _timerImage.color = Color.Lerp(_startColor, _midleColor, (1f - NormalizeValue) * 2);
        else
            _timerImage.color = Color.Lerp(_midleColor, _endColor, (1 - NormalizeValue) / 2 + 0.1f);
    }

    private void StartWarning()
    {
        _timerIconAnimator.SetBool(WARNING, true);
        _timerTextAnimator.SetBool(WARNING, true);

        _timerImage.color = Color.Lerp(_midleColor, _endColor, 1 - NormalizeValue);

        _timerSound -= Time.deltaTime;

        if (_timerSound < 0)
        {
            _timerSound = 1;
            SoundsEffects.Instance.PlayeWarningTimerSound();
        }
    }

    private string ConvertTime()
    {
        int minutes = (int)_timer / 60;
        int seconds = (int)_timer % 60;
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
