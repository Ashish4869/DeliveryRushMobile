using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CarType", menuName = "New Car")]
public class CarSO : ScriptableObject
{
    public float baseCarSpeed;
    public float baseCarHandling;
    public float carhealth;

    public Sprite carBody;
    public Sprite rDoor;
    public Sprite lDoor;
    public float capsuleSize;

    public float GetbaseCarSpeed() => baseCarSpeed;
    public float GetbaseCarHandling() => baseCarHandling;
    public float GetcarHealth() => carhealth;
    public Sprite GetBodySprite() => carBody;
    public Sprite GetLDoorSprite() => lDoor;
    public Sprite GetRDoorSprite() => rDoor;


}
