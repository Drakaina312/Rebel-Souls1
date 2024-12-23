using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SlideButtonsData
{
    public string ButtonsName;

    public bool IsHaveHelpSprite;

    [ShowIf(nameof(IsHaveHelpSprite)), PreviewField(75, ObjectFieldAlignment.Center)]
    public Sprite HelpSprite;

    public bool IsStatAdder;

    [ShowIf(nameof(IsStatAdder))]
    public List<StatKit> StatKit;

    public bool IsCircleChoise;

    [HideInInspector]
    public bool WasChoised;

}
