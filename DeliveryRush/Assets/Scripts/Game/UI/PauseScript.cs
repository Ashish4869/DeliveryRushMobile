using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseScript : MonoBehaviour
{
    /// <summary>
    /// Pauses the game and loads the pause screen
    /// </summary>

    [SerializeField]    
    GameObject PauseMenu;

    private void Update()
    {
        if(Input.GetKey(KeyCode.Escape))
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        PauseMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        PauseMenu.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
        FindObjectOfType<Transition>().LoadLevel(0);
    }


}
