using Mirror;
using System;
using System.Collections.Generic;
using UnityEngine;

public class EventDispatcher : MonoBehaviour
{
    private static Dictionary<MessageCode, Action<NetworkMessage>> _events = new Dictionary<MessageCode, Action<NetworkMessage>>();

    public static void Subscribe(MessageCode code, Action<NetworkMessage> callback)
    {
        if (_events.ContainsKey(code))
        {
            _events[code] += callback;
        }
        else
        {
            _events.Add(code, callback);
        }
    }

    public static void Unsubscribe(MessageCode code, Action<NetworkMessage> callback)
    {
        if (_events.ContainsKey(code))
        {
            _events[code] -= callback;
        }
    }

    public static void Dispatch(MessageCode code, NetworkMessage result)
    {
        if (_events.ContainsKey(code))
        {
            _events[code]?.Invoke(result);
        }
    }
}
