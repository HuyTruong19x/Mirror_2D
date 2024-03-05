using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[MessageAttribute(ActionChannel.ROOM)]
public class ServerRoomHandler : MessageHandler<ServerRoomMessage>
{
    public override void Handle(NetworkConnectionToClient conn, ServerRoomMessage message)
    {
        switch (message.Operation)
        {
            case ServerRoomOperation.CREATE: CreateRoom(conn); break;
            case ServerRoomOperation.JOIN: JoinRoom(conn, message.RoomID); break;
            case ServerRoomOperation.LIST: GetRoomList(conn); break;
        }
    }

    private void CreateRoom(NetworkConnectionToClient conn)
    {
        Debug.Log("Request create room");
        var result = RoomManager.Instance.CreateRoom(conn, new Mirror.Examples.MultipleMatch.PlayerInfo());
        conn.Send(new ClientRoomMessage()
        {
            Operation = ClientRoomOperation.CREATED,
            RoomID = result.ID,
            RoomName = result.Title
        });
    }

    private void JoinRoom(NetworkConnectionToClient conn, string roomID)
    {
        var result = RoomManager.Instance.JoinRoom(roomID, conn);
        conn.Send(new ClientRoomMessage()
        {
            Operation = ClientRoomOperation.JOINED,
            RoomID = result.ID,
            RoomName = result.Title
        });
    }

    private void LeaveRoom(NetworkConnectionToClient conn, string roomID)
    {
        RoomManager.Instance.LeaveRoom(conn, roomID);
    }

    private void GetRoomList(NetworkConnectionToClient conn)
    {
        var roomList = new List<LobbyRoomInfo>();
        foreach (var room in RoomManager.Instance.Rooms.ToArray())
        {
            roomList.Add(new LobbyRoomInfo(room.Value.ID, room.Value.Title, "Random", "map_", "1/22"));
        }
        conn.Send(new ClientRoomMessage()
        {
            Operation = ClientRoomOperation.LIST,
            Rooms = roomList
        });
    }
}
