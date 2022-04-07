using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEventManager : MonoBehaviour
{
    /// <summary>
    /// Deals with calling Tutorial UI when the needed
    /// </summary>

    [SerializeField] string[] TutorialHeaders;
    [TextArea(5,10)][SerializeField] string[] TutorialContent;

    TutorialManager _tutorialManager;

    private void Awake()
    {
        _tutorialManager = FindObjectOfType<TutorialManager>();
    }


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ShowMovementTutorial());
    }

    IEnumerator ShowMovementTutorial()
    {
        yield return new WaitForSeconds(1f);
        _tutorialManager.ShowTutorial(TutorialHeaders[0], TutorialContent[0]);
    }

   public void SetTutorial(string header)
   {
        if (Array.IndexOf(TutorialHeaders, header) != -1)
        {
            FindObjectOfType<AudioManager>().Play("ClickSound");
            int index = Array.IndexOf(TutorialHeaders, header);
            _tutorialManager.ShowTutorial(TutorialHeaders[index], TutorialContent[index]);
            TutorialHeaders[index] = "";
        }
   }
}
