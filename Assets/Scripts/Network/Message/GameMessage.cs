using Mirror;
using UnityEngine;

public struct GameMessage : NetworkMessage
{
    public int Operation;
    public string RoomID;
}

public struct WaitingRoomMessage : NetworkMessage
{
    public bool IsHost;
    public GameState State;
}

public struct ChangeGameSettingMessage : NetworkMessage
{
    public int DiscussTime;
    public int VoteTime;
}

public enum ActionChannel : byte
{
    NONE = 0,
    ROOM = 1,
    GAME = 2,
    WAITING_ROOM = 3,
    MATCH = 4,
}
