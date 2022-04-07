using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HotelUIManager : MonoBehaviour
{
    /// <summary>
    /// Shows the UI and sets the order for delivery
    /// 
    /// NOTE : The gameobjects are destroyed cause the value are different for each hotel , it would be difficult to edit them as the values
    /// and quantity is different ,so they are destroyed and rebuilt whenever needed.
    /// </summary>

    [SerializeField]
    GameObject FoodItemPrefab;

    [SerializeField]
    GameObject HotelUI;

    
    EventManager _eventManager;
    GameObject FoodItemList;
    FoodPrepLoader foodPrepLoader;
    List<FoodPackageSO> CurrentfoodItems;

    bool[,] ButtonActivityStatus = new bool[10,5]; //matrix that contains the info as to which buttons can be interactable and which cannot be

    void Start()
    {
        EventManager.OnOrderingFromRestaurant += ShowMenu;
       
        _eventManager = FindObjectOfType<EventManager>();
        foodPrepLoader = GetComponent<FoodPrepLoader>();

        for(int i = 1; i<=9; i++)
        {
            for(int j = 1; j<=4; j++)
            {
                ButtonActivityStatus[i,j] = true; //initializing all values to true
            }
        }
    }

    //Shows the UI on the event
    void ShowMenu(List<FoodPackageSO> foodItems)
    {
        CurrentfoodItems = foodItems;

        if(HotelUI != null)
        {
            HotelUI.SetActive(true);

            //Sets up the menu as per the food in the hotel
            ConfigureMenu(foodItems);

            //Sets the loading animation of the order status
            foodPrepLoader.LoadingAnimation(CurrentfoodItems);
        }
    }

    void ConfigureMenu(List<FoodPackageSO> FoodItems)
    {
        FoodItemList = GameObject.Find("HotelOrderUI/PlaceOrderComponent/FoodItemList");

        //instantiates a gameobject for food item that the hotel has
        foreach (var Fooditem in FoodItems)
        {
            GameObject child = Instantiate(FoodItemPrefab);
            child.transform.SetParent(FoodItemList.transform);
            child.transform.localScale = new Vector3(1, 1, 1);

            Image[] images = child.GetComponentsInChildren<Image>();

            //Set the food sprites
            foreach (var image in images)
            {
                if (image.color == Color.black)
                {
                    //do nothing
                }
                else
                {
                    image.sprite = Fooditem.GetFoodSprite();
                }
            }

            //set the food text
            TextMeshProUGUI foodDesc = child.GetComponentInChildren<TextMeshProUGUI>();
            foodDesc.text = Fooditem.GetFoodDesc();

            //set the button
            Button itemButton = child.GetComponent<Button>();
            itemButton.onClick.AddListener(delegate { Order(Fooditem); });

            //set the activity status
            int i = Fooditem.GetFoodID() / 10;
            int j = Fooditem.GetFoodID() % 10;

            if (ButtonActivityStatus[i, j] != true)
            {
                itemButton.interactable = false;
            }
        }
    }

    //Gives the food for order
    public void Order(FoodPackageSO fooditem)
    {
        DeactivateButton(fooditem);
        _eventManager.OnPackageOrderedEvent(fooditem);
    }

    //Deactivates the button which was clicked
    void DeactivateButton(FoodPackageSO fooditem)
    {
        int i = fooditem.GetFoodID() / 10;
        int j = fooditem.GetFoodID() % 10;

        ButtonActivityStatus[i,j] = false;

        RefreshUI();
    }

    //Refresh the ui so that ButtonActivityStatus values are maintained
    void RefreshUI()
    {
        foreach (Transform item in FoodItemList.transform)
        {
            Destroy(item.gameObject);
        }

        ShowMenu(CurrentfoodItems);
    }

    //Hide - destroy all gameobjects and gameobjects of order status
    public void HideMenu()
    {
        foreach (Transform item in FoodItemList.transform)
        {
            Destroy(item.gameObject);
        }

        foodPrepLoader.Destroyfood();
        HotelUI.SetActive(false);

        AllowReOrdering();
    }

    public void AllowReOrdering()
    {
        Hotel[] hotels = FindObjectsOfType<Hotel>();

        foreach (var Hotel in hotels)
        {
            if(CurrentfoodItems[0].GetFoodID()/10 == Hotel.GetHotelID())
            {
                Hotel.CanOrder();
            }
        }
    }
    

    //Activate Buttons and hide menu when the pack the food
    public void ActivateButton(FoodPackageSO fooditem)
    {
        int i = fooditem.GetFoodID() / 10;
        int j = fooditem.GetFoodID() % 10;

        ButtonActivityStatus[i, j] = true;
        HideMenu();
    }

    private void OnDestroy()
    {
        EventManager.OnOrderingFromRestaurant += ShowMenu;
    }

}
