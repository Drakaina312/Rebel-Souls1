using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class SlideFavoriteShower : MonoBehaviour
{
    [SerializeField] private Transform _parent;
    [SerializeField] private StatsField _statPattern;
    [SerializeField] private bool _showForClother;
    [SerializeField] private ClotherPanel _clotherPanel;

    private MasterSave _masterSave;
    private InGameDataBase _inGameDataBase;

    private List<StatsField> _trash = new List<StatsField>();

    [Inject]
    private void Construct(MasterSave masterSave, InGameDataBase inGameDataBase)
    {
        _masterSave = masterSave;
        _inGameDataBase = inGameDataBase;
    }


    public void ShowFavorites()
    {
        StatsBook statsBook = _masterSave.CurrentProfile.FindChapterStatsFromSave(_inGameDataBase.StoryLine.ChapterSortingCondition);

        if (_showForClother)
        {
            StatsField favoritePattern = Instantiate(_statPattern, _parent);
            favoritePattern.gameObject.SetActive(true);
            _trash.Add(favoritePattern);
            favoritePattern.StatName.text = statsBook.MainHeroName;
            favoritePattern.StatCount.gameObject.SetActive(false);
            favoritePattern.StatImage.sprite = Resources.Load<Sprite>(statsBook.MainHeroSpritePath);
            favoritePattern.StatButton.onClick.AddListener( () => _clotherPanel.ActivateHeroClotherChange(favoritePattern.StatName.text,true));


            foreach (var item in statsBook.SavedStats["Фавориты"])
            {
                if (item.IsFavoriteAppeared)
                {
                    favoritePattern = Instantiate(_statPattern, _parent);
                    favoritePattern.gameObject.SetActive(true);
                    _trash.Add(favoritePattern);
                    favoritePattern.StatName.text = item.StatisticName;
                    favoritePattern.StatCount.gameObject.SetActive(false);
                    favoritePattern.StatImage.sprite = Resources.Load<Sprite>(item.StatisticSprite);
                    favoritePattern.StatButton.onClick.AddListener(() => _clotherPanel.ActivateHeroClotherChange(favoritePattern.StatName.text));
                }
            }
            return;
        }

        foreach (var item in statsBook.SavedStats["Фавориты"])
        {
            if (item.IsFavoriteAppeared)
            {
                StatsField favoritePattern = Instantiate(_statPattern, _parent);
                favoritePattern.gameObject.SetActive(true);
                _trash.Add(favoritePattern);
                favoritePattern.StatName.text = item.StatisticName;
                favoritePattern.StatCount.text = _inGameDataBase.ActStatistics.ActLovers
                    .FirstOrDefault(x => x.LoverName == item.StatisticName)
                    .GetRelatinShipName(item.StatisticCount);
                favoritePattern.StatImage.sprite = Resources.Load<Sprite>(item.StatisticSprite);
            }
        }
    }

    public void ClearTrah()
    {
        foreach (var item in _trash)
            Destroy(item.gameObject);

        _trash.Clear();
    }
}


