using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodIDHolder : MonoBehaviour
{

    /// <summary>
    /// Holds information as to which food this loader relates to
    /// </summary>
    int FoodID;

    public void SetFoodID(int ID)
    {
        FoodID = ID;
    }

    public int GetFoodID() => FoodID;
}
