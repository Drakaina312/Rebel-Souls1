using System;
using System.IO;
using System.Linq;
using UnityEngine;

public class MasterSave
   
{
    private string _savePuff = Application.dataPath + "/MySaves/playerData.json";
    public Profile CurrentProfile;
    public bool IsDataLoadComnplete;

    public SaveData SaveData { get; private set; } = new SaveData();    
    public void ChooseCurrentProfile(Profile profile)
    {  
            CurrentProfile = profile;
    }

    public void CreatNewProfile(Profile profile)
    {
        if (SaveData.IfCanAddNewProfile(profile))
        {
            CurrentProfile = SaveData.FindProfile(profile);
        }
        else
        {
            Debug.LogError(" невозможно создть профиль ");
        }

        SaveAllData();
    }

    public void SaveAllData()
    {
        string JsonString = JsonUtility.ToJson(SaveData);
        File.WriteAllText(_savePuff, JsonString);
    }
    public void LoadAllData()
    {
        if (File.Exists(_savePuff))
        {
            string jsonString = File.ReadAllText(_savePuff);
            SaveData = JsonUtility.FromJson<SaveData>(jsonString);
            IsDataLoadComnplete = true;
        }
    }

}

[Serializable]
public class SaveData
{
    public string Test;
    public Profile[] Profiles = new Profile[6]
    {
        new Profile("", false),
        new Profile("", false),
        new Profile("", true),
        new Profile("", true),
        new Profile("", true),
        new Profile("", true),
        
    };
  
    public void AddDataToSave(string textToSave)
    { 
        Test = textToSave;

    }

    public Profile FindProfile(Profile profile)
    {
        return Profiles.FirstOrDefault(profilee => profilee.ProfileName == profile.ProfileName);
    }

    public bool IfCanAddNewProfile(Profile profile)
    {
       Profile profileToFind = Profiles.FirstOrDefault(profilee => profilee.IsBlocked == false && profilee.IsEmpty == true);
        if (profileToFind != null)
        {
            profileToFind.ProfileName = profile.ProfileName;
            profileToFind.IsBlocked = false;
            profileToFind.IsEmpty = false;
            return true; 
        }
        else return false;
    }
}
