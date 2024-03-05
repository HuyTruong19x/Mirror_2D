using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayer : MonoBehaviour
{
    [SerializeField]
    private Button _actionButton;

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
}
