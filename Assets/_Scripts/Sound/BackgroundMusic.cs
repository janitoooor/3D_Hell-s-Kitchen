using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    private const string PLAYER_PREFS_MUSIC_VOLUME = "MusicVolume";
    public static BackgroundMusic Instance { get; private set; }

    [Range(0f, 1f)]
    [SerializeField] private float _volume = 0.2f;
    [Space]
    [SerializeField] private float _valueToChangeVolume = 0.1f;

    private AudioSource _audioSource;
    public float Volume { get => _volume; }

    private void Awake()
    {
        Instance = this;

        _audioSource = GetComponent<AudioSource>();

        LoadMusicVolume();
    }

    public void ChangeValue()
    {
        _volume += _valueToChangeVolume;
        if (_volume >= 1 + _valueToChangeVolume)
            _volume = 0;

        SaveMusicVolume();
    }

    private void SaveMusicVolume()
    {
        _audioSource.volume = _volume;
        PlayerPrefs.SetFloat(PLAYER_PREFS_MUSIC_VOLUME, _volume);
        PlayerPrefs.Save();
    }

    private void LoadMusicVolume()
    {
        _volume = PlayerPrefs.GetFloat(PLAYER_PREFS_MUSIC_VOLUME, _volume);
        _audioSource.volume = _volume;
    }
}
