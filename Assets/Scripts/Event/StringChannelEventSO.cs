using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Event/String Event")]
public class StringChannelEventSO : ScriptableObject
{
    private Action<string> _onRaise;

    public void AddListener(Action<string> onRaise)
    {
        _onRaise += onRaise;
    }

    public void RemoveListener(Action<string> onRaise)
    {
        _onRaise -= onRaise;
    }

    public void Raise(string value)
    {
        _onRaise?.Invoke(value);
    }
}
