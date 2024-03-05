using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayer : MonoBehaviour
{
    [SerializeField]
    private Button _actionButton;
    [SerializeField]
    private PlayerRole _role;

    private PlayerRole _targetPlayerRole;

    private void OnEnable()
    {
        EventDispatcher.Subscribe(ActionChannel.GAME, OnGameMessage);
    }

    private void OnGameMessage(NetworkMessage message)
    {
        var gameMessage = (GameMessage)message;
        if(gameMessage.Role < 5)
        {
            _actionButton.gameObject.SetActive(false);
        }    
    }

    public void SetInteract(bool canInteract)
    {
        _actionButton.interactable = canInteract;
    }    

    public void SetTarget(PlayerRole playerRole)
    {
        _targetPlayerRole = playerRole;
    }

    public void Action()
    {
        _role.Execute(_targetPlayerRole);
    }
}
