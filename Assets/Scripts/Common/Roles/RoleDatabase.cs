using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "GamePlayer/Role/New Database")]
public class RoleDatabase : ScriptableObject
{
    public List<RoleDataSO> Normal;
    public List<RoleDataSO> Enemy;
    public List<RoleDataSO> ThirdParty;

    private Queue<RoleDataSO> _queueNormal;
    private Queue<RoleDataSO> _queueEnemy;
    private Queue<RoleDataSO> _queueThirdParty;

    public void Initialized()
    {
        _queueNormal = new Queue<RoleDataSO>(Normal);
        _queueEnemy = new Queue<RoleDataSO>(Enemy);
        _queueThirdParty = new Queue<RoleDataSO>(ThirdParty);
    }

    public RoleDataSO Get(RoleType type, bool isUnique)
    {
        if (isUnique)
        {
            switch (type)
            {
                case RoleType.NONE: return _queueNormal.Dequeue();
                case RoleType.ENEMY: return _queueEnemy.Dequeue();
                case RoleType.THIRD_PARTY: return _queueThirdParty.Dequeue();
            }
        }
        else
        {
            switch (type)
            {
                case RoleType.NONE:
                    {
                        Normal.Shuffer();
                        return Normal.Take(1).Single();
                    }
                case RoleType.ENEMY:
                    {
                        Enemy.Shuffer();
                        return Enemy.Take(1).Single();
                    }
                case RoleType.THIRD_PARTY:
                    {
                        ThirdParty.Shuffer();
                        return ThirdParty.Take(1).Single();
                    }
            }
        }

        return null;
    }

    public RoleDataSO GetById(RoleType type, int roleId)
    {
        switch (type)
        {
            case RoleType.NONE:
                {
                    return Normal.Where(x => x.ID == roleId).Single();
                }
            case RoleType.ENEMY:
                {
                    Enemy.Shuffer();
                    return Enemy.Where(x => x.ID == roleId).Single();
                }
            case RoleType.THIRD_PARTY:
                {
                    ThirdParty.Shuffer();
                    return ThirdParty.Where(x => x.ID == roleId).Single();
                }
        }

        return null;
    }
}
