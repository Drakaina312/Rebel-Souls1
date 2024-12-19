using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TextResizer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textToResize;
    [SerializeField] private RectTransform _textArea;
    [SerializeField] private float _padding;

    public void UpdateSize(string fullText)
    {
        //_textToResize.text = "";
        //_textToResize.text = fullText;
        //int lineCount = _textToResize.textInfo.lineCount;
        int lineCount = fullText.Length / 25;
        //_textToResize.text = "";
        Debug.Log(lineCount);

        if (lineCount > 3)
        {
            int resizeModifare = (lineCount - 3) * -75;
            //Rect myRect = _textArea.rect;
            //myRect.yMax = resizeModifare;
            //_textArea.anchoredPosition = new Vector2(_textArea.anchoredPosition.x, resizeModifare);
            _textArea.offsetMin = new Vector2(_textArea.anchoredPosition.x, resizeModifare);
        }
        else
        {
            _textArea.offsetMin = new Vector2(_textArea.anchoredPosition.x, 0);
            //_textArea.sizeDelta = new Vector2(_textArea.sizeDelta.x, textSize.y + _padding);
        }
    }
}

