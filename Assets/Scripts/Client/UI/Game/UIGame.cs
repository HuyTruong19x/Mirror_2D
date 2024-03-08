using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIGame : MonoBehaviour
{
    [SerializeField]
    private UIWaiting _uiWaiting;

    public void ShowStartButton(bool isHost)
    {
        _uiWaiting.ShowStartButton(isHost);
    }
}
