using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SoundsEffects : MonoBehaviour
{
    public const string PLAYER_PREFS_SOUND_EFFECTS_VOLUME = "SoundsEffectsVolume";

    public static SoundsEffects Instance;

    public event EventHandler OnVolumeChanged;

    [Range(0f, 1f)]
    [SerializeField] private float _volume = 1f;
    [Space]
    [SerializeField] private float _valueToChangeVolume = 0.1f;
    [Space]
    [SerializeField] private AudioClipRefsSO _audioClipRefsSO;

    public float Volume { get => _volume; }

    private DeliveryCounter _deliveryCounter;

    private void Awake()
    {
        Instance = this;

        _volume = PlayerPrefs.GetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, _volume);
    }

    private void Start()
    {
        Player.OnAnyPickedSomething += Player_OnPickSomething;
        DeliveryManager.Instance.OnRecipeSuccess += DeliveryManager_OnRecipeSuccess;
        DeliveryManager.Instance.OnRecipeFailed += DeliveryManager_OnRecipeFailed;
        CuttingCounter.OnAnyCut += CuttingCounter_OnAnyCut;
        TrashCounter.OnAnyObjectTrashed += TrashCounter_OnAnyObjectTrashed;
        BaseCounter.OnAnyObjectPlacedHere += BaseCounter_OnAnyObjectPlacedHere;

        _deliveryCounter = DeliveryCounter.Instance;
    }

    public void ChangeValue()
    {
        _volume += _valueToChangeVolume;
        if (_volume >= 1 + _valueToChangeVolume)
            _volume = 0;

        PlayerPrefs.SetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, _volume);
        PlayerPrefs.Save();
        OnVolumeChanged?.Invoke(this, EventArgs.Empty);
    }

    private void TrashCounter_OnAnyObjectTrashed(object sender, EventArgs e)
    {
        TrashCounter trashCounter = sender as TrashCounter;
        PlaySound(_audioClipRefsSO.Trash, trashCounter.transform.position);
    }

    private void BaseCounter_OnAnyObjectPlacedHere(object sender, EventArgs e)
    {
        BaseCounter baseCounter = sender as BaseCounter;
        PlaySound(_audioClipRefsSO.ObjectPickup, baseCounter.transform.position);
    }

    private void Player_OnPickSomething(object sender, EventArgs e)
    {
        Player player = sender as Player;
        PlaySound(_audioClipRefsSO.ObjectPickup, player.transform.position);
    }

    private void CuttingCounter_OnAnyCut(object sender, EventArgs e)
    {
        CuttingCounter cuttingCounter = sender as CuttingCounter;
        PlaySound(_audioClipRefsSO.Chop, cuttingCounter.transform.position);
    }

    private void DeliveryManager_OnRecipeFailed(object sender, EventArgs e)
    {
        PlaySound(_audioClipRefsSO.DeliveryFail, _deliveryCounter.transform.position);
    }

    private void DeliveryManager_OnRecipeSuccess(object sender, EventArgs e)
    {
        PlaySound(_audioClipRefsSO.DeliverySuccess, _deliveryCounter.transform.position);
    }

    private void PlaySound(AudioClip audioClip, Vector3 position, float volume = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volume);
    }

    private void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volumeMultiplier = 1f)
    {
        PlaySound(audioClipArray[UnityEngine.Random.Range(0, audioClipArray.Length)], position, volumeMultiplier * _volume);
    }

    public void PlayFootStepsSound(Vector3 position, float volumeMultiplier = 1f)
    {
        PlaySound(_audioClipRefsSO.FootStep[UnityEngine.Random.Range(0, _audioClipRefsSO.FootStep.Length)], position, volumeMultiplier * _volume);
    }

    public void PlayCountdownSound()
    {
        PlaySound(_audioClipRefsSO.Warning, Vector3.zero);
    }

    public void PlayBurnWarningSound(Vector3 position)
    {
        PlaySound(_audioClipRefsSO.Warning, position);
    }

    public void PlayeWarningTimerSound()
    {
        PlaySound(_audioClipRefsSO.WarningTimer, Camera.main.transform.position);
    }
}
