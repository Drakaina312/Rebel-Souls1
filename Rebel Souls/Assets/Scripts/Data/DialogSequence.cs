using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogSequence", menuName = "Scriptable Objects/DialogSequence")]
public class DialogSequence : ScriptableObject
{
    public ChapterSortingConditions ChapterSortingCondition;
    public List<StoryHierarhy> StoryHierarhy;
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
}
[Serializable]
public struct ButtonSetting
{
    public string StoryKey;
    public string ButtonsName;
    public DialogSequence HistoryPattern;

}

[Serializable]
public struct ChapterSortingConditions
{
    public string BookName;
    public string ActName;
    public string ChapterName;
}
