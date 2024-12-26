using System;
using System.Linq;

[Serializable]
public class StatsBook
{
    public ChapterSortingConditions ChapterSortingConditions;
    public StatisticInfo[] Statistics;
    public bool IsLastSave;
    public string[] SavedIndexes;

    public StatisticInfo FindStat(string statName)
    {
        StatisticInfo stat = Statistics.FirstOrDefault(x => x.StatisticName == statName);
        if (stat != null)
            return stat;
        else
            throw new Exception($"Unable to find stat {statName} in save file");
    }

    public bool IsSlideIndexExistInSave(string indexName)
    {
        UnityEngine.Debug.Log(indexName + " F ye rf");
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



