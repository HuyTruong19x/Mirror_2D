using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public int MapID;
    [SerializeField]
    private List<Transform> _startPositions = new();

    public Vector3 GetStartPosition()
    {
        return _startPositions[Random.Range(0, _startPositions.Count)].position;
    }
}
