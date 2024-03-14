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
        var role = collision.GetComponent<PlayerUI>();
        if (role != null)
        {
            if(role.gameObject.GetComponent<NetworkMatch>().matchId == _deadPlayer.MatchID.ToGuid())
            {
                role.SetFoundDeadPlayer(_deadPlayer);
            }    
        }
    }

    [ServerCallback]
    private void OnTriggerExit2D(Collider2D collision)
    {
        var role = collision.GetComponent<PlayerUI>();
        if (role != null)
        {
            if (role.gameObject.GetComponent<NetworkMatch>().matchId == _deadPlayer.MatchID.ToGuid())
            {
                role.SetFoundDeadPlayer(null);
            }
        }
    }

    [ServerCallback]
    public void SetPlayer(Player deadPlayer)
    {
        _deadPlayer = deadPlayer;
    }
}
