using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[MessageAttribute(ActionChannel.GAME)]
public class ServerGameHandler : MessageHandler<GameMessage>
{
    private Dictionary<Guid, GameManager> _gameManagers = new();
    private Dictionary<NetworkConnectionToClient, GameObject> _games = new();

    public override void Handle(NetworkConnectionToClient conn, GameMessage message)
    {
        var roomId = "xxxx";
        var roomGuid = roomId.ToGuid();
        if (!_gameManagers.ContainsKey(roomGuid))
        {
            GameObject matchControllerObject = GameObject.Instantiate(NetworkManager.singleton.spawnPrefabs[0]);
            matchControllerObject.GetComponent<NetworkMatch>().matchId = roomId.ToGuid();
            _gameManagers[roomGuid] = matchControllerObject.GetComponent<GameManager>();
            NetworkServer.Spawn(_gameManagers[roomGuid].gameObject);
        }


        foreach (var item in RoomManager.Instance.Rooms["xxxx"].Players)
        {
            if(_games.ContainsKey(item.Key))
            {
                continue;
            }    
            else
            {
                GameObject player = GameObject.Instantiate(NetworkManager.singleton.playerPrefab);
                player.GetComponent<NetworkMatch>().matchId = roomGuid;
                NetworkServer.AddPlayerForConnection(item.Key, player);
                _games.Add(item.Key, player);
            }    
        }
    }
}
