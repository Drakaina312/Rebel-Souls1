using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ButtonsHandler : MonoBehaviour
{
    [SerializeField] private VerticalLayoutGroup _layoutGroup;
    [SerializeField] private List<ButtonsSettings> _buttons;
    private MasterSave _masterSave;
    private InGameDataBase _inGameDataBase;

    [Inject]
    public void Construct(MasterSave masterSave, InGameDataBase inGameDataBase)
    {
        _masterSave = masterSave;
        _inGameDataBase = inGameDataBase;


    }


    public void DeActivatedButtons()
    {
        foreach (var button in _buttons)

        {
            button.Button.onClick.RemoveAllListeners();
            button.gameObject.SetActive(false);
            button.HelpImage.gameObject.SetActive(false);
        }
    }

    public void ActivedButtons(List<ButtonSetting> buttonSetting, HistoryFlowHandler historyFlowHandler)
    {
        switch (buttonSetting.Count)
        {
            case 2:
                _layoutGroup.padding.top = 300;
                _layoutGroup.padding.bottom = 300;
                _layoutGroup.spacing = 100;
                break;

            case 3:
                _layoutGroup.padding.top = 150;
                _layoutGroup.padding.bottom = 150;
                _layoutGroup.spacing = 100;
                break;
            case 4:
                _layoutGroup.padding.top = 50;
                _layoutGroup.padding.bottom = 50;
                _layoutGroup.spacing = 75;
                break;
            case 5:
                _layoutGroup.padding.top = 0;
                _layoutGroup.padding.bottom = 0;
                _layoutGroup.spacing = 50;
                break;

            default: break;
        }
        int index = 0;
        Debug.Log(buttonSetting.Count);
        foreach (var button in buttonSetting)
        {
            _buttons[index].gameObject.SetActive(true);
            _buttons[index].ButtonName.text = button.ButtonsName;
            //Если включены подсказки, то выполняем строку 71, иначе мы должны выключить хелпимэйдж.
            _buttons[index].HelpImage.gameObject.SetActive(true);
            if (button.HelpSprite != null)
                _buttons[index].HelpImage.sprite = button.HelpSprite;
            _buttons[index].Button.onClick.AddListener(() =>
            {
                if (button.IsFalseChoice)
                {
                    Debug.Log("Is False Choise");
                    historyFlowHandler.StopCoroutine(historyFlowHandler.HistoryFlow);
                    if (button.IsStatAdder == true)
                    {
                        StatsBook statisticToWork = _masterSave.CurrentProfile.FindChapterStatsFromSave(_inGameDataBase.DIalogSequenceStart.ChapterSortingCondition);
                        foreach (var item in button.StatKit)
                        {
                            ChangeStat(item, statisticToWork.Statistics, historyFlowHandler);
                        }
                        Debug.Log("brfore await");
                        historyFlowHandler.CanWeGoNext = false;
                        historyFlowHandler.FalseChoiseAsync(button.FalseChoice).Forget();
                        Debug.Log("after await");
                    }
                    else
                    {
                        historyFlowHandler.CanWeGoNext = false;
                        historyFlowHandler.FalseChoiseAsync(button.FalseChoice).Forget();
                    }
                }
                else
                {
                    Debug.Log("Go NExt Slide");
                    if (button.IsStatAdder == false)
                    {
                        historyFlowHandler.ChangeFlowHistory(button.HistoryPattern);
                        historyFlowHandler.CanWeGoNext = true;
                    }
                    else
                    {
                        if (button.HistoryPattern != null)
                        {
                            historyFlowHandler.ChangeFlowHistory(button.HistoryPattern);
                        }
                        StatsBook statisticToWork = _masterSave.CurrentProfile.FindChapterStatsFromSave(_inGameDataBase.DIalogSequenceStart.ChapterSortingCondition);
                        foreach (var item in button.StatKit)
                        {
                            ChangeStat(item, statisticToWork.Statistics, historyFlowHandler);
                        }

                    }
                }

            });

            index++;
        }

    }
    public async UniTask ActivedButtonsAsync(List<ButtonSetting> buttonSetting, HistoryFlowHandler historyFlowHandler)
    {
        switch (buttonSetting.Count)
        {
            case 2:
                _layoutGroup.padding.top = 300;
                _layoutGroup.padding.bottom = 300;
                _layoutGroup.spacing = 100;
                break;

            case 3:
                _layoutGroup.padding.top = 150;
                _layoutGroup.padding.bottom = 150;
                _layoutGroup.spacing = 100;
                break;
            case 4:
                _layoutGroup.padding.top = 50;
                _layoutGroup.padding.bottom = 50;
                _layoutGroup.spacing = 75;
                break;
            case 5:
                _layoutGroup.padding.top = 0;
                _layoutGroup.padding.bottom = 0;
                _layoutGroup.spacing = 50;
                break;

            default: break;
        }
        int index = 0;
        Debug.Log(buttonSetting.Count);
        foreach (var button in buttonSetting)
        {
            _buttons[index].gameObject.SetActive(true);
            _buttons[index].ButtonName.text = button.ButtonsName;
            //Если включены подсказки, то выполняем строку 71, иначе мы должны выключить хелпимэйдж.
            _buttons[index].HelpImage.gameObject.SetActive(true);
            if (button.HelpSprite != null)
                _buttons[index].HelpImage.sprite = button.HelpSprite;
            _buttons[index].Button.onClick.AddListener(async () =>
            {
                if (button.IsFalseChoice)
                {
                    if (button.IsStatAdder == true)
                    {
                        StatsBook statisticToWork = _masterSave.CurrentProfile.FindChapterStatsFromSave(_inGameDataBase.DIalogSequenceStart.ChapterSortingCondition);
                        foreach (var item in button.StatKit)
                        {
                            ChangeStat(item, statisticToWork.Statistics, historyFlowHandler);
                        }
                        await historyFlowHandler.FalseChoiseAsync(button.FalseChoice);
                    }
                }
                else
                {
                    if (button.IsStatAdder == false)
                    {
                        historyFlowHandler.ChangeFlowHistory(button.HistoryPattern);
                        historyFlowHandler.CanWeGoNext = true;
                    }
                    else
                    {
                        if (button.HistoryPattern != null)
                        {
                            historyFlowHandler.ChangeFlowHistory(button.HistoryPattern);
                        }
                        StatsBook statisticToWork = _masterSave.CurrentProfile.FindChapterStatsFromSave(_inGameDataBase.DIalogSequenceStart.ChapterSortingCondition);
                        foreach (var item in button.StatKit)
                        {
                            ChangeStat(item, statisticToWork.Statistics, historyFlowHandler);
                        }

                    }
                }

            });

            index++;
        }

    }
    private void ChangeStat(StatKit statKit, StatisticInfo[] statisticInfos, HistoryFlowHandler historyFlowHandler)
    {
        StatisticInfo statToChange = statisticInfos.FirstOrDefault(stat => stat.StatisticName == statKit.StatName);
        if (statToChange != null)
        {
            statToChange.StatisticCount += statKit.Statpoint;
        }
        _masterSave.SaveAllData();
        historyFlowHandler.CanWeGoNext = true;
    }

}
