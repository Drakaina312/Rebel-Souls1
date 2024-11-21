using System;
using System.Collections.Generic;

[Serializable]
public class Profile
{
    public string ProfileName;
    public int ProfileID;
    public bool IsBlocked;
    public bool IsEmpty = true;
    public SavesActEdge[] MassiveActs;
    public StatisticInfo[] Statistics;
    public Profile(string profileName, bool isBlocked)
    {
        ProfileName = profileName;
        IsBlocked = isBlocked;
    }
    public void AddNewAct(SavesActEdge act)
    {
        if (MassiveActs != null)
        {
            SavesActEdge[] newMassiveActs = new SavesActEdge[MassiveActs.Length + 1];
            for (int i = 0; i < MassiveActs.Length; i++)
            {
                newMassiveActs[i] = MassiveActs[i];
            }
            newMassiveActs[newMassiveActs.Length - 1] = act;
            MassiveActs = newMassiveActs;
        }
        else
        {
            MassiveActs = new SavesActEdge[1] { act };
        }
    }

    internal void AddNewStatistic(StatisticInfo massiveStatistics)
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
}
