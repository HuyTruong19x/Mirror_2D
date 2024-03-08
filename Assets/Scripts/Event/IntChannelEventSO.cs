using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Event/Int Event")]
public class IntChannelEventSO : ScriptableObject
{
    private Action<int> _onRaise;

    public void AddListener(Action<int> onRaise)
    {
        _onRaise += onRaise;
    }

    public void RemoveListener(Action<int> onRaise)
    {
        _onRaise -= onRaise;
    }

    public void Raise(int value)
    {
        _onRaise?.Invoke(value);
    }
}
