using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    public void ExitGame()
    {
        NetworkClient.Send(new GameMessage()
        {
            Operation = 3,
            RoomID = GameNetworkManager.singleton.Client.RoomID
        });
        SceneManager.LoadScene(1);
    }
}
