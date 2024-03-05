using UnityEngine;

public class RoleWolf : Role
{
    public RoleWolf()
    {
        RoleID = 1;
    }

    public override void Action(Role role)
    {
        if (role.RoleID == RoleID)
        {
            return;
        }

        Debug.Log("Kill another player");
    }

    public override void UpdateUI(UIPlayer ui)
    {
        ui.ShowAction();
    }
}
