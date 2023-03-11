using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    [Range(0f, 1f)]
    [SerializeField] float _soundVolume;

    private Player _player;
    private float _footstepTimer;

    private readonly float _footstepTimerMax = 0.1f;

    private void Awake()
    {
        _player = GetComponent<Player>();
    }

    private void Update()
    {
        _footstepTimer -= Time.deltaTime;
        if (_footstepTimer < 0f)
        {
            _footstepTimer = _footstepTimerMax;

            if (_player.IsWalking)
                SoundsEffects.Instance.PlayFootStepsSound(_player.transform.position, _soundVolume);
        }
    }
}
