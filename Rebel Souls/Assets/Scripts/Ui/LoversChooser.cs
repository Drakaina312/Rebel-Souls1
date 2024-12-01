using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class LoversChooser : MonoBehaviour
{
    [SerializeField] private List<LoverUiComponnents> _loversComponnetns;
    [SerializeField] private LoverInfoHolder _loverInfoholder;
    private InGameDataBase _inGameDataBase;

    [Inject]
    private void Construct(InGameDataBase gameData)
    {
        _inGameDataBase = gameData;
    }

    public void ShowLovers()
    {
        int index = 0;
        foreach (var item in _inGameDataBase.ActStatistics.ActLovers)
        {
            _loversComponnetns[index].Image.gameObject.SetActive(true);
            _loversComponnetns[index].Image.sprite = item.LoverViewSprite;
            _loversComponnetns[index].Button.onClick.RemoveAllListeners();
            _loversComponnetns[index].Button.onClick.AddListener(() => ShowLoverInfo(item.LoverViewSprite,item.LoverDescription));

            index++;
        }
        for (int i = index; i < _loversComponnetns.Count; i++)
        {
            _loversComponnetns[i].Button.gameObject.SetActive(false);
        }
    }

    private void ShowLoverInfo(Sprite loverViewSprite, string loverDescription)
    {
        _loverInfoholder.transform.DOScale(1, 0.5f);
        _loverInfoholder.ChangeLoverPanel(loverDescription, loverViewSprite);

    }
}


[Serializable]
public struct LoverUiComponnents
{
    public Image Image;
    public Button Button;
}


