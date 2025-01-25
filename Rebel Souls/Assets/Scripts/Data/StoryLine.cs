using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StoryLine", menuName = "Scriptable Objects/StoryLine")]
[Searchable]
public class StoryLine : SerializedScriptableObject
{
    public ChapterSortingConditions ChapterSortingCondition;
    public string PathToFile;

    //public Dictionary<string, SlideData> SlideData;

    [Searchable(FilterOptions = SearchFilterOptions.PropertyNiceName, FuzzySearch = true)]
    public List<SlideData> SlideDataList;


}

