using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILobbyRoom : MonoBehaviour
{
    private LobbyRoomInfo _roomInfo;
    [SerializeField]
    private Text _hostName;
    [SerializeField]
    private Text _mode;
    [SerializeField]
    private Text _map;
    [SerializeField]
    private Text _status;

    public UILobbyRoom Setup(LobbyRoomInfo info)
    {
        _hostName.text = info.HostName;
        _mode.text = info.Mode;
        _map.text = info.Map;
        _status.text = info.Status;
        _roomInfo = info;
        return this;
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
}

public class LobbyRoomInfo
{
    public string ID;
    public string HostName;
    public string Mode;
    public string Map;
    public string Status;

    public LobbyRoomInfo(string id, string hostName, string mode, string map, string status)
    {
        ID = id;
        HostName = hostName;
        Mode = mode;
        Map = map;
        Status = status;
    }

    public LobbyRoomInfo()
    { }
}
