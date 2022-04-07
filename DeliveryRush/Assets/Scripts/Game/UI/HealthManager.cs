using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    /// <summary>
    /// Deals with showing the health bar and reducing the health based on the speed of the car
    /// </summary>

    [SerializeField]
    Image Loader;

    float Health;
    float MaxHealth;

    EventManager _eventManager;


    private void Awake()
    {
        _eventManager = FindObjectOfType<EventManager>();
        CarSO car = FindObjectOfType<CarData>().GetSelectedCar();
        MaxHealth = car.GetcarHealth();
        Health = MaxHealth;
    }
       


    IEnumerator YouDied() //call game over screen
    {
        FindObjectOfType<AudioManager>().Play("CarEngineFail");
        yield return new WaitForSeconds(1f);
        _eventManager.OnCarTakenTooMuchDamageEvent();
        yield return new WaitForSeconds(2f);

       // gameObject.SetActive(false);
    }

    public void DecrementHealth(float damage)
    {
        StartCoroutine(ReduceHealthGradually(damage));

        Loader.transform.localScale = new Vector3(Health / MaxHealth, Loader.transform.localScale.y, Loader.transform.localScale.z);

        if(Health <= 0)
        {
            StartCoroutine(YouDied());
            Loader.transform.localScale = new Vector3(0, Loader.transform.localScale.y, Loader.transform.localScale.z);
        }
    }

    IEnumerator ReduceHealthGradually(float damage)
    {
        while(damage > 0)
        {
            Health -= 0.2f;
            Loader.transform.localScale = new Vector3(Health / MaxHealth, Loader.transform.localScale.y, Loader.transform.localScale.z);
            damage -= 0.2f;

            if(Health <= 0)
            {
                break;
            }

            yield return null;
        }
    }

    public float GetCurrentHealth() => Health;
}
