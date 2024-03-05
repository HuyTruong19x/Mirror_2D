using System;

[Serializable]
public class MatchInfo
{
    public string ID;
    public string HostName;
    public string Mode;
    public string Map;
    public string Status;
    public int MaxPlayer;

    public MatchInfo(string id, string hostName, string mode, string map, int maxPlayer)
    {
        ID = id;
        HostName = hostName;
        Mode = mode;
        Map = map;
        MaxPlayer = maxPlayer;
    }

    public void UpdateStatus(string status)
    {
        Status = status;
    }

    public MatchInfo()
    { }
}
