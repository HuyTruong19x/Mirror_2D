using Mirror;
using UnityEngine;

public class ClientNetworkManager : MonoBehaviour
{
    public string RoomID;

    public void OnClientConnect()
    {

    }

    public void OnClientDisconnect()
    {
    }

    public void OnStartClient()
    {
        NetworkClient.RegisterHandler<ClientRoomMessage>(OnClientMessage);
        NetworkClient.RegisterHandler<WaitingRoomMessage>(OnWaitingGameMessage);
    }

    public void OnStopClient()
    {
        NetworkClient.UnregisterHandler<ClientRoomMessage>();
        NetworkClient.UnregisterHandler<WaitingRoomMessage>();
    }

    [ClientCallback]
    public void RequestCreateRoom()
    {
        var playerInfo = new PlayerInfo()
        {
            ID = "ID_" + Random.Range(0, 100000),
            Name = "User_" + Random.Range(0, 100000)
        };
        NetworkClient.Send(new ServerMatchMessage()
        {
            Operation = MatchOperation.CREATE,
            MatchInfo = new MatchInfo()
            {
                HostName = playerInfo.Name,
                Mode = "Random",
                Map = "Map_0",
                MaxPlayer = 16
            },
            PlayerInfo = playerInfo// TODO get player info from data
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

    [ClientCallback]
    public void RequestJoinRoom(string roomId)
    {
        var playerInfo = new PlayerInfo()
        {
            ID = "ID_" + Random.Range(0, 100000),
            Name = "User_" + Random.Range(0, 100000)
        };
        NetworkClient.Send(new ServerMatchMessage()
        {
            Operation = MatchOperation.JOIN,
            MatchID = roomId,
            PlayerInfo = playerInfo// TODO get player info from data
        });
    }

    private void OnClientMessage(ClientRoomMessage message)
    {
        EventDispatcher.Dispatch(ActionChannel.ROOM, message);
    }

    private void OnWaitingGameMessage(WaitingRoomMessage message)
    {
        EventDispatcher.Dispatch(ActionChannel.WAITING_ROOM, message);
    }
}
