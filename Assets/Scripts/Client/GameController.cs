using Mirror;
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

    private void OnEnable()
    {
        EventDispatcher.Subscribe(ActionChannel.WAITING_ROOM, OnWaitingMessage);
    }

    void Start()
    {
        Debug.Log("Request room id " + GameNetworkManager.singleton.Client.RoomID);
        NetworkClient.Send(new GameMessage()
        {
            Operation = 1,
            RoomID = GameNetworkManager.singleton.Client.RoomID
        });
    }


    private void OnWaitingMessage(NetworkMessage message)
    {
        var gm = (WaitingRoomMessage)message;
        _uiGame.SetHost(gm.IsHost);
        if(gm.State != GameState.NONE)
        {
            _currentState = gm.State;
        }
        OnStateChange();
    }    

    private void OnStateChange()
    {
        _waitingMap.SetActive(_currentState == GameState.WAITING);
        _gameMap.SetActive(_currentState != GameState.WAITING);
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
