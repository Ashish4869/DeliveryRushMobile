using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarSelect : MonoBehaviour
{

    /// <summary>
    /// Gets the car data that the user as selected and then sends it to the main menu
    /// </summary>

    [Header("Cars")]
    public CarSO[] cars;

    [Header("Car Stats")]
    [SerializeField]
    Image carImage;

    [SerializeField]
    Image carHealth;

    [SerializeField]
    Image carSpeed;

    [SerializeField]
    Image carhandling;

    [Header("Menu")]
    [SerializeField]
    GameObject Menu;

    CarSO SelectedCar;

    float MaxCarhealth = 200;
    float MaxCarSpeed = 20;
    float MaxCarHandling = 200;

    int currentCar = 0;
    int level;


    // Start is called before the first frame update
    void Start()
    {
        ShowData();
    }

   

    void ShowData()
    {
        SelectedCar = cars[currentCar];
        carHealth.transform.localScale = new Vector3(SelectedCar.GetcarHealth() / MaxCarhealth, carHealth.transform.localScale.y, carHealth.transform.localScale.z);
        carSpeed.transform.localScale = new Vector3(SelectedCar.GetbaseCarSpeed() / MaxCarSpeed, carSpeed.transform.localScale.y, carSpeed.transform.localScale.z);
        carhandling.transform.localScale = new Vector3(SelectedCar.GetbaseCarHandling() / MaxCarHandling, carhandling.transform.localScale.y, carhandling.transform.localScale.z);
        carImage.sprite = SelectedCar.GetBodySprite();
    }

    public void NextCar()
    {
        FindObjectOfType<AudioManager>().Play("ButtonClick");
        currentCar = (currentCar + 1) % cars.Length;
        ShowData();
    }

    public void PrevCar()
    {
        FindObjectOfType<AudioManager>().Play("ButtonClick");
        currentCar = (currentCar - 1) % cars.Length;

        if(currentCar < 0)
        {
            currentCar = cars.Length-1;
        }

        ShowData();
    }

    public void StartGame()
    {

        FindObjectOfType<Transition>().LoadLevel(level);
        FindObjectOfType<AudioManager>().Play("ButtonClick");
        FindObjectOfType<CarData>().SetSelectedCar(SelectedCar);
    }

    public void SetLevel(int level)
    {
        this.level = level;
    }

    public void Exit()
    {
        FindObjectOfType<AudioManager>().Play("ButtonClick");
        Menu.GetComponent<MainMenuManager>().LoadMenu();
        gameObject.SetActive(false);
    }

    
}
