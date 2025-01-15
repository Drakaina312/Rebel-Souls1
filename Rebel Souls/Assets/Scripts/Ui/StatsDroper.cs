using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class StatsDroper : MonoBehaviour
{
    [SerializeField] private StatisticShower _statShower;
    [SerializeField] private List<StatsAddingButtons> _statsChangingButtonsFirstLabel;
    [SerializeField] private List<StatsAddingButtons> _statsChangingButtonsSecondLabel;
    [SerializeField] private int _totalCountFirstLabel;
    [SerializeField] private int _totalCountSecondLabel;
    [SerializeField] private TextMeshProUGUI _totalFirstLabel;
    [SerializeField] private TextMeshProUGUI _totalSecondLabel;
    [SerializeField] private GameObject _completeBtn;


    private string _firstLabelKey;
    private string _secondLabelKey;

    private MasterSave _masterSave;
    private InGameDataBase _inGameData;

    [Inject]
    private void Construct(MasterSave masterSave, InGameDataBase inGameDataBase)
    {
        _masterSave = masterSave;
        _inGameData = inGameDataBase;
    }

    [Button]
    public void DropAllStats()
    {
        _completeBtn.gameObject.SetActive(true);
        _totalFirstLabel.transform.parent.gameObject.SetActive(true);
        _totalSecondLabel.transform.parent.gameObject.SetActive(true);
        var lastSave = _masterSave.CurrentProfile.BooksStat.FirstOrDefault(x => x.IsLastSave == true);
        if (lastSave == null)
        {
            Debug.Log("Нет последнего сохранения");
            return;
        }
        int i = 0;
        foreach (var item in _statShower.GetLabels())
        {
            if (i == 0 && item != "Фавориты")
            {
                ChangeFirstLabel(lastSave, item);
                i++;
            }
            else if (i == 1 && item != "Фавориты")
            {
                ChangeSecondLabel(lastSave, item);
                i++;
            }
        }
    }

    private void ChangeFirstLabel(StatsBook lastSave, string item)
    {
        _firstLabelKey = item;
        int btnI = 0;
        foreach (var stats in lastSave.SavedStats[_firstLabelKey])
        {
            _totalCountFirstLabel += stats.StatisticCount;
            stats.StatisticCount = 0;
            stats.ChangeValue(0);
            _totalFirstLabel.text = _totalCountFirstLabel.ToString();

            _statsChangingButtonsFirstLabel[btnI].Plus.gameObject.SetActive(true);
            _statsChangingButtonsFirstLabel[btnI].Minus.gameObject.SetActive(true);
            _statsChangingButtonsFirstLabel[btnI].Plus.onClick.AddListener(() =>
            {

                if (_totalCountFirstLabel == 0)
                    return;

                stats.StatisticCount++;
                stats.ChangeValue(stats.StatisticCount);
                _totalCountFirstLabel--;
                _totalFirstLabel.text = _totalCountFirstLabel.ToString();
            });


            _statsChangingButtonsFirstLabel[btnI].Minus.onClick.AddListener(() =>
            {

                if (stats.StatisticCount == 0)
                    return;

                stats.StatisticCount--;
                stats.ChangeValue(stats.StatisticCount);
                _totalCountFirstLabel++;
                _totalFirstLabel.text = _totalCountFirstLabel.ToString();

            });

            btnI++;
            if (btnI == 2)
                return;
        }
    }
    private void ChangeSecondLabel(StatsBook lastSave, string item)
    {
        _secondLabelKey = item;
        int btnI = 0;
        foreach (var stats in lastSave.SavedStats[_secondLabelKey])
        {
            _totalCountSecondLabel += stats.StatisticCount;
            stats.StatisticCount = 0;
            stats.ChangeValue(0);
            _totalSecondLabel.text = _totalCountSecondLabel.ToString();

            _statsChangingButtonsSecondLabel[btnI].Plus.gameObject.SetActive(true);
            _statsChangingButtonsSecondLabel[btnI].Minus.gameObject.SetActive(true);
            _statsChangingButtonsSecondLabel[btnI].Plus.onClick.AddListener(() =>
            {

                if (_totalCountSecondLabel == 0)
                    return;

                stats.StatisticCount++;
                stats.ChangeValue(stats.StatisticCount);
                _totalCountSecondLabel--;
                _totalSecondLabel.text = _totalCountSecondLabel.ToString();
            });


            _statsChangingButtonsSecondLabel[btnI].Minus.onClick.AddListener(() =>
            {
                if (stats.StatisticCount == 0)
                    return;

                stats.StatisticCount--;
                stats.ChangeValue(stats.StatisticCount);
                _totalCountSecondLabel++;
                _totalSecondLabel.text = _totalCountSecondLabel.ToString();

            });

            btnI++;
            if (btnI == 2)
                return;
        }
    }

    public void CompleteStatChange()
    {
        DiactivateStatsChange();
        _masterSave.SaveAllData();
        _completeBtn.gameObject.SetActive(false);
    }

    public void DiactivateStatsChange()
    {
        foreach (var item in _statsChangingButtonsFirstLabel)
        {
            item.Minus.onClick.RemoveAllListeners();
            item.Plus.onClick.RemoveAllListeners();
            item.Minus.gameObject.SetActive(false);
            item.Plus.gameObject.SetActive(false);
        }
        foreach (var item in _statsChangingButtonsSecondLabel)
        {
            item.Minus.onClick.RemoveAllListeners();
            item.Plus.onClick.RemoveAllListeners();
            item.Minus.gameObject.SetActive(false);
            item.Plus.gameObject.SetActive(false);
        }
        _totalFirstLabel.transform.parent.gameObject.SetActive(false);
        _totalSecondLabel.transform.parent.gameObject.SetActive(false);
    }
}

[Serializable]
public struct StatsAddingButtons
{
    public Button Minus;
    public Button Plus;
}
