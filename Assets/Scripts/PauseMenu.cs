using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject settingsMenu;
    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.SetActive(!pauseMenu.activeSelf);
        }
    }
    public void ResumeGame()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        Debug.Log("thisshouldwork1");
    }
    public void SettingsToggle()
    {
        settingsMenu.SetActive(!settingsMenu.activeSelf);
        Debug.Log("thisshouldwork2");
    }
    public void RestartGame()
    {
        SceneManager.LoadScene("InGame");
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
