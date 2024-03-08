using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RoleAction : ScriptableObject
{
    public abstract void DoAction(Player killer, Player player);
}
