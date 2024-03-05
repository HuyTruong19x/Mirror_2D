public class RoleFox : Role
{
    public RoleFox()
    {
        RoleID = 2;
    }

    public override void Action(Role role)
    {
        throw new System.NotImplementedException();
    }
    public override void UpdateUI(UIPlayer ui)
    {
        ui.ShowAction();
    }
}
