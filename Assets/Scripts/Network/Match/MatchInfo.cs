using System;

[Serializable]
public struct MatchInfo
{
    public string ID;
    public string HostName;
    public string Mode;
    public string Map;
    public string Status;
    public int MaxPlayer;
    public int VoteTime;
    public int DiscussTime;
    public int RaiseTime;
}
