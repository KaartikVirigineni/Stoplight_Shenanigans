using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLightController : MonoBehaviour
{
     private Renderer rend;
    private Color originalColor;
    public bool isRed = true;
    public bool isGreen = false;

    void Start()
    {
        rend = GetComponent<Renderer>();
        originalColor = rend.material.color;
    }

    void OnMouseDown()
    {
        if (isRed)
        {
            rend.material.color = Color.green;
            isRed = false;
            isGreen = true;
        }
        else if (isGreen)
        {
            rend.material.color = Color.red;
            isRed = true;
            isGreen = false;
        }
    }
}
