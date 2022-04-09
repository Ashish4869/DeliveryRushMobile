using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hotel : MonoBehaviour
{
    /// <summary>
    /// Gets the Data of the food to be spawned and sends the data to the hotel manager
    /// </summary>
   
    [Header("Hotel Information")]
    [SerializeField]
    int _HotelID;


    [SerializeField]
    GameObject Order;

    [SerializeField]
    GameObject Map;


    [SerializeField]
    string _hotelName;

    [SerializeField]
    private List<FoodPackageSO> _foodItems;

    [Header("Related Components")]
    [SerializeField]
    Transform _packageSpawnPoint;

    EventManager _eventManager;
    HotelManager _hotelManager;

    bool _CanOrder = false;

    private void Awake()
    {
        _hotelManager = GetComponentInParent<HotelManager>();
        _eventManager = FindObjectOfType<EventManager>();
        EventManager.OnPackageParceled += SendParcel;
        
    }
    public int GetHotelID()
    {
        return _HotelID;
    }

    public string GetHotelName() => _hotelName;

    private void Update()
    {
        /*
        if(Input.GetKeyDown(KeyCode.E) && _CanOrder)
        {
            _eventManager.OnOrderingfromRestaurantEvent(_foodItems);
            _CanOrder = false;
        }
        */
    }

    //Gets the info and sends to the hotelmanager script
    void SendParcel(FoodPackageSO Food)
    {
        int HotelID = Food.GetFoodID() / 10;

        if(HotelID == _HotelID)
        {
            _hotelManager.InitiateOrder(_packageSpawnPoint.position , Food);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _CanOrder = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _CanOrder = false;
    }

    public void CanOrder() => _CanOrder = true; 
    public void CantOrder() => _CanOrder = false;

    private void OnDestroy()
    {
        EventManager.OnPackageParceled -= SendParcel;
    }

    public void ShowMenu()
    {
        _eventManager.OnOrderingfromRestaurantEvent(_foodItems);
        _CanOrder = false;
    }

}
