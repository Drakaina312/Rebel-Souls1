using Sirenix.OdinInspector;
using System;
using UnityEngine;

[Serializable]
public class StatisticInfo
{
    public string StatisticName;
    public int StatisticCount;
    public bool IsRelationship;
    public string PathToFavoriteScin;

    [FilePath]
    public string StatisticSprite;


    public event Action<int> OnValueChange;

    public void InvokeValueChange(int value) => OnValueChange?.Invoke(value);
    public void RemoveAllListners() => OnValueChange = null;
    public void SaveFavoriteScin(string path) => PathToFavoriteScin = path;

    public void AddNewScinForFavoriteScin(string spritePathToGive)
    {
        // добавить в массив шмоток новые шмотки
    }
}
