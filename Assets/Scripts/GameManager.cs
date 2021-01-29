using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniGlow.Utility;

public class GameManager : MonoBehaviour
{



    private void OnEnable()
    {
        GameEvents.GameOver += RestartLevel;
    }

    private void OnDisable()
    {
        GameEvents.GameOver -= RestartLevel;
    }



    public void RestartLevel()
    {
        SceneManager.LoadScene(0);
    }
}
