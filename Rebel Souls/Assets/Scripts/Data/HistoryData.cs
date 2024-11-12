using UnityEngine;
[CreateAssetMenu(fileName = "HistoryData", menuName = "Data/HistoryData")]
public class HistoryData: ScriptableObject

{
    public Sprite Background;
    [TextArea(10,20)]public string HistoryDisc;
    public int NumberScene;

        
}
