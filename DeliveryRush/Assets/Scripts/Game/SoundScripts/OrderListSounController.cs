using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderListSounController : MonoBehaviour
{
    /// <summary>
    /// Deals with sound in this component
    /// </summary>

    AudioManager audioManager;
    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void Start()
    {
        EventManager.OnOrderReceived += PlayNotifSound;
    }

    void PlayNotifSound(string OrderDetails, int FoodID)
    {
        audioManager.Play("NotificationSound");
    }

    public void PlayClickSound()
    {
        audioManager.Play("ClickSound");
    }

    private void OnDestroy()
    {
        EventManager.OnOrderReceived -= PlayNotifSound;
    }
}
