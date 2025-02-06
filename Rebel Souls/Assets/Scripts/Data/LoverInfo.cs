using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "LoverInfo", menuName = "Scriptable Objects/LoverInfo")]
public class LoverInfo : ScriptableObject
{
    public string LoverName;
    public Sprite LoverViewSprite;
    public Sprite LoverProfile;
    public List<RelationShipData> RelationShipsInfo;



    public string GetRelatinShipName(int loverRelationshipCount)
    {
        return RelationShipsInfo.FirstOrDefault(x => loverRelationshipCount >= x.MinValue && loverRelationshipCount <= x.MaxValue).RelationShipName;
    }
}

[Serializable]
public class RelationShipData
{
    public int MinValue, MaxValue;
    public string RelationShipName;
    [TextArea]
    public string LoverDescription;
}
