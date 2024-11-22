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
            ActInfo[i].ActsButton.onClick.AddListener(()=> StartAct(act));
            i ++;
        }
    }

    private void StartAct(ActsInfo act)
    {  
        ChaptersPanel.gameObject.SetActive(true);
        int index = 0;
        foreach (var chapter in act.ActsData.ChaptersButtonsName)
        {
            ChaptersPanel.ChaptersInfo[index].ChaptersButton.gameObject.SetActive(true);
            ChaptersPanel.ChaptersInfo[index].ChaptersName.text = chapter.ChaptersName;
            //По нажатию кнопки главы начать сцену с игрой (данные для игры находятся в  HistoryPattern
            Debug.Log(" назначили главу "+ index);
            index ++;

        }
        for (int i = index; i < ChaptersPanel.ChaptersInfo.Count; i++)
        {
            Debug.Log(" выключаем кнопку " + i);
            ChaptersPanel.ChaptersInfo[i].ChaptersButton.gameObject.SetActive(false);
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

