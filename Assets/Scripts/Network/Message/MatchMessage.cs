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

public struct ClientMatchInfoMessage : NetworkMessage
{
    public MatchInfo Info;
}

public enum MatchOperation
{
    NONE = 0,
    CREATE = 1,
    JOIN = 2,
    QUICK_JOIN = 3,
    LEAVE = 4,
    LIST = 5,
    LOADED_GAME_SCENE = 6,
    START_GAME = 7,
}

public enum Result
{
    NONE = 0,
    SUCCESS = 1,
    FAILED = 2,
}
