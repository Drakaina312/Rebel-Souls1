using System;
using UnityEngine;
using System.Linq;

[Serializable]
public class Profile
{
    public string ProfileName;
    public int ProfileID;
    public bool IsBlocked;
    public bool IsEmpty = true;
    public StatsBook[] BooksStat;
    public Profile(string profileName, bool isBlocked)
    {
        ProfileName = profileName;
        IsBlocked = isBlocked;
    }


    public StatsBook FindChapterStatsFromSave(ChapterSortingConditions chapterSortingConditions)
    {
        return BooksStat.FirstOrDefault(predict =>
        predict.ChapterSortingConditions.BookName == chapterSortingConditions.BookName &&
        predict.ChapterSortingConditions.ActName == chapterSortingConditions.ActName &&
        predict.ChapterSortingConditions.ChapterName == chapterSortingConditions.ChapterName);
    }
    public void SaveStatsForFirstLaunch(ActStatistics actStatistics, ChapterSortingConditions chapterSortingCondition, DialogSequence previousChapter = null)
    {
        if (BooksStat == null)
        {
            BooksStat = new StatsBook[1];
            BooksStat[0] = new StatsBook()
            {
                Statistics = actStatistics.ActStats.ToArray(),
                ChapterSortingConditions = chapterSortingCondition
            };

        }

        StatsBook chapterStats = FindChapterStatsFromSave(chapterSortingCondition);
        if (chapterStats != null)
        {
            Debug.Log("Глава существует");
            //chapterStats.AddNewStatistic(actStatistics.ActStats.ToArray());
        }
        else
        {

            StatsBook[] newChapterStatistics = new StatsBook[BooksStat.Length + 1];
            if (previousChapter == null)
            {
                newChapterStatistics[newChapterStatistics.Length - 1] = new StatsBook()
                {
                    ChapterSortingConditions = chapterSortingCondition,
                    Statistics = actStatistics.ActStats.ToArray()
                };
            }
            else
            {
                StatisticInfo[] previousChapterStats = FindChapterStatsFromSave(previousChapter.ChapterSortingCondition).Statistics;
                StatisticInfo[] newStats = new StatisticInfo[previousChapterStats.Length];
                int i = 0;
                foreach (var prevousStat in previousChapterStats)
                {
                    newStats[i] = new StatisticInfo()
                    {
                        StatisticCount = prevousStat.StatisticCount,
                        StatisticName = prevousStat.StatisticName,
                       
                    };
                    Debug.Log(newStats[i].StatisticName);
                    Debug.Log(newStats[i].StatisticCount);

                    i++;
                }

                newChapterStatistics[newChapterStatistics.Length - 1] = new StatsBook()
                {
                    ChapterSortingConditions = chapterSortingCondition,
                    Statistics = newStats
                };

                Debug.Log(newChapterStatistics[newChapterStatistics.Length - 1].Statistics[0].StatisticName);
                Debug.Log(newChapterStatistics[newChapterStatistics.Length - 1].Statistics[0].StatisticCount);


            }
            for (int i = 0; i < BooksStat.Length; i++)
            {
                newChapterStatistics[i] = BooksStat[i];
            }
            BooksStat = newChapterStatistics;

            foreach (var item in BooksStat)
            {
                foreach (var ss in item.Statistics)
                {
                    Debug.Log(item.ChapterSortingConditions.ChapterName);
                    Debug.Log(ss.StatisticName);
                    Debug.Log(ss.StatisticCount);
                }
            }
        }

    }
}
