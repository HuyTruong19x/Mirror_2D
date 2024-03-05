using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGame : MonoBehaviour
{
    [SerializeField]
    private Button _startGameButton;

    public void SetHost(bool isHost)
    {
        _startGameButton.gameObject.SetActive(isHost);
    }

    public void StartGame()
    {
        NetworkClient.Send(new GameMessage()
        {
            Operation = 2,
            RoomID = GameNetworkManager.singleton.Client.RoomID
        });
    }
}
