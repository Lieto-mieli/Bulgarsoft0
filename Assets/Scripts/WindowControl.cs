using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowControl : MonoBehaviour
{
    [SerializeField] private CanvasGroup resolutionDropdown;
    [SerializeField] private GameObject resolutionManager;
    private void Start()
    {
        if (PlayerPrefs.HasKey("WindowType"))
        {
            ChangeType(PlayerPrefs.GetInt("WindowType"));
        }
    }
    public void ChangeType(int index)
    {
       if (index == 0) { SetWindowed(); }
       if (index == 1) { SetFullscreen(); }
       if (index == 2) { SetBorderless(); }
    }
    public void SetWindowed()
    {
        if (PlayerPrefs.HasKey("WindowWidth")) { Screen.SetResolution(PlayerPrefs.GetInt("WindowWidth"), PlayerPrefs.GetInt("WindowHeight"), FullScreenMode.Windowed, Screen.currentResolution.refreshRateRatio); }
        else { Screen.SetResolution(Display.main.systemWidth, Display.main.systemHeight, FullScreenMode.Windowed, Screen.currentResolution.refreshRateRatio); }
        resolutionDropdown.interactable = true;
        resolutionManager.GetComponent<ResolutionControl>().windowType = 0;
        PlayerPrefs.SetInt("WindowType", 0);
    }
    public void SetFullscreen()
    {
        Screen.SetResolution(Display.main.systemWidth, Display.main.systemHeight, FullScreenMode.ExclusiveFullScreen);
        resolutionDropdown.interactable = false;
        resolutionManager.GetComponent<ResolutionControl>().windowType = 1;
        PlayerPrefs.SetInt("WindowType", 1);
    }
    public void SetBorderless()
    {
        if (PlayerPrefs.HasKey("WindowWidth")) { Screen.SetResolution(PlayerPrefs.GetInt("WindowWidth"), PlayerPrefs.GetInt("WindowHeight"), FullScreenMode.FullScreenWindow, Screen.currentResolution.refreshRateRatio); }
        else { Screen.SetResolution(Display.main.systemWidth, Display.main.systemHeight, FullScreenMode.FullScreenWindow, Screen.currentResolution.refreshRateRatio); }
        resolutionDropdown.interactable = true;
        resolutionManager.GetComponent<ResolutionControl>().windowType = 2;
        PlayerPrefs.SetInt("WindowType", 2);
    }
}