using System;
using System.Collections.Generic;

[Serializable]
public class StatsBook
{
    public string BookName;
    public string SavePuff;
    public string ActName;
    public Acts[] Acts;
    public StatisticInfo[] Statistics;
    public void AddNewStatistic(StatisticInfo massiveStatistics)
    {
        if (Statistics != null)
        {
            StatisticInfo[] newmassiveacts = new StatisticInfo[Statistics.Length + 1];
            for (int i = 0; i < Statistics.Length; i++)
            {
                newmassiveacts[i] = Statistics[i];
            }
            newmassiveacts[newmassiveacts.Length - 1] = massiveStatistics;
            Statistics = newmassiveacts;
        }
        else
        {
            Statistics = new StatisticInfo[1] { massiveStatistics };
        }
    }
    public void AddStatistic(List<StatisticInfo> addStatistic)
    {
        Statistics = addStatistic.ToArray();
    }

    public void SaveAllBookData()
    {

    }
}


[Serializable]
public class Acts
{
    public Chapters[] Chapters;
}
[Serializable]
public class Chapters
{
    public StatisticInfo[] statisticInfos;
}



