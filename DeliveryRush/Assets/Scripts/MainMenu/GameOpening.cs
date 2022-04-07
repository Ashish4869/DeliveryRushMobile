using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOpening : MonoBehaviour
{
    /// <summary>
    /// Deals with the actions of game Opening
    /// </summary>

    [SerializeField] GameObject NPC;
    [SerializeField] Animator heading;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.anyKeyDown)
        {
            heading.SetTrigger("OPEN");
            GetComponent<Animator>().SetTrigger("OPEN");
            NPC.SetActive(true);
            FindObjectOfType<AudioManager>().Play("START");
            Invoke("Disable", 1.5f);
        }
    }

    private void Disable()
    {
        gameObject.SetActive(false);
    }
}
