using Mirror;
using UnityEngine;

public struct GameMessage : NetworkMessage
{
    public int Operation;
    public string RoomID;
    public int Role;
    public GameState State;
    public bool IsHost;
    public Vector3 Position;
}

public enum ActionChannel : byte
{
    NONE = 0,
    ROOM = 1,
    GAME = 2,
}
