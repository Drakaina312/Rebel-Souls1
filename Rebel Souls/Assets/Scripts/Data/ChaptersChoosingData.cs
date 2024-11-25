using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChapterChoosingData", menuName = "Scriptable Objects/ChapterChoosingData")]
public class ChaptersChoosingData : ScriptableObject
{
    public List<ChaptersInfo> ChaptersButtonsName;

}

[Serializable]
public struct ChaptersInfo
{
    public string ChaptersName;
    public DialogSequence FirstDialigues;
    public DialogSequence PreviousChapterForLoadStats;
    public int SceneNumber;
}

