using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsHandler : MonoBehaviour
{
    [SerializeField] private VerticalLayoutGroup _layoutGroup;
    [SerializeField] private List<ButtonsSettings> _buttons;
    

    public void DeActivatedButtons()
    {
        foreach (var button in _buttons)

        { 
            button.Button.onClick.RemoveAllListeners();
            button.gameObject.SetActive(false);
        }
    }

    internal void ActivedButtons(List<ButtonSetting> buttonSetting, HistoryFlowHandler historyFlowHandler)
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
            _buttons[index].Button.onClick.AddListener(() =>
            {
                historyFlowHandler.ChangeFlowHistory(button.HistoryPattern);
                historyFlowHandler.CanWeGoNext = true;
            });
            
            index++;
        }
    }
}
