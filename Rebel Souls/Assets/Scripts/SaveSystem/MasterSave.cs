using System;
using System.IO;
using System.Linq;
using UnityEngine;

public class MasterSave
   
{
    private string _savePuff = Application.dataPath + "/MySaves/playerData.json";
    public Profile CurrentProfile;
    public SaveData SaveData { get; private set; }
    public void ChooseCurrentProfile(Profile profile, bool isNewProfileCreation = false) 
    {  
            CurrentProfile = profile;
        if (isNewProfileCreation == true) 
        {
            SaveAllData(isNewProfileCreation);
        }
    }

    public void SaveAllData()
    {
        SaveData saveData = new SaveData(CurrentProfile);
        saveData.AddDataToSave("Пупка");
        Debug.Log(saveData.Test);
        string JsonString = JsonUtility.ToJson(saveData);
        File.WriteAllText(_savePuff, JsonString);
        Debug.Log("сохранение успешно" + JsonString);
    }
    public void SaveAllData(bool isNewProfileCreation)
    { 
        SaveData saveData = new SaveData(CurrentProfile, isNewProfileCreation);
        saveData.AddDataToSave("Пупка");
        Debug.Log(saveData.Profiles[0].ProfileName);
        string JsonString = JsonUtility.ToJson(saveData);
        File.WriteAllText(_savePuff, JsonString);
        Debug.Log("сохранение успешно" + JsonString);
    }

    public void LoadAllData()
    {
        if (File.Exists(_savePuff))
        {
            string jsonString = File.ReadAllText(_savePuff);
            SaveData = JsonUtility.FromJson<SaveData>(jsonString);
            Debug.Log(SaveData.Test + " загрузка данных ");
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
        new Profile("", true),
        new Profile("", true),
        new Profile("", true),
        new Profile("", true),
        new Profile("", true),
        
    };
    public SaveData(Profile profileToSave, bool isNewProfileCreation = false)
    {
       
        if (isNewProfileCreation)
        {
            
            //Profile profile = Profiles.FirstOrDefault(profile=> profile.IsBlocked == false && profile.IsEmpty == true);
            Profile profile = Profiles[0];
            if (profile != null)
            {
             profile.ProfileName = profileToSave.ProfileName;    
            }
        }
        foreach (Profile profile in Profiles)
        { 
            Debug.Log(profile.ProfileName);
        }
    }
    public void AddDataToSave(string textToSave)
    { 
        Test = textToSave;

    }
       
}
