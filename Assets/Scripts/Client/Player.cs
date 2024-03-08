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
    [SyncVar(hook = nameof(OnPlayerStateChanged))] public int State = 0;

    [SerializeField]
    private VoidChannelEventSO _starGameEventSO;

    private PlayerRole _role;
    [SerializeField]
    private LayerMask _ghostViewLayer;

    public override void OnStartLocalPlayer()
    {
        LocalPlayer = this;
        GameController.Instance.ChangeHost(IsHost);
    }

    private void Start()
    {
        _role = GetComponent<PlayerRole>();
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
    private void OnPlayerStateChanged(int _, int nextState)
    {
        if (nextState == 1)
        {
            Dead();
            gameObject.layer = LayerMask.NameToLayer("Ghost");
            if(isLocalPlayer)
            {
                GetComponent<PlayerCamera>().UpdateViewLayer(_ghostViewLayer);
                GetComponent<PlayerRole>().Dead();
            }
        }
    }
}
