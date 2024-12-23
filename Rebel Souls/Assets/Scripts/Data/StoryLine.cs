using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StoryLine", menuName = "Scriptable Objects/StoryLine")]
public class StoryLine : ScriptableObject
{
    [Searchable]
    public List<SlideData> SlideData;
}
