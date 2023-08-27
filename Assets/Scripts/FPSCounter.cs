using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    private float deltaTime = 0.0f;
    private GUIStyle style;
    private Rect rect;

    private void Start()
    {
        int w = Screen.width, h = Screen.height;
        rect = new Rect(0, 0, w, h * 2 / 100);
        style = new GUIStyle();
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = h * 2 / 100;
        style.normal.textColor = Color.white;

        UpdatePositionInSafeArea();
    }

    private void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
    }

    private void OnGUI()
    {
        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;
        string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
        GUI.Label(rect, text, style);
    }

    private void UpdatePositionInSafeArea()
    {
        Rect safeArea = Screen.safeArea;
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // Offset the safe area by a certain amount from the bottom and right edges
        float xOffset = 10f;
        float yOffset = 500f;

        // Calculate the position within the safe area
        float textWidth = style.CalcSize(new GUIContent("Sample Text")).x; // Calculate width based on your sample text
        float textHeight = style.fontSize; // Height based on the font size

        float posX = screenWidth - safeArea.xMax + xOffset;
        float posY = screenHeight - safeArea.yMax + yOffset + textHeight;

        rect = new Rect(posX, posY, textWidth, textHeight);
    }
}

