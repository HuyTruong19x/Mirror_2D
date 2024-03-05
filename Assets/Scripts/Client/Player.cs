using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : NetworkBehaviour
{
    public static Player LocalPlayer;
    [SyncVar] public string MatchID = string.Empty;
    public PlayerInfo PlayerInfo;

    public override void OnStartLocalPlayer()
    {
        LocalPlayer = this;
    }
}
