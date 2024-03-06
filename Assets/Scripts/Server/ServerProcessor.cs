using Mirror;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class ServerProcessor
{
    protected static readonly Dictionary<MessageCode, IMessageHandler> _packetHandlers = new();

    public ServerProcessor()
    {
        _packetHandlers.Clear();
        int num = SearchPacketHandlers(Assembly.GetExecutingAssembly());
        Debug.Log($"PacketProcessor: Loaded " + num + $" handlers from {Assembly.GetExecutingAssembly()} Assembly!");
    }

    public static void RegisterPacketHandler(MessageCode packetCode, IMessageHandler handler)
    {
        _packetHandlers.Add(packetCode, handler);
    }

    protected static int SearchPacketHandlers(Assembly assembly)
    {
        int num = 0;
        Type[] types = assembly.GetTypes();
        Type[] array = types;
        foreach (Type type in array)
        {
            if (type.IsClass && typeof(IMessageHandler).IsAssignableFrom(type))
            {
                MessageAttribute[] array2 = (MessageAttribute[])type.GetCustomAttributes(typeof(MessageAttribute), inherit: true);
                if (array2.Length != 0)
                {
                    num++;
                    RegisterPacketHandler(array2[0].Code, (IMessageHandler)Activator.CreateInstance(type));
                }
            }
        }
        return num;
    }

    public void Handle(NetworkConnectionToClient conn, MessageCode channel, NetworkMessage message)
    {
        if (_packetHandlers.ContainsKey(channel))
        {
            _packetHandlers[channel].Handle(conn, message);
        }
        else
        {
            Debug.Log($"Could not found channel {channel} on processor");
        }    
    }
}
