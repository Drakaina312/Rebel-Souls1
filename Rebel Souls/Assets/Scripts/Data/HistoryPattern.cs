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
    public int Indexator;
    public HeroType HeroType;
    public int ButtonsAmound;
    public Sprite HeroSprite;
    public Sprite Background;
    [TextArea(1,10)]public string Text;
}