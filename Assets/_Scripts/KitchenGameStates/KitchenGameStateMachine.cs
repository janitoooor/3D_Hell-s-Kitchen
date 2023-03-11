using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class KitchenGameStateMachine : NetworkBehaviour
{
    public static KitchenGameStateMachine Instance { get; private set; }

    public event EventHandler OnStateChanged;
    public event EventHandler OnLocalGamePaused;
    public event EventHandler OnLocalGameUnPaused;
    public event EventHandler OnMultiplayerGamePaused;
    public event EventHandler OnMultiplayerGameUnPaused;
    public event EventHandler OnLocalPlayerReadyChanged;
    public event EventHandler OnCountdownToStartTimerChanged;

    [SerializeField] private Transform _playerPrefab;
    [Space]
    [SerializeField] private GameStartCountdownUI _gameStartCountdownUI;
    [Space]
    [SerializeField] private NetworkVariable<float> _countdownToStartTimer = new(3f);
    [SerializeField] private NetworkVariable<float> _gamePlayingTimer = new(60f);
    [SerializeField] private float _chekStateTimer = 1f;

    public float CountdownToStartTimer { get => _countdownToStartTimer.Value; }
    public float GamePlayingTimer { get => _gamePlayingTimer.Value; }
    public bool IsGameOver { get => _state.Value == State.GameOver; }
    public bool IsGamePlaying { get => _state.Value == State.GamePlaying; }
    public bool IsCountdownToStartActive { get => _state.Value == State.CountdownToStart; }
    public bool IsWaitingToStart { get => _state.Value == State.WaitingToStart; }

    private bool _isLocalPlayerReady;
    public bool IsLocalPlayerReady { get => _isLocalPlayerReady; }

    private bool _isLocalGamePaused;

    private bool _autoTestGamePausedState;

    private Dictionary<ulong, bool> _playerReadyDictionary;
    private Dictionary<ulong, bool> _playerPauseDictionary;

    private enum State
    {
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver,
    }

    private readonly NetworkVariable<bool> _isGamePaused = new(false);
    private readonly NetworkVariable<State> _state = new(State.WaitingToStart);

    private void Awake()
    {
        Instance = this;

        _playerReadyDictionary = new();
        _playerPauseDictionary = new();
    }

    private void Start()
    {
        GameInput.Instance.OnPauseAction += GameInput_OnPauseAction;
        GameInput.Instance.OnInteractAction += GameInput_OnInteractAction;
    }

    private void LateUpdate()
    {
        if (_autoTestGamePausedState)
        {
            _autoTestGamePausedState = false;
            TestGamePauseState();
        }
    }

    public override void OnNetworkSpawn()
    {
        _state.OnValueChanged += State_OnValueChanged;
        _countdownToStartTimer.OnValueChanged += CountDownToStartTimer_OnValueChanged;
        _isGamePaused.OnValueChanged += IsGamePaused_OnValueChanged;

        if (IsServer)
        {
            NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectCallback;
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += NetworkManager_OnLoadEventCompleted;
        }
    }

    private void NetworkManager_OnLoadEventCompleted(string sceneName, UnityEngine.SceneManagement.LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            Transform playerTransform = Instantiate(_playerPrefab);
            playerTransform.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId, true);
        }
    }

    private void NetworkManager_OnClientDisconnectCallback(ulong clientId)
    {
        _autoTestGamePausedState = true;
        TestGamePauseState();
    }

    private void IsGamePaused_OnValueChanged(bool previousValue, bool newValue)
    {
        if (_isGamePaused.Value)
        {
            Time.timeScale = 0;
            OnMultiplayerGamePaused?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            Time.timeScale = 1;
            OnMultiplayerGameUnPaused?.Invoke(this, EventArgs.Empty);
        }
    }

    private void CountDownToStartTimer_OnValueChanged(float previousValue, float newValue)
    {
        OnCountdownToStartTimerChanged?.Invoke(this, EventArgs.Empty);
    }

    private void State_OnValueChanged(State previousValue, State newValue)
    {
        OnStateChanged?.Invoke(this, EventArgs.Empty);
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e)
    {
        if (_state.Value == State.WaitingToStart)
        {
            _isLocalPlayerReady = true;
            OnLocalPlayerReadyChanged?.Invoke(this, EventArgs.Empty);

            SetPlayerReadyServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerReadyServerRpc(ServerRpcParams serverRpcParams = default)
    {
        _playerReadyDictionary.Add(serverRpcParams.Receive.SenderClientId, true);

        bool allClientsReady = true;

        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            if (!_playerReadyDictionary.ContainsKey(clientId) || !_playerReadyDictionary[clientId])
            {
                allClientsReady = false;
                break;
            }
        }

        if (allClientsReady)
        {
            _state.Value = State.CountdownToStart;
            StartCoroutine(ChekStateTimer());
        }
    }

    public void TogglePauseGame()
    {
        _isLocalGamePaused = !_isLocalGamePaused;
        if (_isLocalGamePaused)
        {
            PauseGameServerRpc();
            OnLocalGamePaused?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            UnPauseGameServerRpc();
            OnLocalGameUnPaused(this, EventArgs.Empty);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void PauseGameServerRpc(ServerRpcParams serverRpcParams = default)
    {
        _playerPauseDictionary[serverRpcParams.Receive.SenderClientId] = true;

        TestGamePauseState();
    }

    [ServerRpc(RequireOwnership = false)]
    private void UnPauseGameServerRpc(ServerRpcParams serverRpcParams = default)
    {
        _playerPauseDictionary[serverRpcParams.Receive.SenderClientId] = false;

        TestGamePauseState();
    }

    private void TestGamePauseState()
    {
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            if (_playerPauseDictionary.ContainsKey(clientId) && _playerPauseDictionary[clientId])
            {
                _isGamePaused.Value = true;
                return;
            }
        }

        _isGamePaused.Value = false;
    }

    private void GameInput_OnPauseAction(object sender, EventArgs e)
    {
        TogglePauseGame();
    }

    private void StateMachine()
    {
        if (!IsServer)
            return;

        switch (_state.Value)
        {
            case State.WaitingToStart:
                break;
            case State.CountdownToStart:
                _countdownToStartTimer.Value -= _chekStateTimer;
                if (_countdownToStartTimer.Value < 0f)
                    ChangeState(State.GamePlaying);
                break;
            case State.GamePlaying:
                _gamePlayingTimer.Value -= _chekStateTimer;
                if (_gamePlayingTimer.Value <= 0f)
                    ChangeState(State.GameOver);
                break;
            case State.GameOver:
                break;
        }
    }

    private void ChangeState(State state)
    {
        _state.Value = state;
    }

    private IEnumerator ChekStateTimer()
    {
        yield return new WaitForSeconds(_chekStateTimer);
        StateMachine();
        StartCoroutine(ChekStateTimer());
    }
}
