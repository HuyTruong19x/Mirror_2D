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
        GameNetworkManager.singleton.Client.RequestStartGame();
    }

    public void ExitGame()
    {
        GameNetworkManager.singleton.Client.RequestLeaveGame();
        SceneManager.LoadScene("Main");
    }
}
