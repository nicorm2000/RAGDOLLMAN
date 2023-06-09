﻿using TMPro;
using UnityEngine;


//-------------------------------------------------------------
    //--APR Player
    //--FPSCounter
    //
    //--Unity Asset Store - Version 1.0
    //
    //--By The Famous Mouse
    //
    //--Twitter @FamousMouse_Dev
    //--Youtube TheFamouseMouse
    //-------------------------------------------------------------


namespace ARP.Examples.Dependencies.Scripts
{
    public class FPSCounter : MonoBehaviour
    {
        private TextMeshProUGUI fpsText;
    
        float frameCount = 0f, dt = 0f, fps = 0f, updateRate = 4f;
    
        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        
            fpsText = GetComponent<TMPro.TextMeshProUGUI>();
        }

        void Update()
        {
            frameCount++;
            dt += Time.deltaTime;
            if (dt > 1.0f/updateRate)
            {
                fps = frameCount / dt ;
                frameCount = 0f;
                dt -= 1.0f/updateRate;
            
                fps = Mathf.RoundToInt(fps);
                fpsText.text = "50 APR Players - FPS: " + fps.ToString();
            }
        }
    }
}
