using DG.Tweening;
using System.Collections;
using UnityEngine;

public class ChapterLevitation : MonoBehaviour
{
    [SerializeField] private RectTransform _levitationEndPoint;
    [SerializeField] private RectTransform _levitationObject;
    [SerializeField] private float _delay;
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(_delay);
        _levitationObject.DOAnchorPos(_levitationEndPoint.anchoredPosition, 1).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
    }
} 

