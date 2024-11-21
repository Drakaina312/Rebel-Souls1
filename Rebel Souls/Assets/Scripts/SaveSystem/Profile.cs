using System;
using System.Collections.Generic;

[Serializable]
public class Profile
{
    public string ProfileName;
    public int ProfileID;
    public bool IsBlocked;
    public bool IsEmpty = true;
    public StatsBook[] SavesBooks;
    public Profile(string profileName, bool isBlocked)
    {
        ProfileName = profileName;
        IsBlocked = isBlocked;
    }

    public void SaveBooks(StatsBook statsBook)
    {
        if (SavesBooks != null)
        {
            StatsBook[] newmassiveacts = new StatsBook[SavesBooks.Length + 1];
            for (int i = 0; i < SavesBooks.Length; i++)
            {
                newmassiveacts[i] = SavesBooks[i];
            }
            newmassiveacts[newmassiveacts.Length - 1] = statsBook;
            SavesBooks = newmassiveacts;
        }
        else
        {
            SavesBooks = new StatsBook[1] { statsBook };
        }

    }
}
