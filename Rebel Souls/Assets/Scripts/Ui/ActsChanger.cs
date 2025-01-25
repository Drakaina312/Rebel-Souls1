using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class ActsChanger : MonoBehaviour
{
    public List<UIActInfo> ActInfo;
    public ChaptersPanelInfo ChaptersPanel;
    private InGameDataBase _inGameDataBase;
    private MasterSave _masterSave;
    public Image BGShapters;

    [Inject]
    public void Construct(InGameDataBase inGameDataBase, MasterSave masterSave)
    {
        _inGameDataBase = inGameDataBase;
        _masterSave = masterSave;
    }

    public void ChangePanel(List<ActsInfo> actsInfo)
    {
        int i = 0;
        foreach (ActsInfo act in actsInfo)
        {
            Debug.Log(act.ActsName);
            Debug.Log(ActInfo[i].ActsButton);
            ActInfo[i].ActsBG.sprite = act.ActsBG;
            ActInfo[i].ActsDisc.text = act.ActsName;
            ActInfo[i].ActsButton.onClick.AddListener(() => StartAct(act));
            i++;
        }
    }

    private void StartAct(ActsInfo act)
    {
        BGShapters.sprite = act.BGChapter;
        Debug.Log(" Открытие глав ");
        ChaptersPanel.gameObject.SetActive(true);
        _inGameDataBase.ActStatistics = act.ActStatistics;

        int index = 0;
        foreach (var chapter in act.ChaptersToLoadData.ChaptersButtonsName)
        {
            ChaptersPanel.ChaptersInfo[index].ChaptersButton.gameObject.SetActive(true);
            ChaptersPanel.ChaptersInfo[index].ChaptersName.text = chapter.ChaptersName;
            Debug.Log(" назначили главу " + index);
            ChaptersPanel.ChaptersInfo[index].ChaptersButton.onClick.RemoveAllListeners();
            if (chapter.PreviousChapterForLoadStats == null)
                ChaptersPanel.ChaptersInfo[index].ChaptersButton.onClick.AddListener(() => AddActionOnChapterClick(chapter.FirstDialigues));
            else
                ChaptersPanel.ChaptersInfo[index].ChaptersButton.onClick.AddListener(() => AddActionOnChapterClick(chapter.FirstDialigues, chapter.PreviousChapterForLoadStats));


            index++;

        }
        for (int i = index; i < ChaptersPanel.ChaptersInfo.Count; i++)
        {
            ChaptersPanel.ChaptersInfo[i].ChaptersButton.gameObject.SetActive(false);
        }
    }

    private void AddActionOnChapterClick(StoryLine dialogSequence)
    {
        _inGameDataBase.StoryLine = dialogSequence;
        _masterSave.CurrentProfile.LastSaveChapterPath = dialogSequence.PathToFile;
        _masterSave.CurrentProfile.LastSaveSlideIndex = dialogSequence.SlideDataList.First().SlideIndex;
        _inGameDataBase.IsContiniueStory = false;
        _inGameDataBase.IsRestartChapter = true;
        _masterSave.CurrentProfile.SaveStatsForFirstLaunch(_inGameDataBase.ActStatistics, dialogSequence.ChapterSortingCondition);

        SceneManager.LoadScene(1);
    }
    private void AddActionOnChapterClick(StoryLine dialogSequence, StoryLine previousChapter)
    {
        _inGameDataBase.StoryLine = dialogSequence;
        _masterSave.CurrentProfile.LastSaveChapterPath = dialogSequence.PathToFile;
        _masterSave.CurrentProfile.LastSaveSlideIndex = dialogSequence.SlideDataList.First().SlideIndex;
        _inGameDataBase.IsContiniueStory = false;
        _inGameDataBase.IsRestartChapter = true;
        _masterSave.CurrentProfile.LoadPreviousChapterStats(dialogSequence.ChapterSortingCondition, previousChapter);
        SceneManager.LoadScene(1);
    }
}

[Serializable]
public struct UIActInfo
{
    public Image ActsBG;
    public TextMeshProUGUI ActsDisc;
    public Button ActsButton;

}

