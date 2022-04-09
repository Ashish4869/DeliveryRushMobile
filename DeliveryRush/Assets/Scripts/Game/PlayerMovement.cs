using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    /// <summary>
    /// This class does the following
    /// 1.Control movement
    /// 2.Controls speed based on whether the car is on the road
    /// 3.Disables Player when any overlay ui is shown
    /// 4.Gets the data of the selected car and sets the value 
    /// </summary>


    float _OnRoadRotationSpeed;
    float _speed;
    float _HighGearSpeed;
    float _HighGearRotationSpeed;
    float _lowGearSpeed;
    float _lowGearRotationSpeed;
    float _rotationSpeed;
    bool _onRoad = true;
    public bool _canDrive = true;
    float _onRoadSpeed;
    float _offRoadSpeed = 3f;

    //Mobile input related vars
    float verticalDirection;
    float horizontalDirection;
    int direction;

    int Gear = 2;


    CarSO currentCar;
    CapsuleCollider2D carCapsule;
    SpriteRenderer _carSprite;
    PlayerSoundController _playerSoundController;
    GameManager gameManager;
    CarData carData;

    private void Awake()
    {
        EventManager.OnShowMap += DisablePlayer;
        EventManager.OnOrderingFromRestaurant += DisablePlayer;
        EventManager.OnPackageParceled += EnablePlayer;
        EventManager.OnGameOver += DisablePlayer;
        EventManager.OnCarTakenTooMuchDamage += DisablePlayer;
        _playerSoundController = GetComponent<PlayerSoundController>();

        gameManager = FindObjectOfType<GameManager>();
        carCapsule = GetComponent<CapsuleCollider2D>();
        carData = FindObjectOfType<CarData>();
    }

    private void Start()
    {
        _carSprite = GetComponent<SpriteRenderer>();
        GetCarData();

        _HighGearSpeed = _onRoadSpeed + 5;
        _lowGearSpeed = _onRoadSpeed - 7;
        _HighGearRotationSpeed = _OnRoadRotationSpeed - 25f;
        _lowGearRotationSpeed = _OnRoadRotationSpeed + 25f;

        _rotationSpeed = _OnRoadRotationSpeed;
    }

    void Update()
    {
        CalcMovment();
        CheckPress();
    }

    void CheckPress()
    {
        switch(direction)
        {
            case 0:
                verticalDirection = 0;
                horizontalDirection = 0;
                break;

            case 1:
                verticalDirection = 1;
                horizontalDirection = 0;
                break;

            case 2:
                verticalDirection = 1;
                horizontalDirection = 1;
                break;

            case 3:
                verticalDirection = -1;
                horizontalDirection = 1;
                break;

            case 4:
                verticalDirection = -1;
                horizontalDirection = 0;
                break;

            case 5:
                verticalDirection = -1;
                horizontalDirection = -1;
                break;

            case 6:
                verticalDirection = 1;
                horizontalDirection = -1;
                break;
        }
    }

    void CalcMovment()
    {
        //make sure the player cant drive when the seeing the map
        if (!_canDrive)
        {
            _playerSoundController.CarIdle();
            return;
        }

        //change the speed on going offroad/onroad
        _speed = (_onRoad ? _onRoadSpeed : _offRoadSpeed);

        if(_onRoad)
        {
            switch (Gear)
            {
                case 1:
                    _speed = _lowGearSpeed;
                    _rotationSpeed = _lowGearRotationSpeed;
                    break;

                case 2:
                    _speed = _onRoadSpeed;
                    _rotationSpeed = _OnRoadRotationSpeed;
                    break;

                case 3:
                    _speed = _HighGearSpeed;
                    _rotationSpeed = _HighGearRotationSpeed;
                    break;
            }
        }
        

        //set Speed as per the gear
        /*
        if(Input.GetKey(KeyCode.LeftShift) && _onRoad)
        {
            Gear = 1; //Gear 1 = SlowSpeed
            _speed = _lowGearSpeed;
            _rotationSpeed = _lowGearRotationSpeed;
        }
        else if(Input.GetKey(KeyCode.Space) && _onRoad)
        {
            Gear = 3; //Gear 3 = HighSpeed
            _speed = _HighGearSpeed;
            _rotationSpeed = _HighGearRotationSpeed;
        }
        else if(_onRoad)
        {
            Gear = 2; //Gear 2 = NormalSpeed
            _speed = _onRoadSpeed;
            _rotationSpeed = _OnRoadRotationSpeed;
        }
        */


        //forward-backward movement of the vehicle
        if (verticalDirection != 0)
        {
            transform.Translate(Vector3.up * (verticalDirection > 0 ? _speed : _speed /3) * verticalDirection * Time.deltaTime);
        }

        //turn only when you are moving
        if (horizontalDirection != 0 && verticalDirection !=0)
        {
            transform.Rotate(new Vector3(0, 0, _rotationSpeed * (verticalDirection > 0 ? horizontalDirection : -horizontalDirection) * Time.deltaTime));
        }

        _playerSoundController.SoundHandler(verticalDirection, horizontalDirection, _onRoad , Gear);
    }

    //Deals with the speed of the car based on the on its on
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("OffRoad"))
        {
            _onRoad =false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("OffRoad"))
        {
            _onRoad = true;
        }
    }

    public int GetGear() => Gear;

    public void DisablePlayer() => _canDrive = !_canDrive;

    void DisablePlayer(List<FoodPackageSO> nouse) => _canDrive = false;

    public void EnablePlayer(FoodPackageSO nouse) => _canDrive = true;

    void GetCarData()
    {
        currentCar = carData.GetSelectedCar();
        carCapsule.size = new Vector2(carCapsule.size.x ,currentCar.capsuleSize);
        AssignSprite();
        AssignSpeeds();
    }

    void AssignSprite()
    {
        SpriteRenderer[] CarParts = GetComponentsInChildren<SpriteRenderer>();
        CarParts[0].sprite = currentCar.GetBodySprite();
        CarParts[1].sprite = currentCar.GetRDoorSprite();
        CarParts[2].sprite = currentCar.GetLDoorSprite();
    }

    void AssignSpeeds()
    {
        _onRoadSpeed = currentCar.baseCarSpeed;
        _OnRoadRotationSpeed = currentCar.baseCarHandling;
    }

    private void OnDestroy()
    {
        EventManager.OnShowMap -= DisablePlayer;
        EventManager.OnOrderingFromRestaurant -= DisablePlayer;
        EventManager.OnPackageParceled -= EnablePlayer;
        EventManager.OnCarTakenTooMuchDamage -= DisablePlayer;
    }

    public void SetDirection(int directionNumber)
    {
        direction = directionNumber;
    }

    public void NoDirection()
    {
        direction = 0;
    }

    public void SetGear(int gear)
    {
        Gear = gear;
    }

}
