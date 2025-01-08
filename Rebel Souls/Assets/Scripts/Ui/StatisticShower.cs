using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class StatisticShower : MonoBehaviour
{
    [SerializeField]
    private List<TextMeshProUGUI> _statsName;
    [SerializeField]
    private List<TextMeshProUGUI> _statsCount;
    private MasterSave _masterSave;
    private InGameDataBase _inGameDataBase;
    [SerializeField]
    private List<Image> _statsImage;
    [SerializeField] private Image _bGGStatistic;
    [SerializeField] private StatsDroper _statsDroper;


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
            Debug.Log(" ��� ����� ");
            return;
        }
        int index = 0;
        for (int i = 0; i < statsBook.Statistics.Length; i++)
        {
            if (statsBook.Statistics[i].IsRelationship)
                continue;

            Debug.Log(_inGameDataBase.ActStatistics);
            _statsName[i].transform.parent.gameObject.SetActive(true);
            _statsCount[i].transform.parent.gameObject.SetActive(true);
            _statsName[i].text = statsBook.Statistics[i].StatisticName;
            _statsCount[i].text = statsBook.Statistics[i].StatisticCount.ToString();
            Debug.Log("�������� �� = " + statsBook.Statistics[i].StatisticName);
            int currentINdex = i;
            statsBook.Statistics[currentINdex].OnValueChange += x =>
            {
                Debug.Log("Index = " + currentINdex);
                _statsCount[currentINdex].text = statsBook.Statistics[currentINdex].StatisticCount.ToString();
            };
            _statsImage[i].sprite = _inGameDataBase.ActStatistics.ActStats[i].StatisticSprite;
            Debug.Log(index + " ���");
            index++;
            Debug.Log("Index = " + i);

        }
        for (int i = index; i < _statsName.Count; i++)
        {
            _statsName[i].transform.parent.gameObject.SetActive(false);
            _statsCount[i].transform.parent.gameObject.SetActive(false);
            Debug.Log(i + "����");
        }
    }

    public List<StatisticInfo> GetOpenedStats()
    {
        StatsBook statsBook = _masterSave.CurrentProfile.BooksStat
            .FirstOrDefault(predict => predict.IsLastSave == true && predict.ChapterSortingConditions.BookName == _inGameDataBase.BookName);
        
        List<StatisticInfo> stats = new List<StatisticInfo>();
        foreach (var item in statsBook.Statistics)
        {
            if (!item.IsRelationship)
                stats.Add(item);
        }
        return stats;
    }

    public void OpenStatisticPanel()
    {
        transform.DOScale(1, 0.1f);
        ShowStats();

    }
    public void CloseStatisticPanel()
    {
        transform.DOScale(0, 0.1f);

        StatsBook statsBook = _masterSave.CurrentProfile.BooksStat
            .FirstOrDefault(predict => predict.IsLastSave == true && predict.ChapterSortingConditions.BookName == _inGameDataBase.BookName);

        foreach (var item in statsBook.Statistics)
            item.RemoveAllListners();

        _statsDroper.DiactivateStatsChange();

    }


}
