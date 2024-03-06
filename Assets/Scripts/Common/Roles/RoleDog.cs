using UnityEngine;

public class RoleDog : Role
{
    public override void Action(Role role)
    {
        Debug.Log("Fuck dog");
    }
    public override void UpdateUI(UIPlayer ui)
    {
        ui.HideAction();
    }
}
