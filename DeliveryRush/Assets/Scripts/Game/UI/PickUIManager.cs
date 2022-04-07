using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickUIManager : MonoBehaviour
{
    /// <summary>
    /// Shows the UI if the food has been selected
    /// </summary>

    [SerializeField]
    Image _FoodInCar;

    Sprite _foodImage;

    [SerializeField]
    GameObject PickupUI;


    // Start is called before the first frame update
    void Start()
    {
        EventManager.OnPackageDelivered += RemoveUI;
    }

    public void SetSprite(Sprite sprite)
    {
        _foodImage = sprite;
    }

    public void SetUpUI()
    {
        PickupUI.SetActive(true);
        _FoodInCar.sprite = _foodImage;
    }

    public void RemoveUI()
    {
        PickupUI.SetActive(false);
    }

    private void OnDestroy()
    {
        EventManager.OnPackageDelivered -= RemoveUI;
    }



}
