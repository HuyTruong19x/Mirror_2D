using Mirror;
using System.Collections.Generic;

public class Room
{
    public LobbyRoomInfo Info;
    public Dictionary<NetworkConnectionToClient, PlayerInfo> Players = new();

    private NetworkConnectionToClient _hostPlayer;

    public bool IsFull => Players.Count == Info.MaxPlayer;

    public Room()
    {

    }

    public Room(NetworkConnectionToClient user, PlayerInfo info, LobbyRoomInfo roomInfo)
    {
        Info = roomInfo;
        _hostPlayer = user;
        Players.Add(user, info);
        UpdateRoomStatus();
    }

    private void UpdateRoomStatus()
    {
        Info.UpdateStatus($"{Players.Count}/{Info.MaxPlayer}");
    }

    public void AddPlayer(NetworkConnectionToClient user, PlayerInfo info)
    {
        Players.Add(user, info);
        UpdateRoomStatus();
    }

    public void RemovePlayer(NetworkConnectionToClient user)
    {
        Players.Remove(user);
        UpdateRoomStatus();
    }

    public bool IsHost(NetworkConnectionToClient player)
    {
        return _hostPlayer == player;
    }
}