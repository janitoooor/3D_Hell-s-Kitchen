using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconsToFail : MonoBehaviour
{
    [SerializeField] private Image _redStepImage;
    [SerializeField] private Image _yellowStepImage;
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private Image _greenStepImage;

    private float _timer;
    private float _timerMax;


    private void Start()
    {
        _timer = _timerMax;

        _yellowStepImage.color = Color.green;
        _redStepImage.color = Color.green;
    }
    private void Update()
    {
        if (!gameObject.activeInHierarchy)
            return;

        _timer -= Time.deltaTime;
        if (_timer < 0)
        {
            _yellowStepImage.color = Color.green;
            _redStepImage.color = Color.green;
            _backgroundImage.fillAmount = 0;
            return;
        }

        _backgroundImage.fillAmount = 1 - _timer / _timerMax;

        if (_timer <= 10f)
            _yellowStepImage.color = Color.yellow;
        if (_timer <= 5f)
            _redStepImage.color = Color.red;
    }
}