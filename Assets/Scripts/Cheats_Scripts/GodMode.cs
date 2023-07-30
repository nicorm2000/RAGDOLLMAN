using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodMode : MonoBehaviour
{
    [Header("Player Controller Configuration")]
    [SerializeField] PlayerController playerController;

    public bool godMode = false;

    /// <summary>
    /// Toggles the god mode ability on or off, adjusting the player's capability of dying or not.
    /// </summary>
    public void ToggleGodMode()
    {
        godMode = !godMode;
    }
}
