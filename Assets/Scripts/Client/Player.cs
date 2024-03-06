using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : NetworkBehaviour
{
    public static Player LocalPlayer;

    public PlayerInfo PlayerInfo;

    [SyncVar] public string MatchID = string.Empty;
    [SyncVar(hook = nameof(OnHostChanged))] public bool IsHost = false;
    [SyncVar(hook = nameof(OnGameStateChanged))] public GameState GameState;

    private PlayerRole _role;

    public override void OnStartLocalPlayer()
    {
        LocalPlayer = this;

    }

    [ServerCallback]
    private void OnDestroy()
    {
        Disconnected(connectionToClient);
    }

    [ServerCallback]
    public void Disconnected(NetworkConnectionToClient conn)
    {
        MatchManager.Instance.LeaveMatch(conn, MatchID);
    }

    private void OnHostChanged(bool _, bool isHost)
    {

    }

    private void OnGameStateChanged(GameState _, GameState state)
    {
        GameController.Instance.ChangeState(state);
    }

    [ClientRpc]
    public void StartGame()
    {

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
