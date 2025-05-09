using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    //c
    public static GameController Instance;
    public ValueTracker valuetracker;

    
    //void Awake()
    //{
    //    // Ensure there is only one GameController in the scene
    //    if (Instance == null)
    //    {
    //        Instance = this;
    //    }
    //    else
    //    {
    //        Destroy(gameObject); // Destroy any duplicate GameController instances
    //    }
    //}

    //// Example method for GameOver
    //public void GameOver()
    //{
    //    // Replace this with actual logic for when the game is over
    //    SceneManager.LoadScene("EndMenu");
    //}
}
