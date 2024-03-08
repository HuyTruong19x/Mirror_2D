using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="GamePlayer/New Role")]
public class RoleDataSO : ScriptableObject
{
    public int ID;
    public Sprite Avatar;
    public Sprite SpaceIcon;
    public Sprite SpecialIcon;
    public RoleAction Action;
}
