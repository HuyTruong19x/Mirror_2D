using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MatchManager : Singleton<MatchManager>
{
    private readonly Dictionary<string, Match> _matchs = new Dictionary<string, Match>();

    public bool CreateMatch(NetworkConnectionToClient conn, ref MatchInfo matchInfo)
    {
        var gameID = GetRandomID();
        matchInfo.ID = gameID;

        GameObject matchControllerObject = GameObject.Instantiate(NetworkManager.singleton.spawnPrefabs[0]);
        matchControllerObject.GetComponent<NetworkMatch>().matchId = gameID.ToGuid();

        _matchs.Add(gameID, matchControllerObject.GetComponent<Match>());
        _matchs[gameID].Initialize(conn, matchInfo);

        matchControllerObject.name = "Match_" + gameID;

        return true;
    }

    public Match JoinMatch(NetworkConnectionToClient conn, string matchID)
    {
        if (_matchs.ContainsKey(matchID))
        {
            if(_matchs[matchID].JoinMatch(conn))
            {
                return _matchs[matchID];
            }
        }
        return null;
    }

    public Match GetMatch(string matchID)
    {
        if (_matchs.ContainsKey(matchID))
        {
            return _matchs[matchID];
        }

        return null;
    }

    public List<MatchInfo> GetMatchs()
    {
        return _matchs.Values.ToList().Select(x => x.Info).ToList();
    }

    public GameObject GetMatchObject(string matchID)
    {
        if (_matchs.ContainsKey(matchID))
        {
            return _matchs[matchID].gameObject;
        }

        return null;
    }

    public bool AddPlayerToMatch(string matchId, Player player)
    {
        if (_matchs.ContainsKey(matchId) && !_matchs[matchId].Players.Contains(player))
        {
            player.MatchID = matchId;
            _matchs[matchId].AddPlayer(player);
            return true;
        }

        return false;
    }

    public void RemovePlayerFromMatch(Player player)
    {
        if (_matchs.ContainsKey(player.MatchID))
        {
            _matchs[player.MatchID].RemovePlayer(player);
            if (_matchs[player.MatchID].Players.Count <= 0)
            {
                _matchs.Remove(player.MatchID);
                GameObject.Destroy(_matchs[player.MatchID].gameObject);
            }
        }
    }

    public void LeaveMatch(NetworkConnectionToClient conn, string matchID)
    {
        if (_matchs.ContainsKey(matchID))
        {
            _matchs[matchID].LeaveMatch(conn);
            if (_matchs[matchID].Connections.Count <= 0)
            {
                GameObject.Destroy(_matchs[matchID].gameObject);
            }
        }
    }

    public void StartMatch(NetworkConnectionToClient conn, string matchID)
    {
        if (_matchs.ContainsKey(matchID))
        {
            _matchs[matchID].StartMatch();
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
