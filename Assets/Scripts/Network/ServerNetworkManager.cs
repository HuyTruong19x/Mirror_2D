using Mirror;
using System.Collections;
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
        NetworkServer.RegisterHandler<ServerMatchMessage>(OnServerMatchMessageReceive);
        _processor = new ServerProcessor();
    }

    public void OnStopServer()
    {

    }

    #region callback   

    private void OnServerMatchMessageReceive(NetworkConnectionToClient conn, ServerMatchMessage message)
    {
        ProcessMessage(conn, MessageCode.MATCH, message);
    }

    private void ProcessMessage(NetworkConnectionToClient conn, MessageCode channel, NetworkMessage message)
    {
        if (_processor == null)
        {
            Debug.Log($"Could not process action {channel}");
        }

        _processor.Handle(conn, channel, message);
    }

    #endregion

    [ServerCallback]
    public void LeaveGame(NetworkConnectionToClient conn)
    {
        ProcessMessage(conn, MessageCode.MATCH, new ServerMatchMessage()
        {
            Operation = MatchOperation.LEAVE
        });

    }    
}
