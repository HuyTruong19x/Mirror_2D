using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonBehavior<T> : MonoBehaviour where T : MonoBehaviour
{
    #region Singleton
    private static T _instance;
    [SerializeField]
    private bool _isPersistent = true;
    public static T Instance
    {
        get
        {
            if (!_instance)
            {
                var instances = FindObjectsOfType<T>();
                if (instances.Length > 0)
                {
                    if (instances.Length > 1)
                    {
                        for (int i = 1; i < instances.Length; i++)
                        {
                            Destroy(instances[i].gameObject);
                        }
                    }
                    _instance = instances[0];
                }
                else
                {
                    _instance = new GameObject($"{nameof(SingletonBehavior<T>)}_{typeof(T)}").AddComponent<T>();
                }
            }
            return _instance;
        }
    }
    public virtual void Awake()
    {
        var instances = FindObjectsOfType<T>();
        if (instances.Length > 1)
        {
            for (int i = 0; i < instances.Length; i++)
            {
                if (instances[i].GetInstanceID() != Instance.GetInstanceID())
                {
                    Debug.Log("<color=red>Already another " + this.name + " object, will destroy this </color>" + instances[i].GetInstanceID());
                    Destroy(instances[i].gameObject);
                }
            }
        }
        if (_isPersistent)
        {
            DontDestroyOnLoad(gameObject);
        }
        OnAwake();
    }
    private void OnDestroy()
    {
        _instance = null;
    }
    protected virtual void OnAwake() { }
    #endregion
}
