using Mirror;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.Security.Cryptography;
using Mirror.Examples.MultipleMatch;

public class RoomManager : Singleton<RoomManager>
{
    public Dictionary<string, Room> Rooms = new();

    public Room CreateRoom(NetworkConnectionToClient conn, PlayerInfo info)
    {
        if(Rooms.ContainsKey("xxxx"))
        {
            JoinRoom("xxxx", conn);
            return Rooms["xxxx"];
        }    
        var id = GetRandomID();
        var room = new Room(id, 4, "Tesst_" + UnityEngine.Random.Range(0, 9999), conn, info);
        Rooms.Add("xxxx", room);
        return room;
    }

    public Room JoinRoom(string id, NetworkConnectionToClient conn)
    {
        Rooms[id].Players.Add(conn, new PlayerInfo());
        return null;
    }

    public void LeaveRoom(NetworkConnectionToClient conn, string id)
    {
    }

    public void StartGame(string id)
    {
        for (int i = 0; i < Rooms.Count; i++)
        {
            //if (Rooms[i].ID == id)
            //{
            //    Rooms[i].StartGame();
            //}
        }
    }

    public static string GetRandomID()
    {
        string _id = string.Empty;
        for (int i = 0; i < 5; i++)
        {
            int random = UnityEngine.Random.Range(0, 36);
            if (random < 26)
            {
                _id += (char)(random + 65);
            }
            else
            {
                _id += (random - 26).ToString();
            }
        }
        Debug.Log($"Random ID: {_id}");
        return _id;
    }
}

public static class RoomExtensions
{
    public static Guid ToGuid(this string id)
    {
        var provider = new MD5CryptoServiceProvider();
        byte[] inputBytes = Encoding.Default.GetBytes(id);
        byte[] hashBytes = provider.ComputeHash(inputBytes);

        return new Guid(hashBytes);
    }
}

