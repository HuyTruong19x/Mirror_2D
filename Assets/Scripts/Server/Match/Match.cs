using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Match : NetworkBehaviour
{
    public string ID => _info.ID;
    public bool IsPlaying => _isPlaying;
    public bool IsEmpty => _Player.Count == 0;

    public MatchInfo Info => _info;

    [SerializeField]
    [SyncVar] private MatchInfo _info;

    private Dictionary<NetworkConnectionToClient, Player> _Player = new();

    [SerializeField]
    private Map _map;

    private bool _isPlaying = false;

    [SyncVar(hook = nameof(OnStatusChanged))] private string _status;
    [SerializeField]
    private StringChannelEventSO _onStatusChanged;

    [ServerCallback]
    public void Initialize(NetworkConnectionToClient conn, MatchInfo info)
    {
        _info = info;
        _Player.Add(conn, null);
    }

    [ServerCallback]
    public bool JoinMatch(NetworkConnectionToClient conn)
    {
        if (_Player.Count >= _info.MaxPlayer)
        {
            return false;
        }

        _Player.Add(conn, null);
        return true;
    }

    [ServerCallback]
    public bool AddPlayer(NetworkConnectionToClient conn, Player player)
    {
        if (_Player.ContainsKey(conn))
        {
            player.MatchID = ID;
            player.IsHost = _Player.Where(x => x.Value != null).ToList().Count == 0;
            player.GameState = GameState.WAITING;
            _Player[conn] = player;
            UpdateMatchStatus();
            return true;
        }

        return false;
    }

    [ServerCallback]
    public void RemovePlayer(NetworkConnectionToClient conn)
    {
        if (_Player.ContainsKey(conn))
        {
            _Player[conn] = null;
            UpdateMatchStatus();
        }
    }

    [ServerCallback]
    public void LeaveMatch(NetworkConnectionToClient conn)
    {
        if (_Player.ContainsKey(conn))
        {
            var isHost = _Player[conn].IsHost;
            _Player.Remove(conn);

            if (_Player.Count > 0)
            {
                _Player.ElementAt(0).Value.IsHost = isHost;
            }
            UpdateMatchStatus();
        }
    }

    [ServerCallback]
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

    [ServerCallback]
    private void UpdateMatchStatus()
    {
        _status = ($"{_Player.Where(x => x.Value != null).ToList().Count} / {_info.MaxPlayer}");
        _info.Status = _status;
    }

    [ServerCallback]
    public List<NetworkConnectionToClient> GetConnections()
    {
        return _Player.Keys.ToList();
    }

    [ClientCallback]
    private void OnStatusChanged(string _, string status)
    {
        _onStatusChanged.Raise(status);
    }
}
