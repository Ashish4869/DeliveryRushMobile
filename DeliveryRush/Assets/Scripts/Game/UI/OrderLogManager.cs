using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OrderLogManager : MonoBehaviour
{
    /// <summary>
    /// Updates the log of the orders
    /// </summary>

    [SerializeField]
    TextMeshProUGUI _OrdersPendingText;

    [SerializeField]
    GameObject _tableComponent;

    [SerializeField]
    GameObject _OrderPrefab;

    [SerializeField]
    Animator _notifAnimator;

    GameObject OrderLogListGameObject;
    int _orderPendingCount = 0; //counts the number of orders to be served;
    bool _showTable = false;
    bool _noMoreOrders = false;
    public string oldFood =""; //this stores the old food so that the event is not fired unnecessarily

    GameManager _gameManager;
    EventManager _eventManager;
    TipsManager _tipsManager;

    List<List<string>> _OrderPendingList = new List<List<string>>(); //this stores the orders in a form of a matrix of size NX3 ,
                                                                     //where the rows have values such that
                                                                     // a[0] = timestamp
                                                                     // a[1] = food
                                                                     // a[2] = hotel name

    void Start()
    {
        _tipsManager = FindObjectOfType<TipsManager>();
        _gameManager = FindObjectOfType<GameManager>();
        _eventManager = FindObjectOfType<EventManager>();
        EventManager.OnOrderReceived += UpdateOrderLog;
        EventManager.OnPackageDelivered += FoodDelivered;
        EventManager.OnGameOver += DisableUI;
    }

    void DisableUI()
    {
        gameObject.SetActive(false);
    }
    private void Update()
    {
        if(_noMoreOrders)
        {
            if(_orderPendingCount == 0)
            {
                StartCoroutine("ShowScores");
            }
        }
    }

    IEnumerator ShowScores()
    {
        yield return new WaitForSeconds(2f);
        _eventManager.OnGameOverEvent();
    }

    //Updates the Orderlog UI
    void UpdateOrderLog(string OrderDetails , int foodID)
    {
        //split the string and store in the matrix
        string[] order = OrderDetails.Split('-');

        _OrderPendingList.Add(new List<string>());

        foreach (var item in order)
        {
            _OrderPendingList[_orderPendingCount].Add(item);
        }

        _orderPendingCount += 1;

        //In case we have picked food but the order has not come yet , when the food comes we shall activate the navigation
        if(order[1] == _gameManager.GetCurrentFoodInCar() && order[1] != oldFood)
        {
            oldFood = order[1];
            _eventManager.OnPackagePickedEvent(order[1]);
        }

        //triggers animation for notication
        _notifAnimator.SetTrigger("Animate");

        UpdateUI();
    }


    //Updates the UI - the header and its contents
    void UpdateUI()
    {
        _OrdersPendingText.text = "Orders Pending (" + _orderPendingCount.ToString() + ")";

        if(_showTable == true)
        {
            ConfigureOrderList();
        }
    }

    //shows the list of orders present
    public void ShowTable()
    {
        _showTable = !_showTable;
        _tableComponent.SetActive(_showTable);

        if(_showTable == false)
        {
            return;
        }
        ConfigureOrderList();
    }

    //Configures the table of orders
    void ConfigureOrderList()
    {
        OrderLogListGameObject = GameObject.Find("OrderStatus/TableComponent/OrderLogList");

        //destroy objects if we have any
        if (OrderLogListGameObject.transform.childCount != 0)
        {
            foreach (Transform item in OrderLogListGameObject.transform)
            {
                Destroy(item.gameObject);
            }
        }

        int max = Mathf.Min(_OrderPendingList.Count, 8); // we will show at max only 8 elements , as it will go out of screen

        //instantiates objects as children
        for (int i = 0; i < max; i++)
        {
            GameObject child = Instantiate(_OrderPrefab);
            child.transform.SetParent(OrderLogListGameObject.transform);
            child.transform.localScale = new Vector3(1, 1, 1);

            TextMeshProUGUI[] texts = child.GetComponentsInChildren<TextMeshProUGUI>();

            for (int j = 0; j < 3; j++)
            {
                texts[j].text = _OrderPendingList[i][j]; // assigns the text for the table like format
            }
        }
    }

    //removes an element from the list
    void RemoveItemFromOrderList(string FoodName)
    {
        //finds the element to remove then removes it from the list
        foreach  (var OrderDetails in _OrderPendingList)
        {
            if(OrderDetails[1] == FoodName)
            {
                _OrderPendingList.Remove(OrderDetails);
                _tipsManager.IncrementTips(OrderDetails[0]);
                _orderPendingCount -= 1;
                break;
            }
        }

        UpdateUI();
    }

    //In case Food Delivered , we remove the element from the list
     void FoodDelivered()
     {
        string FoodDelivered = _gameManager.GetCurrentFoodInCar();
        oldFood = "";
        RemoveItemFromOrderList(FoodDelivered);
     }

    //checking if the food obtained is in the order log list
    public bool CheckIfInOrderList()
    {
        string food = _gameManager.GetCurrentFoodInCar();

        foreach (var OrderDetails in _OrderPendingList)
        {
            if (OrderDetails[1] == food)
            {
                return true;
            }
        }

        return false;
    }

   public void NoMoreOrder()
   {
        _noMoreOrders = true;
   }

    private void OnDestroy()
    {
        EventManager.OnOrderReceived -= UpdateOrderLog;
        EventManager.OnPackageDelivered -= FoodDelivered;
        EventManager.OnGameOver -= DisableUI;
    }
}
