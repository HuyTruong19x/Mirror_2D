using Mirror;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GameController : SingletonBehavior<GameController>
{
    public GameState State => _currentState;
    private GameState _currentState;

    [SerializeField]
    private UIGame _uiGame;
    [SerializeField]
    private GameObject _waitingMap;
    [SerializeField]
    private GameObject _gameMap;
    private bool _isHost;
    [SerializeField]
    private UIDead _uiDead;
    [SerializeField]
    private UIMeeting _uiMeeting;

    void Start()
    {
        GameNetworkManager.singleton.Client.RequestLoadedGame();
    } 

    public void ChangeState(GameState state)
    {
        _currentState = state;
        OnStateChange();
    }

    private void OnStateChange()
    {
        _uiGame.ShowStartButton(_currentState == GameState.WAITING && _isHost);
        _waitingMap.SetActive(_currentState == GameState.WAITING);
        _gameMap.SetActive(_currentState != GameState.WAITING);
    }

    public void ChangeHost(bool isHost)
    {
        _isHost = isHost;
        _uiGame.ShowStartButton(isHost && _currentState == GameState.WAITING);
    }

    public void Dead()
    {
        _uiDead.Show();
    }

    public void Meeting(List<Player> players, Player player)
    {
        _uiMeeting.Show(players, player);
    }
}

public enum GameState
{
    NONE = 0,
    WAITING = 1,
    PLAYING = 2,
    TALKING = 3,
    ENDING = 4,
}
