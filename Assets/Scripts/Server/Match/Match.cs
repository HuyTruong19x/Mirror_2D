using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Match : NetworkBehaviour
{
    public List<Player> Players => _players;
    public List<NetworkConnectionToClient> Connections => _connections;
    public MatchInfo Info => _info;

    [SerializeField]
    [SyncVar] private MatchInfo _info;

    private List<Player> _players = new List<Player>();
    private List<NetworkConnectionToClient> _connections = new();

    [SerializeField]
    private Map _map;

    public void Initialize(NetworkConnectionToClient conn, MatchInfo info)
    {
        _info = info;
        _connections.Add(conn);
    }

    public bool JoinMatch(NetworkConnectionToClient conn)
    {
        if(_connections.Count >= _info.MaxPlayer)
        {
            return false;
        }

        _connections.Add(conn);
        return true;
    }

    public void AddPlayer(Player player)
    {
        player.IsHost = _players.Count == 0;
        player.GameState = GameState.WAITING;
        _players.Add(player);
    }

    public void RemovePlayer(Player player)
    {
        _players.Remove(player);
    }

    public void LeaveMatch(NetworkConnectionToClient conn)
    {
        if (_connections.Contains(conn))
        {
            _connections.Remove(conn);
        }
    }

    public void StartMatch()
    {
        _map.SetupRole(Players.Count);

        foreach(Player player in _players)
        {
            player.SetRole(_map.GetRandomRole());
            player.StartGame();
            player.MoveToPosition(_map.GetStartPosition());
        }
    }
}
