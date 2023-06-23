using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenURL : MonoBehaviour
{
    /// <summary>
    /// Opens a website link using the provided URL. The function calls the Application.OpenURL method, which opens the URL in the default browser.
    /// </summary>
    /// <param name="url">The URL of the website to be opened.</param>
    public void OpenLink(string url)
    {
        Application.OpenURL(url);
    }
}