using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Event/Void Event")]
public class VoidChannelEventSO : ScriptableObject
{
    private Action _onRaise;

    public void AddListener(Action onRaise)
    {
        _onRaise += onRaise;
    }    

    public void RemoveListener(Action onRaise)
    {
        _onRaise -= onRaise;
    }

    public void Raise()
    {
        _onRaise?.Invoke();
    }    
}
