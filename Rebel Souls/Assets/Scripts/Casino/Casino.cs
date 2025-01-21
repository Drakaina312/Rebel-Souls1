using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using System.Linq;
using Zenject;

public class Casino : SerializedMonoBehaviour
{
    [SerializeField] private Image _fortunaImage;
    [SerializeField] private Dictionary<int, CasinoChanses> _casinoChanses;
    private MasterSave _masterSave;

    [Inject]
    private void Construct(MasterSave masterSave)
    {
        _masterSave = masterSave;
    }



    public void StartFortunaWheel()
    {
        _fortunaImage.transform.DORotate(Vector3.zero, 0, RotateMode.FastBeyond360);
        int randomAngle = UnityEngine.Random.Range(2160, 2880);
        Debug.Log(" Angle= " + randomAngle);

        int winingAngle = randomAngle - ((randomAngle / 360) * 360);
        Debug.Log("выЙГРЫШНЫЙ УГОЛ  = " + winingAngle);

        int winningPrise = 0;
        foreach (var item in _casinoChanses)
        {
            if (winingAngle >= item.Value.StartChance && winingAngle <= item.Value.EndChance)
            {
                Debug.Log($"{winingAngle} больше {item.Value.StartChance} и меньше {item.Value.EndChance}");
                winningPrise = item.Key;
            }

            Debug.Log("Предварительный результат = " + winningPrise);
        }
        if (winningPrise == 0)
            winningPrise = 5;

        _fortunaImage.transform.DORotate(Vector3.forward * randomAngle, 5, RotateMode.FastBeyond360)
            .OnComplete( () => _masterSave.CurrentProfile.AddMoney(winningPrise));

        Debug.Log("ВЫ Выйграли " + winningPrise);

    }


}

[Serializable]
public struct CasinoChanses
{
    public int StartChance;
    public int EndChance;
}
