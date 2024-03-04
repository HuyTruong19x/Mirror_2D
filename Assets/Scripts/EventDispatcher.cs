using Mirror;
using System;
using System.Collections.Generic;
using UnityEngine;

public class EventDispatcher : MonoBehaviour
{
    private static Dictionary<ActionChannel, Action<NetworkMessage>> _events = new Dictionary<ActionChannel, Action<NetworkMessage>>();

    public static void Subscribe(ActionChannel code, Action<NetworkMessage> callback)
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

    public static void Unsubscribe(ActionChannel code, Action<NetworkMessage> callback)
    {
        if (_events.ContainsKey(code))
        {
            _events[code] -= callback;
        }
    }

    public static void Dispatch(ActionChannel code, NetworkMessage result)
    {
        if (_events.ContainsKey(code))
        {
            _events[code]?.Invoke(result);
        }
    }
}
