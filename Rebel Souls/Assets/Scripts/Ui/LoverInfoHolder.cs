using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class LoverInfoHolder : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _descriptionComponent;
    [SerializeField] private Image _image;
    [SerializeField] private Image _loverProfile;
    [SerializeField] private TextMeshProUGUI _statusRelationShip;


    public void ChangeLoverPanel(Sprite loverImage, Sprite loverProfile, int statisticCount, List<RelationShipData> relationShipData)
    {
        _image.sprite = loverImage;
        _loverProfile.sprite = loverProfile;
        CalculateRelationShipStatus(statisticCount, relationShipData);

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
}
