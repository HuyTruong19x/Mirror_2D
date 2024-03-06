using Mirror;
using System.Collections.Generic;

public struct ServerMatchMessage : NetworkMessage
{
    public MatchOperation Operation;
    public string MatchID;
    public MatchInfo MatchInfo;
    public PlayerInfo PlayerInfo;
}

public struct ClientMatchMessage : NetworkMessage
{
    public MatchOperation Operation;
    public Result Result;
    public string MatchID;
    public List<MatchInfo> MatchInfos;
}

public enum MatchOperation
{
    NONE = 0,
    CREATE = 1,
    JOIN = 2,
    LEAVE = 3,
    LIST = 4,
    LOADED_GAME_SCENE = 5,
    START_GAME = 6,
}

public enum Result
{
    NONE = 0,
    SUCCESS = 1,
    FAILED = 2,
}
