using DG.Tweening;
using System;
using TMPro;
using UnityEngine;

public class NotationHandler : MonoBehaviour
{
    public TextMeshProUGUI NotationText;
    public void ActivaidNotation(string TextToNotate) 
    {
        transform.DOScale(1, 0.5f);
        NotationText.text = TextToNotate;
        
    }

    public void DeActivaidNotation()
    {
        transform.DOScale(0, 0.5f);
    }
}
