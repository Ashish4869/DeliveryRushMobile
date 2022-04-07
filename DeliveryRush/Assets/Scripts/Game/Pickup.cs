using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    ///<summary>
    ///This script triggers the event that a pickup as been obtained
    ///</summary>

    EventManager eventManager;
    GameManager gameManager;
    OrderLogManager _orderLogManager;
    PickUIManager pickUIManager;

    string _foodName = "";

    private void Awake()
    {
        eventManager = FindObjectOfType<EventManager>();
        gameManager = FindObjectOfType<GameManager>();
        _orderLogManager = FindObjectOfType<OrderLogManager>();
        pickUIManager = FindObjectOfType<PickUIManager>();
    }
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            FindObjectOfType<AudioManager>().Play("PickOrder");

            //shows the food that we have picked
            pickUIManager.SetUpUI();

            //stores the food obtained in game manager
            StoreObtainedFood();

            //initiates the navigation if order is present in the order list
            if(IsInOrdersList())
            {
                eventManager.OnPackagePickedEvent(_foodName);
            }
            
            //Reposioning element after picking it up
            gameManager.RepositionElement(this.gameObject);
        }
    }

    void StoreObtainedFood()
    {
        gameManager.CurrentPackage(_foodName);
    }

    public void SetFoodName(string FoodName)
    {
        _foodName = FoodName;
    }

    bool IsInOrdersList() => _orderLogManager.CheckIfInOrderList();


}
