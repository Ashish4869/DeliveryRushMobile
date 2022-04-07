using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundController : MonoBehaviour
{
    /// <summary>
    /// Deals with the audio for the car
    /// </summary>

    AudioManager _audioManager;
    PlayerDamage _playerDamage;

    //Enum that olds the different sound that car engine can make
    enum CarEngineSounds
    {
        CarForwardEngine,
        CarLowGearEngine,
        CarHighGearEngine,
        CarReverseEngine,
        CarIdleEngine,
        CarMudEngine,
    }

    //Enum that olds the different sound effects the car can play
    enum CarEffectSounds
    {
        GearFilp,
        NONE
    }


    CarEngineSounds _newcarEngineSounds = CarEngineSounds.CarIdleEngine;
    CarEngineSounds _oldCarEngineSounds = CarEngineSounds.CarIdleEngine;

    CarEffectSounds _carNewEffectSounds;

    private void Awake()
    {
        _audioManager = FindObjectOfType<AudioManager>();
        _playerDamage = FindObjectOfType<PlayerDamage>();
    }

    private void Start()
    {
        _audioManager.Play("CarEngineStart");
    }

    public void SoundHandler(float verticalDirection ,float horizontalDirection , bool OnRoad , int Gear) //plays the engine sound based on the current player input
    {
        _newcarEngineSounds = verticalDirection > 0 ? CarEngineSounds.CarForwardEngine : CarEngineSounds.CarReverseEngine;

        if (Gear == 1)
        {
            _newcarEngineSounds = CarEngineSounds.CarLowGearEngine;
        }
        else if (Gear == 3)
        {
            _newcarEngineSounds = CarEngineSounds.CarHighGearEngine;
        }

        if (!OnRoad)
        {
            _newcarEngineSounds = CarEngineSounds.CarMudEngine;
        }

        if (verticalDirection == 0)
        {
            _newcarEngineSounds = CarEngineSounds.CarIdleEngine;
        }

        //EngineSound
        if (_newcarEngineSounds != _oldCarEngineSounds)
        {
            _audioManager.Stop(_oldCarEngineSounds.ToString());
            _oldCarEngineSounds = _newcarEngineSounds;
            _audioManager.Play(_oldCarEngineSounds.ToString());

            _carNewEffectSounds = CarEffectSounds.GearFilp;
            _audioManager.Play(_carNewEffectSounds.ToString());
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        _audioManager.Play("CarHit");
        _playerDamage.CalcDamage();
    }

    public void CarIdle()
    {
        SoundHandler(0, 0, false, 0);
    }
}
