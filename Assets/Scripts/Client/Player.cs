using Mirror;
using Mirror.Examples.MultipleMatch;
using System.Collections.Generic;
using UnityEngine;


public class Player : NetworkBehaviour
{
    public static Player Local;

    public Match Match;

    [SyncVar] public string MatchID = string.Empty;
    [SyncVar(hook = nameof(OnPlayerInfoChanged))] public PlayerInfo PlayerInfo;
    [SyncVar(hook = nameof(OnHostChanged))] public bool IsHost = false;
    [SyncVar(hook = nameof(OnGameStateChanged))] public GameState GameState;
    [SyncVar(hook = nameof(OnPlayerStateChanged))] public PlayerState State = PlayerState.LIVE;

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
        GameController.Instance.ChangeState(state);
    }

    [ClientCallback]
    private void OnPlayerStateChanged(PlayerState _, PlayerState nextState)
    {
        if (nextState == PlayerState.DEAD)
        {
            gameObject.layer = _deadLayer;
            Dead();
        }
    }

    [ClientCallback]
    private void OnPlayerInfoChanged(PlayerInfo _, PlayerInfo playerInfo)
    {
        gameObject.name = playerInfo.Name;
    }

    [ClientRpc]
    public void StartGame()
    {
        _starGameEventSO.Raise();
        gameObject.layer = _normalLayer;
        if (isLocalPlayer)
        {
            _camera.UpdateViewLayer(_normaViewlLayer);
        }
    }

    [ClientRpc]
    public void SetRole(int roleId)
    {
        _role.UpdateRole(roleId);
    }

    [ClientRpc]
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
            GetComponent<SpriteRenderer>().color = Color.red;
            GameController.Instance.Dead();
        }
    }

    [ServerCallback]
    public void RaiseMetting(Player player = null)
    {
        Match.RaiseMetting(player);
    }

    [ClientRpc]
    public void Meeting(List<Player> players, Player player)
    {
        if (isLocalPlayer)
        {
            GameController.Instance.Meeting(players, player);
        }
    }

    public void Vote(string playerID)
    {
        CmdVote(playerID);
    }

    [Command]
    private void CmdVote(string playerID)
    {
        Match.Vote(playerID);
    }
}

public enum PlayerState
{
    NONE = 0,
    LIVE = 1,
    DEAD = 2,
}
