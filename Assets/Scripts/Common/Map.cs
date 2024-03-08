using Mirror;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class Map : MonoBehaviour
{
    public int MapID;
    [SerializeField]
    private List<Transform> _startPositions = new();
    public readonly List<int> Roles = new();

    public Vector3 GetStartPosition()
    {
        return _startPositions[Random.Range(0, _startPositions.Count)].position;
    }

    [ServerCallback]
    public int GetRandomRole()
    {
        var role = Roles[0];
        Roles.RemoveAt(0);
        return role;
    }

    [ServerCallback]
    public void SetupRole(int TotalPlayer)
    {
        Roles.Clear();

        int wolfCount = 1;
        int foxCount = 1;

        if (TotalPlayer < 9)
        {
            wolfCount = 1;
            foxCount = 1;
        }
        else if (TotalPlayer < 12)
        {
            wolfCount = 2;
            foxCount = 2;
        }
        else
        {
            wolfCount = 3;
            foxCount = 3;
        }

        for (int i = 0; i < wolfCount; i++)
        {
            Roles.Add(2);
        }

        for (int i = 0; i < foxCount; i++)
        {
            Roles.Add(1);
        }

        int playerCount = TotalPlayer - wolfCount - foxCount;
        for (int i = 0; i < playerCount; i++)
        {
            Roles.Add(0);
        }

        Roles.Shuffer();
    }
}
