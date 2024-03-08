using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GamePlayer/Role/new Action")]
public class RoleActionKill : RoleAction
{
    public override void DoAction(Player killer, Player player)
    {
        Debug.Log($"{killer.name} killed player " + player.name);
        killer.KillPlayer(player);
    }
}
