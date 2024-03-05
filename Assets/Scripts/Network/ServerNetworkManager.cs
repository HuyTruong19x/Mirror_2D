using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerNetworkManager : MonoBehaviour
{
    private ServerProcessor _processor;

    public void OnServerReady(NetworkConnectionToClient conn)
    {

    }

    public IEnumerator OnServerDisconnect(NetworkConnectionToClient conn)
    {
        yield break;
    }

    [ServerCallback]
    public void OnStartServer()
    {
        NetworkServer.RegisterHandler<ServerRoomMessage>(OnRoomMessage);
        NetworkServer.RegisterHandler<GameMessage>(OnGameMessage);
        _processor = new ServerProcessor();
    }

    public void OnStopServer()
    {

    }

    #region callback

    private void OnRoomMessage(NetworkConnectionToClient conn, ServerRoomMessage message)
    {
        ProcessMessage(conn, ActionChannel.ROOM, message);
    }

    private void OnGameMessage(NetworkConnectionToClient conn, GameMessage message)
    {
        ProcessMessage(conn, ActionChannel.GAME, message);
    }    

    private void ProcessMessage(NetworkConnectionToClient conn, ActionChannel channel, NetworkMessage message)
    {
        if (_processor == null)
        {
            Debug.Log($"Could not process action {channel}");
        }

        _processor.Handle(conn, channel, message);
    }

    #endregion

    [ServerCallback]
    public void OnGameLoaded(string roomId)
    {
           

    }    
}
