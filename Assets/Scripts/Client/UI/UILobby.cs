using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UILobby : MonoBehaviour
{
    [SerializeField]
    private Transform _lobbyRoomParent;
    [SerializeField]
    private UILobbyRoom _lobbyRoom;

    private ClientNetworkManager _networkManager;
    private MatchInfo _selectedMatch;

    private void OnEnable()
    {
        EventDispatcher.Subscribe(MessageCode.MATCH, Handle);
    }

    private void OnDisable()
    {
        EventDispatcher.Unsubscribe(MessageCode.MATCH, Handle);
    }

    private IEnumerator Start()
    {
        while(!NetworkClient.isConnected)
        {
            yield return null;
        }    
        _networkManager = GameNetworkManager.singleton.Client;
        RefreshRoom();
    }

    private void Handle(NetworkMessage data)
    {
        var roomData = (ClientMatchMessage)data;
        Debug.Log($"Room id {roomData.MatchID}");
        switch (roomData.Operation)
        {
            case MatchOperation.CREATE:
            case MatchOperation.JOIN: LoadGameScene( roomData); break;
            case MatchOperation.LIST: UpdateRoomList(roomData.MatchInfos); break;
        }
    }

    private void LoadGameScene(ClientMatchMessage result)
    {
        if(result.Result == Result.SUCCESS)
        {
            _networkManager.SetMatchID(result.MatchID);
            this.gameObject.SetActive(false);
            SceneManager.LoadScene("Game");
        }
    }

    private void UpdateRoomList(List<MatchInfo> infos)
    {
        for (int i = 1; i < _lobbyRoomParent.childCount; i++)
        {
            Destroy(_lobbyRoomParent.GetChild(i).gameObject);
        }

        foreach (var match in infos)
        {
            Instantiate(_lobbyRoom, _lobbyRoomParent)
                .Setup(match)
                .OnButtonClick((matchInfo) => _selectedMatch = matchInfo)
                .Show();
        }
    }   

    #region UI_Action_Click

    public void CreateRoom()
    {
        _networkManager.RequestCreateRoom();
    }

    public void RefreshRoom()
    {
        _networkManager.RequestRoomList();
    }

    public void JoinRoom()
    {
        _networkManager.RequestJoinRoom(_selectedMatch);
    }

    public void QuickJoin()
    {
        _networkManager.RequestQuickJoin();
    }

    #endregion
}
