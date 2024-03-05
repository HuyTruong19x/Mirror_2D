using Mirror;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private Dictionary<Guid, Game> _games = new();

    public bool HasRoom(Guid roomId)
    {
        return _games.ContainsKey(roomId);
    }

    public GameObject CreateGame(Guid gameID)
    {
        GameObject matchControllerObject = GameObject.Instantiate(NetworkManager.singleton.spawnPrefabs[0]);
        matchControllerObject.GetComponent<NetworkMatch>().matchId = gameID;
        _games[gameID] = matchControllerObject.GetComponent<Game>();
        return matchControllerObject;
    }

    public void AddPlayerToGame(Guid gameID, PlayerController playerController)
    {
        if (_games.ContainsKey(gameID))
        {
            playerController.GameId = gameID;
            _games[gameID].AddPlayer(playerController);
        }
    }

    public void RemovePlayerFromGame(Guid gameID, PlayerController playerController)
    {
        if (_games.ContainsKey(gameID))
        {
            _games[gameID].RemovePlayer(playerController);
            if (_games[gameID].Players.Count <= 0)
            {
                GameObject.Destroy(_games[gameID].gameObject);
            }
        }
    }

    public void StartGame(Guid gameID)
    {
        if (_games.ContainsKey(gameID))
        {
            _games[gameID].StartGame();
        }
        else
        {
            Debug.LogWarning($"Could not start game {gameID} due to not found");
        }
    }
}


