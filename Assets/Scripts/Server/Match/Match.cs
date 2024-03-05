using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Match : NetworkBehaviour
{
    public List<Player> Players => _players;
    public List<NetworkConnectionToClient> Connections => _connections;

    [SerializeField]
    [SyncVar] private MatchInfo _Info;
    private List<Player> _players = new List<Player>();
    private List<NetworkConnectionToClient> _connections = new();

    public void Initialize(NetworkConnectionToClient conn, MatchInfo info)
    {
        _Info = info;
        _connections.Add(conn);
    }

    public bool JoinMatch(NetworkConnectionToClient conn)
    {
        if(_connections.Count >= _Info.MaxPlayer)
        {
            return false;
        }

        _connections.Add(conn);
        return true;
    }

    public void AddPlayer(Player player)
    {
        _players.Add(player);
    }

    public void RemovePlayer(Player player)
    {
        _players.Remove(player);
    }
}
