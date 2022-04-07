using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCollider : MonoBehaviour
{
    /// <summary>
    /// Triggers all the tutorials that are triggers with triggers or colliders
    /// </summary>
    /// 

    TutorialEventManager tutorialEventManager;

    private void Awake()
    {
        tutorialEventManager = FindObjectOfType<TutorialEventManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string name = collision.gameObject.name;
        tutorialEventManager.SetTutorial(name);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        string name = collision.gameObject.name;
        tutorialEventManager.SetTutorial(name);
    }
}
