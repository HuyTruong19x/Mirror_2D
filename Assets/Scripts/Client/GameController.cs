using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GameController : SingletonBehavior<GameController>
{
    public GameState State => _currentState;
    private GameState _currentState;

    [SerializeField]
    private GameObject _waitingMap;
    [SerializeField]
    private GameObject _gameMap;
    private bool _isHost;

    [Header("UI")]
    [SerializeField]
    private UIDead _uiDead;
    [SerializeField]
    private UIMeeting _uiMeeting;
    [SerializeField]
    private UIEndGame _uiEndGame;
    [SerializeField]
    private UIFakeStartGame _uiFakeStartGame;
    [SerializeField]
    private UIWaiting _uiWaiting;

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
        _uiWaiting.ShowStartButton(_currentState == GameState.WAITING && _isHost);
        _waitingMap.SetActive(_currentState == GameState.WAITING);
        _gameMap.SetActive(_currentState != GameState.WAITING);
    }

    public void ChangeHost(bool isHost)
    {
        _isHost = isHost;
        _uiWaiting.ShowStartButton(isHost && _currentState == GameState.WAITING);
    }

    public void Dead()
    {
        _uiDead.Show();
    }

    public void Meeting(string raisePlayerId, List<Player> players, Player player)
    {
        _uiMeeting.Show(raisePlayerId, players, player);
    }

    public void Vote(string playerIdVoted, string playerIdTarget)
    {
        _uiMeeting.UpdateVote(playerIdVoted, playerIdTarget);
    }

    public IEnumerator EndVote(string playerName)
    {
        yield return _uiMeeting.EndVote(playerName);
    }

    public void EndGame()
    {
        _uiEndGame.Show();
    }

    public void StartGame(Player ownPlayer, List<Player> players)
    {
        _uiWaiting.Hide();
        _uiFakeStartGame.Show(ownPlayer, players);
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
