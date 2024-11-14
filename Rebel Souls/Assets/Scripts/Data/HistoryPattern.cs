using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "HistoryPattern", menuName = "Scriptable Objects/HistoryPattern")]
public class HistoryPattern : ScriptableObject
{
    public string StoryChapterName;
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
    [TextArea(1,3)]public string Notation;
    [TextArea(1,10)]public string Text;
}
[Serializable]
public struct ButtonSetting
{ 
    public string  StoryKey;
    public string ButtonsName;
    public HistoryPattern HistoryPattern;

}