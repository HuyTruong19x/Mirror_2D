using Mirror;
using Mirror.Examples.MultipleMatch;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Match : NetworkBehaviour
{
    public string ID => _info.ID;
    public bool IsPlaying => _isPlaying;
    public bool IsEmpty => _player.Count == 0;

    public MatchInfo Info => _info;

    [SerializeField]
    [SyncVar] private MatchInfo _info;

    private Dictionary<NetworkConnectionToClient, Player> _player = new();
    private Dictionary<NetworkConnectionToClient, PlayerInfo> _playerInfo = new();

    [SerializeField]
    private Map _map;

    private bool _isPlaying = false;

    [SyncVar(hook = nameof(OnStatusChanged))] private string _status;

    [Header("Local Event")]
    [SerializeField]
    private StringChannelEventSO _onStatusChanged;
    [SerializeField]
    private IntChannelEventSO _onMaxPlayerChanged;

    [SerializeField]
    private DeadObject _deadPrefab;

    private Dictionary<string, int> _votes = new();

    [ClientCallback]
    private void OnEnable()
    {
        _onMaxPlayerChanged.AddListener(RequestChangeMaxPlayer);
    }

    [ClientCallback]
    private void OnDisable()
    {
        _onMaxPlayerChanged.RemoveListener(RequestChangeMaxPlayer);
    }

    [ServerCallback]
    public void Initialize(NetworkConnectionToClient conn, MatchInfo info, PlayerInfo playerInfo)
    {
        _info = info;
        _player.Add(conn, null);
        _playerInfo.Add(conn, playerInfo);
    }

    [ServerCallback]
    public bool JoinMatch(NetworkConnectionToClient conn, PlayerInfo playerInfo)
    {
        if (_player.Count >= _info.MaxPlayer)
        {
            return false;
        }

        _player.Add(conn, null);
        _playerInfo.Add(conn, playerInfo);
        return true;
    }

    [ServerCallback]
    public bool AddPlayer(NetworkConnectionToClient conn, Player player)
    {
        if (_player.ContainsKey(conn))
        {
            player.MatchID = ID;
            player.IsHost = _player.Where(x => x.Value != null).ToList().Count == 0;
            player.GameState = GameState.WAITING;
            player.PlayerInfo = _playerInfo[conn];
            _player[conn] = player;
            UpdateMatchStatus();
            return true;
        }

        return false;
    }

    [ServerCallback]
    public void RemovePlayer(NetworkConnectionToClient conn)
    {
        if (_player.ContainsKey(conn))
        {
            _player[conn] = null;
            _playerInfo[conn] = null;
            UpdateMatchStatus();
        }
    }

    [ServerCallback]
    public void LeaveMatch(NetworkConnectionToClient conn)
    {
        if (_player.ContainsKey(conn))
        {
            var isHost = _player[conn].IsHost;
            _player.Remove(conn);
            _playerInfo.Remove(conn);
            if (_player.Count > 0)
            {
                _player.ElementAt(0).Value.IsHost = isHost;
            }
            UpdateMatchStatus();
        }
    }

    [ServerCallback]
    public void StartMatch()
    {
        _votes.Clear();
        _isPlaying = true;
        _map.SetupRole(_player.Count);

        foreach (var player in _player)
        {
            player.Value.Match = this;
            player.Value.GameState = GameState.PLAYING;
            player.Value.State = PlayerState.LIVE;
            player.Value.SetRole(_map.GetRandomRole());
            player.Value.StartGame();
            player.Value.MoveToPosition(_map.GetStartPosition());
        }
    }

    [ServerCallback]
    private void UpdateMatchStatus()
    {
        _status = ($"{_player.Where(x => x.Value != null).ToList().Count} / {_info.MaxPlayer}");
        _info.Status = _status;
    }

    [ServerCallback]
    public List<NetworkConnectionToClient> GetConnections()
    {
        return _player.Keys.ToList();
    }

    [ClientCallback]
    private void OnStatusChanged(string _, string status)
    {
        _onStatusChanged.Raise(status);
    }

    [ClientCallback]
    private void RequestChangeMaxPlayer(int maxPlayer)
    {
        CmdChangeMaxPlayer(maxPlayer);
    }

    [Command(requiresAuthority = false)]
    private void CmdChangeMaxPlayer(int maxPlayer, NetworkConnectionToClient conn = null)
    {
        _info.MaxPlayer = maxPlayer;
        UpdateMatchStatus();
    }

    #region GamePlay

    [ServerCallback]
    public void KillPlayer(Player player)
    {
        var go = Instantiate(_deadPrefab, player.gameObject.transform.position, Quaternion.identity);
        go.GetComponent<NetworkMatch>().matchId = ID.ToGuid();
        go.SetPlayer(player);
        NetworkServer.Spawn(go.gameObject);
        player.State = PlayerState.DEAD;
    }

    [ServerCallback]
    public void RaiseMetting(Player playerDead)
    {
        foreach (var player in _player.Values)
        {
            player.Meeting(_player.Values.ToList(), playerDead);
        }
    }

    [ServerCallback]
    public void Vote(string playerID)
    {
        if(_votes.ContainsKey(playerID))
        {
            _votes[playerID]++;
        }
        else
        {
            _votes.Add(playerID, 1);
        }

        Debug.Log($"Vote {playerID} has {_votes[playerID]}");
    }
    #endregion
}
