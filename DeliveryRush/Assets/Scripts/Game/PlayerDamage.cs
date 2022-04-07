using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    /// <summary>
    /// Calculates damages and updates so in the UI
    /// </summary>

    PlayerMovement _playerMovement;
    HealthManager healthManager;

    [SerializeField]
    int HighSpeedDamage;
    [SerializeField]
    int LowSpeedDamage;
    [SerializeField]
    int NormalSpeedDamage;

    private void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        healthManager = FindObjectOfType<HealthManager>();
    }

    public void CalcDamage() //Calculates the damage to be done on the car
    {
        int Gear = _playerMovement.GetGear();

        if(Gear == 1)
        {
            healthManager.DecrementHealth(LowSpeedDamage);
        }
        else if(Gear == 2)
        {
            healthManager.DecrementHealth(NormalSpeedDamage);
        }
        else if(Gear == 3)
        {
            healthManager.DecrementHealth(HighSpeedDamage);
        }
    }
}
