using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorDropdown : MonoBehaviour
{
    //red, blue, yellow, orange, green, white
    private Color[] Normal = { new Color(0.8627451f, 0f, 0f, 1f), new Color(0f, 0f, 0.8627451f, 1f), new Color(1f, 0.9215686f, 0.01568628f, 1f), new Color(0.9411765f, 0.5f, 0f, 1f), new Color(0f, 0.8627451f, 0f, 1f), Color.white};
    private Color[] Black = { new Color(0.8627451f, 0f, 0f, 1f), new Color(0f, 0f, 0.8627451f, 1f), new Color(1f, 0.9215686f, 0.01568628f, 1f), new Color(0.9411765f, 0.5f, 0f, 1f), new Color(0f, 0.8627451f, 0f, 1f), new Color(.1f,.1f,.1f)};
    private Color[] Bright = { Color.red, Color.blue, Color.yellow, Color.cyan, Color.green, Color.magenta };

    private Color[][] ColorOptions;

    public void Start()
    {
        ColorOptions = new Color[][] { Normal, Black, Bright};
    }
    //val 0 is normal
    //val 1 is black
    //val 2 is neon
    public void SetColors(int colorIndex)
    {
        FindObjectOfType<GlobalControl>().SetColors(ColorOptions[colorIndex]);
    }
}
