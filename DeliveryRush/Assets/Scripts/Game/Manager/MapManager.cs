using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    /// <summary>
    /// Shows the map and ui when the P key is pressed
    /// </summary>

    [SerializeField]
    GameObject GameMap;

    [SerializeField]
    GameObject LegendUI;

    [SerializeField]
    CanvasGroup[] canvasGroups;


    EventManager eventManager;

    bool _showMap = false;
    bool _canUseMap = true;

    // Start is called before the first frame update
    void Start()
    {
        eventManager = FindObjectOfType<EventManager>();
        EventManager.OnShowMap += ShowMap;
        EventManager.OnGameOver += CantUseMap;
        EventManager.OnCarTakenTooMuchDamage += CantUseMap;
        EventManager.OnOrderingFromRestaurant += CantUseMap;
        EventManager.OnPackageParceled += CanUseMap;
    }

    // Update is called once per frame
    void Update()
    {
        if(!_canUseMap)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            eventManager.OnShowMapEvent();
        }

        if (_showMap)
        {
            HideUI();
            GameMap.SetActive(true);
            LegendUI.SetActive(true);
        }   
        else
        {
            ShowUI();
            GameMap.SetActive(false);
            LegendUI.SetActive(false);
        }
        
    }

    void HideUI()
    {
        foreach (var canvas in canvasGroups)
        {
            canvas.alpha = 0;
            canvas.interactable = false;
            canvas.blocksRaycasts = false;
        }
    }

    void ShowUI()
    {
        foreach (var canvas in canvasGroups)
        {
            canvas.alpha = 1;
            canvas.interactable = true;
            canvas.blocksRaycasts = true;
        }
    }

    void ShowMap()
    {
        FindObjectOfType<AudioManager>().Play("MapSound");
        _showMap = !_showMap;
    }

    private void OnDestroy()
    {
        EventManager.OnShowMap -= ShowMap;
        EventManager.OnGameOver -= CantUseMap;
        EventManager.OnCarTakenTooMuchDamage -= CantUseMap;
        EventManager.OnOrderingFromRestaurant -= CantUseMap;
    }

    void CantUseMap()
    {
        _canUseMap = false;
    }

    void CantUseMap(List<FoodPackageSO> foodItems)
    {
        _canUseMap = false;
    }

    void CanUseMap(FoodPackageSO foodItems)
    {
        _canUseMap = true;
    }

    public void CanUseMap()
    {
        _canUseMap = true;
    }

    public void ShowMaps()
    {
        if (!_canUseMap)
        {
            return;
        }

        eventManager.OnShowMapEvent();
    }
}
