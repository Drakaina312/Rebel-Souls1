using TMPro;
using UnityEngine;
using Zenject;

public class ProfileButton : MonoBehaviour
{
    public TextMeshProUGUI ProfileName;
    public bool IsBlocked;
    public bool IsEmpty;
    public Profile ProfileToChoose;
    private MasterSave _masterSave;
    [SerializeField] private GameObject _profileChooseCanvas;
    [Inject]
    private void Construct(MasterSave masterSave) 
    { 
        _masterSave = masterSave;
    }

    public void ChooseProfile()
    {
        _masterSave.CurrentProfile = ProfileToChoose;
        Debug.Log(ProfileToChoose);
        if (ProfileToChoose.ProfileName != "")
        { 
        _profileChooseCanvas.SetActive(false);
        Debug.Log(" выбран профиль " + ProfileToChoose.ProfileName);
        
        }


    
    }
}
