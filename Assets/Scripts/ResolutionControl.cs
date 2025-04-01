using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResolutionControl : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    private Resolution[] resolutions;
    private List<Resolution> filteredResolutions;

    private float currentRefreshRate;
    private int currentResolutionIndex = 0;
    public int windowType;
    void Start()
    {
        resolutions = Screen.resolutions;
        filteredResolutions = new List<Resolution>();

        resolutionDropdown.ClearOptions();
        currentRefreshRate = (float)Screen.currentResolution.refreshRateRatio.value;
        //foreach (var res in resolutions)
        //{
        //    Debug.Log(res.width + "x" + res.height + " : " + res.refreshRateRatio);
        //}
        //Debug.Log((float)Screen.currentResolution.refreshRateRatio.value);
        //Debug.Log(resolutions.Length);
        //for (int i = 0; i < resolutions.Length; i++)
        //{
        //    //Debug.Log((float)resolutions[i].refreshRate);
        //    if ((float)resolutions[i].refreshRateRatio.value == currentRefreshRate)
        //    {
        //        filteredResolutions.Add(resolutions[i]);
        //    }
        //}

        //filteredResolutions.Sort((a, b) =>
        //{
        //    if (a.width != b.width)
        //        return b.width.CompareTo(a.width);
        //    else
        //        return b.height.CompareTo(a.height);
        //});

        List<string> options = new List<string>();
        //Debug.Log(filteredResolutions.Count);
        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            string resolutionOption = Screen.resolutions[i].width + "x" + Screen.resolutions[i].height + " " + Screen.resolutions[i].refreshRateRatio.value.ToString("0.##") + " Hz";
            options.Add(resolutionOption);
            if (Screen.resolutions[i].width == Screen.width && Screen.resolutions[i].height == Screen.height && (float)Screen.resolutions[i].refreshRateRatio.value == currentRefreshRate)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex = 0;
        resolutionDropdown.RefreshShownValue();
        SetResolution(currentResolutionIndex);
    }

    public void SetResolution(int resolutionIndex)
    {
        //Debug.Log(resolutionIndex);
        Resolution resolution = Screen.resolutions[resolutionIndex];
        if (windowType == 0)
        {
            Screen.SetResolution(resolution.width, resolution.height, false);
        }
        if (windowType == 2)
        {
            Screen.SetResolution(resolution.width, resolution.height, true);
        }
        PlayerPrefs.SetInt("WindowWidth", resolution.width);
        PlayerPrefs.SetInt("WindowHeight", resolution.height);  
    }
}