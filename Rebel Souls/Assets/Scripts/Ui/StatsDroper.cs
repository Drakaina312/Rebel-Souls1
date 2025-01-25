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
    [SerializeField] private GameObject _completeBtn;

    public event Action StatsDropingOn;
    public event Action StatsDropingOff;


    private MasterSave _masterSave;
    private InGameDataBase _inGameData;
    public int TotalPoints;

    [Inject]
    private void Construct(MasterSave masterSave, InGameDataBase inGameDataBase)
    {
        _masterSave = masterSave;
        _inGameData = inGameDataBase;
    }

    public void DropAllStats()
    {
        if (TotalPoints != 0)
            return;

        StatsDropingOn?.Invoke();
        _completeBtn.gameObject.SetActive(true);

    }

    public void CompleteStatChange()
    {
        if (TotalPoints != 0)
            return;

        StatsBook statsBook = _masterSave.CurrentProfile.BooksStat
            .FirstOrDefault(predict => predict.IsLastSave == true && predict.ChapterSortingConditions.BookName == _inGameData.BookName);

        statsBook.SavedIndexes = new int[0];

        StatsDropingOff?.Invoke();
        _masterSave.SaveAllData();
        StatsDropingOff = null;
        StatsDropingOn = null;
        _completeBtn.gameObject.SetActive(false);

    }

}

[Serializable]
public struct StatsAddingButtons
{
    public Button Minus;
    public Button Plus;
}
