using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class GameContinuer : MonoBehaviour
{
    private MasterSave _masterSave;
    private InGameDataBase _inGameDataBase;

    [Inject]
    private void Construct(MasterSave masterSave, InGameDataBase inGameDataBase)
    {
        _masterSave = masterSave;
        _inGameDataBase = inGameDataBase;
    }

    public void ContinueGame()
    {
        _inGameDataBase.DIalogSequenceStart = Resources.Load<DialogSequence>(_masterSave.CurrentProfile.LastSaveChapterPath);
        _inGameDataBase.DialogIndex = _masterSave.CurrentProfile.DialogIndex;
        SceneManager.LoadScene(1);
    }
}
