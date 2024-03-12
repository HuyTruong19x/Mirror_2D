using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadObject : NetworkBehaviour
{
    private Player _deadPlayer;

    [ServerCallback]
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var role = collision.GetComponent<PlayerRole>();
        if (role != null)
        {
            role.CanReport(_deadPlayer);
        }
    }

    [ServerCallback]
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
