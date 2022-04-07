using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSoundController : MonoBehaviour
{
    /// <summary>
    /// Deals with audio when the menu is accessed
    /// </summary>

    AudioManager _audioManager;

    private void Awake()
    {
        _audioManager = FindObjectOfType<AudioManager>();
        EventManager.OnOrderingFromRestaurant += OpenUpMenu;
        EventManager.OnPackageParceled += CloseMenu;
        EventManager.OnPackageOrdered += PlacedOrder;
    }

    void OpenUpMenu(List<FoodPackageSO> foodItems) //arguements are useless 
    {
        PlaySound("MenuOn");
    }

    void PlaySound(string name)
    {
        _audioManager.Play(name);
    }

    void PlacedOrder(FoodPackageSO FoodID)
    {
        _audioManager.Play("PlaceOrder");
    }

    public void CloseMenu(FoodPackageSO FoodID)
    {
        _audioManager.Play("MenuClose");
    }

    private void OnDestroy()
    {
        EventManager.OnOrderingFromRestaurant -= OpenUpMenu;
        EventManager.OnPackageParceled -= CloseMenu;
        EventManager.OnPackageOrdered -= PlacedOrder;
    }

}
