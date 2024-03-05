using Mirror;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.Security.Cryptography;
using Mirror.Examples.MultipleMatch;
using System.Linq;

public class RoomManager : Singleton<RoomManager>
{
    private Dictionary<string, Room> Rooms = new();
    private List<LobbyRoomInfo> RoomInfos = new();
    private int _maxPlayer = 16;

    public Room CreateRoom(NetworkConnectionToClient conn, PlayerInfo info)
    {
        var id = GetRandomID();
        var roomInfo = new LobbyRoomInfo(id, info.Name, "Random", "Map_1", _maxPlayer);
        var room = new Room(conn, info, roomInfo);
        Rooms.Add(id, room);
        RoomInfos.Add(room.Info);
        return room;
    }

    public Room JoinRoom(string id, NetworkConnectionToClient conn, PlayerInfo playerInfo)
    {
        if (Rooms.ContainsKey(id))
        {
            Rooms[id].AddPlayer(conn, playerInfo);
            return Rooms[id];
        }
        return null;
    }

    public void LeaveRoom(NetworkConnectionToClient conn, string id)
    {

    }

    public List<LobbyRoomInfo> GetRoomListInfo()
    {
        return RoomInfos;
    }

    public Room GetRoom(string id)
    {
        if(Rooms.ContainsKey(id))
        {
            return Rooms[id];
        }

        return null;
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

