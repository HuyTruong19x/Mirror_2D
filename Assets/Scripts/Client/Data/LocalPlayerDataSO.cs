using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="GamePlay/Player Data")]
public class LocalPlayerDataSO : ScriptableObject
{
    public PlayerInfo Data;

    private void Awake()
    {
        Data.ID = "ID_" + Random.Range(0, 100000);
        Data.Name = "User_" + Random.Range(0, 100000);
    }
}
