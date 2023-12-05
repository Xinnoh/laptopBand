using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorCycle : MonoBehaviour
{
    public Color[] colors; // Array of colors to cycle through
    public float transitionTime = 2f; // Time it takes to transition to the next color

    private SpriteRenderer spriteRenderer;
    private int currentColorIndex = 0;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer component not found on the object.");
            return;
        }

        if (colors.Length == 0)
        {
            Debug.LogError("No colors defined in the colors array.");
            return;
        }

        StartCoroutine(CycleColors());
    }

    IEnumerator CycleColors()
    {
        while (true)
        {
            Color startColor = spriteRenderer.color;
            Color endColor = colors[currentColorIndex];

            for (float t = 0; t < 1; t += Time.deltaTime / transitionTime)
            {
                spriteRenderer.color = Color.Lerp(startColor, endColor, t);
                yield return null;
            }

            spriteRenderer.color = endColor;
            currentColorIndex = (currentColorIndex + 1) % colors.Length;
        }
    }
}
