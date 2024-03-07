using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="GamePlay/Match Info")]
public class MatchInfoSO : ScriptableObject
{
    public MatchInfo Info;
    public Action<MatchInfo> OnMatchInfoChanged;

    public void SetInfo(MatchInfo matchInfo)
    {
        Info = matchInfo;
        OnMatchInfoChanged?.Invoke(Info);
    }    
}
