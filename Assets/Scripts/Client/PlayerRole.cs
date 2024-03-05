using Mirror;
using UnityEngine;

public class PlayerRole : NetworkBehaviour
{
    [SerializeField]
    private UIPlayer _uiPlayer;
    private UIPlayer _currentView;
    public PlayerRole _target;

    [SyncVar(hook = nameof(UpdateGameUI))] public Role Role;

    public void Execute()
    {
        Role.Action(_target.Role);
    }

    public void UpdateGameUI(Role oldRole, Role newRole)
    {
        _currentView ??= Instantiate(_uiPlayer);
        newRole.UpdateUI(_currentView);
    }

    public UIPlayer GetUI()
    {
        _currentView ??= Instantiate(_uiPlayer);
        return _currentView;
    }

    public void Show(PlayerRole role)
    {
        _currentView.SetInteract(true);
        _target = role;
    }

    public void Hide()
    {
        _currentView.SetInteract(false);
    }
}
