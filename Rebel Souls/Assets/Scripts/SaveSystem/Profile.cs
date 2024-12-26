using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

[Serializable]
public class Profile
{
    public string ProfileName;
    public int ProfileID;
    public bool IsBlocked;
    public bool IsEmpty = true;
    public StatsBook[] BooksStat;
    public string LastSaveChapterPath;
    public int DialogIndex;
    internal bool IsHelpOn;

    public Profile(string profileName, bool isBlocked)
    {
        ProfileName = profileName;
        IsBlocked = isBlocked;
    }


    public StatsBook FindChapterStatsFromSave(ChapterSortingConditions chapterSortingConditions)
    {
        return BooksStat.FirstOrDefault(predict =>
        predict.ChapterSortingConditions.BookName == chapterSortingConditions.BookName &&
        //predict.ChapterSortingConditions.ActName == chapterSortingConditions.ActName &&
        predict.ChapterSortingConditions.ChapterName == chapterSortingConditions.ChapterName);
    }
    public void SaveStatsForFirstLaunch(ActStatistics actStatistics, ChapterSortingConditions chapterSortingCondition, StoryLine previousChapter = null)
    {
        if (BooksStat == null)
        {
            BooksStat = new StatsBook[1];
            BooksStat[0] = new StatsBook()
            {
                Statistics = actStatistics.ActStats.ToArray(),
                ChapterSortingConditions = chapterSortingCondition,      
            };

        }

        StatsBook chapterStats = FindChapterStatsFromSave(chapterSortingCondition);
        if (chapterStats != null)
        {
            Debug.Log("Глава существует");


            foreach (var item in BooksStat)
                item.IsLastSave = false;
            chapterStats.IsLastSave = true;

            //chapterStats.AddNewStatistic(actStatistics.ActStats.ToArray());
        }
        else
        {
            List<StatsBook> newChapterStatistic = new List<StatsBook>();

            foreach (StatsBook oldStat in BooksStat)
            {
                oldStat.IsLastSave = false;
                newChapterStatistic.Add(oldStat);
            }

            if (previousChapter == null)
            {
                newChapterStatistic.Add(
                    new StatsBook()
                    {
                        ChapterSortingConditions = chapterSortingCondition,
                        Statistics = actStatistics.ActStats.ToArray()
                    });
            }
            else
            {
                StatisticInfo[] previousChapterStats = FindChapterStatsFromSave(previousChapter.ChapterSortingCondition).Statistics;
                StatisticInfo[] newStatistics = new StatisticInfo[previousChapterStats.Length];
                for (int i = 0; i < previousChapterStats.Length; i++)
                {
                    newStatistics[i].StatisticCount = previousChapterStats[i].StatisticCount;
                    newStatistics[i].StatisticName = previousChapterStats[i].StatisticName;
                }
                newChapterStatistic.Add(
                    new StatsBook()
                    {
                        ChapterSortingConditions = chapterSortingCondition,
                        Statistics = newStatistics
                    });
            }
            BooksStat = newChapterStatistic.ToArray();
        }

    }
}
