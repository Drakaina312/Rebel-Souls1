using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TextResizer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textToResize;
    [SerializeField] private RectTransform _textArea;
    [SerializeField] private float _padding;
    public void UpdateSize()
    {
        Vector2 textSize = _textToResize.GetPreferredValues();
        int lineCount = _textToResize.textInfo.lineCount;
        if (lineCount > 3)
        {
            int resizeModifare = (lineCount - 3) * -80;
            //Rect myRect = _textArea.rect;
            //myRect.yMax = resizeModifare;
            //_textArea.anchoredPosition = new Vector2(_textArea.anchoredPosition.x, resizeModifare);
            _textArea.offsetMin = new Vector2(_textArea.anchoredPosition.x, resizeModifare);
        }
        
        //_textArea.sizeDelta = new Vector2(_textArea.sizeDelta.x, textSize.y + _padding);
        


    }
    public void UpdateSize(string  fullText)
    {
        _textToResize.text = fullText;
        Vector2 textSize = _textToResize.GetPreferredValues();
        int lineCount = _textToResize.textInfo.lineCount;
        _textToResize.text = "";
        if (lineCount > 3)
        {
            int resizeModifare = (lineCount - 3) * -80;
            //Rect myRect = _textArea.rect;
            //myRect.yMax = resizeModifare;
            //_textArea.anchoredPosition = new Vector2(_textArea.anchoredPosition.x, resizeModifare);
            _textArea.offsetMin = new Vector2(_textArea.anchoredPosition.x, resizeModifare);
        }

        //_textArea.sizeDelta = new Vector2(_textArea.sizeDelta.x, textSize.y + _padding);



    }
}

