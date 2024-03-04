using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> where T : new()
{
    #region Singleton
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new T();
            }    
            return _instance;
        }
    }
    public void Clean()
    {
        _instance = default(T);
    }
    #endregion
}
