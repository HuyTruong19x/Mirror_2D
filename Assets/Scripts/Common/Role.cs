using System;
using UnityEngine;

[Serializable]
public class Role
{
    public int RoleID;

    public virtual void Action(Role role)
    {
        Debug.Log("my Role is");
    }

    public virtual void UpdateUI(UIPlayer ui)
    {

    }
}
