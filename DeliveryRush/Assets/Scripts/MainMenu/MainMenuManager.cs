using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    ///
    /// Deals with Main Menu functions
    ///

    [SerializeField]
    GameObject LevelSelect;

    [SerializeField]
    GameObject MainMenu;

    [SerializeField]
    GameObject CarSelect;

    [SerializeField]
    GameObject Credits;

    [SerializeField]
    Animator MenuMenuUI;

    [SerializeField]
    Animator LevelSelectUI;

    LevelSelectManager _levelSelectManager;
    AudioManager _audioManager;

    private void Awake()
    {
        _audioManager = FindObjectOfType<AudioManager>();
    }



    public void ShowLevelSelect()
    {
        _audioManager.Play("ButtonClick");
        MenuMenuUI.SetBool("ShowMenu", false);
        LevelSelectUI.SetBool("ShowMenu", true);
        _levelSelectManager = FindObjectOfType<LevelSelectManager>(); //Configures the levels based on the score stored in the disk
        _levelSelectManager.ConfigureLevels();
    }

    public void ShowMainMenu()
    {
        _audioManager.Play("ButtonClick");
        LevelSelectUI.SetBool("ShowMenu", false);
        MenuMenuUI.SetBool("ShowMenu", true);
    }

    public void LoadLevel(int level)
    {
        _audioManager.Play("ButtonClick");
        CarSelect.SetActive(true);
        FindObjectOfType<CarSelect>().SetLevel(level);
        MenuMenuUI.SetBool("ShowMenu", false);
        LevelSelectUI.SetBool("ShowMenu", false);
       
    }

    public void LoadCredits()
    {
        _audioManager.Play("ButtonClick");
        Credits.SetActive(true);
        MenuMenuUI.SetBool("ShowMenu", false);
        LevelSelectUI.SetBool("ShowMenu", false);
    }

    public void CloseCredits()
    {
        _audioManager.Play("ButtonClick");
        Credits.SetActive(false);
        MenuMenuUI.SetBool("ShowMenu", true);
        LevelSelectUI.SetBool("ShowMenu", false);
    }

    public void LoadMenu()
    {
        LevelSelectUI.SetBool("ShowMenu", true);
    }

    public void QuitGame()
    {
        _audioManager.Play("ButtonClick");
        Application.Quit();
    }

    public void LoadTutorialLevel()
    {
        FindObjectOfType<Transition>().LoadLevel(5);
        _audioManager.Play("ButtonClick");
        FindObjectOfType<CarData>().SetDefaultCar();
    }

}
