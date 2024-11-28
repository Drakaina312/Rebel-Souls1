using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Zenject;

public class StatisticShower : MonoBehaviour
{
    [SerializeField]
    private List<TextMeshProUGUI> _statsName;
    [SerializeField]
    private List<TextMeshProUGUI> _statsCount;
    private MasterSave _masterSave;
    private InGameDataBase _inGameDataBase;

    [Inject]
    private void Construct(MasterSave masterSave, InGameDataBase inGameDataBase)
    {
        _masterSave = masterSave;
        _inGameDataBase = inGameDataBase;
    }
    public void ShowStats()
    {
        StatsBook statsBook = _masterSave.CurrentProfile.BooksStat
            .FirstOrDefault(predict => predict.IsLastSave == true && predict.ChapterSortingConditions.BookName == _inGameDataBase.BookName);

        if(statsBook == null)
            return;

        int index = 0;
        for (int i = 0; i < statsBook.Statistics.Length; i++)
        {
            _statsName[i].transform.parent.gameObject.SetActive(true);
            _statsCount[i].transform.parent.gameObject.SetActive(true);
            _statsName[i].text = statsBook.Statistics[i].StatisticName;
            _statsCount[i].text = statsBook.Statistics[i].StatisticCount.ToString();
            Debug.Log(index + " Вкл");
            index++;
        }
        for (int i = index;i < _statsName.Count; i++)
        {
            _statsName[i].transform.parent.gameObject.SetActive(false);
            _statsCount[i].transform.parent.gameObject.SetActive(false);
            Debug.Log(i + "Выкл");
        }
    }
    public void OpenStatisticPanel()
    {
        transform.DOScale(1, 0.1f);
        ShowStats();

    }
    public void CloseStatisticPanel()
    {
        transform.DOScale(0, 0.1f);
    }


}
