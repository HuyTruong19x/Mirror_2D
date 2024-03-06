using Mirror;
using UnityEngine;

public class PlayerCollider : NetworkBehaviour
{
    [SerializeField]
    private float _physicRadius = 3;
    [SerializeField]
    private LayerMask _playerLayer;

    private PlayerRole _playerRole;

    [SyncVar(hook = nameof(OnNearestPlayerChanged))]
    private PlayerRole _nearestPlayer;


    private void Awake()
    {
        _playerRole = GetComponent<PlayerRole>();
    }

    [ServerCallback]
    private void Update()
    {
        var colliders = Physics2D.OverlapCircleAll(transform.position, _physicRadius, _playerLayer);
        if (colliders.Length > 0)
        {
            _nearestPlayer = null;

            float min = 999999;
            for (int i = 0; i < colliders.Length; i++)
            {
                if (min > Vector2.Distance(transform.position, colliders[i].transform.position)
                    && colliders[i].gameObject != gameObject)
                {
                    _nearestPlayer = colliders[i].GetComponent<PlayerRole>();
                }
            }
        }
        else
        {
            _nearestPlayer = null;
        }
    }

    [ServerCallback]
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Has collider action");
        }
    }

    [ServerCallback]
    private void OnTriggerExit2D(Collider2D collision)
    {
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _physicRadius);
    }

    [ClientRpc]
    private void ShowUI(PlayerRole playerRole)
    {
        if (isLocalPlayer)
        {
            _playerRole.Show(playerRole);
        }
    }

    [ClientRpc]
    private void HideUI()
    {
        if (isLocalPlayer)
        {
            Debug.Log("hide ui");
            _playerRole.Hide();
        }
    }

    [ClientCallback]
    private void OnNearestPlayerChanged(PlayerRole old, PlayerRole newPlayer)
    {
        if(newPlayer != null)
        {
            _playerRole.Show(newPlayer);
        }
        else
        {
            _playerRole.Hide();
        }
    }
}
