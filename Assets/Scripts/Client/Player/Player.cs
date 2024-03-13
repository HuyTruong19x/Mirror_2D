using Mirror;
using Mirror.Examples.MultipleMatch;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Player : NetworkBehaviour
{
    public static Player Local;

    public Match Match;

    [SyncVar] public string MatchID = string.Empty;
    [SyncVar(hook = nameof(OnPlayerInfoChanged))] public PlayerInfo PlayerInfo;
    [SyncVar(hook = nameof(OnHostChanged))] public bool IsHost = false;
    [SyncVar(hook = nameof(OnGameStateChanged))] public GameState GameState;
    [SyncVar(hook = nameof(OnPlayerStateChanged))] public PlayerState State = PlayerState.LIVE;
    [SyncVar] public int RoleId;
    public RoleType RoleType;

    [SerializeField]
    private Text _txtPlayerName;

    [Header("Event SO")]
    [SerializeField]
    private VoidChannelEventSO _starGameEventSO;

    [Header("Player Component")]
    [SerializeField]
    private PlayerRole _role;
    [SerializeField]
    private PlayerCamera _camera;

    [Header("Layer Setting")]
    [SerializeField]
    private LayerMask _normaViewlLayer;
    [SerializeField]
    private LayerMask _ghostViewLayer;
    [SerializeField]
    private string _layerNameOnDead = "Ghost";
    [SerializeField]
    private string _layerNamePlayer = "Player";
    private int _deadLayer;
    private int _normalLayer;

    public override void OnStartLocalPlayer()
    {
        Local = this;
        GameController.Instance.ChangeHost(IsHost);
    }

    private void Start()
    {
        _deadLayer = LayerMask.NameToLayer(_layerNameOnDead);
        _normalLayer = LayerMask.NameToLayer(_layerNamePlayer);
    }

    [ServerCallback]
    private void OnDestroy()
    {
        Disconnected(connectionToClient);
    }

    [ServerCallback]
    public void Disconnected(NetworkConnectionToClient conn)
    {
        GameNetworkManager.singleton.Server.LeaveGame(conn);
    }

    [ClientCallback]
    private void OnHostChanged(bool _, bool isHost)
    {
        GameController.Instance.ChangeHost(isHost);
    }

    [ClientCallback]
    private void OnGameStateChanged(GameState _, GameState state)
    {
        if (isLocalPlayer)
        {
            GameController.Instance.ChangeState(state);
        }
    }

    [ClientCallback]
    private void OnPlayerStateChanged(PlayerState _, PlayerState nextState)
    {
        if (nextState == PlayerState.DEAD)
        {
            gameObject.layer = _deadLayer;
            GetComponent<SpriteRenderer>().color = Color.red;
            Dead();
        }
        else
        {
            gameObject.layer = _normalLayer;
            GetComponent<SpriteRenderer>().color = Color.white;
            Live();
        }
    }

    [ClientCallback]
    private void OnPlayerInfoChanged(PlayerInfo _, PlayerInfo playerInfo)
    {
        gameObject.name = isLocalPlayer ? "Local_" + playerInfo.Name : playerInfo.Name;
        _txtPlayerName.text = playerInfo.Name;
        _txtPlayerName.gameObject.transform.parent.gameObject.SetActive(!isServer);
    }

    [TargetRpc]
    public void StartGame(List<Player> players)
    {
        gameObject.layer = _normalLayer;
        if (isLocalPlayer)
        {
            _role.UpdateRole(RoleId);
            GameController.Instance.StartGame(this, players);
            _camera.UpdateViewLayer(_normaViewlLayer);
        }
    }

    [TargetRpc]
    public void MoveToPosition(Vector3 pos)
    {
        transform.position = pos;
    }


    public void KillPlayer(Player player)
    {
        CmdKill(player);
    }

    [Command]
    private void CmdKill(Player player)
    {
        Match.KillPlayer(player);
    }

    [ClientCallback]
    public void Dead()
    {
        if (isLocalPlayer)
        {
            _camera.UpdateViewLayer(_ghostViewLayer);
            _role.Dead();
            GameController.Instance.Dead();
        }
    }

    [ClientCallback]
    public void Live()
    {
        if (isLocalPlayer)
        {
            _camera.UpdateViewLayer(_normaViewlLayer);
            _role.Hide();
        }
    }

    [ServerCallback]
    public void RaiseMetting(Player player = null)
    {
        Match.RaiseMetting();
        Meeting(PlayerInfo.ID, Match.Players, player);
    }

    [ClientRpc]
    public void Meeting(string raisePlayerId, List<Player> players, Player player)
    {
        GameState = GameState.TALKING;
        GameController.Instance.Meeting(raisePlayerId, players, player);
    }

    public void Vote(string playerID)
    {
        CmdVote(PlayerInfo.ID, playerID);
    }

    [Command]
    private void CmdVote(string playerIdVoted, string playerIDTarget)
    {
        Match.Vote(connectionToClient, playerIDTarget);
        RpcVote(playerIdVoted, playerIDTarget);
    }

    [ClientRpc]
    private void RpcVote(string whoVoted, string targetVote)
    {
        GameController.Instance.Vote(whoVoted, targetVote);
    }

    [ClientRpc]
    public void RpcEndVote(string playerName)
    {
        StartCoroutine(CoPlayVoteResult(playerName));
    }

    private IEnumerator CoPlayVoteResult(string playerName)
    {
        yield return GameController.Instance.EndVote(playerName);
        if (IsHost)
        {
            CheckNextRound();
        }
    }

    public void EndGame()
    {
        if (isLocalPlayer)
        {
            GameController.Instance.EndGame();
        }
    }

    [Command]
    public void CheckNextRound()
    {
        Match.CheckNextRound();
    }
}

public enum PlayerState
{
    NONE = 0,
    LIVE = 1,
    DEAD = 2,
}