using System;
using UnityEngine;

[Serializable]
public class Profile
{
    public string ProfileName;
    public int ProfileID;
    public bool IsBlocked;
    public bool IsEmpty = true;
    public Profile(string profileName,bool isBlocked)
    {
        ProfileName = profileName;
        IsBlocked = isBlocked;
    }

}
