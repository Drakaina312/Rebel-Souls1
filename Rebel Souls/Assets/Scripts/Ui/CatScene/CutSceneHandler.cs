using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutSceneHandler : MonoBehaviour
{
    [SerializeField] private Canvas _cutSceneCanvas;

    [SerializeField] private List<Transform> _pointsToMove;
    [SerializeField] private Image _mainImage;
    [SerializeField] private int _totalCutSceneAnimation;
    public event Action OnCutSceneEnd;


    public void ActivateCutScene(Sprite cutSceneSprite)
    {
        _cutSceneCanvas.enabled = true;
        _mainImage.sprite = cutSceneSprite;
        StartCoroutine(ActivateImageMoovingCoroutine());
    }

    private IEnumerator ActivateImageMoovingCoroutine()
    {
        float animationPartsDuration = _totalCutSceneAnimation / _pointsToMove.Count;

        foreach (Transform t in _pointsToMove)
        {
            Tween tween = _mainImage.transform.DOMove(t.position, animationPartsDuration).SetAutoKill(false).SetEase(Ease.Linear);
            yield return tween.WaitForCompletion();
        }

        _mainImage.transform.position = Vector3.zero;
        _cutSceneCanvas.enabled = false;
        OnCutSceneEnd?.Invoke();
        yield break;
    }
}
