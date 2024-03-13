using Mirror;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class Map : MonoBehaviour
{
    public int MapID;
    [SerializeField]
    private List<Transform> _startPositions = new();
    [SerializeField]
    private RoleDatabase _roleDatabase;
    private readonly Queue<RoleDataSO> _roles = new();

    [SerializeField]
    private bool _isUniqueRole = false;

    public Vector3 GetStartPosition()
    {
        return _startPositions[Random.Range(0, _startPositions.Count)].position;
    }

    [ServerCallback]
    public RoleDataSO GetRandomRole()
    {
        return _roles.Dequeue();
    }

    [ServerCallback]
    public void SetupRole(int totalPlayer)
    {
        _roleDatabase.Initialized();
        _roles.Clear();

        int wolfCount;
        int foxCount;
        if(totalPlayer <= 2)
        {
            wolfCount = 1;
            foxCount = 0;
        }
        else if (totalPlayer < 9)
        {
            wolfCount = 1;
            foxCount = 1;
        }
        else if (totalPlayer < 12)
        {
            wolfCount = 2;
            foxCount = 2;
        }
        else
        {
            wolfCount = 3;
            foxCount = 3;
        }

        Debug.Log($"Total player {totalPlayer} - total enemy {wolfCount} - total thirdparty {foxCount}");

        for (int i = 0; i < wolfCount; i++)
        {
            _roles.Enqueue(_roleDatabase.Get(RoleType.ENEMY, _isUniqueRole));
        }

        for (int i = 0; i < foxCount; i++)
        {
            _roles.Enqueue(_roleDatabase.Get(RoleType.THIRD_PARTY, _isUniqueRole));
        }

        int playerCount = totalPlayer - wolfCount - foxCount;
        for (int i = 0; i < playerCount; i++)
        {
            _roles.Enqueue(_roleDatabase.Get(RoleType.NONE, _isUniqueRole));
        }
    }
}
