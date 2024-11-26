using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class ProfileChuser : MonoBehaviour
{
    [SerializeField] private ProfileButtonHendler _profileButtonHendler;
    private MasterSave _masterSave;
    [Inject]
    public void Conctruct(MasterSave masterSave) 
    {
        _masterSave = masterSave;
        StartCoroutine(LoadDataCourutine());
    }
    public void AddNewProfile(string profileName)
    {
        ProfileButton profileButton = _profileButtonHendler.ProfileButtonsList.FirstOrDefault(button => button.IsBlocked != true && button.IsEmpty == true);

        if (profileName == "")
        { 
            Debug.Log(" Имя пустое");
            return; 
        }
        if (profileButton != null)
        {
            profileButton.ProfileName.text = profileName;
            profileButton.IsEmpty = false;
            Profile newProfile = new Profile(profileName, false);
            newProfile.IsEmpty = false;
            _masterSave.CreatNewProfile(newProfile);
            profileButton.ProfileToChoose = _masterSave.CurrentProfile;

        }
        else 
        {
            Debug.LogError(" Не удалось создать профиль ");
        }
    }

    private IEnumerator LoadDataCourutine()
    {
        yield return new WaitWhile(() => _masterSave.IsDataLoadComnplete == false);
        int i = 0;
        foreach (var profileButton in _profileButtonHendler.ProfileButtonsList)
        {
            if (profileButton.IsBlocked == false && _masterSave.SaveData.Profiles[i].IsEmpty == false)

            { 
                profileButton.ProfileName.text = _masterSave.SaveData.Profiles[i].ProfileName;
                profileButton.ProfileToChoose = _masterSave.SaveData.Profiles[i];
                profileButton.IsEmpty = false;
                i ++;
            }
        }

    }
}
