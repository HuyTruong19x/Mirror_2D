using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameObjectExtension
{
    public static bool HasComponent(this GameObject gameObject, System.Type component)
    {
        if(gameObject.GetComponent(component) != null)
        {
            return true;
        }
        return false;
    }
    public static bool HasComponent<T>(this GameObject gameObject) where T : Component
    {
        if (gameObject.GetComponent<T>() != null)
        {
            return true;
        }
        return false;
    }

    public static bool GetOrAddComponent(this GameObject gameobject, System.Type component)
    {
        if(gameobject.HasComponent(component))
        {
            return gameobject.GetComponent(component);
        }
        return gameobject.AddComponent(component);
    }
    public static T GetOrAddComponent<T>(this GameObject gameobject) where T : Component
    {
        if (gameobject.HasComponent<T>())
        {
            return gameobject.GetComponent<T>();
        }
        return gameobject.AddComponent<T>();
    }
}
