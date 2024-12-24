using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SlideData 
{
    //public string SlideIndex;

    [VerticalGroup("Split"),FoldoutGroup("Split/Settings", false)]
    [PreviewField(75, ObjectFieldAlignment.Center)]
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
    public List<SlideButtonsData> ButtonSetting;

    [FoldoutGroup("Split/Settings", false)]
    public bool IsHaveNotation;

    [ShowIf(nameof(IsHaveNotation)), TextArea(1, 3)]
    [FoldoutGroup("Split/Settings", false)]
    public string Notation;


    [OnValueChanged(nameof(ChangeTextLength))]
    [FoldoutGroup("Split/Settings", false)]
    [TextArea(1, 10)] public string Text;


    [FoldoutGroup("Split/Settings", false)]
    public string TextSize;

    private void ChangeTextLength() =>  TextSize = Text.Length.ToString();

    [FoldoutGroup("Split/Settings", false)]
    public bool IsHaveVoice;

    [FoldoutGroup("Split/Settings", false)]
    [ShowIf(nameof(IsHaveVoice))]
    public AudioClip VoiceClip;

    [FoldoutGroup("Split/Settings", false)]
    public bool IsHaveAudioEffects;

    [FoldoutGroup("Split/Settings", false)]
    [ShowIf(nameof(IsHaveAudioEffects))]
    public AudioClip AudioEffectsClip;

    [FoldoutGroup("Split/Settings", false)]
    public bool IsHaveChecking—ondition;

    [FoldoutGroup("Split/Settings", false)]
    [HideIf(nameof(IsHaveChecking—ondition))]
    public string NextSlideToOpen;

    [FoldoutGroup("Split/Settings", false)]
    [ShowIf(nameof(IsHaveChecking—ondition))]
    [TableList]
    public List<ChekingConditions> ChekingConditions;
}


[Serializable]
public struct ChekingConditions
{

    [TableList]
    public List<ChekingMultiConditions> Stat;

    public string SlideToOpen;
}

[Serializable]
public struct ChekingMultiConditions
{
    public string StatName;
    public bool IsSC;
    public ChekingEnums Cnd;
    [HideIf(nameof(IsSC))]
    public int StatValue;
    [ShowIf(nameof(IsSC))]
    public string StatName2;
}


public enum ChekingEnums
{
    More = 0,
    Less = 1,
    Equal = 2,
    MoreOrEqual = 3,
    LessOrEqual = 4,

}
