using Mirror;
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

    void Start()
    {
        Debug.Log("Request match id " + GameNetworkManager.singleton.Client.MatchID);
        NetworkClient.Send(new ServerMatchMessage()
        {
            Operation = MatchOperation.LOADED_GAME_SCENE,
            MatchID = GameNetworkManager.singleton.Client.MatchID
        });

        _uiGame.SetMatchID(GameNetworkManager.singleton.Client.MatchID);
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
}

public enum GameState
{
    NONE = 0,
    WAITING = 1,
    PLAYING = 2,
    TALKING = 3,
    ENDING = 4,
}
