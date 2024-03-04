using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ClientRoomMessage : NetworkMessage
{
    public ClientRoomOperation Operation;
    public string RoomID;
    public string RoomName;
    public List<LobbyRoomInfo> Rooms;
}

public struct ServerRoomMessage : NetworkMessage
{
    public ServerRoomOperation Operation;
    public string RoomID;
}

public enum ClientRoomOperation : byte
{
    NONE = 0,
    LIST = 1,
    CREATED = 2,
    JOINED = 3,
    UPDATED = 4,
    CANCELLED = 5,
    LEAVED = 6,
}

public enum ServerRoomOperation : byte
{
    NONE = 0,
    CREATE = 1,
    JOIN = 2,
    REMOVE = 3,
    LIST
}
