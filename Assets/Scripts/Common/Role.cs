using System;

[Serializable]
public class Role
{
    public int RoleID;

    public virtual void Action(Role role)
    {

    }

    public virtual void UpdateUI(UIPlayer ui)
    {

    }
}
