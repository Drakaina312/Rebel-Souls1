using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActsChanger : MonoBehaviour
{
    public List<UIActInfo> ActInfo;
    public ChaptersPanelInfo ChaptersPanel;
    public void InicializeActs() 
    {
        
    }

    public void ChangePanel(List<ActsInfo> actsInfo)
    {
        int i = 0;
        foreach (ActsInfo act in actsInfo)
        {
            ActInfo[i].ActsBG.sprite = act.ActsBG;
            ActInfo[i].ActsDisc.text = act.ActsName;
            //ActInfo[i].ActsButton.onClick.AddListener();
        }
    }
}

[Serializable]
public struct UIActInfo 
{
    public Image ActsBG;
    public TextMeshProUGUI ActsDisc;
    public Button ActsButton;

}

