using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPlayerProgress : MonoBehaviour
{
    public int minValue = 0;
    public int maxValue = 120;
    public int currentValue = 13;

    private void Awake()
    {
        CalculateAndDisplayPercentage();
    }

    void CalculateAndDisplayPercentage()
    {
        double completionPercentage = CalculatePercentage(currentValue, minValue, maxValue);
        var completionText = $"Выполнено: {completionPercentage}%";
        Debug.Log(completionText);
    }

    double CalculatePercentage(int currentValue, int minValue, int maxValue)
    {
        if (currentValue < minValue)
            return 0.0;
        else if (currentValue > maxValue)
            return 100.0;
        else
            return (double)(currentValue - minValue) / (maxValue - minValue);
    }

    private void NextLevel()
    {

    }
}
