using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SnapScroller : MonoBehaviour
{
    [SerializeField] private ScrollRect _scrollRect;
    [SerializeField] private float _snapSpeed = 10;
    [SerializeField] private bool _isSnaping;
    private Vector2 _snapPosition;

    private void Update()
    {
        if (_isSnaping)
            return;
        if (Input.GetMouseButtonUp(0))
        {
            StartCoroutine(SnapToNearestItem());
        }
        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Ended)

            {
                StartCoroutine(SnapToNearestItem());
            }
        }


    }

    private IEnumerator SnapToNearestItem() 
    {
        _isSnaping = true;
        float targetPosition = 0;
        if (_snapPosition.x >= 0 && _snapPosition.x < 0.40f) 
        {
            targetPosition = 0;

        }
        //float contentPosition = _scrollRect.content.anchoredPosition.x -_scrollRect.content.;
        Debug.Log(targetPosition);
        Debug.Log(" привязка начата ");
       
        //float itemWidth = _scrollRect.content.rect.width;
        //int itemCount = _scrollRect.content.childCount;


        //float nearestItemIndex = Mathf.RoundToInt(contentPosition / itemWidth);
        //nearestItemIndex = Mathf.Clamp(nearestItemIndex, 0, itemCount - 1);





        //while (Mathf.Abs(contentPosition - targetPosition) > 0.01f) 
        //{
        //    contentPosition = Mathf.Lerp(contentPosition,targetPosition, Time.deltaTime * _snapSpeed);
        //    _scrollRect.content.anchoredPosition = new Vector2(contentPosition, _scrollRect.content.anchoredPosition.y);
        //    yield return null;
        //}


        //_scrollRect.content.anchoredPosition = new Vector2(targetPosition, _scrollRect.content.anchoredPosition.y);
        _scrollRect.content.DOLocalMoveX(targetPosition, 1);
        _isSnaping = false;
        yield return null;
       
    }

    public void GetCurrentItemSnapPosition(Vector2 snapPosition) 
    {
        _snapPosition = snapPosition;
      
    }
}
