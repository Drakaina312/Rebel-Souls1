using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsHandler : MonoBehaviour
{
    [SerializeField] private VerticalLayoutGroup _layoutGroup;
    [SerializeField] private List<Button> _buttons;
    


    public void ActivedButtons(int amoundButtons, List<string> buttonsName)
    {
        if (amoundButtons == 2)
        {
            _layoutGroup.padding.top = 300;
            _layoutGroup.padding.bottom = 300;

        }
        if (amoundButtons == 3)
        {
            _layoutGroup.padding.top = 150;
            _layoutGroup.padding.bottom = 150;

        }
        if (amoundButtons == 4)
        {
            _layoutGroup.padding.top = 50;
            _layoutGroup.padding.bottom = 50;

        }
        if (amoundButtons == 5)
        {
            _layoutGroup.spacing = 50;

        }
        int index = 0;
        for (int i = 0; i < amoundButtons; i++)
        {
            _buttons[index].gameObject.SetActive(true);
            index++;
        }
    }

    public void DeActivatedButtons()
    {
        foreach (var button in _buttons)

        { 
            button.gameObject.SetActive(false);
        }
    }
}
