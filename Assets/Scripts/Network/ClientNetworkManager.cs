using Mirror;
using Mirror.Examples.MultipleMatch;
using UnityEngine;

public class ClientNetworkManager : MonoBehaviour
{
    public string MatchID;

    public void OnClientConnect()
    {

    }

    public void OnClientDisconnect()
    {
    }

    public void OnStartClient()
    {
        NetworkClient.RegisterHandler<ClientMatchMessage>(OnClientMessage);
    }

    public void OnStopClient()
    {
        NetworkClient.UnregisterHandler<ClientMatchMessage>();
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
        NetworkClient.Send(new ServerMatchMessage()
        {
            Operation = MatchOperation.LIST,
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

    private void OnClientMessage(ClientMatchMessage message)
    {
        EventDispatcher.Dispatch(MessageCode.MATCH, message);
    }
}
