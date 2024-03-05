using Mirror;
using Mirror.Examples.MultipleMatch;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[MessageAttribute(ActionChannel.MATCH)]
public class ServerMatchHandler : MessageHandler<ServerMatchMessage>
{
    private readonly Dictionary<string, GameObject> _matchs = new();
    private Dictionary<NetworkConnectionToClient, GameObject> _players = new();

    public override void Handle(NetworkConnectionToClient conn, ServerMatchMessage message)
    {
        switch (message.Operation)
        {
            case MatchOperation.CREATE: CreateMatch(conn, message); break;
            case MatchOperation.JOIN: JoinMatch(conn, message); break;
            case MatchOperation.LOADED_GAME_SCENE: CreateMatchObject(conn, message.MatchID); break;
        }
    }

    private void CreateMatch(NetworkConnectionToClient conn, ServerMatchMessage message)
    {
        var matchInfo = message.MatchInfo;
        var isSuccess = MatchManager.Instance.CreateMatch(conn, ref matchInfo);
        conn.Send(new ClientRoomMessage()
        {
            Operation = isSuccess ? ClientRoomOperation.CREATED : ClientRoomOperation.CREATE_FAIL,
            RoomID = matchInfo.ID,
            RoomName = matchInfo.HostName
        });
    }

    private void JoinMatch(NetworkConnectionToClient conn, ServerMatchMessage message)
    {
        conn.Send(new ClientRoomMessage()
        {
            Operation = MatchManager.Instance.JoinMatch(conn, message.MatchID) ? ClientRoomOperation.JOINED : ClientRoomOperation.JOIN_FAIL,
        });
    }

    private void CreateMatchObject(NetworkConnectionToClient conn, string matchID)
    {
        if (!_matchs.ContainsKey(matchID))
        {
            NetworkServer.Spawn(MatchManager.Instance.GetMatchObject(matchID));
        }

        SpawnPlayer(conn, matchID);
    }

    private void SpawnPlayer(NetworkConnectionToClient conn, string matchID)
    {
        var match = MatchManager.Instance.GetMatch(matchID);

        if (match == null)
        {
            Debug.Log("Not found match id " + matchID);
            return;
        }

        foreach (var item in match.Connections)
        {
            if (_players.ContainsKey(item))
            {
                continue;
            }
            else
            {
                GameObject player = GameObject.Instantiate(NetworkManager.singleton.playerPrefab);
                player.GetComponent<NetworkMatch>().matchId = matchID.ToGuid();
                NetworkServer.AddPlayerForConnection(conn, player);
                _players.Add(item, player);
            }
        }
    }
}
