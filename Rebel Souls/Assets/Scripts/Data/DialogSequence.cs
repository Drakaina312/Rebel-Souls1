using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using TMPro;
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
public struct ButtonSetting
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


public interface IFunnelStruct
{
    public string Text { get; set; }
    public List<FalseChoiseButtons> FalseChoiseButtons { get; set; }
    public bool IsHaveButtons {  get; set; }
    Sprite BG { get; set; }
}


[Serializable]
public struct FalseChoiseLine : IFunnelStruct
{
    [PreviewField(75, ObjectFieldAlignment.Center), VerticalGroup("SplitFalseChoise")]
    [FoldoutGroup("SplitFalseChoise/Settings", false),SerializeField]
    public Sprite _background;


    [FoldoutGroup("SplitFalseChoise/Settings", false)]
    public HeroType HeroType;

    [HideIf("HeroType", HeroType.NoHero), PreviewField(75, ObjectFieldAlignment.Center)]
    [FoldoutGroup("SplitFalseChoise/Settings", false)]
    public Sprite HeroSprite;

    [FoldoutGroup("SplitFalseChoise/Settings", false),SerializeField]
    public bool _isHaveButtons;
    [FoldoutGroup("SplitFalseChoise/Settings", false)]
    [ShowIf(nameof(_isHaveButtons)),SerializeField]
    private List<FalseChoiseButtons> _falseChoiseButtons;

    [FoldoutGroup("SplitFalseChoise/Settings", false)]
    public bool IsHaveNotation;
    [ShowIf(nameof(IsHaveNotation)), TextArea(1, 3)]
    [FoldoutGroup("SplitFalseChoise/Settings", false)]
    public string Notation;

    [FoldoutGroup("SplitFalseChoise/Settings", false)]
    [TextArea(1, 10),SerializeField] private string _text;

    string IFunnelStruct.Text { get => _text; set => _text = value; }
    List<FalseChoiseButtons> IFunnelStruct.FalseChoiseButtons { get => _falseChoiseButtons; set => _falseChoiseButtons = value; }
    bool IFunnelStruct.IsHaveButtons { get => _isHaveButtons; set => _isHaveButtons = value; }
    public Sprite BG { get => _background; set => _background = value; }
}
[Serializable]
public struct CircleChoiseLine : IFunnelStruct
{
    [PreviewField(75, ObjectFieldAlignment.Center), VerticalGroup("SplitFalseChoise")]
    [FoldoutGroup("SplitFalseChoise/Settings", false),SerializeField]
    public Sprite _background;


    [FoldoutGroup("SplitFalseChoise/Settings", false)]
    public HeroType HeroType;

    [HideIf("HeroType", HeroType.NoHero), PreviewField(75, ObjectFieldAlignment.Center)]
    [FoldoutGroup("SplitFalseChoise/Settings", false)]
    public Sprite HeroSprite;

    [FoldoutGroup("SplitFalseChoise/Settings", false),SerializeField]
    public bool _isHaveButtons;
    [FoldoutGroup("SplitFalseChoise/Settings", false)]
    [ShowIf(nameof(_isHaveButtons))]
    public List<FalseChoiseButtons> _falseChoiseButtons;

    [FoldoutGroup("SplitFalseChoise/Settings", false)]
    public bool IsHaveNotation;
    [ShowIf(nameof(IsHaveNotation)), TextArea(1, 3)]
    [FoldoutGroup("SplitFalseChoise/Settings", false)]
    public string Notation;

    [FoldoutGroup("SplitFalseChoise/Settings", false)]
    [TextArea(1, 10)] public string _text;



    string IFunnelStruct.Text { get => _text; set => _text = value; }
    List<FalseChoiseButtons> IFunnelStruct.FalseChoiseButtons { get => _falseChoiseButtons; set => _falseChoiseButtons = value; }
    bool IFunnelStruct.IsHaveButtons { get => _isHaveButtons; set => _isHaveButtons = value; }
    public Sprite BG { get => _background; set => _background = value; }
}

[Serializable]
public struct FalseChoiseButtons
{
    public string ButtonsName;
    #region HelpSprite
    public bool IsHaveHelpSprite;

    [ShowIf(nameof(IsHaveHelpSprite)), PreviewField(75, ObjectFieldAlignment.Center)]
    public Sprite HelpSprite;
    #endregion


    public bool IsStatAdder;

    [ShowIf(nameof(IsStatAdder))]
    public List<StatKit> StatKit;


    [HideInInspector]
    public bool WasChoosed;
}
