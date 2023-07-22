using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Timer : MonoBehaviour
{
    [SerializeField] private TMP_Text _clockFace;
    
    private int _minutes;
    private int _seconds;

    public IEnumerator Countdown(int minutes, int seconds)
    {
        _minutes = minutes;
        _seconds = seconds;

        DisplayTime();

        while (true)
        {
            if (_seconds == 0)
            {
                if (_minutes == 0)
                {
                    yield break;
                }

                _seconds = 60;
                _minutes--;
            }

            _seconds--;
            DisplayTime();

            yield return new WaitForSeconds(1f);
        }
    }

    private void DisplayTime()
    {
        string seconds = _seconds >= 10 ? _seconds.ToString() : $"0{_seconds}";
        string minutes = _minutes >= 10 ? _minutes.ToString() : $"0{_minutes}";

        _clockFace.text = $"{minutes}:{seconds}";
    }
}