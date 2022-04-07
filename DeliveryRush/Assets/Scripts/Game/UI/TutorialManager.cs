using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    /// <summary>
    /// Deals with Tutorial showing and all
    /// </summary>

    [SerializeField] GameObject TutorialComponent;
    [SerializeField] TextMeshProUGUI TutorialHeader;
    [SerializeField] TextMeshProUGUI TutorialBody;

    public void ShowTutorial(string Tutorialheader , string Tutorialbody)
    {
        TutorialComponent.SetActive(true);
        TutorialHeader.text = Tutorialheader;
        TutorialBody.text = Tutorialbody;
        Time.timeScale = 0;
    }

    public void HideTutorial()
    {
        FindObjectOfType<AudioManager>().Play("ClickSound");
        Time.timeScale = 1;
        TutorialComponent.SetActive(false);
    }
}
