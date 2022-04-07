using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameOverManager : MonoBehaviour
{
    /// <summary>
    /// Deals with Ending the game prematurely if car has taken too much damage
    /// </summary>

    [SerializeField]
    GameObject GameOverUI;

    private void Start()
    {
        EventManager.OnCarTakenTooMuchDamage += GameOver;
    }

    public void GameOver()
    {
        StartCoroutine(WaitSomeTime());
    }

    IEnumerator WaitSomeTime()
    {
        yield return new WaitForSeconds(2f);
        GameOverUI.SetActive(true);
    }

    public void ReplayLevel()
    {
        FindObjectOfType<AudioManager>().Play("ButtonClick");
        FindObjectOfType<Transition>().LoadLevel(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        FindObjectOfType<AudioManager>().Play("ButtonClick");
        FindObjectOfType<Transition>().LoadLevel(0);
    }

    private void OnDestroy()
    {
        EventManager.OnCarTakenTooMuchDamage -= GameOver;
    }

}
