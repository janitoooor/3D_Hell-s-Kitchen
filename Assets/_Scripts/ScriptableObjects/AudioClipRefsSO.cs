using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/AudioSO/AudioClipRefsSO")]
public class AudioClipRefsSO : ScriptableObject
{
    [SerializeField] private AudioClip[] _chop;
    [SerializeField] private AudioClip[] _deliveryFail;
    [SerializeField] private AudioClip[] _deliverySuccess;
    [SerializeField] private AudioClip[] _footstep;
    [SerializeField] private AudioClip[] _objectDrop;
    [SerializeField] private AudioClip[] _objectPickup;
    [SerializeField] private AudioClip[] _trash;
    [SerializeField] private AudioClip[] _warning;
    [SerializeField] private AudioClip[] _stoveSizzle;

    public AudioClip[] Chop { get => _chop; }
    public AudioClip[] DeliveryFail { get => _deliveryFail; }
    public AudioClip[] DeliverySuccess { get => _deliverySuccess; }
    public AudioClip[] FootStep { get => _footstep; }
    public AudioClip[] ObjectDrop { get => _objectDrop; }
    public AudioClip[] ObjectPickup { get => _objectPickup; }
    public AudioClip[] Trash { get => _trash; }
    public AudioClip[] Warning { get => _warning; }
    public AudioClip[] StoveSizzle { get => _stoveSizzle; }
}
