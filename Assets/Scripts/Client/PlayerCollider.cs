using Mirror;
using UnityEngine;

public class PlayerCollider : NetworkBehaviour
{
    private UIPlayer _playerUI;
    private GameController _controller;
    public override void OnStartLocalPlayer()
    {
        _playerUI = GameObject.FindObjectOfType<UIPlayer>();
        _controller = GameObject.FindObjectOfType<GameController>();
    }

    [ServerCallback]
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Debug.Log("Has collider action");
            ShowUI();
        }    
    }

    [ServerCallback]
    private void OnTriggerExit2D(Collider2D collision)
    {
        HideUI();
    }

    [ClientRpc]
    private void ShowUI()
    {
        if(!isLocalPlayer || _controller.State != GameState.PLAYING)
        {
            return;
        }    

        Debug.Log("Show ui");
        _playerUI.SetInteract(true);
    }

    [ClientRpc]
    private void HideUI()
    {
        if (!isLocalPlayer || _controller.State != GameState.PLAYING)
        {
            return;
        }

        Debug.Log("hide ui");
        _playerUI.SetInteract(false);
    }
}
