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
    private SlideHandler _slideHandler;

    [Inject]
    public void Construct(MasterSave masterSave, InGameDataBase inGameDataBase)
    {
        _masterSave = masterSave;
        _inGameDataBase = inGameDataBase;
    }

    public bool ActivateButtons(SlideData slideData, SlideHandler slideHandler)
    {
        _slideHandler = slideHandler;

        foreach (var button in _buttons)
        {
            button.Button.onClick.RemoveAllListeners();
            button.gameObject.SetActive(false);
            button.HelpImage.gameObject.SetActive(false);
        }

        if (!slideData.IsHaveButtons)
            return true;

        _slideHandler.IsMainFlowActive = false;
        _slideHandler.ActivateInputDelay().Forget();

        List<SlideButtonsData> slideButtonsDatas = slideData.ButtonSetting.FindAll(x => x.WasChoised == false);

        if (_slideHandler.IsCirleChoise && slideButtonsDatas.Count == 0)
        {
            _slideHandler.IsCirleChoise = false;
            _slideHandler.CalculateSlideWork();
            return false;
        }



        if (slideButtonsDatas != null && slideButtonsDatas.Count > 0)
            ResizeButtonsArea(slideButtonsDatas.Count);

        int index = 0;
        foreach (var buttonData in slideData.ButtonSetting)
        {
            TurnOnButton(index, buttonData.HelpSprite, buttonData.ButtonsName, buttonData.WasChoised);
            _buttons[index].Button.onClick.RemoveAllListeners();

            _buttons[index].Button.onClick.AddListener(() => ButtonOnClickProccesor(buttonData));


            index++;
        }
        return true;
    }
    private void ButtonOnClickProccesor(SlideButtonsData buttonData)
    {
        if (!_slideHandler.IsInputActive)
            return;

        AddStatIfCan(buttonData.StatKit, buttonData.IsStatAdder);
        _slideHandler.ChangeStoryLineIfCan(buttonData);
    }

    private void ResizeButtonsArea(int buttonCount)
    {
        if (buttonCount == 1)
            _slideHandler.IsCirleChoise = false;


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

    private void AddStatIfCan(List<StatKit> statKit, bool isStatAdder)
    {
        if (!isStatAdder)
            return;

        StatsBook statisticToWork = _masterSave.CurrentProfile.FindChapterStatsFromSave(_inGameDataBase.StoryLine.ChapterSortingCondition);
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
}
