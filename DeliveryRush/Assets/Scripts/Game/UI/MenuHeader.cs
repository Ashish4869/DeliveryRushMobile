using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuHeader : MonoBehaviour
{
    /// <summary>
    /// Assigns the header of the menu
    /// </summary>


    [SerializeField]
    TextMeshProUGUI _headerText;

    // Start is called before the first frame update
    void Start()
    {
        EventManager.OnOrderingFromRestaurant += SetHeader;
    }

    void SetHeader(List<FoodPackageSO> fooditem)
    {
        string hotelName;

        Hotel[] hotels = FindObjectsOfType<Hotel>();

        foreach (Hotel hotel in hotels)
        {
            if(hotel.GetHotelID() == fooditem[0].GetFoodID()/10)
            {
                hotelName = hotel.GetHotelName();
                _headerText.text = "Welcome To " + hotelName;
                return;
            }
        }
        
    }

    private void OnDestroy()
    {
        EventManager.OnOrderingFromRestaurant -= SetHeader;
    }
}
