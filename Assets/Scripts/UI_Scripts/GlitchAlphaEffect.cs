using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlitchAlphaEffect : MonoBehaviour
{
    [Header("Image Values")]
    [SerializeField] private Image glitchImage;
    [SerializeField] private float glitchImageMin;
    [SerializeField] private float glitchImageMax;

    [Header("Time value")]
    [SerializeField] float speed;

    private float timer = 0;

    private bool isMovingToAlphaMin = true;

    private void Update()
    {
        if (isMovingToAlphaMin)
        {
            timer += speed * Time.deltaTime;
        }
        else
        {
            timer -= speed * Time.deltaTime;
        }

        float aux = Mathf.Lerp(glitchImageMin, glitchImageMax, timer);

        glitchImage.color = new Color(1f, 1f, 1f, aux);

        if (aux >= glitchImageMax)
        {
            isMovingToAlphaMin = false;
        }
        else if (aux <= glitchImageMin)
        {
            isMovingToAlphaMin = true;
        }
    }
}