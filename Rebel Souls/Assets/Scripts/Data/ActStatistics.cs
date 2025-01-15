using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "ActStatistics", menuName = "Scriptable Objects/ActStatistics")]
public class ActStatistics : SerializedScriptableObject
{
    //public List<StatisticInfo> ActStats;

    public List<LoverInfo> ActLovers;

    public Dictionary<string, StatisticInfo[]> Stats;

    public Sprite BGLovers;

    public Sprite BGStatistic;
}
