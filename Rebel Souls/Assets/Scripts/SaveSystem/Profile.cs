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


    public StatsBook FindChapterStats(ChapterSortingConditions chapterSortingConditions)
    {
        return BooksStat.FirstOrDefault(predict =>
        predict.ChapterSortingConditions.BookName == chapterSortingConditions.BookName &&
        predict.ChapterSortingConditions.ActName == chapterSortingConditions.ActName &&
        predict.ChapterSortingConditions.ChapterName == chapterSortingConditions.ChapterName);
    }
    public void SaveStats(ActStatistics actStatistics, ChapterSortingConditions chapterSortingCondition)
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

        StatsBook chapterStats = FindChapterStats(chapterSortingCondition);
        if (chapterStats != null)
        {
            chapterStats.AddNewStatistic(actStatistics.ActStats.ToArray());
            Debug.Log(chapterStats.Statistics[0].StatisticName);
        }
        else
        {
            StatsBook[] newStatistics = new StatsBook[BooksStat.Length + 1];
            newStatistics[newStatistics.Length - 1] = new StatsBook();
            newStatistics[newStatistics.Length - 1].Statistics = actStatistics.ActStats.ToArray();
            newStatistics[newStatistics.Length - 1].ChapterSortingConditions = chapterSortingCondition;

            for (int i = 0; i < BooksStat.Length; i++)
            {
                newStatistics[i] = BooksStat[i];
            }
            BooksStat = newStatistics;
        }

    }
}
