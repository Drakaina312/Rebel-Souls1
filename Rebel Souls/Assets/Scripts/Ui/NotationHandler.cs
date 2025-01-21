using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NotationHandler : MonoBehaviour
{
    public TextMeshProUGUI NotationText;
    [SerializeField] private TextResizer _notationResizer;
    [SerializeField] private Image _notationImage;
    [SerializeField] private Sprite _authorNoationImage;
    [SerializeField] private Sprite _systemNoationImage;

    public void ActivaidNotation(DifficultyType difficultyType, bool isBeenSystemNotation, bool isBeenAuthorNotation, string textToNotateSystem = null, string textToNotateAuthor = null)
    {
        if (isBeenSystemNotation || isBeenAuthorNotation)
            DeActivaidNotation();


        if (difficultyType == DifficultyType.Easy || difficultyType == DifficultyType.Medium)
            StartCoroutine(Notation(isBeenSystemNotation, isBeenAuthorNotation, textToNotateSystem, textToNotateAuthor));
        else
            StartCoroutine(Notation(false, isBeenAuthorNotation, textToNotateSystem, textToNotateAuthor));


    }

    private IEnumerator Notation(bool isBeenSystemNotation, bool isBeenAuthorNotation, string textToNotateSystem = null, string textToNotateAuthor = null)
    {
        if (isBeenSystemNotation)
        {
            _notationImage.sprite = _systemNoationImage;
            transform.DOScale(1, 0.5f);
            _notationResizer.UpdateSize(textToNotateSystem);
            NotationText.text = textToNotateSystem;

            yield return new WaitForSeconds(2);
            transform.DOScale(0, 0.5f);
            yield return new WaitForSeconds(0.5f);
        }

        if (isBeenAuthorNotation)
        {
            _notationImage.sprite = _authorNoationImage;
            transform.DOScale(1, 0.5f);
            _notationResizer.UpdateSize(textToNotateAuthor);
            NotationText.text = textToNotateAuthor;

            yield return new WaitForSeconds(2);
            transform.DOScale(0, 0.5f);
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void DeActivaidNotation()
    {

        StopAllCoroutines();
        transform.DOScale(0, 0.2f);
    }
}
