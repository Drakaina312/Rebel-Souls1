using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct FunnelChoiseButtons
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

    public bool IsChoiseAdder;
    [ShowIf(nameof(IsChoiseAdder))]
    public FindChoiseLine AnotherFindLine;

    public bool IsRightChoise;

    [HideInInspector]
    public bool WasChoosed;
}
