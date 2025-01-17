using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] private Image _timerImage;
    public event Action OnTimerComplete;


    public void ActivateTimer(float timerValue)
    {
        Debug.Log("Таймер активирован");
        _timerImage.transform.parent.gameObject.SetActive(true);
        StartCoroutine(TimerCoroutine(timerValue));
    }

    public void StopTimer()
    {
        _timerImage.transform.parent.gameObject.SetActive(false);
        StopAllCoroutines();
    }

    private IEnumerator TimerCoroutine(float timerValue)
    {
        Debug.Log(timerValue + " время таймера ");
        float startTime = 0;
        yield return new WaitForSeconds(2);
        while (startTime < timerValue)
        {
            startTime += Time.deltaTime;
            Debug.Log(startTime);
            _timerImage.fillAmount = (timerValue - (timerValue - startTime)) / timerValue;
            yield return null;
        }
        _timerImage.transform.parent.gameObject.SetActive(false);
        OnTimerComplete?.Invoke();
    }
}
