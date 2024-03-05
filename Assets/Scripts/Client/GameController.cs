using Mirror;
using StinkySteak.MirrorBenchmark;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
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
        EventDispatcher.Subscribe(ActionChannel.GAME, OnReceiveMessage);
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


    private void OnReceiveMessage(NetworkMessage message)
    {
        var gm = (GameMessage)message;
        if(gm.Operation == 1)
        {
            _uiGame.SetHost(gm.IsHost);
            _currentState = gm.State;
        }
        if(gm.Operation == 2)
        {
            _currentState = gm.State;
            Debug.Log("Position = " + gm.Position);
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
