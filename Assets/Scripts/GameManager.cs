using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private enum GamePhase
    {
        Expectation, Hiding, Survival, End
    }

    [SerializeField] private Timer _timer;
    [SerializeField] private EnemyManager _enemyManager;
    [SerializeField] private PlayerAIManager _playerAIManager;
    [SerializeField] private RoomManager _roomManager;

    private GamePhase _gamePhase;

    public RoomManager RoomManager => _roomManager;

    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        _gamePhase = GamePhase.Expectation;
        StartTimerHiding();
    }

    public void EndTimerHiding()
    {
        if (_gamePhase == GamePhase.Hiding)
        {
            StartTimerSurvival();
            _roomManager.CloseAllDors();
        }
    }

    private void Init()
    {
        _roomManager.Init(this);
        _playerAIManager.Init(this);
        _enemyManager.Init(this);
    }

    private void StartTimerHiding()
    {
        int minutes = 0;
        int seconds = 30;
        int totalSeconds = CalculationTime(minutes, seconds);
        _gamePhase = GamePhase.Hiding;

        _playerAIManager.SpawnAI();

        StartCoroutine(_timer.Countdown(minutes, seconds));
        Invoke(nameof(EndTimerHiding), totalSeconds);
    }

    private void StartTimerSurvival()
    {
        int minutes = 7;
        int seconds = 0;
        int totalSeconds = CalculationTime(minutes, seconds);
        _gamePhase = GamePhase.Survival;

        _enemyManager.StartSpaw();
        StartCoroutine(_timer.Countdown(minutes, seconds));
        Invoke(nameof(EndTimerSurvival), totalSeconds);
    }

    private void EndTimerSurvival()
    {

    }

    private int CalculationTime(int minutes, int seconds)
    {
        return (minutes * 60) + seconds;
    }
}
