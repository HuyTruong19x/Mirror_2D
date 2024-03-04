using Mirror;
using Mirror.Examples.MultipleMatch;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    public string ID;
    public string Title;
    public int MaxPlayer;
    public bool IsInGame;
    public Dictionary<NetworkConnectionToClient, PlayerInfo> Players = new();

    public bool IsFull => Players.Count == MaxPlayer;

    public Room()
    {

    }

    public Room(string id, int maxPlayer, string title, NetworkConnectionToClient user, PlayerInfo info)
    {
        ID = id;
        Title = title;
        MaxPlayer = maxPlayer;
        Players.Add(user, info);
    }

    public void Add(NetworkConnectionToClient user, PlayerInfo info)
    {
        foreach (var playerInRoom in Players)
        {
            //playerInRoom.RpcPlayerJoinRoom(user.Info);
        }

        Players.Add(user, info);
    }

    public void Remove(NetworkIdentity user)
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

    public void StartGame()
    {
        for (int i = 0; i < Players.Count; i++)
        {
            //Players[i].RpcStartGame();
        }
    }
}
