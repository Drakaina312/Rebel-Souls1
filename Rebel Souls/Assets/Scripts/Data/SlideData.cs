using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SlideData
{
    public int SlideIndex;
    public string SlideName;

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
    public string InterpritationName;

    [HideIf("HeroType", HeroType.NoHero)]
    [FoldoutGroup("Split/Settings", false)]
    public bool IsFavorite;

    [HideIf("HeroType", HeroType.NoHero)]
    [FoldoutGroup("Split/Settings", false)]
    public bool IsMainHero;

    [ShowIf(nameof(IsFavorite))]
    [FoldoutGroup("Split/Settings", false)]
    public string FavoriteName;

    [ShowIf(nameof(IsFavorite))]
    [FoldoutGroup("Split/Settings", false)]
    public bool IsFirstAppearance;

    [HideIf("HeroType", HeroType.NoHero)]
    [FoldoutGroup("Split/Settings", false)]
    public bool IsThinking;

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
    public bool IsHaveSystemNotation;

    [ShowIf(nameof(IsHaveSystemNotation)), TextArea(1, 3)]
    [FoldoutGroup("Split/Settings", false)]
    public string SystemNotation;

    [FoldoutGroup("Split/Settings", false)]
    public bool IsHaveAuthorNotation;

    [ShowIf(nameof(IsHaveAuthorNotation)), TextArea(1, 3)]
    [FoldoutGroup("Split/Settings", false)]
    public string AuthorNotation;

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

    [FoldoutGroup("Split/Settings", false)]
    public bool IsHaveCutScene;

    [FoldoutGroup("Split/Settings", false)]
    [ShowIf(nameof(IsHaveCutScene))]
    public Sprite CutSceneSprite;



    [Title("Conditions")]
    [FoldoutGroup("Split/Settings", false)]
    public bool IsHaveChecking—ondition;


    [FoldoutGroup("Split/Settings", false)]
    [ShowIf(nameof(IsHaveChecking—ondition))]
    [TableList]
    public List<ChekingConditions> ChekingConditions;


    [Title("Other")]
    [FoldoutGroup("Split/Settings", false)]
    public bool IsTextWritingSlide;

    [ShowIf(nameof(IsTextWritingSlide))]
    public bool MainHeroTextBox;

    [ShowIf(nameof(IsTextWritingSlide))]
    public bool GameTextBox;

    [ShowIf(nameof(IsTextWritingSlide))]
    public bool HorrorTextBox;

    [ShowIf(nameof(IsTextWritingSlide))]
    public bool FunTextBox;

    [FoldoutGroup("Split/Settings", false)]
    [HideIf(nameof(IsHaveChecking—ondition))]
    public int NextSlideToOpen;

}


[Serializable]
public struct ChekingConditions
{

    //[TableList]
    public List<ChekingMultiConditions> Stat;

    public int SlideToOpen;
}

[Serializable]
public struct ChekingMultiConditions
{
    public ConditionsEnums ConditionEnums;

    [ShowIf(nameof(ConditionEnums), ConditionsEnums.CheckSlideIndexPassing)]
    public int IndexToCheck;

    [ShowIf(nameof(StatNameNeeding))]
    public string StatName;

    [ShowIf(nameof(FavoriteNameNeeding))]
    public string FavoriteName;

    [ShowIf(nameof(BothConditionsIsActive))]
    public ChekingEnums Cnd;

    [ShowIf(nameof(CheckStatWithValue))]
    public int CheckingValue;

    [ShowIf(nameof(ConditionEnums), ConditionsEnums.CheckStatWithAnotherStat)]
    public string ChekingStatName;

    [ShowIf(nameof(ConditionEnums), ConditionsEnums.CheckFavoriteWithAnotherFavorite)]
    public string ChekingFavoriteName;


    //public string StatName;

    //[HideIf(nameof(SlideCheck))]
    //public bool Var1;

    //[HideIf(nameof(Var1))]
    //public bool SlideCheck;


    //[HideIf(nameof(BothConditionsUnActive))]
    //public int StatValue;

    //[ShowIf(nameof(Var1))]
    //public string StatName2;


    //public bool IsBigStat;
    //public bool IsBigFavorite;


    private bool CheckStatWithValue()
    {
        if (ConditionEnums == ConditionsEnums.CheckStatWithValue || ConditionEnums == ConditionsEnums.CheckFavoriteWithValue)
            return true;
        else return false;
    }

    private bool FavoriteNameNeeding()
    {
        if (ConditionEnums == ConditionsEnums.CheckFavoriteWithValue || ConditionEnums == ConditionsEnums.CheckMoreBigFavorite || ConditionEnums == ConditionsEnums.CheckFavoriteWithAnotherFavorite)
            return true;
        else return false;
    }

    private bool StatNameNeeding()
    {
        if (ConditionEnums == ConditionsEnums.CheckStatWithValue || ConditionEnums == ConditionsEnums.CheckStatWithAnotherStat || ConditionEnums == ConditionsEnums.CheckMoreBigStat)
            return true;
        else return false;
    }


    private bool BothConditionsIsActive()
    {
        if (ConditionEnums == ConditionsEnums.CheckStatWithValue || ConditionEnums == ConditionsEnums.CheckFavoriteWithValue)
            return true;
        else if (ConditionEnums == ConditionsEnums.CheckFavoriteWithAnotherFavorite || ConditionEnums == ConditionsEnums.CheckStatWithAnotherStat)
            return true;
        else return false;
    }

}

public enum ConditionsEnums
{
    Nothing,
    CheckSlideIndexPassing,
    CheckMoreBigStat,
    CheckMoreBigFavorite,
    CheckStatWithValue,
    CheckFavoriteWithValue,
    CheckStatWithAnotherStat,
    CheckFavoriteWithAnotherFavorite,
}


public enum ChekingEnums
{
    More = 0,
    Less = 1,
    Equal = 2,
    MoreOrEqual = 3,
    LessOrEqual = 4,

}
