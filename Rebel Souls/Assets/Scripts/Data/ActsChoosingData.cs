using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(fileName = "HistoryData", menuName = "Data/HistoryData")]
public class ActsChoosingData : ScriptableObject

{
    public List<ActsInfo> ActsInfo;
    public Sprite Background;
    //public Sprite Discription;
    public int NumberScene;


}
[Serializable]
public struct ActsInfo
{
    public Sprite ActsBG;
    public string BookName;
    public string ActsName;
    public ChaptersChoosingData ChaptersToLoadData;
    public ActStatistics ActStatistics;
}