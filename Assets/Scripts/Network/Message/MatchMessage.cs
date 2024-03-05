using Mirror;

public struct ServerMatchMessage : NetworkMessage
{
    public MatchOperation Operation;
    public string MatchID;
    public MatchInfo MatchInfo;
    public PlayerInfo PlayerInfo;
}

public enum MatchOperation
{
    NONE = 0,
    CREATE = 1,
    JOIN = 2,
    LOADED_GAME_SCENE = 3,
}
