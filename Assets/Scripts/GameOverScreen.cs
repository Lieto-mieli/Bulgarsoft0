using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//C
public class GameOverScreen : MonoBehaviour
{
    public Text pointsText;

    public void Quit()
    {
        Application.Quit();
        Debug.Log("MENE POIS");
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void Restart()
    {
        SceneManager.LoadScene("InGame");
    }
    public void setup(int score)
    {

    }
}
//C