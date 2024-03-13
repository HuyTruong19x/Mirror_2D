using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="GamePlayer/New Role")]
public class RoleDataSO : ScriptableObject
{
    public int ID;
    public RoleType Type;
    public Sprite Avatar;
    public Sprite SpaceIcon;
    public Sprite SpecialIcon;
    public RoleAction Action;
}

public enum RoleType
{
    NONE = 0,
    ENEMY = 1,
    THIRD_PARTY = 2,
}
