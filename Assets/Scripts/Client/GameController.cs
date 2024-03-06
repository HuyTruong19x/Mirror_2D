using Mirror;
using System.Collections;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameState State => _currentState;
    private GameState _currentState;

    [SerializeField]
    private UIGame _uiGame;
    [SerializeField]
    private GameObject _waitingMap;
    [SerializeField]
    private GameObject _gameMap;

    void Start()
    {
        Debug.Log("Request match id " + GameNetworkManager.singleton.Client.MatchID);
        NetworkClient.Send(new ServerMatchMessage()
        {
            Operation = MatchOperation.LOADED_GAME_SCENE,
            MatchID = GameNetworkManager.singleton.Client.MatchID
        });
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
