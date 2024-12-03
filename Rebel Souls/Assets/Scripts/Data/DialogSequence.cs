using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogSequence", menuName = "Scriptable Objects/DialogSequence")]
public class DialogSequence : ScriptableObject
{
    public ChapterSortingConditions ChapterSortingCondition;
    public List<StoryHierarhy> StoryHierarhy;
    public string PathToFile;
    //public int DialogIndex;
}

[Serializable]
public struct StoryHierarhy
{
    public string LineKey;
    public HeroType HeroType;
    public List<ButtonSetting> ButtonSetting;
    public Sprite HeroSprite;
    public Sprite Background;
    [TextArea(1, 3)] public string Notation;
    [TextArea(1, 10)] public string Text;
    public bool IsFalseChoiseFinish;
    public int IndexToStartFlow;
}
[Serializable]
public struct ButtonSetting
{
    public string StoryKey;
    public string ButtonsName;
    public Sprite HelpSprite;
    public DialogSequence HistoryPattern;
    public bool IsStatAdder;
    public List<StatKit> StatKit;
    public bool IsFalseChoice;
    public List<StoryHierarhy> FalseChoice;
    


}
[Serializable]
public struct StatKit
{
    public string StatName;
    public int Statpoint;
}

[Serializable]
public struct ChapterSortingConditions
{
    public string BookName;
    //public string ActName;
    public string ChapterName;
}
