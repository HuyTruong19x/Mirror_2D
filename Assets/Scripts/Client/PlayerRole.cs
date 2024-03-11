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
    private Player _deadPlayer;

    [SerializeField]
    private RoleDataSO _myRole;
    [SerializeField]
    private List<RoleDataSO> _roles;
    private Dictionary<int, RoleDataSO> _roleData = new();

    private void Awake()
    {
        _roleData = _roles.ToDictionary((role) => role.ID);
    }

    public override void OnStartLocalPlayer()
    {
        InitializeView();
    }

    private void InitializeView()
    {
        if(_currentView == null)
        {
            _currentView ??= Instantiate(_uiPlayer);
            _currentView.OnActionClick = Execute;
            _currentView.Hide();
        }
    }

    public void Execute()
    {
        _myRole.Action?.DoAction(GetComponent<Player>(), _target);
    }

    private void Report()
    {
        //TODO report
    }

    public void UpdateRole(int roleID)
    {
        if (isLocalPlayer && _roleData.TryGetValue(roleID, out var role))
        {
            _myRole = role;
            _currentView.Show();
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

    public void CanReport(Player player)
    {
        _deadPlayer = player;
        _currentView.SetReportInteract(player != null);
    }
}
