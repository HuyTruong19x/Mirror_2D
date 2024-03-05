using Mirror;
using Mirror.Examples.MultipleMatch;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Room
{
    public LobbyRoomInfo Info;
    public Dictionary<NetworkConnectionToClient, PlayerInfo> Players = new();

    private NetworkConnectionToClient _hostPlayer;

    public bool IsFull => Players.Count == Info.MaxPlayer;

    public Room()
    {

    }

    public Room(string id, int maxPlayer, string title, NetworkConnectionToClient user, PlayerInfo info)
    {
        Info = new LobbyRoomInfo(id, info.Name, "Random", "Map_1", maxPlayer);
        Players.Add(user, info);
        _hostPlayer = user;
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

    public void RemovePlayer(NetworkIdentity user)
    {
        //for (int i = 0; i < Players.Count; i++)
        //{
        //    if (Players[i].netId == user.netId)
        //    {
        //        foreach (var playerInRoom in Players)
        //        {
        //            //playerInRoom.RpcPlayerLeaveRoom(PlayerInfos[i]);
        //        }

        //        Players.RemoveAt(i);
        //        PlayerInfos.RemoveAt(i);
        //        return;
        //    }
        //}
    }

    public bool IsHost(NetworkConnectionToClient player)
    {
        return _hostPlayer == player;
    }
}
