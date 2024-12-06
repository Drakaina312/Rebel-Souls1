using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public abstract class FunnelChoiseLine
{
    [PreviewField(75, ObjectFieldAlignment.Center), VerticalGroup("SplitFalseChoise")]
    [FoldoutGroup("SplitFalseChoise/Settings", false), SerializeField]
    public Sprite Background;


    [FoldoutGroup("SplitFalseChoise/Settings", false)]
    public HeroType HeroType;

    [HideIf("HeroType", HeroType.NoHero), PreviewField(75, ObjectFieldAlignment.Center)]
    [FoldoutGroup("SplitFalseChoise/Settings", false)]
    public Sprite HeroSprite;

    [FoldoutGroup("SplitFalseChoise/Settings", false), SerializeField]
    public bool IsHaveButtons;
    [FoldoutGroup("SplitFalseChoise/Settings", false)]
    [ShowIf(nameof(IsHaveButtons)), SerializeField]
    public List<FunnelChoiseButtons> FunnelChoiseButtons;

    [FoldoutGroup("SplitFalseChoise/Settings", false)]
    public bool IsHaveNotation;
    [ShowIf(nameof(IsHaveNotation)), TextArea(1, 3)]
    [FoldoutGroup("SplitFalseChoise/Settings", false)]
    public string Notation;

    [FoldoutGroup("SplitFalseChoise/Settings", false)]
    [TextArea(1, 10), SerializeField] public string Text;
}
