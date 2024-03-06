using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Match : NetworkBehaviour
{
    public bool IsPlaying => _isPlaying;
    public bool IsEmpty => _Player.Count == 0;
    public MatchInfo Info => _info;

    [SerializeField]
    [SyncVar] private MatchInfo _info;
    private Dictionary<NetworkConnectionToClient, Player> _Player = new();

    [SerializeField]
    private Map _map;

    private bool _isPlaying = false;

    public void Initialize(NetworkConnectionToClient conn, MatchInfo info)
    {
        _info = info;
        _Player.Add(conn, null);
    }

    public bool JoinMatch(NetworkConnectionToClient conn)
    {
        if (_Player.Count >= _info.MaxPlayer)
        {
            return false;
        }

        _Player.Add(conn, null);
        return true;
    }

    public bool AddPlayer(NetworkConnectionToClient conn, Player player)
    {
        if (_Player.ContainsKey(conn))
        {
            player.MatchID = _info.ID;
            player.IsHost = _Player.Where(x => x.Value != null).ToList().Count == 0;
            player.GameState = GameState.WAITING;
            _Player[conn] = player;
            UpdateMatchStatus();
            return true;
        }

        return false;
    }

    public void RemovePlayer(NetworkConnectionToClient conn)
    {
        if (_Player.ContainsKey(conn))
        {
            _Player[conn] = null;
            UpdateMatchStatus();
        }
    }

    public void LeaveMatch(NetworkConnectionToClient conn)
    {
        if (_Player.ContainsKey(conn))
        {
            var isHost = _Player[conn].IsHost;
            _Player.Remove(conn);

            if(_Player.Count > 0)
            {
                _Player.ElementAt(0).Value.IsHost = isHost;
            }
            UpdateMatchStatus();
        }
    }

    public void StartMatch()
    {
        _isPlaying = true;
        _map.SetupRole(_Player.Count);

        foreach (var player in _Player)
        {
            player.Value.GameState = GameState.PLAYING;
            player.Value.SetRole(_map.GetRandomRole());
            player.Value.StartGame();
            player.Value.MoveToPosition(_map.GetStartPosition());
        }
    }

    private void UpdateMatchStatus()
    {
        _info.UpdateStatus($"{_Player.Where(x => x.Value != null).ToList().Count} / {_info.MaxPlayer}");
    }

    public List<NetworkConnectionToClient> GetConnections()
    {
        return _Player.Keys.ToList();
    }
}
