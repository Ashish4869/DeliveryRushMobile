using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    /// <summary>
    /// Takes care of the events in the game , the events are :
    /// 
    /// 1.OnPackagePicked - event fired when the package has been picked by the player and the following takes place when the event is fired
    ///     1.Arrow.cs - The arrow functionality is activated
    ///     2.DeliveryManager.cs - The destination for delivery is selected and is brought in scene
    ///     
    /// 2.OnPackageDelivered - Event fired when the package has been delivered to its respective proper destination , following takes when event is fired
    ///     1.Arrow.cs - The arrow is repositioned to spot player cannot see and it not made to follow the player
    ///     2.PlayerMovement.cs - Used to remove the highlight on the car , later should change to some UI change
    ///     
    /// 3.OnPackageOrdered - Event fired when a order is placed on the hotel , following takes place when the event is fired
    ///     1.HotelManager.cs - Spawns the ordered food as package on the scene after some time
    ///     2.FoodPreoLoader.cs - Shows the loading of the food
    ///     
    /// 4.OnShowMapEvent - Event is fired when we press on the tab button , following takes place when the event is fired
    ///     1.PlayerMovment.cs - Player is disabled
    ///     2.MapManager.cs - MiniMap is shown
    ///     
    /// 5.OnOrderingFromRestaurant - Event is fired when the we want to order food from a restuarant , following takes place when the event is fired
    ///     1.HotelUIManager.cs - Shows the UI for buying food
    ///     2.FoodPrepLoader.cs - Gets the information on food that has been ordered and displays as so
    ///     3.MenuHeader.cs - Sets the heading to the approprite heading
    ///     
    /// 6.OnPackageParceled - Event is fired when we pack the food for parcel , following takes place when the event is fired
    ///     1.Hotel.cs - The package is given the right sprite and put in the appropriate place
    ///
    /// 7.OnOrderReceived - Event is fired when a new order is made from a customer , the following happends when the event is fired
    /// 
    /// 8.OnGameOver - Event is called when we have done all the deliveries or we run out of time
    /// 
    /// 9.OnCarTakenTooMuchDamage - event is called when the car health reached zero
    /// </summary>


    public delegate void PackagePicked(string FoodName);
    public static event PackagePicked OnPackagePicked;

    public delegate void PackageDelivered();
    public static event PackageDelivered OnPackageDelivered;

    public delegate void PackageOrdered(FoodPackageSO foodItem);
    public static event PackageOrdered OnPackageOrdered;

    public delegate void PackageParcel(FoodPackageSO FoodID);
    public static event PackageParcel OnPackageParceled;

    public delegate void OrderingFromRestaurant(List<FoodPackageSO> foodItems);
    public static event OrderingFromRestaurant OnOrderingFromRestaurant;

    public delegate void ShowMap();
    public static event ShowMap OnShowMap;

    public delegate void OrderReceived(string OrderDetails , int FoodID);
    public static event OrderReceived OnOrderReceived;

    public delegate void GameOver();
    public static event GameOver OnGameOver;

    public delegate void CarTakenTooMuchDamage();
    public static event CarTakenTooMuchDamage OnCarTakenTooMuchDamage;

    public void OnPackagePickedEvent(string FoodName)
    {
        if (OnPackagePicked != null)
        {
            OnPackagePicked(FoodName);
        }
    }

    public void OnPackageDeliveredEvent()
    {
        if (OnPackageDelivered != null)
        {
            OnPackageDelivered();
        }
    }

    public void OnPackageOrderedEvent(FoodPackageSO foodItem)
    {
        if (OnPackageOrdered != null)
        {
            OnPackageOrdered(foodItem);
        }
    }

    public void OnShowMapEvent()
    {
        if (OnShowMap != null)
        {
            OnShowMap();
        }
    }

    public void OnOrderingfromRestaurantEvent(List<FoodPackageSO> foodItems)
    {
        if(OnOrderingFromRestaurant != null)
        {
            OnOrderingFromRestaurant(foodItems);
        }
    }

    public void OnPackageParceledEvent(FoodPackageSO FoodID)
    {
        if (OnPackageParceled != null)
        {
            OnPackageParceled(FoodID);
        }
    }

    public void OnOrderRecievedEvent(string OrderDetails, int FoodID)
    {
        if(OnOrderReceived != null)
        {
            OnOrderReceived(OrderDetails,FoodID);
        }
    }

    public void OnGameOverEvent()
    {
        if(OnGameOver != null)
        {
            OnGameOver();
        }
    }

    public void OnCarTakenTooMuchDamageEvent()
    {
        if (OnCarTakenTooMuchDamage != null)
        {
            OnCarTakenTooMuchDamage();
        }
    }
}
