using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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
    //public string LineKey;
    [PreviewField(75, ObjectFieldAlignment.Center), VerticalGroup("Split")]
    [FoldoutGroup("Split/Settings", false)]
    public Sprite Background;

    [FoldoutGroup("Split/Settings", false)]
    public HeroType HeroType;

    [HideIf("HeroType", HeroType.NoHero), PreviewField(75, ObjectFieldAlignment.Center)]
    [FoldoutGroup("Split/Settings", false)]
    public Sprite HeroSprite;

    [FoldoutGroup("Split/Settings", false)]
    public bool IsHaveButtons;
    [FoldoutGroup("Split/Settings", false)]
    [ShowIf(nameof(IsHaveButtons))]
    public List<ButtonSetting> ButtonSetting;

    [FoldoutGroup("Split/Settings", false)]
    public bool IsHaveNotation;
    [ShowIf(nameof(IsHaveNotation)), TextArea(1, 3)]
    [FoldoutGroup("Split/Settings", false)]
    public string Notation;

    [FoldoutGroup("Split/Settings", false)]
    [TextArea(1, 10)] public string Text;

    [FoldoutGroup("Split/Settings", false)]
    public bool IsFalseChoiseFinish;

    [FoldoutGroup("Split/Settings", false)]
    public int IndexToStartFlow;
}
[Serializable]
public class ButtonSetting
{
    //public string StoryKey;
    public string ButtonsName;
    #region HelpSprite
    public bool IsHaveHelpSprite;

    [ShowIf(nameof(IsHaveHelpSprite)), PreviewField(75, ObjectFieldAlignment.Center)]
    public Sprite HelpSprite;
    #endregion

    #region LineSplitter
    public bool IsLineSplitter;

    [ShowIf(nameof(IsLineSplitter))]
    public DialogSequence HistoryPattern;
    #endregion


    public bool IsStatAdder;

    [ShowIf(nameof(IsStatAdder))]
    public List<StatKit> StatKit;


    public bool IsFalseChoice;

    [ShowIf(nameof(IsFalseChoice))]
    public List<FalseChoiseLine> FalseChoiceLines;


    public bool IsCircleChoise;

    [ShowIf(nameof(IsCircleChoise))]
    public List<CircleChoiseLine> CircleChoiceLines;

    public bool IsFindChoise;

    [ShowIf(nameof(IsFindChoise))]
    public List<FindChoiseLine> FindChoiseLines;

    [HideInInspector]
    public bool WasChoised;
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
