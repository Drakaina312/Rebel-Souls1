using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class StatsBook
{
    public ChapterSortingConditions ChapterSortingConditions;
    public bool IsLastSave;
    public int[] SavedIndexes;
    public Dictionary<string, StatisticInfo[]> SavedStats;


    public string MainHeroName;
    public string MainHeroSpritePath;
    public string FunTextBox;
    public string HorrorTextBox;
    public string GameTextBox;

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
                    statChapter = item.Key;
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

    public bool IsSlideIndexExistInSave(int slideIndexToCheck)
    {
        foreach (int index in SavedIndexes)
            UnityEngine.Debug.Log("Вот =" + index);


        if (SavedIndexes.Contains(slideIndexToCheck))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}



