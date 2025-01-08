using System;
using UnityEngine;

[Serializable]
public class StatisticInfo
{
    public string StatisticName;
    public int StatisticCount;
    public bool IsRelationship;
    public Sprite StatisticSprite;


    public event Action<int> OnValueChange;

    public void ChangeValue(int value) => OnValueChange(value);
    public void RemoveAllListners() => OnValueChange = null;
}
