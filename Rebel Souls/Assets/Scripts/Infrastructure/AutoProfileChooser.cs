using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class AutoProfileChooser : MonoBehaviour
{
    private InGameDataBase _ingameDataBase;
    private MasterSave _masterSave;
    [SerializeField] private GameObject _profileChooseCanvas;

    [Inject]
    private void Construct(InGameDataBase inGameDataBase, MasterSave masterSave)
    {
        _ingameDataBase = inGameDataBase;
        _masterSave = masterSave;
    }


    private void Start()
    {
        if (_masterSave.CurrentProfile != null)
        {
            _profileChooseCanvas.SetActive(false);
            _masterSave.OnProfileChoosed?.Invoke();
        }
    }
}
