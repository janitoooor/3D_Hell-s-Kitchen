using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenGameManager : MonoBehaviour
{
    public static KitchenGameManager Instance { get; private set; }

    public event EventHandler OnStateChanged;
    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnPaused;

    [SerializeField] private GameStartCountdownUI _gameStartCountdownUI;
    [Space]
    [SerializeField] private float _waitingToStartTimer = 1f;
    [SerializeField] private float _countdownToStartTimer = 3f;
    [SerializeField] private float _gamePlayingTimer = 10f;
    [SerializeField] private float _chekStateTimer = 1f;

    public float GamePlayingTimer { get => _gamePlayingTimer; }
    public bool IsGameOver { get => _state == State.GameOver; }
    public bool IsGamePlaying { get => _state == State.GamePlaying; }
    public bool IsCountdownToStartActive { get => _state == State.CountdownToStart; }

    private bool _isGamePaused;

    private enum State
    {
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver,
    }

    private State _state;

    private void Awake()
    {
        _state = State.WaitingToStart;
        Instance = this;
    }

    private void Start()
    {
        StartCoroutine(ChekStateTimer());
        GameInput.Instance.OnPauseAction += GameInput_OnPauseAction;
    }

    public void TogglePauseGame()
    {
        _isGamePaused = !_isGamePaused;
        if (_isGamePaused)
        {
            Time.timeScale = 0f;
            OnGamePaused?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            Time.timeScale = 1f;
            OnGameUnPaused?.Invoke(this, EventArgs.Empty);
        }
    }

    private void GameInput_OnPauseAction(object sender, EventArgs e)
    {
        TogglePauseGame();
    }

    private void StateMachine()
    {
        switch (_state)
        {
            case State.WaitingToStart:
                ChangeState(ref _waitingToStartTimer, State.CountdownToStart);
                break;
            case State.CountdownToStart:
                ChangeState(ref _countdownToStartTimer, State.GamePlaying);
                _gameStartCountdownUI.UpdateCountDownText(_countdownToStartTimer);
                break;
            case State.GamePlaying:
                ChangeState(ref _gamePlayingTimer, State.GameOver);
                break;
            case State.GameOver:
                break;
        }
    }

    private void ChangeState(ref float timer, State state)
    {
        timer -= _chekStateTimer;
        if (timer <= 0)
        {
            _state = state;
            OnStateChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    private IEnumerator ChekStateTimer()
    {
        yield return new WaitForSeconds(_chekStateTimer);
        StateMachine();
        StartCoroutine(ChekStateTimer());
    }
}
