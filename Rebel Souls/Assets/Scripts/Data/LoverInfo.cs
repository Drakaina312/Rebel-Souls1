using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "LoverInfo", menuName = "Scriptable Objects/LoverInfo")]
public class LoverInfo : ScriptableObject
{
    public string LoverName;
    public Sprite LoverViewSprite;
    public string LoverDescription;
}
