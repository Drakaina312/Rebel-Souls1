using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class NotationHandler : MonoBehaviour
{
    public TextMeshProUGUI NotationText;
    [SerializeField] private TextResizer _notationResizer;
    public void ActivaidNotation(string TextToNotate)
    {
        Debug.Log("activatin notation");
        StartCoroutine(Notation(TextToNotate));

    }

    private IEnumerator Notation(string TextToNotate)
    {

        transform.DOScale(1, 0.5f);
        _notationResizer.UpdateSize(TextToNotate);
        NotationText.text = TextToNotate;
        yield return new WaitForSeconds(0.01f);
    }

    public void DeActivaidNotation()
    {
        transform.DOScale(0, 0.5f);
    }
}
