using Mirror;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    [SyncVar] public int DiscussTime = 180;
    [SyncVar] public int VoteTime = 30;
    [SyncVar] public int MinWolf = 3;
    [SyncVar] public int MinFox = 3;
    public SyncList<Role> Roles = new SyncList<Role>();
    public List<NetworkConnectionToClient> Players = new List<NetworkConnectionToClient>();

    [ServerCallback]
    public void AddPlayer(NetworkConnectionToClient player)
    {
        Players.Add(player);
    }

    [ServerCallback]
    public void StartGame()
    {
        SetupRole();
        foreach(var player in Players)
        {
            player.Send(new GameMessage()
            {
                Operation = 2,
                State = GameState.PLAYING,
                Role = GetRandomRole().RoleID,
                Position = Vector3.zero
            });
        }    
    }

    [ServerCallback]
    public Role GetRandomRole()
    {
        var role = Roles[0];
        Roles.RemoveAt(0);
        return role;
    }

    [ServerCallback]
    public void SetupRole()
    {
        Roles.Clear();

        int wolfCount = 1;
        int foxCount = 1;

        if (Players.Count < 9)
        {
            wolfCount = 1;
            foxCount = 1;
        }
        else if (Players.Count < 12)
        {
            wolfCount = 2;
            foxCount = 2;
        }   
        else
        {
            wolfCount = 3;
            foxCount = 3;
        }    

        for (int i = 0; i < wolfCount;i++)
        {
            Roles.Add(new Role());//TODO add wolf
        }

        for (int i = 0; i < foxCount; i++)
        {
            Roles.Add(new Role());//TODO add fox
        }

        int playerCount = Players.Count - wolfCount - foxCount;
        for (int i = 0; i < playerCount; i++)
        {
            Roles.Add(new Role());//TODO add dog
        }

        Roles.Shuffer();
    }
}

public static class SyncListExtension
{
    public static T GetRandom<T>(this SyncList<T> list)
    {
        return list[UnityEngine.Random.Range(0, list.Count)];
    }

    public static void Shuffer<T>(this SyncList<T> list)
    {
        int index = 0;
        for (int i = 0; i < list.Count; i++)
        {
            index = UnityEngine.Random.Range(0, list.Count);
            var temp = list[i];
            list[i] = list[index];
            list[index] = temp;
        }
    }
    public static SyncList<T> Clone<T>(this SyncList<T> list)
    {
        var newList = new SyncList<T>();
        newList.AddRange(list);
        return newList;
    }
}
