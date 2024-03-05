using Mirror;
using System;
using System.Collections.Generic;
using UnityEngine;

[MessageAttribute(ActionChannel.GAME)]
public class ServerGameHandler : MessageHandler<GameMessage>
{
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
        if (!GameManager.Instance.HasRoom(roomGuid))
        {
            NetworkServer.Spawn(GameManager.Instance.CreateGame(roomGuid));
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
                GameManager.Instance.AddPlayerToGame(roomGuid, player.GetComponent<PlayerController>());
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
        GameManager.Instance.StartGame(roomGuid);
    }
}
