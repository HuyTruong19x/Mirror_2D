using Mirror;
using System.Collections.Generic;
using UnityEngine;

[MessageAttribute(MessageCode.MATCH)]
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
            case MatchOperation.LEAVE: LeaveMatch(conn, message); break;
            case MatchOperation.LIST: ListMatch(conn); break;
            case MatchOperation.LOADED_GAME_SCENE: CreateMatchObject(conn, message.MatchID); break;
            case MatchOperation.START_GAME: StartMatch(conn, message.MatchID); break;
        }
    }

    private void CreateMatch(NetworkConnectionToClient conn, ServerMatchMessage message)
    {
        var matchInfo = message.MatchInfo;
        var isSuccess = MatchManager.Instance.CreateMatch(conn, ref matchInfo);
        conn.Send(new ClientMatchMessage()
        {
            Operation = MatchOperation.CREATE,
            Result = isSuccess ? Result.SUCCESS : Result.FAILED,
            MatchID = matchInfo.ID
        });
    }

    private void JoinMatch(NetworkConnectionToClient conn, ServerMatchMessage message)
    {
        conn.Send(new ClientMatchMessage()
        {
            Operation = MatchOperation.JOIN,
            Result = MatchManager.Instance.JoinMatch(conn, message.MatchID) ? Result.SUCCESS : Result.FAILED
        });
    }

    private void CreateMatchObject(NetworkConnectionToClient conn, string matchID)
    {
        if (!_matchs.ContainsKey(matchID))
        {
            var go = MatchManager.Instance.GetMatchObject(matchID);
            if(go != null)
            {
                NetworkServer.Spawn(go);
            }    
            else
            {
                Debug.Log("Could not spawn match object due to not found");
            }    
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
                if (MatchManager.Instance.AddPlayerToMatch(matchID, player.GetComponent<Player>()))
                {
                    player.GetComponent<NetworkMatch>().matchId = matchID.ToGuid();
                    NetworkServer.AddPlayerForConnection(conn, player);
                    _players.Add(item, player);
                }
                else
                {
                    GameObject.Destroy(player);
                }
            }
        }
    }

    private void LeaveMatch(NetworkConnectionToClient conn, ServerMatchMessage message)
    {
        MatchManager.Instance.LeaveMatch(conn, message.MatchID);
    }

    private void ListMatch(NetworkConnectionToClient conn)
    {
        var matchs = MatchManager.Instance.GetMatchs();

        conn.Send(new ClientMatchMessage()
        {
            Operation = MatchOperation.LIST,
            MatchInfos = matchs
        });
    }

    private void StartMatch(NetworkConnectionToClient conn, string matchID)
    {
        MatchManager.Instance.StartMatch(conn, matchID);
    }
}
