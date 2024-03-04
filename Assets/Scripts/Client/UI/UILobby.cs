using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILobby : MonoBehaviour
{
    [SerializeField]
    private Transform _lobbyRoomParent;
    [SerializeField]
    private UILobbyRoom _lobbyRoom;

    private void OnEnable()
    {
        EventDispatcher.Subscribe(ActionChannel.ROOM, Handle);
    }

    private void OnDisable()
    {
        EventDispatcher.Subscribe(ActionChannel.ROOM, Handle);
    }

    private void Handle(NetworkMessage data)
    {
        var roomData = (ClientRoomMessage)data;
        Debug.Log($"Create room id {roomData.RoomID}");
        switch (roomData.Operation)
        {
            case ClientRoomOperation.CREATED: OnCreatedRoom(); break;
            case ClientRoomOperation.LIST: UpdateRoomList(roomData.Rooms); break;
        }
    }

    private void OnCreatedRoom()
    {
        this.gameObject.SetActive(false);
    }

    private void UpdateRoomList(List<LobbyRoomInfo> infos)
    {
        for (int i = 1; i < _lobbyRoomParent.childCount; i++)
        {
            Destroy(_lobbyRoomParent.GetChild(i).gameObject);
        }

        foreach (LobbyRoomInfo roomInfo in infos)
        {
            Instantiate(_lobbyRoom, _lobbyRoomParent).Setup(roomInfo).Show();
        }
    }

    #region UI_Action_Click

    public void CreateRoom()
    {
        GameNetworkManager.singleton.Client.RequestCreateRoom();
    }

    public void RefreshRoom()
    {
        GameNetworkManager.singleton.Client.RequestRoomList();
    }   
    
    #endregion
}
