using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class StatisticShower : MonoBehaviour
{
    [SerializeField] private bool CanDropStat;



    [SerializeField] private Transform _parent;
    [SerializeField] private StatsField _statPattern;
    [SerializeField, ShowIf(nameof(CanDropStat))] private StatsLabel _statLabelPattern;

    private MasterSave _masterSave;
    private InGameDataBase _inGameDataBase;


    private List<Transform> _trash = new List<Transform>();

    [SerializeField, ShowIf(nameof(CanDropStat))] private Image _bGGStatistic;
    [SerializeField, ShowIf(nameof(CanDropStat))] private StatsDroper _statsDroper;


    [Inject]
    private void Construct(MasterSave masterSave, InGameDataBase inGameDataBase)
    {
        _masterSave = masterSave;
        _inGameDataBase = inGameDataBase;
    }
    public void ShowStats()
    {
        if (CanDropStat)
            _bGGStatistic.sprite = _inGameDataBase.ActStatistics.BGStatistic;

        StatsBook statsBook = _masterSave.CurrentProfile.BooksStat
            .FirstOrDefault(predict => predict.IsLastSave == true && predict.ChapterSortingConditions.BookName == _inGameDataBase.BookName);

        if (statsBook == null)
        {
            Debug.Log(" нет статы ");
            return;
        }

        foreach (var savedStatPair in statsBook.SavedStats)
        {
            if (savedStatPair.Key == "Фавориты")
            {
                continue;
            }

            if (!CanDropStat)
            {
                foreach (var item in savedStatPair.Value)
                {
                    StatsField stat = Instantiate(_statPattern, _parent);
                    _trash.Add(stat.transform);
                    stat.gameObject.SetActive(true);
                    stat.StatName.text = item.StatisticName;
                    stat.StatCount.text = item.StatisticCount.ToString();
                    stat.StatImage.sprite = Resources.Load<Sprite>(item.StatisticSprite);
                }
                continue;
            }


            Debug.Log("Раздел = " + savedStatPair.Key);
            StatsLabel statsLabel = Instantiate(_statLabelPattern, _parent);
            statsLabel.StatName.text = savedStatPair.Key;
            _trash.Add(statsLabel.transform);

            statsLabel.gameObject.SetActive(true);
            _statsDroper.StatsDropingOn += () => statsLabel.LabelTotalPoints.transform.parent.gameObject.SetActive(true);
            _statsDroper.StatsDropingOff += () => statsLabel.LabelTotalPoints.transform.parent.gameObject.SetActive(false);


            foreach (var item in savedStatPair.Value)
            {
                StatsField stat = Instantiate(_statPattern, _parent);
                stat.gameObject.SetActive(true);
                _trash.Add(stat.transform);

                stat.StatName.text = item.StatisticName;
                stat.StatCount.text = item.StatisticCount.ToString();
                stat.StatImage.sprite = Resources.Load<Sprite>(item.StatisticSprite);
                item.OnValueChange += x => stat.StatCount.text = x.ToString();

                _statsDroper.StatsDropingOn += () => stat.StatAddingButtons.Minus.gameObject.SetActive(true);
                _statsDroper.StatsDropingOn += () => stat.StatAddingButtons.Plus.gameObject.SetActive(true);
                _statsDroper.StatsDropingOn += () =>
                {
                    int totalPoints = Int32.Parse(statsLabel.LabelTotalPoints.text);
                    totalPoints += item.StatisticCount;
                    _statsDroper.TotalPoints += item.StatisticCount;
                    item.StatisticCount = 0;
                    item.InvokeValueChange(item.StatisticCount);
                    statsLabel.LabelTotalPoints.text = totalPoints.ToString();
                };

                _statsDroper.StatsDropingOff += () => stat.StatAddingButtons.Minus.gameObject.SetActive(false);
                _statsDroper.StatsDropingOff += () => stat.StatAddingButtons.Plus.gameObject.SetActive(false);

                stat.StatAddingButtons.Plus.onClick.AddListener(() =>
                {

                    int totalPoints = Int32.Parse(statsLabel.LabelTotalPoints.text);

                    if (totalPoints >= 1)
                        totalPoints--;
                    else
                        return;

                    _statsDroper.TotalPoints--;
                    Debug.Log(_statsDroper.TotalPoints);
                    item.StatisticCount++;
                    item.InvokeValueChange(item.StatisticCount);

                    statsLabel.LabelTotalPoints.text = totalPoints.ToString();
                });

                stat.StatAddingButtons.Minus.onClick.AddListener(() =>
                {
                    int totalPoints = Int32.Parse(statsLabel.LabelTotalPoints.text);


                    if (item.StatisticCount >= 1)
                        item.StatisticCount--;
                    else
                        return;

                    _statsDroper.TotalPoints++;
                    Debug.Log(_statsDroper.TotalPoints);
                    item.InvokeValueChange(item.StatisticCount);
                    totalPoints++;
                    statsLabel.LabelTotalPoints.text = totalPoints.ToString();
                });
            }
        }
    }


    public void OpenStatisticPanel()
    {

        if (_masterSave.CurrentProfile.DifficultyType == DifficultyType.Hard)
            return;

        if (CanDropStat)
            transform.DOScale(1, 0.1f);
        ShowStats();

    }
    public void CloseStatisticPanel()
    {
        if (CanDropStat && _statsDroper.TotalPoints != 0)
            return;

        if (CanDropStat)
            transform.DOScale(0, 0.1f);

        StatsBook statsBook = _masterSave.CurrentProfile.BooksStat
            .FirstOrDefault(predict => predict.IsLastSave == true && predict.ChapterSortingConditions.BookName == _inGameDataBase.BookName);

        if (CanDropStat)
            _statsDroper.CompleteStatChange();

        foreach (var savedStatPair in statsBook.SavedStats)
        {
            foreach (var item in savedStatPair.Value)
            {
                item.RemoveAllListners();
            }
        }

        foreach (var item in _trash)
        {
            Destroy(item.gameObject);
        }
        _trash.Clear();
    }


}
