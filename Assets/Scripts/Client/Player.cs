using Mirror;
using UnityEngine;


public class Player : NetworkBehaviour
{
    public static Player LocalPlayer;

    public Match Match;
    public PlayerInfo PlayerInfo;

    [SyncVar] public string MatchID = string.Empty;
    [SyncVar(hook = nameof(OnHostChanged))] public bool IsHost = false;
    [SyncVar(hook = nameof(OnGameStateChanged))] public GameState GameState;
    [SyncVar(hook = nameof(OnPlayerStateChanged))] public PlayerState State = PlayerState.LIVE;

    [SerializeField]
    private VoidChannelEventSO _starGameEventSO;

    private PlayerRole _role;

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
        LocalPlayer = this;
        GameController.Instance.ChangeHost(IsHost);
    }

    private void Start()
    {
        _role = GetComponent<PlayerRole>();
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

    private void OnHostChanged(bool _, bool isHost)
    {
        GameController.Instance.ChangeHost(isHost);
    }

    private void OnGameStateChanged(GameState _, GameState state)
    {
        GameController.Instance.ChangeState(state);
    }

    [ClientRpc]
    public void StartGame()
    {
        _starGameEventSO.Raise();
        gameObject.layer = _normalLayer;
        if (isLocalPlayer)
        {
            GetComponent<PlayerCamera>().UpdateViewLayer(_normaViewlLayer);
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
        Debug.Log("I'm Dead");
    }

    [ClientCallback]
    public void Dead()
    {
        GetComponent<SpriteRenderer>().color = Color.red;
    }

    [ClientCallback]
    private void OnPlayerStateChanged(PlayerState _, PlayerState nextState)
    {
        if (nextState == PlayerState.DEAD)
        {
            Dead();
            gameObject.layer = _deadLayer;
            if(isLocalPlayer)
            {
                GetComponent<PlayerCamera>().UpdateViewLayer(_ghostViewLayer);
                GetComponent<PlayerRole>().Dead();
            }
        }
    }
}

public enum PlayerState
{
    NONE = 0,
    LIVE = 1,
    DEAD = 2,
}
