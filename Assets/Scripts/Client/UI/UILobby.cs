using Mirror;
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
    private string _selectRoomId = string.Empty;

    private void OnEnable()
    {
        EventDispatcher.Subscribe(MessageCode.MATCH, Handle);
    }

    private void OnDisable()
    {
        EventDispatcher.Unsubscribe(MessageCode.MATCH, Handle);
    }

    private void Start()
    {
        _networkManager = GameNetworkManager.singleton.Client;
    }

    private void Handle(NetworkMessage data)
    {
        var roomData = (ClientMatchMessage)data;
        Debug.Log($"Create room id {roomData.MatchID}");
        switch (roomData.Operation)
        {
            case MatchOperation.CREATE:
            case MatchOperation.JOIN: LoadGameScene(roomData.MatchID, roomData.Result); break;
            case MatchOperation.LIST: UpdateRoomList(roomData.MatchInfos); break;
        }
    }

    private void LoadGameScene(string roomId, Result result)
    {
        if(result == Result.SUCCESS)
        {
            GameNetworkManager.singleton.Client.MatchID = roomId;
            this.gameObject.SetActive(false);
            SceneManager.LoadScene(2);
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
                .Setup(new LobbyRoomInfo(match.ID, match.HostName, match.Mode, match.Map, match.MaxPlayer))
                .OnButtonClick((id) => _selectRoomId = id)
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
        _networkManager.RequestJoinRoom(_selectRoomId);
    }

    #endregion
}
