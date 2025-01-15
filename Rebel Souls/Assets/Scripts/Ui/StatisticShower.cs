using DG.Tweening;
using Mono.Cecil.Cil;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class StatisticShower : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _firstLabel;
    [SerializeField]
    private TextMeshProUGUI _secondLabel;

    [SerializeField]
    private List<TextMeshProUGUI> _statsNameFirstLabel;
    [SerializeField]
    private List<TextMeshProUGUI> _statsCountFirstLabel;
    [SerializeField]
    private List<Image> _statsImageFirstLabel;

    [SerializeField]
    private List<TextMeshProUGUI> _statsNameSecondLabel;
    [SerializeField]
    private List<TextMeshProUGUI> _statsCountSecondLabel;
    [SerializeField]
    private List<Image> _statsImageSecondLabel;

    private MasterSave _masterSave;
    private InGameDataBase _inGameDataBase;


    [SerializeField] private Image _bGGStatistic;
    [SerializeField] private StatsDroper _statsDroper;


    public List<string> GetLabels()
    {
        var labels = new List<string>();
        labels.Add(_firstLabel.text);
        labels.Add(_secondLabel.text);

        return labels;
    }

    [Inject]
    private void Construct(MasterSave masterSave, InGameDataBase inGameDataBase)
    {
        _masterSave = masterSave;
        _inGameDataBase = inGameDataBase;
    }
    public void ShowStats()
    {
        _bGGStatistic.sprite = _inGameDataBase.ActStatistics.BGStatistic;
        StatsBook statsBook = _masterSave.CurrentProfile.BooksStat
            .FirstOrDefault(predict => predict.IsLastSave == true && predict.ChapterSortingConditions.BookName == _inGameDataBase.BookName);

        if (statsBook == null)
        {
            Debug.Log(" нет статы ");
            return;
        }

        int pair = 0;
        foreach (var savedStatPair in statsBook.SavedStats)
        {
            if (savedStatPair.Key == "Фавориты")
            {
                continue;
            }
            ActivateStatsTexts(savedStatPair, pair);
            pair++;
        }


        //for (int i = 0; i < statsBook.Statistics.Length; i++)
        //{
        //    if (statsBook.Statistics[i].IsRelationship)
        //        continue;

        //    Debug.Log(_inGameDataBase.ActStatistics);
        //    _statsNameFirstLabel[i].transform.parent.gameObject.SetActive(true);
        //    _statsCount[i].transform.parent.gameObject.SetActive(true);
        //    _statsNameFirstLabel[i].text = statsBook.Statistics[i].StatisticName;
        //    _statsCount[i].text = statsBook.Statistics[i].StatisticCount.ToString();
        //    Debug.Log("Подписка на = " + statsBook.Statistics[i].StatisticName);
        //    int currentINdex = i;
        //    statsBook.Statistics[currentINdex].OnValueChange += x =>
        //    {
        //        Debug.Log("Index = " + currentINdex);
        //        _statsCount[currentINdex].text = statsBook.Statistics[currentINdex].StatisticCount.ToString();
        //    };
        //    _statsImage[i].sprite = _inGameDataBase.ActStatistics.ActStats[i].StatisticSprite;
        //    index++;

        //}
        //for (int i = index; i < _statsNameFirstLabel.Count; i++)
        //{
        //    _statsNameFirstLabel[i].transform.parent.gameObject.SetActive(false);
        //    _statsCount[i].transform.parent.gameObject.SetActive(false);
        //}
    }

    private void ActivateStatsTexts(KeyValuePair<string, StatisticInfo[]> savedStatPair, int pair)
    {

        int ind = 0;
        if (pair == 0)
        {
            _firstLabel.text = savedStatPair.Key;

            foreach (var stat in savedStatPair.Value)
            {
                _statsNameFirstLabel[ind].text = stat.StatisticName;
                //Sprite imageForStat = _inGameDataBase.ActStatistics.Stats[savedStatPair.Key].FirstOrDefault(x => x.StatisticName == stat.StatisticName).StatisticSprite;
                Sprite imageForStat = Resources.Load<Sprite>(stat.StatisticSprite);
                _statsImageFirstLabel[ind].sprite = imageForStat;
                _statsCountFirstLabel[ind].text = stat.StatisticCount.ToString();
                int index = ind;
                stat.OnValueChange += x =>
                {
                    Debug.Log("ff" + index);
                    _statsCountFirstLabel[index].text = x.ToString();
                };
                ind++;
                if (ind == 2)
                {
                    Debug.Log("Exit = " + ind);
                    break;
                }
            }
            for (int i = ind; i < _statsNameFirstLabel.Count; i++)
            {
                _statsNameFirstLabel[i].gameObject.SetActive(false);
                _statsImageFirstLabel[i].gameObject.SetActive(false);
                _statsCountFirstLabel[i].gameObject.SetActive(false);
            }

        }
        if (pair == 1)
        {
            Debug.Log("Вторая пара");
            _secondLabel.text = savedStatPair.Key;

            foreach (var stat in savedStatPair.Value)
            {
                _statsNameSecondLabel[ind].text = stat.StatisticName;
                Sprite imageForStat = Resources.Load<Sprite>(stat.StatisticSprite);
                _statsImageSecondLabel[ind].sprite = imageForStat;
                _statsCountSecondLabel[ind].text = stat.StatisticCount.ToString();
                int index = ind;
                stat.OnValueChange += x =>
                {
                    _statsCountSecondLabel[index].text = x.ToString();
                };
                ind++;
                if (ind == 2)
                    break;
            }
            for (int i = ind; i < _statsNameFirstLabel.Count; i++)
            {
                _statsNameSecondLabel[i].gameObject.SetActive(false);
                _statsImageSecondLabel[i].gameObject.SetActive(false);
                _statsCountSecondLabel[i].gameObject.SetActive(false);
            }
        }
    }

    //public List<StatisticInfo> GetOpenedStats()
    //{
    //    StatsBook statsBook = _masterSave.CurrentProfile.BooksStat
    //        .FirstOrDefault(predict => predict.IsLastSave == true && predict.ChapterSortingConditions.BookName == _inGameDataBase.BookName);

    //    List<StatisticInfo> stats = new List<StatisticInfo>();
    //    foreach (var item in statsBook.Statistics)
    //    {
    //        if (!item.IsRelationship)
    //            stats.Add(item);
    //    }
    //    return stats;
    //}

    public void OpenStatisticPanel()
    {
        if (_masterSave.CurrentProfile.DifficultyType == DifficultyType.Hard)
            return;

        transform.DOScale(1, 0.1f);
        ShowStats();

    }
    public void CloseStatisticPanel()
    {
        transform.DOScale(0, 0.1f);

        StatsBook statsBook = _masterSave.CurrentProfile.BooksStat
            .FirstOrDefault(predict => predict.IsLastSave == true && predict.ChapterSortingConditions.BookName == _inGameDataBase.BookName);


        _statsDroper.DiactivateStatsChange();

    }


}
