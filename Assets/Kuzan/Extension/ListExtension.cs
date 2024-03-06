using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ListExtension
{
    public static T GetRandom<T>(this List<T> list)
    {
        return list[UnityEngine.Random.Range(0, list.Count)];
    }

    public static void Shuffer<T>(this List<T> list)
    {
        int index = 0;
        for(int i = 0; i < list.Count; i++)
        {
            index = UnityEngine.Random.Range(0, list.Count);
            var temp = list[i];
            list[i] = list[index];
            list[index] = temp;
        }
    }
    public static List<T> Clone<T>(this List<T> list)
    {
        var newList = new List<T>();
        newList.AddRange(list);
        return newList;
    }   
}
