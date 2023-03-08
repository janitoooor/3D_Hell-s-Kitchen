using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterSound : MonoBehaviour
{
    private const string PLAYER_PREFS_STOVE_COUNTER_VOLUME = "StoveCounterVolume";

    [SerializeField] private StoveCounter _stoveCounter;
    [Space]
    [Range(0f, 1f)]
    [SerializeField] private float _timerWarningSound = 0.2f;

    private AudioSource _audioSource;
    private float _volume = 1f;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _volume = PlayerPrefs.GetFloat(PLAYER_PREFS_STOVE_COUNTER_VOLUME, _volume);
        _audioSource.volume = _volume;
    }

    private void Start()
    {
        _stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
        SoundsEffects.Instance.OnVolumeChanged += SoundsEffects_OnVolumeChanged;
    }

    private void SoundsEffects_OnVolumeChanged(object sender, System.EventArgs e)
    {
        _volume = SoundsEffects.Instance.Volume;
        _audioSource.volume = _volume;
        PlayerPrefs.SetFloat(PLAYER_PREFS_STOVE_COUNTER_VOLUME, _volume);
        PlayerPrefs.Save();
    }

    private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangeEventArgs e)
    {
        bool playSound = e.State == StoveCounter.State.Fried || e.State == StoveCounter.State.Frying;
        if (playSound)
            _audioSource.Play();
        else
            _audioSource.Pause();

        if (_stoveCounter.IsFried)
            StartCoroutine(PlayWarningSound());
        else
            StopCoroutine(PlayWarningSound());
    }

    private IEnumerator PlayWarningSound()
    {
        yield return new WaitForSeconds(_timerWarningSound);
        SoundsEffects.Instance.PlayBurnWarningSound(_stoveCounter.transform.position);
        if (_stoveCounter.IsFried)
            StartCoroutine(PlayWarningSound());
    }
}
