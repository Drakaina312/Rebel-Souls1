using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class StatsBook
{
    public ChapterSortingConditions ChapterSortingConditions;
    public StatisticInfo[] Statistics;
    public void AddNewStatistic(StatisticInfo[] massiveStatistics)
    {
        if (Statistics != null)
        {
            for (int i = 0; i < massiveStatistics.Length; i++)
            {
                StatisticInfo statToChange = Statistics.FirstOrDefault(stat => stat.StatisticName == massiveStatistics[i].StatisticName);
                if (statToChange != null)
                    statToChange.StatisticCount = massiveStatistics[i].StatisticCount;
                else if (statToChange == null)
                {
                    StatisticInfo[] newStatistics = new StatisticInfo[Statistics.Length + 1];

                    for (int j = 0; j < Statistics.Length; j++)
                    {
                        newStatistics[j] = Statistics[j];
                    }
                    newStatistics[newStatistics.Length - 1] = massiveStatistics[i];
                    Statistics = newStatistics;
                }
            }
        }
        else
        {
            Statistics = massiveStatistics;
        }
    }
}



