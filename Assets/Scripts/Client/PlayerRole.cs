using Mirror;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerRole : NetworkBehaviour
{
    [SerializeField]
    private UIPlayer _uiPlayer;
    private UIPlayer _currentView;
    public Player _target;

    [SerializeField]
    private RoleDataSO _myRole;
    [SerializeField]
    private List<RoleDataSO> _roles;
    private Dictionary<int, RoleDataSO> _roleData = new();

    private void Awake()
    {
        _roleData = _roles.ToDictionary((role) => role.ID);
    }

    public void Execute()
    {
        _myRole.Action?.DoAction(GetComponent<Player>(), _target);
    }

    public void UpdateRole(int roleID)
    {
        if (isLocalPlayer && _roleData.TryGetValue(roleID, out var role))
        {
            _myRole = role;
            _currentView ??= Instantiate(_uiPlayer);
            _currentView.OnClick = Execute;
            _currentView.UpdateUI(_myRole);
        }
    }

    public void Show(Player role)
    {
        if (isLocalPlayer)
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

    public void Dead()
    {
        _currentView.HideAction();
    }
}
