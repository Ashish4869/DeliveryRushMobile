using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delivery : MonoBehaviour
{
    ///<summary>
    ///deals with the delivery of the food to customer and triggers event package has been delivered
    ///</summary>

    GameManager gameManager;
    EventManager eventManager;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        eventManager = FindObjectOfType<EventManager>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            FindObjectOfType<AudioManager>().Play("PackageDelivered");
            eventManager.OnPackageDeliveredEvent();
        }
        gameManager.RepositionElement(this.gameObject);
    }

    
}
