using Mirror;
using System;
using System.Collections.Generic;
using UnityEngine;

[MessageAttribute(ActionChannel.GAME)]
public class ServerGameHandler : MessageHandler<GameMessage>
{
    private Dictionary<Guid, GameManager> _gameManagers = new();
    private Dictionary<NetworkConnectionToClient, GameObject> _games = new();

    public override void Handle(NetworkConnectionToClient conn, GameMessage message)
    {
        switch(message.Operation)
        {
            case 1: OnAlreadyJoinGame(conn, message); break;
            case 2: OnStartGame(conn, message); break;
        }    
    }

    private void OnAlreadyJoinGame(NetworkConnectionToClient conn, GameMessage message)
    {
        var roomGuid = message.RoomID.ToGuid();
        if (!_gameManagers.ContainsKey(roomGuid))
        {
            GameObject matchControllerObject = GameObject.Instantiate(NetworkManager.singleton.spawnPrefabs[0]);
            matchControllerObject.GetComponent<NetworkMatch>().matchId = message.RoomID.ToGuid();
            _gameManagers[roomGuid] = matchControllerObject.GetComponent<GameManager>();
            NetworkServer.Spawn(_gameManagers[roomGuid].gameObject);
        }

        var room = RoomManager.Instance.GetRoom(message.RoomID);

        if(room == null)
        {
            Debug.Log("Not found room id " + message.RoomID);
            return;
        }

        foreach (var item in room.Players)
        {
            if (_games.ContainsKey(item.Key))
            {
                continue;
            }
            else
            {
                GameObject player = GameObject.Instantiate(NetworkManager.singleton.playerPrefab);
                player.GetComponent<NetworkMatch>().matchId = roomGuid;
                NetworkServer.AddPlayerForConnection(item.Key, player);
                _games.Add(item.Key, player);
                _gameManagers[roomGuid].AddPlayer(player.GetComponent<PlayerController>());
            }
        }

        conn.Send(new WaitingRoomMessage()
        {
            State = GameState.WAITING,
            IsHost = room.IsHost(conn)
        });
    }

    public void OnStartGame(NetworkConnectionToClient conn, GameMessage message)
    {
        var roomGuid = message.RoomID.ToGuid();
        if(_gameManagers.ContainsKey(roomGuid))
        {
            _gameManagers[roomGuid].StartGame();
        }
        else
        {
            Debug.LogWarning($"Could not start room {message.RoomID} due to not found");
        }    
    }
}
