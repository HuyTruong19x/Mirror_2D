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
        NetworkClient.Send(new ServerMatchMessage()
        {
            Operation = MatchOperation.START_GAME,
            MatchID = GameNetworkManager.singleton.Client.MatchID
        });
    }

    public void ExitGame()
    {
        NetworkClient.Send(new ServerMatchMessage()
        {
            Operation = MatchOperation.LEAVE,
            MatchID = GameNetworkManager.singleton.Client.MatchID
        });
        SceneManager.LoadScene(1);
    }
}
