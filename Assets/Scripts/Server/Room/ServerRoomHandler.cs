using Mirror;
using Mono.CecilX.Cil;
using System.Collections.Generic;
using System.Linq;

[MessageAttribute(ActionChannel.ROOM)]
public class ServerRoomHandler : MessageHandler<ServerRoomMessage>
{
    public override void Handle(NetworkConnectionToClient conn, ServerRoomMessage message)
    {
        switch (message.Operation)
        {
            case ServerRoomOperation.CREATE: CreateRoom(conn, message); break;
            case ServerRoomOperation.JOIN: JoinRoom(conn, message); break;
            case ServerRoomOperation.LIST: GetRoomList(conn); break;
        }
    }

    private void CreateRoom(NetworkConnectionToClient conn, ServerRoomMessage message)
    {
        var result = RoomManager.Instance.CreateRoom(conn, message.PlayerInfo);
        
        if (result != null)
        {
            conn.Send(new ClientRoomMessage()
            {
                Operation = ClientRoomOperation.CREATED,
                RoomID = result.Info.ID,
                RoomName = result.Info.HostName
            });
        }
        else
        {
            conn.Send(new ClientRoomMessage()
            {
                Operation = ClientRoomOperation.JOIN_FAIL,
            });
        }
    }

    private void JoinRoom(NetworkConnectionToClient conn, ServerRoomMessage message)
    {
        var result = RoomManager.Instance.JoinRoom(message.RoomID, conn, message.PlayerInfo);
        if (result != null)
        {
            conn.Send(new ClientRoomMessage()
            {
                Operation = ClientRoomOperation.JOINED,
                RoomID = result.Info.ID,
                RoomName = result.Info.HostName
            });
        }
        else
        {
            conn.Send(new ClientRoomMessage()
            {
                Operation = ClientRoomOperation.JOIN_FAIL,
            });
        }
    }

    private void LeaveRoom(NetworkConnectionToClient conn, string roomID)
    {
        RoomManager.Instance.LeaveRoom(conn, roomID);
    }

    private void GetRoomList(NetworkConnectionToClient conn)
    {
        conn.Send(new ClientRoomMessage()
        {
            Operation = ClientRoomOperation.LIST,
            Rooms = RoomManager.Instance.GetRoomListInfo()
        });
    }
}
