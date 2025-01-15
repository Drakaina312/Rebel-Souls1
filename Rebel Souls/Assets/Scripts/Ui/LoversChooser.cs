using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class LoversChooser : MonoBehaviour
{
    [SerializeField] private List<LoverUiComponnents> _loversComponnetns;
    [SerializeField] private LoverInfoHolder _loverInfoholder;
    private InGameDataBase _inGameDataBase;
    private MasterSave _masterSave;
    [SerializeField] private Image _bGLovers;



    [Inject]
    private void Construct(InGameDataBase gameData, MasterSave masterSave)
    {
        _inGameDataBase = gameData;
        _masterSave = masterSave;
    }

    public void ShowLovers()
    {
        if (_masterSave.CurrentProfile.DifficultyType == DifficultyType.Hard)
        {
            return;
        }
        gameObject.SetActive(true);
        _bGLovers.sprite = _inGameDataBase.ActStatistics.BGLovers;
        int index = 0;
        foreach (var item in _inGameDataBase.ActStatistics.ActLovers)
        {
            _loversComponnetns[index].Image.gameObject.SetActive(true);
            _loversComponnetns[index].Image.sprite = item.LoverViewSprite;
            _loversComponnetns[index].Button.onClick.RemoveAllListeners();

            StatsBook statsBook = _masterSave.CurrentProfile.BooksStat.FirstOrDefault(prediction => prediction.IsLastSave && prediction.ChapterSortingConditions.BookName == _inGameDataBase.BookName);
            Debug.Log(statsBook);
            StatisticInfo loverStatistic = statsBook.FindStat(item.LoverName);
            Debug.Log(loverStatistic.StatisticName);
            _loversComponnetns[index].Button.onClick.AddListener(() => ShowLoverInfo(item.LoverViewSprite, item.LoverProfile, loverStatistic.StatisticCount, item.RelationShipsInfo));

            index++;
        }
        for (int i = index; i < _loversComponnetns.Count; i++)
        {
            _loversComponnetns[i].Button.gameObject.SetActive(false);

        }
    }

    private void ShowLoverInfo(Sprite loverViewSprite, Sprite loverProfile, int statisticCount, List<RelationShipData> relationShipDatas)
    {


        _loverInfoholder.transform.DOScale(1, 0.5f);
        _loverInfoholder.ChangeLoverPanel(loverViewSprite, loverProfile, statisticCount, relationShipDatas);

    }
}


[Serializable]
public struct LoverUiComponnents
{
    public Image Image;
    public Button Button;
}


