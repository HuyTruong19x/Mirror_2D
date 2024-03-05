using Mirror;

public class PlayerRole : NetworkBehaviour
{
    [SyncVar] public Role Role;

    public void Execute(PlayerRole role)
    {
        Role.Action();
    }
}
