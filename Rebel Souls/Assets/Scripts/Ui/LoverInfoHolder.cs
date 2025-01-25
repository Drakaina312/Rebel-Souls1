using DG.Tweening;
using PolyAndCode.UI;
using Sirenix.OdinInspector.Demos.RPGEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoverInfoHolder : MonoBehaviour, IRecyclableScrollRectDataSource
{
    [SerializeField] private TextMeshProUGUI _descriptionComponent;
    [SerializeField] private Image _image;
    [SerializeField] private Image _loverProfile;
    [SerializeField] private TextMeshProUGUI _statusRelationShip;
    [SerializeField] private List<Image> _imageList;
    [SerializeField] private RecyclableScrollRect _recyclableScrollRect;
    private int _index;

    private void Awake()
    {
        _recyclableScrollRect.DataSource = this;
        _imageList.AddRange(_imageList);
        _imageList.AddRange(_imageList);
        _imageList.AddRange(_imageList);
        _imageList.AddRange(_imageList);

    }



    public void ChangeLoverPanel(Sprite loverImage, Sprite loverProfile, int statisticCount, List<RelationShipData> relationShipData)
    {
        _image.sprite = loverImage;
        _loverProfile.sprite = loverProfile;
        CalculateRelationShipStatus(statisticCount, relationShipData);


        //_scrollRect.Do
    }


    public void CheckInfinityScroll()
    {

    }

    private void CalculateRelationShipStatus(int statisticCount, List<RelationShipData> relationShipData)
    {
        Debug.Log(" статистика = " + statisticCount);
        Debug.Log(relationShipData);
        RelationShipData status = relationShipData.FirstOrDefault(predicate => predicate.MinValue <= statisticCount && predicate.MaxValue >= statisticCount);
        Debug.Log(status);
        if (status != null)
        {
            _statusRelationShip.text = status.RelationShipName;
            _descriptionComponent.text = status.LoverDescription;
        }
    }

    public int GetItemCount()
    {
        return _imageList.Count;
    }

    public void SetCell(ICell cell, int index)
    {
        var item = cell as FavoriteCell;

        _index++;
        if (_index >= _imageList.Count)
            _index = 0;
        item.Image.sprite = _imageList[_index].sprite;
    }
}
