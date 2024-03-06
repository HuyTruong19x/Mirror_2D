using Mirror;
using UnityEngine;

public class PlayerCollider : NetworkBehaviour
{
    private PlayerRole _playerRole;
    public override void OnStartLocalPlayer()
    {
        _playerRole = GetComponent<PlayerRole>();
    }

    [ServerCallback]
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Debug.Log("Has collider action");
        }    
    }

    [ServerCallback]
    private void OnTriggerExit2D(Collider2D collision)
    {
    }

    [ClientRpc]
    private void ShowUI(PlayerRole playerRole)
    {
        if(isLocalPlayer)
        {
            _playerRole.Show(playerRole);
        }
    }

    [ClientRpc]
    private void HideUI()
    {
        if(isLocalPlayer)
        {
            Debug.Log("hide ui");
            _playerRole.Hide();
        }
    }
}
