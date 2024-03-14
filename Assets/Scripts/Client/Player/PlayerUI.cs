using Mirror;
using System.Linq;
using UnityEngine;

public class PlayerUI : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnCanUseChanged))] private bool _canUse;
    [SyncVar(hook = nameof(OnCanReportChanged))] private bool _canReport;
    [SyncVar(hook = nameof(OnCanActionChanged))] private bool _canAction;
    [SyncVar] private bool _canSpecialAction;

    [SerializeField]
    private UIPlayer _ui;
    private UIPlayer _view;


    [SerializeField]
    private float _physicRadius = 3;
    [SerializeField]
    private LayerMask _playerLayer;

    private Player _player;

    private Player _nearestPlayer;
    private Player _deadPlayer;

    private Quest _quest;

    public override void OnStartLocalPlayer()
    {
        InitializeView();
    }

    private void InitializeView()
    {
        if (_view == null)
        {
            _view = Instantiate(_ui);
            _view.OnActionClick = Execute;
            _view.OnReportClick = Report;
            _view.OnUseClick = Use;
            _view.Hide();
        }
    }

    private void Awake()
    {
        _player = GetComponent<Player>();
    }

    [ServerCallback]
    private void Update()
    {
        if (_player.GameState != GameState.PLAYING)
        {
            return;
        }

        var colliders = Physics2D.OverlapCircleAll(transform.position, _physicRadius, _playerLayer)
            .Where(x => x.GetComponent<NetworkMatch>().matchId == _player.MatchID.ToGuid())
            .Where(y => y.GetComponent<Player>().State != PlayerState.DEAD).ToArray();
        if (colliders.Length > 0)
        {
            _canAction = false;
            _nearestPlayer = null;

            float min = 999999;
            for (int i = 0; i < colliders.Length; i++)
            {
                if (min > Vector2.Distance(transform.position, colliders[i].transform.position)
                    && colliders[i].gameObject != gameObject)
                {
                    min = Vector2.Distance(transform.position, colliders[i].transform.position);
                    _nearestPlayer = colliders[i].GetComponent<Player>();
                    _canAction = true;
                }
            }
        }
        else
        {
            _nearestPlayer = null;
            _canAction = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _physicRadius);
    }

    [ServerCallback]
    public void SetFoundDeadPlayer(Player deadPlayer)
    {
        _deadPlayer = deadPlayer;
        _canReport = deadPlayer != null;
    }

    public void Execute()
    {
        CmdExecute();
    }

    [Command(requiresAuthority = false)]
    private void CmdExecute()
    {
        _player.DoAction(_nearestPlayer);
    }

    private void Report()
    {
        CmdReport();
    }

    [Command(requiresAuthority = false)]
    private void CmdReport()
    {
        _player.RaiseMetting(_deadPlayer);
    }

    private void OnCanReportChanged(bool _, bool canReport)
    {
        if (isLocalPlayer)
        {
            _view.SetReportInteract(canReport);
        }
    }

    private void OnCanActionChanged(bool _, bool canAction)
    {
        if (isLocalPlayer)
        {
            _view.SetActionInteract(canAction);
        }
    }

    private void OnCanUseChanged(bool _, bool canUse)
    {
        if (isLocalPlayer)
        {
            _view.SetUseInteract(canUse);
        }
    }

    public void UpdateUIByRoleId(RoleDataSO roleData)
    {
        _view.UpdateUI(roleData);
    }

    public void ShowDeadUI()
    {
        _view.HideAction();
    }

    public void Show()
    {
        if (isLocalPlayer)
        {
            _view.Show();
        }
    }

    public void Hide()
    {
        if (isLocalPlayer)
        {
            _view.Hide();
        }
    }

    public void SetQuestAction(Quest quest)
    {
        if (isLocalPlayer)
        {
            _quest = quest;
            _canUse = quest != null;
            _view?.SetUseInteract(_canUse);
        }
    }    

    private void Use()
    {
        _quest?.Show();
        if(_quest != null)
        {
            _quest.OnFinish = () =>
            {
                _player.FinishQuest(_quest.ID);
            };
        }    
    }    
}
