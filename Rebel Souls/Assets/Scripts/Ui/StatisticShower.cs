using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class StatisticShower : MonoBehaviour
{
    [SerializeField]
    private List<TextMeshProUGUI> _statsName;
    [SerializeField]
    private List<TextMeshProUGUI> _statsCount;
    private MasterSave _masterSave;

    [Inject]
    private void Construct(MasterSave masterSave)
    { 
        _masterSave = masterSave;
    }
    public void OpenStatisticPanel()
    {
        transform.DOScale(1, 0.1f);

    }
    public void CloseStatisticPanel()
    {
        transform.DOScale(0, 0.1f);
    }

    
}
