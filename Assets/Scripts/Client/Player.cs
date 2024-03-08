using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Player : NetworkBehaviour
{
    public static Player LocalPlayer;

    public PlayerInfo PlayerInfo;

    [SyncVar] public string MatchID = string.Empty;
    [SyncVar(hook = nameof(OnHostChanged))] public bool IsHost = false;
    [SyncVar(hook = nameof(OnGameStateChanged))] public GameState GameState;

    [SerializeField]
    private VoidChannelEventSO _starGameEventSO;

    private PlayerRole _role;

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
    public void SetRole(Role role)
    {
        _role.UpdateRole(role);
    }

    [ClientRpc]
    public void MoveToPosition(Vector3 pos)
    {
        transform.position = pos;
    }
}
