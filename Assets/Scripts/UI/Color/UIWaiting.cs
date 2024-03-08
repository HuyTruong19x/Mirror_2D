using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIWaiting : MonoBehaviour
{
    [SerializeField]
    private Button _startGameButton;
    [SerializeField]
    private InputField _matchId;
    private bool _showId = false;
    [SerializeField]
    private Text _matchName;
    [SerializeField]
    private Text _playerStatus;
    [SerializeField]
    private Text _gameMode;
    [SerializeField]
    private MatchInfoSO _matchInfo;
    [SerializeField]
    private StringChannelEventSO _onStatusChanged;

    private void OnEnable()
    {
        SetMatchID(_matchInfo.Info.ID);
        SetMatchName(_matchInfo.Info.HostName);
        SetGameMode(_matchInfo.Info.Mode);

        _onStatusChanged.AddListener(ChangeStatus);
    }

    private void OnDisable()
    {
        _onStatusChanged.RemoveListener(ChangeStatus);
    }

    public void ShowStartButton(bool isHost)
    {
        _startGameButton.gameObject.SetActive(isHost);
    }

    public void SetMatchID(string matchId)
    {
        _matchId.text = matchId;
    }

    public void SetMatchName(string matchName)
    {
        _matchName.text = matchName;
    }    

    public void SetGameMode(string gameMode)
    {
        _gameMode.text = gameMode;
    }    

    public void ShowHideMathcID()
    {
        _showId = !_showId;
        _matchId.contentType = _showId ? InputField.ContentType.Standard : InputField.ContentType.Password;
        _matchId.ForceLabelUpdate();
    }

    public void ChangeStatus(string status)
    {
        _playerStatus.text = status;
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
