using Mirror;
using Mirror.Examples.MultipleMatch;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameNetworkManager : NetworkManager
{
    [Space(10)]
    [Header("Custom script")]
    [SerializeField]
    private ServerNetworkManager _serverNetwork;
    [SerializeField]
    private ClientNetworkManager _clientNetwork;

    #region Server System Callbacks

    /// <summary>
    /// Called on the server when a client is ready.
    /// <para>The default implementation of this function calls NetworkServer.SetClientReady() to continue the network setup process.</para>
    /// </summary>
    /// <param name="conn">Connection from client.</param>
    public override void OnServerReady(NetworkConnectionToClient conn)
    {
        base.OnServerReady(conn);
        _serverNetwork.OnServerReady(conn);
    }

    /// <summary>
    /// Called on the server when a client disconnects.
    /// <para>This is called on the Server when a Client disconnects from the Server. Use an override to decide what should happen when a disconnection is detected.</para>
    /// </summary>
    /// <param name="conn">Connection from client.</param>
    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        StartCoroutine(DoServerDisconnect(conn));
    }

    IEnumerator DoServerDisconnect(NetworkConnectionToClient conn)
    {
        yield return _serverNetwork.OnServerDisconnect(conn);
        base.OnServerDisconnect(conn);
    }

    #endregion

    #region Client System Callbacks

    /// <summary>
    /// Called on the client when connected to a server.
    /// <para>The default implementation of this function sets the client as ready and adds a player. Override the function to dictate what happens when the client connects.</para>
    /// </summary>
    public override void OnClientConnect()
    {
        base.OnClientConnect();
        _clientNetwork.OnClientConnect();
    }

    /// <summary>
    /// Called on clients when disconnected from a server.
    /// <para>This is called on the client when it disconnects from the server. Override this function to decide what happens when the client disconnects.</para>
    /// </summary>
    public override void OnClientDisconnect()
    {
        _clientNetwork.OnClientDisconnect();
        base.OnClientDisconnect();
    }

    #endregion

    #region Start & Stop Callbacks

    /// <summary>
    /// This is invoked when a server is started - including when a host is started.
    /// <para>StartServer has multiple signatures, but they all cause this hook to be called.</para>
    /// </summary>
    public override void OnStartServer()
    {
        _serverNetwork.OnStartServer();
    }

    /// <summary>
    /// This is invoked when the client is started.
    /// </summary>
    public override void OnStartClient()
    {
        _clientNetwork.OnStartClient();
    }

    /// <summary>
    /// This is called when a server is stopped - including when a host is stopped.
    /// </summary>
    public override void OnStopServer()
    {
        _serverNetwork.OnStopServer();
    }

    /// <summary>
    /// This is called when a client is stopped.
    /// </summary>
    public override void OnStopClient()
    {
        _clientNetwork.OnStopClient();
    }

    #endregion
}