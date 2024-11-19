using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class NotationHandler : MonoBehaviour
{
    public TextMeshProUGUI NotationText;
    [SerializeField] private TextResizer _notationResizer;
    public IEnumerator ActivaidNotation(string TextToNotate) 
    {
        transform.DOScale(1, 0.5f);
        NotationText.text = TextToNotate;
        yield return new WaitForSeconds(0.01f);
        _notationResizer.UpdateSize();
        
    }

    public void DeActivaidNotation()
    {
        transform.DOScale(0, 0.5f);
    }
}
