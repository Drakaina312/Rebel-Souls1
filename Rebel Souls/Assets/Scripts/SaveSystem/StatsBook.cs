using System;
using System.Linq;

[Serializable]
public class StatsBook
{
    public ChapterSortingConditions ChapterSortingConditions;
    public StatisticInfo[] Statistics;
    public bool IsLastSave;

    internal StatisticInfo FindStat(string statName)
    {
        StatisticInfo stat = Statistics.FirstOrDefault(x => x.StatisticName == statName);
        if (stat != null)
            return stat;
        else
            throw new Exception($"Unable to find stat {statName} in save file");
    }
}



