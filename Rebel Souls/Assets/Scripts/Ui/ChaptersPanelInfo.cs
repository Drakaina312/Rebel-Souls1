using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChaptersPanelInfo : MonoBehaviour
{
    public List<UIChapterInfo> ChaptersInfo;
    
}
[Serializable]
public struct UIChapterInfo 
{
    public Button ChaptersButton;
    public TextMeshProUGUI ChaptersName;
}