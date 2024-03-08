using System;
using UnityEngine;
using UnityEngine.UI;

public class UILobbyRoom : MonoBehaviour
{
    private MatchInfo _info;
    [SerializeField]
    private Text _hostName;
    [SerializeField]
    private Text _mode;
    [SerializeField]
    private Text _map;
    [SerializeField]
    private Text _status;

    private Action<MatchInfo> OnClick;

    public UILobbyRoom Setup(MatchInfo info)
    {
        _hostName.text = info.HostName;
        _mode.text = info.Mode;
        _map.text = info.Map;
        _status.text = info.Status;
        _info = info;
        return this;
    }

    public UILobbyRoom OnButtonClick(Action<MatchInfo> onClick)
    {
        OnClick = onClick;
        return this;
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Click()
    {
        OnClick(_info);
    }    
}

public class LobbyRoomInfo
{
    public string ID;
    public string HostName;
    public string Mode;
    public string Map;
    public string Status;
    public int MaxPlayer;

    public LobbyRoomInfo(string id, string hostName, string mode, string map, int maxPlayer)
    {
        ID = id;
        HostName = hostName;
        Mode = mode;
        Map = map;
        MaxPlayer = maxPlayer;
    }

    public void UpdateStatus(string status)
    {
        Status = status;
    }

    public LobbyRoomInfo()
    { }
}
