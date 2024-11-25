using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "ActStatistics", menuName = "Scriptable Objects/ActStatistics")]
public class ActStatistics : ScriptableObject
{
    public List<StatisticInfo> ActStats;
}
