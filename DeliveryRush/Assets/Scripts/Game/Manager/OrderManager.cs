using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderManager : MonoBehaviour
{

    /// <summary>
    /// Deals the order system in the game
    /// </summary>
    
    [SerializeField]
    int[] FoodItemAmount;

    [SerializeField]
    FoodPackageSO[] FoodItems;

    int _allOrders;
    int _currentOrderCount = 0;
    float _timebetweenOrderNotification;
    [SerializeField] int _timeLowerBound;
    [SerializeField] int _timeHigherBound;
    string _timeStamp;
    string _OrderDetails;

    int prev = -1;

    Clock gameClock;
    EventManager _eventManager;
    OrderLogManager _orderLogManager;
    GameManager gameManager;

    Dictionary<FoodPackageSO, int> Orders = new Dictionary<FoodPackageSO, int>(); //stores all the orders and the amount of orders in them
   string[] _hotelNames = new string[10];

    private void Awake()
    {
        _eventManager = FindObjectOfType<EventManager>();
        gameClock = FindObjectOfType<Clock>();
        gameManager = FindObjectOfType<GameManager>();

        Hotel[] hotels = FindObjectsOfType<Hotel>();

        foreach (Hotel hotel in hotels)
        {
            _hotelNames[hotel.GetHotelID()] = hotel.GetHotelName();
        }
    }

    private void Start()
    {

        //Create a dictionary for all the food items
        for(int i = 0; i < FoodItemAmount.Length; i++)
        {
            Orders.Add(FoodItems[i], FoodItemAmount[i]);
            _allOrders += FoodItemAmount[i];
        }

        //sending the data of the food in this level
        gameManager.GetFoodItemsOnLevel(FoodItems);


        //Initializing values
        _timebetweenOrderNotification = Random.Range(5, 10);

        _orderLogManager = FindObjectOfType<OrderLogManager>();
    }

    private void Update()
    {
        ProcessOrders();
    }


    void ProcessOrders()
    {
        if(_timebetweenOrderNotification <= 0)
        {
            NotifyOrder();
        }
        else
        {
            _timebetweenOrderNotification -= Time.deltaTime;
        }
    }
    
    void NotifyOrder()
    {
        // if all the orders have been processed disable game object
        if(_currentOrderCount >= _allOrders)
        {
            _orderLogManager.NoMoreOrder();
            gameObject.SetActive(false);
            return;
        }

        int rand = RandomizeWithoutRepeating();

        //if we dont have any orders on that item , get another order
        if (Orders[FoodItems[rand]] == 0)
        {
            _timebetweenOrderNotification = 0.5f;
            return;
        }

        _timeStamp = gameClock.GetTime();
        
        //pass the time that the food had been ordered to the order log
        int foodID = FoodItems[rand].GetFoodID() / 10;
        _OrderDetails = _timeStamp + "-" + FoodItems[rand].GetFoodName() + "-" + _hotelNames[FoodItems[rand].GetFoodID() / 10];
        _eventManager.OnOrderRecievedEvent(_OrderDetails , foodID);

        


        Orders[FoodItems[rand]]--;
        _currentOrderCount++;
        _timebetweenOrderNotification = Random.Range(_timeLowerBound, _timeHigherBound);
    }

    int RandomizeWithoutRepeating()
    {
        int rand = Random.Range(0, FoodItems.Length);

        if (prev == rand)
        {
            rand = Random.Range(0, FoodItems.Length);
        }

        prev = rand;

        return rand;
    }

    public FoodPackageSO[] GetFoodItems()
    {
        return FoodItems;
    }

    private void OnDestroy()
    {
        Orders.Clear();
        gameManager.ClearOrderCount();
        
    }
}
