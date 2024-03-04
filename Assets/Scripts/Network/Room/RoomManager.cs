using Mirror;
using Mirror.Examples.MultipleMatch;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : Singleton<RoomManager>
{
    public Dictionary<string, Room> Rooms = new();

    public Room CreateRoom(NetworkConnectionToClient conn, PlayerInfo info)
    {
        var id = GetRandomID();
        var room = new Room(id, 4, "Tesst_" + UnityEngine.Random.Range(0, 9999), conn, info);
        Rooms.Add(id, room);
        return room;
    }

    public Room JoinRoom(string id, NetworkConnectionToClient conn)
    {

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
