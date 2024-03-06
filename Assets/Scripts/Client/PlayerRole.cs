using Mirror;
using UnityEngine;

public class PlayerRole : NetworkBehaviour
{
    [SerializeField]
    private UIPlayer _uiPlayer;
    private UIPlayer _currentView;
    public PlayerRole _target;

    public Role Role;

    public void Execute()
    {
        Role.Action(_target.Role);
    }

    public void UpdateRole(Role newRole)
    {
        if (isLocalPlayer)
        {
            _currentView ??= Instantiate(_uiPlayer);
            newRole.UpdateUI(_currentView);
        }
    }

    public void Show(PlayerRole role)
    {
        if(isLocalPlayer)
        {
            _currentView.SetInteract(true);
            _target = role;
        }
    }

    public void Hide()
    {
        if (isLocalPlayer)
        {
            _currentView?.SetInteract(false);
        }
    }
}
