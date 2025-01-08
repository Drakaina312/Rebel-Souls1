using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class StatsDroper : MonoBehaviour
{
    [SerializeField] private StatisticShower _statShower;
    [SerializeField] private List<StatsAddingButtons> _statsAddingButtons;

    private MasterSave _masterSave;
    private InGameDataBase _inGameData;
    private int TotalStatsCount;

    [Inject]
    private void Construct(MasterSave masterSave, InGameDataBase inGameDataBase)
    {
        _masterSave = masterSave;
        _inGameData = inGameDataBase;
    }

    [Button]
    public void DropAllStats()
    {
        var lastSave = _masterSave.CurrentProfile.BooksStat.FirstOrDefault(x => x.IsLastSave == true);
        if (lastSave == null)
        {
            Debug.Log("Нет последнего сохранения");
            return;
        }

        foreach (var item in lastSave.Statistics)
        {
            if (!item.IsRelationship)
            {
                TotalStatsCount += item.StatisticCount;
                item.StatisticCount = 0;
                item.ChangeValue(0);
            }
        }
        ActivateStatsChanging();
        Debug.Log(TotalStatsCount);
    }

    public void DiactivateStatsChange()
    {
        foreach (var item in _statsAddingButtons)
        {
            item.Minus.onClick.RemoveAllListeners();
            item.Plus.onClick.RemoveAllListeners();
            item.Minus.gameObject.SetActive(false);
            item.Plus.gameObject.SetActive(false);
        }
    }

    public void ActivateStatsChanging()
    {

        int i = 0;
        foreach (var item in _statShower.GetOpenedStats())
        {
            Debug.Log("s1");
            _statsAddingButtons[i].Plus.gameObject.SetActive(true);
            _statsAddingButtons[i].Minus.gameObject.SetActive(true);
            _statsAddingButtons[i].Plus.onClick.AddListener(() =>
            {
                Debug.Log("Увеличение" + " Очков = " + TotalStatsCount);

                if (TotalStatsCount == 0)
                    return;

                item.StatisticCount++;
                item.ChangeValue(item.StatisticCount);
                TotalStatsCount--;
                Debug.Log(TotalStatsCount);

            });
            _statsAddingButtons[i].Minus.onClick.AddListener(() =>
            {
                Debug.Log("Уменьшение" + " Очков = " + TotalStatsCount);

                if (item.StatisticCount == 0)
                    return;

                item.StatisticCount--;
                item.ChangeValue(item.StatisticCount);
                TotalStatsCount++;
                Debug.Log(TotalStatsCount);

            });
            i++;
            if (i == 2)
                return;
        }
    }

}

[Serializable]
public struct StatsAddingButtons
{
    public Button Minus;
    public Button Plus;
}
