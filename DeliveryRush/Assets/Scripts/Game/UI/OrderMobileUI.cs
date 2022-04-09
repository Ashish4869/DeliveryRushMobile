using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderMobileUI : MonoBehaviour
{
    /// <summary>
    /// Says which hotel to show
    /// </summary>

    EventManager _eventManager;
    
    List<FoodPackageSO> Fooditems;

    private void Awake()
    {
        _eventManager = FindObjectOfType<EventManager>();
    }
    public void ShowMenu()
    {
        _eventManager.OnOrderingfromRestaurantEvent(Fooditems);
    }

    public void SendHotelData(List<FoodPackageSO> _fooditems)
    {
        Fooditems = _fooditems;
    }


}
