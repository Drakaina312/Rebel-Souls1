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
        _inGameDataBase.StoryLine = Resources.Load<StoryLine>(_masterSave.CurrentProfile.LastSaveChapterPath);
        _inGameDataBase.SlideIndex = _masterSave.CurrentProfile.LastSaveSlideIndex;
        _inGameDataBase.IsContiniueStory = true;
        _inGameDataBase.IsRestartChapter = false;
        SceneManager.LoadScene(1);
    }
}
