using System.IO;
using UnityEngine;
using Zenject;

public class ClotherPanel : MonoBehaviour
{
    [SerializeField] private Canvas _mainCanvas;
    [SerializeField] private SpriteRenderer _heroOnScene;
    [SerializeField] private PhotoCamera _photoCamera;
    private MasterSave _masterSave;
    private InGameDataBase _inGameDataBase;
    private StatsBook _statsBook;
    private StatisticInfo _heroData;

    [Inject]
    private void Construct(MasterSave masterSave, InGameDataBase inGameDataBase)
    {
        _masterSave = masterSave;
        _inGameDataBase = inGameDataBase;

        _statsBook = _masterSave.CurrentProfile
            .FindChapterStatsFromSave(_inGameDataBase.StoryLine.ChapterSortingCondition);
    }



    public void ActivateHeroClotherChange(string heroName, bool isMainHero = false)
    {
        _mainCanvas.enabled = false;

        Debug.Log("Activation");

        if (isMainHero)
            _heroOnScene.sprite = Resources.Load<Sprite>(_statsBook.MainHeroSpritePath);
        else
        {

            var hero = _statsBook.FindStat(heroName);
            _heroData = hero;
            var path = hero.PathToFavoriteScin;
            if (path == null)
            {
                path = hero.StatisticSprite;
                return;
            }

            Debug.Log($"hero = {hero.StatisticName}, path = {path}");
            Sprite sprite = Resources.Load<Sprite>(hero.StatisticName);
            if (sprite == null)
                _heroOnScene.sprite = Resources.Load<Sprite>(hero.StatisticSprite);
            else
                _heroOnScene.sprite = sprite;
            Debug.Log("Activation");
        }

    }


    public void ChooseClother()
    {
        var scale = _heroOnScene.transform.localScale;
        _heroOnScene.transform.localScale = Vector3.one * 0.6f;
        string dataPath = Path.Combine(Application.dataPath, "Resources");
        dataPath = Path.Combine(dataPath, _heroData.StatisticName + ".png");
        if (File.Exists(dataPath))
        {
            File.Delete(dataPath);
            _inGameDataBase.Sprite = _photoCamera.CaptureAndSave(dataPath);
            _heroData.PathToFavoriteScin = dataPath;
        }
        else
        {
            _inGameDataBase.Sprite = _photoCamera.CaptureAndSave(dataPath);
            _heroData.PathToFavoriteScin = dataPath;
        }
        _heroOnScene.transform.localScale = scale;

        DeactivateHeroClotherChange();
    }





    public void DeactivateHeroClotherChange()
    {
        _mainCanvas.enabled = true;
    }
}
