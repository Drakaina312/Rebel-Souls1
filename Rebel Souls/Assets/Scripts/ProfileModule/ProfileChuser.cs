using System;
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
    }
    public void AddNewProfile(string profileName)
    {
        Debug.Log(profileName);
        ProfileButton profileButton = _profileButtonHendler.ProfileButtonsList.FirstOrDefault(button => button.IsBlocked != true && button.IsEmpty == true);
        Debug.Log(profileButton);

        if (profileButton != null)
        {
            profileButton.ProfileName.text = profileName;
            profileButton.IsEmpty = false;
            Profile newProfile = new Profile(profileName, false);
            newProfile.IsEmpty = false;
            _masterSave.ChooseCurrentProfile(newProfile, true);
           
        }

    }
}
