using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StoryLine", menuName = "Scriptable Objects/StoryLine")]
public class StoryLine : SerializedScriptableObject
{
    public ChapterSortingConditions ChapterSortingCondition;
    public string PathToFile;
    
    [Searchable]
    public Dictionary <string,SlideData>  SlideData;


}

