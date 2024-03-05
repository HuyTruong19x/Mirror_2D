using Mirror;

public struct GameMessage : NetworkMessage
{
    public int doing;
}

public enum ActionChannel : byte
{
    NONE = 0,
    ROOM = 1,
    GAME = 2,
}
