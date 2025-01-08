using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class SnapScroller : MonoBehaviour
{
    [SerializeField] private ScrollRect _scrollRect;

    [SerializeField] private float _center;
    [SerializeField] private float _distance;

    private Tween _tween;
    private float _childIndex;
    private float _range = 10000;
    private bool _isPanelOpened;

    //private void Start()
    //{
    //    Debug.Log("Center = " + _scrollRect.content.GetChild(0).transform.position);
    //}

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && gameObject.transform.parent.localScale.x > 0)
            _tween.Kill();
        if (Input.GetMouseButtonUp(0) && gameObject.transform.parent.localScale.x > 0)
        {
            SnapToNearestItem();
        }
    }

    public void ResetScrollValue()
    {
        Debug.Log("reset");
        _scrollRect.DOHorizontalNormalizedPos(0, 0.3f).SetDelay(0.2f).OnComplete(() => _isPanelOpened = true);
    }
    public void ClosePanel()
    {
        _isPanelOpened = false;
    }

    private void SnapToNearestItem()
    {
        if (!_isPanelOpened)
            return;

        for (int i = 0; i < _scrollRect.content.childCount; i++)
        {
            RectTransform child = _scrollRect.content.GetChild(i).GetComponent<RectTransform>();
            if (Mathf.Abs(_center - child.position.x) < _range)
            {
                _childIndex = i;
                _range = Mathf.Abs(_center - child.position.x);
            }
        }
        _range = 100000;

        float percent = _childIndex * _distance;
        Debug.Log("Percent = " + percent);

        _tween = _scrollRect.DOHorizontalNormalizedPos(percent, 1f);

    }
}
