using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class StatsBook
{
    public ChapterSortingConditions ChapterSortingConditions;
    public bool IsLastSave;
    public string[] SavedIndexes;
    public Dictionary<string, StatisticInfo[]> SavedStats;


    public StatisticInfo FindStat(string statName, out string statChapter)
    {
        StatisticInfo stat = null;
        foreach (var item in SavedStats)
        {
            foreach (var stats in item.Value)
            {
                if (stats.StatisticName == statName)
                {
                    stat = stats;
                    statChapter =  item.Key;
                    return stat;
                }

            }
        }

        //StatisticInfo stat = Statistics.FirstOrDefault(x => x.StatisticName == statName);
        throw new Exception($"Unable to find stat {statName} in save file");
    }
    public StatisticInfo FindStat(string statName)
    {
        StatisticInfo stat = null;
        foreach (var item in SavedStats)
        {
            foreach (var stats in item.Value)
            {
                if (stats.StatisticName == statName)
                {
                    stat = stats;
                    return stat;
                }

            }
        }

        //StatisticInfo stat = Statistics.FirstOrDefault(x => x.StatisticName == statName);
        throw new Exception($"Unable to find stat {statName} in save file");
    }

    public bool IsSlideIndexExistInSave(string indexName)
    {
        foreach (string index in SavedIndexes)
            UnityEngine.Debug.Log("Вот =" + index);


        if (SavedIndexes.FirstOrDefault(x => x == indexName) != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}



