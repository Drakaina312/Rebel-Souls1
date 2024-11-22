using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UIActData", menuName = "Scriptable Objects/UIActData")]
public class UIActData : ScriptableObject
{
    public List<ChaptersInfo> ChaptersButtonsName;
    
}

[Serializable]
public struct ChaptersInfo
{
    public string ChaptersName;
    public HistoryPattern FirstDialigues;

        
}

