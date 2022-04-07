using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    /// <summary>
    /// Deals with bringin up the score board , showing the score and saving them in the disk
    /// </summary>

    [SerializeField]
    GameObject ScoreSystem;

    [SerializeField]
    GameObject FoodScoreStatusPrefab;

    [SerializeField]
    GameObject OrderDeliveryStatus;

    [SerializeField]
    GameObject[] Stars;

    [SerializeField]
    Image ScoreLoader;

    [SerializeField]
    GameObject ScoreButtons;

    [SerializeField]
    int MaxScoreOfLevel;

    [SerializeField]
    int MaxMinutes  ;

    GameManager _gameManager;
    OrderManager _orderManager;
    TipsManager tipsManager;
    HealthManager healthManger;

    Dictionary<FoodPackageSO, int> OrdersDelivered = new Dictionary<FoodPackageSO, int>(); //stores the amount of food we have delivered so far
    Dictionary<string, int> OrderCount;
    
    FoodPackageSO[] FoodItems;

    int TipsCount = 0;
    int StartTime;
    int EndTime;
    float Carhealth;
    
    public float Scorecount = 0;
    public float ScoreCountingTotal = 0;

    float ScoreScored = 0;

    int level;

    private void Awake()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _orderManager = FindObjectOfType<OrderManager>();
        tipsManager = FindObjectOfType<TipsManager>();
        healthManger = FindObjectOfType<HealthManager>();
    }

    private void Start()
    {
        EventManager.OnGameOver += ShowScores;
        OrdersDelivered.Clear();
        level = SceneManager.GetActiveScene().buildIndex;
    }

    void ShowScores() //initiates the score values
    {
        GetScoreValues(); //gets the value required to make a score from various places
        ScoreSystem.SetActive(true);
        UpdateDeliveryStatusCount(); //counts up the food that we have ordered
        LoadScoreBar(); // loads the score bar
    }

    void UpdateDeliveryStatusCount()
    {
        //for each fooditem that we have delivered
        foreach (var Food in FoodItems)
        {
            //create a child
            GameObject child = Instantiate(FoodScoreStatusPrefab);
            child.transform.SetParent(OrderDeliveryStatus.transform);
            child.transform.localScale = new Vector3(1, 1, 1);

            //get the image and set the image to the food item we are talking about
            Image[] images = child.GetComponentsInChildren<Image>();

            foreach (var image in images)
            {
                if (image.sprite != null)
                {
                    image.sprite = Food.GetFoodSprite();
                }
            }

            //get the text component and update the values
            TextMeshProUGUI scoreText = child.GetComponentInChildren<TextMeshProUGUI>();

            StartCoroutine(CountUp(scoreText, OrdersDelivered[Food]));

        }
    }
    IEnumerator CountUp(TextMeshProUGUI text, int score) //updates the values as if they were counting
    {
        int value = 0;

        while (value != score)
        {
            yield return new WaitForSeconds(0.5f);
            text.text = "X " + (value).ToString();
            value++;
        }

        yield return new WaitForSeconds(0.5f);
        text.text = "X " + (score).ToString();
    }

    void LoadScoreBar()
    {
        float LoaderValue = 0;
        StartCoroutine(ScoreBar(LoaderValue));
    }

    IEnumerator ScoreBar(float LoaderValue) //shows the loading of the score bar
    {
        int i = 0 ;

        while((ScoreCountingTotal < ScoreScored) && (ScoreCountingTotal < MaxScoreOfLevel)) //keep increasing the bar untill the current score is greater than total score
        {
            yield return null;
            LoaderValue = Scorecount / (MaxScoreOfLevel / 3);

            if(LoaderValue > 1) // once we have filled the bar
            {
                LoaderValue = 0;
                Scorecount = 0; //reset the value of the bar to load from the start
                Stars[i].GetComponent<Animator>().SetTrigger("LightUp"); //light up the star 
                i++;
                FindObjectOfType<AudioManager>().Play("PackageDelivered");
            }

            ScoreCountingTotal += 0.3f;
            Scorecount += 0.3f;
            ScoreLoader.transform.localScale = new Vector3(LoaderValue, ScoreLoader.transform.localScale.y, ScoreLoader.transform.localScale.z);
        }

        SaveStartsObtainedInThisLevel(i);
        ShowButtons();
        PlayerPrefs.Save();// Saves all values
    }

    void ShowButtons()
    {
        ScoreButtons.SetActive(true);
    }

    void GetScoreValues()
    {
        OrderCount = _gameManager.GetOrderCount();
        FoodItems = _orderManager.GetFoodItems();
        TipsCount = tipsManager.GetTipsCount();
        StartTime = tipsManager.GetStartTime();
        EndTime = tipsManager.GetEndTime();
        Carhealth = healthManger.GetCurrentHealth();

        //Forms the dictionary for amount of food we have delivered
        for (int i = 0; i < FoodItems.Length; i++)
        {
            if(!OrdersDelivered.ContainsKey(FoodItems[i]))
            {
                OrdersDelivered.Add(FoodItems[i], OrderCount[FoodItems[i].GetFoodName()]);
            }
        }


        //calculates the score from the tips and time taken to complete the level
        if (MaxMinutes - (EndTime - StartTime) < 0)
        {
            ScoreScored = TipsCount;
        }
        else
        {
            ScoreScored = TipsCount + (MaxMinutes - (EndTime - StartTime));
        }

        if(Carhealth <= 0 )
        {
            ScoreScored = 0;
        }
        else
        {
            ScoreScored += Carhealth;
        }

        SaveLevelsBestTime(EndTime, StartTime);
        SaveLevelsMaxTips(TipsCount);
    }

    void SaveStartsObtainedInThisLevel(int i) //saves the stars amount if the we cros the highest score
    {
        if (!PlayerPrefs.HasKey("Level" + level + "Stars"))
        {
            PlayerPrefs.SetInt("Level" + level + "Stars", i);
        }
        else if (PlayerPrefs.GetInt("Level" + level + "Stars") < i)
        {
            PlayerPrefs.SetInt("Level" + level + "Stars", i);
        }
    }

    void SaveLevelsBestTime(int EndTime , int StartTime) //saves the best time in this game
    {
        if (PlayerPrefs.HasKey("Level" + level + "BestTime"))
        {
            if (PlayerPrefs.GetInt("Level" + level + "BestTime") > (EndTime - StartTime))
            {
                PlayerPrefs.SetInt("Level" + level + "BestTime", (EndTime - StartTime));
            }
        }
        else
        {
            PlayerPrefs.SetInt("Level" + level + "BestTime", (EndTime - StartTime));
        }
    }

    void SaveLevelsMaxTips(int TipsCount) // saves the max tips obtained in this game
    {
        if (PlayerPrefs.HasKey("Level" + level + "Tips"))
        {
            if (PlayerPrefs.GetInt("Level" + level + "Tips") < TipsCount)
            {
                PlayerPrefs.SetInt("Level" + level + "Tips", TipsCount);
            }
        }
        else
        {
            PlayerPrefs.SetInt("Level" + level + "Tips", TipsCount);
        }
    }

    public void ReplayLevel()
    {
        FindObjectOfType<AudioManager>().Play("ButtonClick");
        FindObjectOfType<Transition>().LoadLevel(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        FindObjectOfType<AudioManager>().Play("ButtonClick");
        FindObjectOfType<Transition>().LoadLevel(0);
    }

    private void OnDestroy()
    {
        EventManager.OnGameOver -= ShowScores;
    }
    
}
