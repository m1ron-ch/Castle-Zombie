using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Util
{
    public static void Invoke(this MonoBehaviour mb, Action f, float delay)
    {
        mb.StartCoroutine(InvokeRoutine(f, delay));
    }

    private static IEnumerator InvokeRoutine(System.Action f, float delay)
    {
        yield return new WaitForSeconds(delay);
        f();
    }

    public static float CalculatePercentage(int value, int maxValue, int minValue = 0)
    {
        if (value < minValue)
            return 0.0f;
        else if (value > maxValue)
            return 100.0f;
        else
            return (float)(value - minValue) / (maxValue - minValue);
    }

    public static string FormatNumber(int number)
    {
        List<char> _suffixes = new List<char>() { 'K', 'M', 'B' /* etc */ };
        char? suffix = null;

        for (int i = 0; i < _suffixes.Count; i++)
        {
            if (number >= 1000)
            {
                suffix = _suffixes[i];
                number /= 1000;
            }
            else
            {
                break;
            }
        }

        return string.Format($"{number.ToString("#.##")}{suffix}");
    }
}
