using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIGame : MonoBehaviour
{
    [SerializeField]
    private Button _startGameButton;
    [SerializeField]
    private InputField _matchId;

    public void ShowStartButton(bool isHost)
    {
        _startGameButton.gameObject.SetActive(isHost);
    }

    public void SetMatchID(string matchId)
    {
        _matchId.text = matchId;
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
