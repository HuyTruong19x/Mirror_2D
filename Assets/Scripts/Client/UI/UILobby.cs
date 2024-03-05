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
    private string _selectRoomId = string.Empty;

    private void OnEnable()
    {
        EventDispatcher.Subscribe(ActionChannel.ROOM, Handle);
    }

    private void OnDisable()
    {
        EventDispatcher.Subscribe(ActionChannel.ROOM, Handle);
    }

    private void Start()
    {
        _networkManager = GameNetworkManager.singleton.Client;
    }

    private void Handle(NetworkMessage data)
    {
        var roomData = (ClientRoomMessage)data;
        Debug.Log($"Create room id {roomData.RoomID}");
        switch (roomData.Operation)
        {
            case ClientRoomOperation.CREATED:
            case ClientRoomOperation.JOINED: LoadGameScene(roomData.RoomID); break;
            case ClientRoomOperation.LIST: UpdateRoomList(roomData.Rooms); break;
        }
    }

    private void LoadGameScene(string roomId)
    {
        GameNetworkManager.singleton.Client.RoomID = roomId;
        this.gameObject.SetActive(false);
        SceneManager.LoadScene(2);
    }

    private void UpdateRoomList(List<LobbyRoomInfo> infos)
    {
        for (int i = 1; i < _lobbyRoomParent.childCount; i++)
        {
            Destroy(_lobbyRoomParent.GetChild(i).gameObject);
        }

        foreach (LobbyRoomInfo roomInfo in infos)
        {
            Instantiate(_lobbyRoom, _lobbyRoomParent).Setup(roomInfo).OnButtonClick((id) => _selectRoomId = id).Show();
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
