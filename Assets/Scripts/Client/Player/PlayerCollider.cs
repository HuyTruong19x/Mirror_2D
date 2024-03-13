using Mirror;
using UnityEngine;

public class PlayerCollider : NetworkBehaviour
{
    [SerializeField]
    private float _physicRadius = 3;
    [SerializeField]
    private LayerMask _playerLayer;

    private PlayerRole _playerRole;
    private Player _player;

    [SyncVar(hook = nameof(OnNearestPlayerChanged))]
    private Player _nearestPlayer;


    private void Awake()
    {
        _playerRole = GetComponent<PlayerRole>();
        _player = GetComponent<Player>();
    }

    [ServerCallback]
    private void Update()
    {
        if (_player.GameState != GameState.PLAYING)
        {
            return;
        }

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
                    min = Vector2.Distance(transform.position, colliders[i].transform.position);
                    _nearestPlayer = colliders[i].GetComponent<Player>();
                }
            }
        }
        else
        {
            _nearestPlayer = null;
        }
    }

    [ServerCallback]
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Quest"))
        {
            
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _physicRadius);
    }

    [ClientCallback]
    private void OnNearestPlayerChanged(Player _, Player newPlayer)
    {
        if(newPlayer != null)
        {
            _playerRole.SetTargetPlayer(newPlayer);
        }
        else
        {
            _playerRole.Hide();
        }
    }
}
