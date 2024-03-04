using Mirror;
using UnityEngine;

public class ClientNetworkManager : MonoBehaviour
{
    public void OnClientConnect()
    {

    }

    public void OnClientDisconnect()
    {

    }

    public void OnStartClient()
    {
        NetworkClient.RegisterHandler<ClientRoomMessage>(OnClientMessage);
    }

    public void OnStopClient()
    {

    }

    [ClientCallback]
    public void RequestCreateRoom()
    {
        NetworkClient.Send(new ServerRoomMessage()
        {
            Operation = ServerRoomOperation.CREATE
        });
    }

    [ClientCallback]
    public void RequestRoomList()
    {
        NetworkClient.Send(new ServerRoomMessage()
        {
            Operation = ServerRoomOperation.LIST,
        });
    }

    private void OnClientMessage(ClientRoomMessage message)
    {
        EventDispatcher.Dispatch(ActionChannel.ROOM, message);
    }
}
