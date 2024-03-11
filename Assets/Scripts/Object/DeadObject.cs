using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadObject : NetworkBehaviour
{
    [SyncVar]
    private Player _deadPlayer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var role = collision.GetComponent<PlayerRole>();
        if (role != null)
        {
            role.CanReport(_deadPlayer);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var role = collision.GetComponent<PlayerRole>();
        if (role != null)
        {
            role.CanReport(null);
        }
    }

    public void SetPlayer(Player deadPlayer)
    {
        _deadPlayer = deadPlayer;
    }
}
