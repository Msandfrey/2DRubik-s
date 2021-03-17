using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalControl : MonoBehaviour 
{
    public static GlobalControl Instance;
    private Color[] colorsToUse;
    void Awake ()   
   {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy (gameObject);
        }
    }

    public void SetColors(Color[] NewColors)
    {
        colorsToUse = NewColors;
    }

    public Color[] GetColors()
    {
        return colorsToUse;
    }
}
