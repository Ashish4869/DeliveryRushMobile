using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class FoodPrepLoader : MonoBehaviour
{
    /// <summary>
    /// For updating the Loading bar , making it consistent throughout all hotels and then making it possible to pack the food to parcel
    /// 
    /// NOTE : the calculations for _OrdersRemainingPrepTime are done irrespective of the UI being actively used or not
    /// the calculations for CurrentPrepTimes and _loader are done only when the UI is active , thus saving some space and frames
    /// </summary>

    [SerializeField]
    GameObject PrepFoodUIPrefab;

    int _havePlacedOrder = 0; //This decides if we have to calculate if no orders are there to process

    float[,] _OrdersRemainingPrepTime = new float[10,5]; //Matrix that contains the remaining time that a order needs to be ready for parcel
    //  _OrdersRemainingPrepTime[1,j] = -1 , indicates that there is no order for this at the moment
    // _OrdersRemainingPrepTime[1,j] = 0-1 , indicates that there is an order and it it being processed
    // _OrdersRemainingPrepTime[1,j] = 1 , indicates that order is processed and ready for parcel

    float[] CurrentPrepTimes = new float[5]; //Holds info on the prep times of a current hotel
    Image[] _loader = new Image[4]; //Holds the references of the images in the hotel
    int CurrentHotel = 0; //Current hotel which the UI corresponds to
    FoodPackageSO selectedFood; //food selected for pacelling

   

    bool _showLoading = false;

    [SerializeField]
    TextMeshProUGUI PackText;

    List<FoodPackageSO> currentFoodItems;
    HotelUIManager hoteluiManager;
    EventManager _eventManager;
    GameObject FoodStatus;

    void Start()
    {
        hoteluiManager = FindObjectOfType<HotelUIManager>();
        _eventManager = FindObjectOfType<EventManager>();
        EventManager.OnPackageOrdered += AddItemToLoad;
        EventManager.OnOrderingFromRestaurant += RetainPrepInformation;

        for (int i = 1; i <=9; i++)
        {
            for(int j = 1; j <=4; j++)
            {
                _OrdersRemainingPrepTime[i,j] = -1; //intializing the values to all -1
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(_havePlacedOrder != 0)
        {
            CalculateRemainingTime(); //this function is called only if we have order to calculate or else no calculation is done
        }

        if(_showLoading)
        {
            ShowAnimation(); // shows the animation if the UI is active
        }
    }

    //Calculates the time of the orders
    void CalculateRemainingTime()
    {
        for (int i = 1; i <= 9; i++)
        {
            for (int j = 1; j <= 4; j++)
            {
                if(_OrdersRemainingPrepTime[i, j] != -1)
                {
                    _OrdersRemainingPrepTime[i, j] = (_OrdersRemainingPrepTime[i, j] > 0 ? _OrdersRemainingPrepTime[i, j] - Time.deltaTime : 0);
                }
            }
        }
    }

    //Sets up the Orders Status UI compoment
    public void LoadingAnimation(List<FoodPackageSO> FoodItems)
    {
        PackText.text = "Place your order and wait for it to be ready";
        CurrentHotel = FoodItems[0].GetFoodID() / 10;
        currentFoodItems = FoodItems;

        //ConfigureImages(); 

        //Store all the values of prep times
        for(int j = 0; j < FoodItems.Count; j++)
        {
            CurrentPrepTimes[j+1] = FoodItems[j].GetPrepTime();
        }
    }

    //This for retaining the values of the current food orders
    public void RetainPrepInformation(List<FoodPackageSO> food)
    {
        int currentHotel = food[0].GetFoodID() / 10;

        if(HasOrders(currentHotel))
        {
            for (int j = 1; j <= 4; j++)
            {
                if (_OrdersRemainingPrepTime[currentHotel, j] != -1)
                {
                    StartCoroutine(WaitForOneFrame(j));
                }
            }
        }
    }

    IEnumerator WaitForOneFrame(int j )
    {
        yield return null;
        AddItemToLoad(currentFoodItems[j - 1]);
    }
    

    //Checks if we have orders in the first place to show the status -Edge case
    bool HasOrders(int currentHotel)
    {
        for(int i = 1; i <= 4; i++)
        {
            if(_OrdersRemainingPrepTime[currentHotel,i] != -1)
            {
                return true;
            }
        }
        return false;
    }

   

    void ConfigureImages()
    {
        FoodStatus = GameObject.Find("HotelOrderUI/OrderStatusComponent/FoodStatus");

        //get reference to the loaders
        if (FoodStatus.transform.childCount > 0)
        {
            foreach (Transform child in FoodStatus.transform)
            {
                Image Loader = null;
                Image[] Loaders = child.GetComponentsInChildren<Image>();

                //Get the correct Image component that has the loader
                foreach (var item in Loaders)
                {
                    if(item.sprite == null)
                    {
                        Loader = item;
                    }
                }

                //gives the UI component a ID so that it is easy to identify later
                FoodIDHolder foodIdHolder = child.GetComponent<FoodIDHolder>();
                int i = foodIdHolder.GetFoodID() / 10;
                int j = foodIdHolder.GetFoodID() % 10;

                _loader[j] = Loader; //sets the reference to the main Loader component
            }

            _showLoading = true;
        }
    }

    //Animates the loading of the menu
    void ShowAnimation()
    {
       for(int i = 1; i < _loader.Length; i++)
       {
            if (_loader[i] != null)
            {
                float loaderValue = (CurrentPrepTimes[i] - _OrdersRemainingPrepTime[CurrentHotel, i]) / CurrentPrepTimes[i];
                _loader[i].rectTransform.localScale = new Vector3(_loader[i].rectTransform.localScale.x, loaderValue, _loader[i].rectTransform.localScale.z);
            }
       }
    }

    //Adds a food to the order status
    void AddItemToLoad(FoodPackageSO FoodItem)
    {
        FoodStatus = GameObject.Find("HotelOrderUI/OrderStatusComponent/FoodStatus");
        FoodIDHolder foodIDHolder;

        //instantiates a child to the parent component , so that vertical grouping component can do this job
        GameObject child = Instantiate(PrepFoodUIPrefab);
        child.transform.SetParent(FoodStatus.transform);
        child.transform.localScale = new Vector3(1, 1, 1);

        foodIDHolder = child.GetComponent<FoodIDHolder>();

        Image[] images = child.GetComponentsInChildren<Image>();

        //Set the food sprites
        foreach (var image in images)
        {
            if (image.sprite == null)
            {
                //do nothing
            }
            else
            {
                image.sprite = FoodItem.GetFoodSprite();
            }
        }


        //Set the button
        Button ReadyFoodButton = child.GetComponent<Button>();
        ReadyFoodButton.onClick.AddListener(delegate { PackFood(FoodItem); });

        //get the which food this belongs to 
        foodIDHolder.SetFoodID(FoodItem.GetFoodID());
        int i = FoodItem.GetFoodID() / 10;
        int j = FoodItem.GetFoodID() % 10;

        //Dont reset the timer for the food once the UI is opened after some time
        if (_OrdersRemainingPrepTime[i, j] == -1)
        {
            _OrdersRemainingPrepTime[i, j] = FoodItem.GetPrepTime();
            _havePlacedOrder += 1;
        }

        ConfigureImages();//Configures the Images to be shown in the Order Status component

    }

    //Food Selected via order status can made to order
    void PackFood(FoodPackageSO fooditem)
    {
        if(_OrdersRemainingPrepTime[fooditem.GetFoodID()/10 , fooditem.GetFoodID()%10] == 0)
        {
            FindObjectOfType<AudioManager>().Play("ClickSound");
            PackText.text = "Pack " + fooditem.GetFoodName();
            selectedFood = fooditem;
        }
    }

    //Food is parcelled
    public void PackFoodForDelivery()
    {
        if(PackText.text == "Place your order and wait for it to be ready")
        {
            return;
        }
        else
        {
            int i = selectedFood.GetFoodID() / 10;
            int j = selectedFood.GetFoodID() % 10;

            _OrdersRemainingPrepTime[i, j] = -1; //making so that order does exist
            _havePlacedOrder -= 1; //reducing ordercount

            //get the hotel we ordered this from
            _eventManager.OnPackageParceledEvent(selectedFood);
            hoteluiManager.ActivateButton(selectedFood);
            //Destroyfood();
        }
    }


    public void Destroyfood()
    {
        FoodStatus = GameObject.Find("HotelOrderUI/OrderStatusComponent/FoodStatus");

        if (FoodStatus.transform.childCount > 0)
        {
            foreach (Transform item in FoodStatus.transform)
            {
                Destroy(item.gameObject);
            }
        }
        
    }

    private void OnDestroy()
    {
        EventManager.OnPackageOrdered -= AddItemToLoad;
        EventManager.OnOrderingFromRestaurant -= RetainPrepInformation;
    }
}
