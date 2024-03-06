using Mirror;
using UnityEngine;

public class PlayerRole : NetworkBehaviour
{
    [SerializeField]
    private UIPlayer _uiPlayer;
    private UIPlayer _currentView;
    public PlayerRole _target;

    public Role Role;

    private void Awake()
    {
    }

    public void Execute()
    {
        Role.Action(_target.Role);
    }

    public void UpdateRole(Role newRole)
    {
        if (isLocalPlayer)
        {
            Role = newRole;
            _currentView ??= Instantiate(_uiPlayer);
            _currentView.OnClick = Execute;
            newRole.UpdateUI(_currentView);
        }
    }

    public void Show(PlayerRole role)
    {
        if(isLocalPlayer)
        {
            _currentView ??= Instantiate(_uiPlayer);
            _currentView.SetActionInteract(true);
            _target = role;
        }
    }

    public void Hide()
    {
        if (isLocalPlayer)
        {
            _currentView?.SetActionInteract(false);
        }
    }

    public void SetQuest()
    {
        if (isLocalPlayer)
        {
            _currentView?.SetUseInteract(true);
        }
    }
}
