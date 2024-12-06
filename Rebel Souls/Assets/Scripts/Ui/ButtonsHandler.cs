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
    [SerializeField] private FunnelHandler _funnelHandler;
    private MasterSave _masterSave;
    private InGameDataBase _inGameDataBase;
    private HistoryFlowHandler _historyFlowHandler;

    [Inject]
    public void Construct(MasterSave masterSave, InGameDataBase inGameDataBase)
    {
        _masterSave = masterSave;
        _inGameDataBase = inGameDataBase;


    }

    public void ActivedButtons(StoryHierarhy storyHierarhy, HistoryFlowHandler historyFlowHandler)
    {
        _historyFlowHandler = historyFlowHandler;

        if (!storyHierarhy.IsHaveButtons)
        {
            foreach (var button in _buttons)
            {
                button.Button.onClick.RemoveAllListeners();
                button.gameObject.SetActive(false);
                button.HelpImage.gameObject.SetActive(false);
            }

            return;
        }
        _historyFlowHandler.IsMainFlowActive = false;

        ResizeButtonsArea(storyHierarhy.ButtonSetting.FindAll(x => x.WasChoised == false).Count());

        int index = 0;
        foreach (var buttonData in storyHierarhy.ButtonSetting)
        {
            TurnOnButton(index, buttonData.HelpSprite, buttonData.ButtonsName, buttonData.WasChoised);

            Debug.Log("Была включена = " + buttonData.WasChoised);
            _buttons[index].Button.onClick.RemoveAllListeners();

            _buttons[index].Button.onClick.AddListener(() => ButtonOnClickProccesor(buttonData));

            index++;
        }
    }
    public void ActivedButtonsForFunnelChoise(List<FunnelChoiseButtons> funnelChoiseButtons, bool ishaveButtons)
    {
        if (!ishaveButtons)
        {
            foreach (var button in _buttons)
            {
                button.Button.onClick.RemoveAllListeners();
                button.gameObject.SetActive(false);
                button.HelpImage.gameObject.SetActive(false);
            }
            return;
        }

        _funnelHandler.CanSwitchToNextDialog = false;

        ResizeButtonsArea(funnelChoiseButtons.FindAll(x => x.WasChoosed == false).Count());
        int index = 0;
        foreach (var buttonData in funnelChoiseButtons)
        {
            TurnOnButton(index, buttonData.HelpSprite, buttonData.ButtonsName, buttonData.WasChoosed);
            _buttons[index].Button.onClick.RemoveAllListeners();

            _buttons[index].Button.onClick.AddListener(() => FunnelChoiseClickProccesor(buttonData));
            index++;
        }
    }

    private void FunnelChoiseClickProccesor(FunnelChoiseButtons buttonData)
    {
        AddStatIfCan(buttonData.StatKit, buttonData.IsStatAdder);
        _funnelHandler.CanSwitchToNextDialog = true;

        if (buttonData.IsChoiseAdder)
        {
            _funnelHandler.ActivateFunnelChoiseLine(buttonData.AnotherFindLine);
            _funnelHandler.IsRightChoiseFinded = buttonData.IsRightChoise;
            return;
        }
        _funnelHandler.SwipeDialogWhenClicked();
    }

    private void ResizeButtonsArea(int buttonCount)
    {
        switch (buttonCount)
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
    }

    private void ButtonOnClickProccesor(ButtonSetting buttonData)
    {
        ActivateFunnelChoiseIfCan(buttonData);
        AddStatIfCan(buttonData.StatKit, buttonData.IsStatAdder);

        ChangeStoryLineIfCan(buttonData, buttonData.IsLineSplitter);
    }


    #region OnClickActions
    private void ActivateFunnelChoiseIfCan(ButtonSetting buttonData)
    {
        if (!buttonData.IsFalseChoice && !buttonData.IsCircleChoise && !buttonData.IsFindChoise)
            return;

        if (buttonData.IsCircleChoise)
            buttonData.WasChoised = true;

        _historyFlowHandler.IsMainFlowActive = false;
        _historyFlowHandler.IsFunnelChoiseActive = true;
        if (buttonData.IsCircleChoise)
        {
            _funnelHandler.IsCircleFunnel = true;
            _funnelHandler.ActivateFunnelChoiseLine(buttonData.CircleChoiceLines.Cast<FunnelChoiseLine>().ToList());
        }
        else if (buttonData.IsFalseChoice)
        {
            _funnelHandler.ActivateFunnelChoiseLine(buttonData.FalseChoiceLines.Cast<FunnelChoiseLine>().ToList());

        }
        else if (buttonData.IsFindChoise)
        {
            _funnelHandler.IsFindChoise = true;
            _funnelHandler.ActivateFunnelChoiseLine(buttonData.FindChoiseLines);
        }
    }

    private void ChangeStoryLineIfCan(ButtonSetting buttonData, bool isLineSplitter)
    {
        if (!isLineSplitter)
            return;

        _historyFlowHandler.ChangeFlowHistory(buttonData.HistoryPattern);
    }

    private void AddStatIfCan(List<StatKit> statKit, bool isStatAdder)
    {
        if (!isStatAdder)
            return;

        StatsBook statisticToWork = _masterSave.CurrentProfile.FindChapterStatsFromSave(_inGameDataBase.DIalogSequenceStart.ChapterSortingCondition);
        foreach (var item in statKit)
        {
            StatisticInfo statToChange = statisticToWork.Statistics.FirstOrDefault(stat => stat.StatisticName == item.StatName);

            if (statToChange == null)
            {
                Debug.LogErrorFormat($"UnEble to finf stat {item.StatName}");
                continue;
            }

            statToChange.StatisticCount += item.Statpoint;
            _masterSave.SaveAllData();
        }
        _historyFlowHandler.IsMainFlowActive = true;
    }

    private void TurnOnButton(int index, Sprite helpSprite, string buttonName, bool wasChoised)
    {
        if (wasChoised)
            return;

        _buttons[index].gameObject.SetActive(true);
        _buttons[index].ButtonName.text = buttonName;

        if (helpSprite != null && _masterSave.CurrentProfile.IsHelpOn)
        {
            _buttons[index].HelpImage.gameObject.SetActive(true);
            _buttons[index].HelpImage.sprite = helpSprite;
        }
    }


    #endregion
}
