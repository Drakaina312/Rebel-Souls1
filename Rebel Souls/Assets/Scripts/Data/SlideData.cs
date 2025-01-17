using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SlideData
{
    //public string SlideIndex;

    [VerticalGroup("Split"), FoldoutGroup("Split/Settings", false)]
    [PreviewField(75, ObjectFieldAlignment.Center)]
    [FoldoutGroup("Split/Settings", false)]
    public Sprite Background;

    [Title("HeroSprite")]
    [FoldoutGroup("Split/Settings", false)]
    public HeroType HeroType;

    [HideIf("HeroType", HeroType.NoHero), PreviewField(75, ObjectFieldAlignment.Center)]
    [FoldoutGroup("Split/Settings", false)]
    public Sprite HeroSprite;

    [HideIf("HeroType", HeroType.NoHero)]
    [FoldoutGroup("Split/Settings", false)]
    public bool IsFavorite;

    [ShowIf(nameof(IsFavorite))]
    [FoldoutGroup("Split/Settings", false)]
    public string FavoriteName;

    [ShowIf(nameof(IsFavorite))]
    [FoldoutGroup("Split/Settings", false)]
    public bool IsImportantScin;

    [Title("Buttons")]
    [FoldoutGroup("Split/Settings", false)]
    public bool IsHaveButtons;

    [FoldoutGroup("Split/Settings", false)]
    [ShowIf(nameof(IsHaveButtons))]
    public bool IsHaveTimer;

    [FoldoutGroup("Split/Settings", false)]
    [ShowIf(nameof(IsHaveTimer))]
    [Range(0, 100)]
    public float TimerSeconds;

 
    [FoldoutGroup("Split/Settings", false)]
    [ShowIf(nameof(IsHaveButtons))]
    public List<SlideButtonsData> ButtonSetting;

    [Title("Text and Notation")]
    [FoldoutGroup("Split/Settings", false)]
    public bool IsHaveNotation;

    [ShowIf(nameof(IsHaveNotation)), TextArea(1, 3)]
    [FoldoutGroup("Split/Settings", false)]
    public string Notation;

    [ShowIf(nameof(IsHaveNotation))]
    [FoldoutGroup("Split/Settings", false)]
    public bool IsTipNotation;

    [FoldoutGroup("Split/Settings", false)]
    public bool IsHaveText = true;

    [OnValueChanged(nameof(ChangeTextLength))]
    [ShowIf(nameof(IsHaveText))]
    [FoldoutGroup("Split/Settings", false)]
    [TextArea(1, 10)] public string Text;

    [ShowIf(nameof(IsHaveText))]
    [FoldoutGroup("Split/Settings", false)]
    public string TextSize;

    private void ChangeTextLength() => TextSize = Text.Length.ToString();

    [Title("Audio")]
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

    [Title("Achievement")]
    [FoldoutGroup("Split/Settings", false)]
    public bool IsHaveAchievement;

    [ShowIf(nameof(IsHaveAchievement))]
    [FoldoutGroup("Split/Settings", false)]
    public string AchievementText;

    [ShowIf(nameof(IsHaveAchievement))]
    [FoldoutGroup("Split/Settings", false)]
    public bool IsAchievemntGiveGift;

    [ShowIf(nameof(IsAchievemntGiveGift))]
    [FoldoutGroup("Split/Settings", false)]
    public string FavoriteNameForPrisent;

    [ShowIf(nameof(IsAchievemntGiveGift))]
    [FoldoutGroup("Split/Settings", false)]
    public string SpritePathToGive;

    [ShowIf(nameof(IsHaveAchievement))]
    [FoldoutGroup("Split/Settings", false)]
    public Sprite AchievemntSprite;

    [Title("Conditions")]
    [FoldoutGroup("Split/Settings", false)]
    public bool IsHaveChecking—ondition;


    [FoldoutGroup("Split/Settings", false)]
    [ShowIf(nameof(IsHaveChecking—ondition))]
    [TableList]
    public List<ChekingConditions> ChekingConditions;





    [FoldoutGroup("Split/Settings", false)]
    [HideIf(nameof(IsHaveChecking—ondition))]
    public string NextSlideToOpen;

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

    [HideIf(nameof(SlideCheck))]
    public bool Var1;

    [HideIf(nameof(Var1))]
    public bool SlideCheck;

    [HideIf(nameof(SlideCheck))]
    public ChekingEnums Cnd;

    [HideIf(nameof(BothConditionsUnActive))]
    public int StatValue;

    [ShowIf(nameof(Var1))]
    public string StatName2;


    public bool IsBigStat;
    public bool IsBigFavorite;




    private bool BothConditionsUnActive()
    {
        if (Var1 || SlideCheck || IsBigStat)
            return true;
        else if (Var1 && SlideCheck && IsBigStat)
            return true;
        else return false;
    }

}


public enum ChekingEnums
{
    More = 0,
    Less = 1,
    Equal = 2,
    MoreOrEqual = 3,
    LessOrEqual = 4,

}
