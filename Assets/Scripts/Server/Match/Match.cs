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
    public List<Player> Players => _player.Values.ToList();

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
    private List<GameObject> _deadObjects = new();

    private Dictionary<string, int> _votes = new();

    [SerializeField]
    private int _questCount = 5;
    private int _totalQuestCount = 0;
    private int _finishQuestCount = 0;

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
            //TODO check on game is playing
            UpdateMatchStatus();
        }
    }

    [ServerCallback]
    public void StartMatch()
    {
        _deadObjects.Clear();
        _votes.Clear();
        _isPlaying = true;
        _map.SetupRole(_player.Count);
        _totalQuestCount = _questCount * _player.Count;
        _finishQuestCount = 0;

        foreach (var player in _player.Values)
        {
            player.Match = this;
            player.GameState = GameState.PLAYING;
            player.State = PlayerState.LIVE;
            var role = _map.GetRandomRole();
            player.SetRole(role.Type, role.ID);
            player.SetQuests(_map.GetRandomQuests(_questCount));
            player.SetTotalQuest(_totalQuestCount);
            player.StartGame(Players);
            player.MoveToPosition(_map.GetStartPosition());
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
        _deadObjects.Add(go.gameObject);

        if (IsEndGame())
        {
            EndGame();
        }
    }

    [ServerCallback]
    public void RaiseMetting()
    {
        _votes.Clear();
        voteCount = 0;
    }

    private int voteCount = 0;
    [ServerCallback]
    public void Vote(NetworkConnectionToClient conn, string playerID)
    {
        voteCount++;
        if (string.IsNullOrEmpty(playerID))
        {
            Debug.Log($"Vote skip");
            playerID = string.Empty;
        }

        if (_votes.ContainsKey(playerID))
        {
            _votes[playerID]++;
        }
        else
        {
            _votes.Add(playerID, 1);
        }

        Debug.Log($"Vote {playerID} has {_votes[playerID]}");
        Debug.Log("Vote count : " + voteCount + " player live " + _player.Values.Count(x => x.State == PlayerState.LIVE));
        if (voteCount >= _player.Values.Count(x => x.State == PlayerState.LIVE))
        {
            //End vote
            var deadId = CalculatorDeadPlayerAfterVoted();

            if (!string.IsNullOrEmpty(deadId))
            {
                var dead = _player.Where(x => x.Value.PlayerInfo.ID == deadId).Single();
                dead.Value.State = PlayerState.DEAD;
                _player[conn].RpcEndVote(dead.Value.PlayerInfo.Name);
            }
            else
            {
                _player[conn].RpcEndVote(null);
            }

            RemoveDeadObject();
        }
    }

    private string CalculatorDeadPlayerAfterVoted()
    {
        var resultList = _votes.OrderByDescending(x => x.Value).ToList();

        if (resultList.Count > 2)
        {
            if (resultList[0].Value == resultList[1].Value)
            {
                return null;
            }

            return resultList[0].Key;
        }
        else if (resultList.Count == 1)
        {
            return resultList[0].Key;
        }

        return null;
    }

    public void CheckNextRound()
    {
        if (IsEndGame())
        {
            EndGame();
        }
        else
        {
            //new Round
            foreach (var player in _player.Values)
            {
                player.GameState = GameState.PLAYING;
                player.MoveToPosition(_map.GetStartPosition());
            }
        }
    }

    private void EndGame()
    {
        foreach (var item in _player.Values)
        {
            item.GameState = GameState.ENDING;
            item.EndGame();
        }
    }    

    private bool IsEndGame()
    {
        var enemyCount = _player.Values.Count(x => x.State == PlayerState.LIVE && x.Role.Type == RoleType.ENEMY);
        var thirdCount = _player.Values.Count(x => x.State == PlayerState.LIVE && x.Role.Type == RoleType.THIRD_PARTY);
        var normalCount = _player.Values.Count(x => x.State == PlayerState.LIVE && x.Role.Type == RoleType.NONE);

        if(enemyCount > normalCount && thirdCount == 0)
        {
            Debug.Log("End game due to enemy > normal");
            return true;
        }

        if(thirdCount > normalCount && enemyCount == 0)
        {
            Debug.Log("End game due to third > normal");
            return true;
        }    

        if(enemyCount == 0 && thirdCount == 0)
        {
            Debug.Log("End game due to normal win");
            return true;
        }

        Debug.Log($"Normal = {normalCount} - Enemy = {enemyCount} - Third = {thirdCount}");
        return false;
    }

    public void NewGame()
    {
        RemoveDeadObject();

        foreach (var item in _player.Values)
        {
            item.GameState = GameState.WAITING;
            item.State = PlayerState.LIVE;
            item.MoveToPosition(Vector3.zero);
            item.RpcNewGame();
        }
    }    

    private void RemoveDeadObject()
    {
        //Remove dead object
        foreach (var item in _deadObjects)
        {
            NetworkServer.Destroy(item);
        }
        _deadObjects.Clear();
    }    

    public void FinishQuest(int id)
    {
        _finishQuestCount += 1;
        if(_finishQuestCount >= _totalQuestCount)
        {
            EndGame();
        }    
    }    
    #endregion
}
