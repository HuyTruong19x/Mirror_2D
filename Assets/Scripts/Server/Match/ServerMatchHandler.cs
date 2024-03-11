using Mirror;
using Mirror.Examples.MultipleMatch;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[MessageAttribute(MessageCode.MATCH)]
public class ServerMatchHandler : MessageHandler<ServerMatchMessage>
{
    private readonly Dictionary<NetworkConnectionToClient, string> _matchIDSaver = new();
    private readonly Dictionary<string, GameObject> _matchs = new();
    private Dictionary<NetworkConnectionToClient, GameObject> _players = new();

    public override void Handle(NetworkConnectionToClient conn, ServerMatchMessage message)
    {
        switch (message.Operation)
        {
            case MatchOperation.CREATE: CreateMatch(conn, message); break;
            case MatchOperation.JOIN: JoinMatch(conn, message); break;
            case MatchOperation.QUICK_JOIN: QuickJoinMatch(conn, message); break;
            case MatchOperation.LEAVE: LeaveMatch(conn); break;
            case MatchOperation.LIST: ListMatch(conn); break;
            case MatchOperation.LOADED_GAME_SCENE: CreateMatchObject(conn); break;
            case MatchOperation.START_GAME: StartMatch(conn); break;
        }
    }

    private void CreateMatch(NetworkConnectionToClient conn, ServerMatchMessage message)
    {
        var matchInfo = message.MatchInfo;
        var isSuccess = MatchManager.Instance.CreateMatch(conn, ref matchInfo, message.PlayerInfo);
        conn.Send(new ClientMatchMessage()
        {
            Operation = MatchOperation.CREATE,
            Result = isSuccess ? Result.SUCCESS : Result.FAILED,
            MatchID = matchInfo.ID
        });

        _matchs.Add(matchInfo.ID, null);

        if (isSuccess)
        {
            _matchIDSaver.Add(conn, matchInfo.ID);
        }
    }

    private void QuickJoinMatch(NetworkConnectionToClient conn, ServerMatchMessage message)
    {
        foreach (var match in _matchs.ToArray())
        {
            Match mat = MatchManager.Instance.JoinMatch(conn, match.Key, message.PlayerInfo);
            if (mat != null)
            {
                conn.Send(new ClientMatchMessage()
                {
                    Operation = MatchOperation.JOIN,
                    Result = Result.SUCCESS,
                    MatchID = match.Key,
                });

                _matchIDSaver.Add(conn, match.Key);
                return;
            }
        }

        conn.Send(new ClientMatchMessage()
        {
            Operation = MatchOperation.JOIN,
            Result = Result.FAILED
        });
    }

    private void JoinMatch(NetworkConnectionToClient conn, ServerMatchMessage message)
    {
        var result = MatchManager.Instance.JoinMatch(conn, message.MatchID, message.PlayerInfo);
        conn.Send(new ClientMatchMessage()
        {
            Operation = MatchOperation.JOIN,
            Result = result != null ? Result.SUCCESS : Result.FAILED,
            MatchID = message.MatchID
        });

        if (result != null)
        {
            _matchIDSaver.Add(conn, message.MatchID);
        }
    }

    private void CreateMatchObject(NetworkConnectionToClient conn)
    {
        if (_matchIDSaver.TryGetValue(conn, out var matchID))
        {
            if (_matchs[matchID] == null)
            {
                var go = MatchManager.Instance.GetMatchObject(matchID);
                if (go != null)
                {
                    NetworkServer.Spawn(go);
                    _matchs[matchID] = go;
                }
                else
                {
                    Debug.Log("Could not spawn match object due to not found");
                }
            }

            SpawnPlayer(matchID);

            conn.Send(new ClientMatchInfoMessage()
            {
                Info = MatchManager.Instance.GetMatch(matchID).Info
            });
        }
    }

    private void SpawnPlayer(string matchID)
    {
        var match = MatchManager.Instance.GetMatch(matchID);

        if (match == null)
        {
            Debug.Log("Not found match id " + matchID);
            return;
        }

        foreach (var item in match.GetConnections())
        {
            if (_players.ContainsKey(item))
            {
                continue;
            }
            else
            {
                GameObject player = GameObject.Instantiate(NetworkManager.singleton.playerPrefab);
                if (MatchManager.Instance.AddPlayerToMatch(item, matchID, player.GetComponent<Player>()))
                {
                    player.name = "Player_" + _players.Count;
                    player.GetComponent<NetworkMatch>().matchId = matchID.ToGuid();
                    NetworkServer.AddPlayerForConnection(item, player);
                    _players.Add(item, player);
                }
                else
                {
                    GameObject.Destroy(player);
                }
            }
        }
    }

    private void LeaveMatch(NetworkConnectionToClient conn)
    {
        if (_matchIDSaver.TryGetValue(conn, out string matchID))
        {
            GameObject.Destroy(_players[conn]);
            _players.Remove(conn);
            MatchManager.Instance.LeaveMatch(conn, matchID);
            _matchIDSaver.Remove(conn);
        }

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

    private void StartMatch(NetworkConnectionToClient conn)
    {
        if (_matchIDSaver.TryGetValue(conn, out var matchID))
        {
            MatchManager.Instance.StartMatch(conn, matchID);
        }

    }
}
