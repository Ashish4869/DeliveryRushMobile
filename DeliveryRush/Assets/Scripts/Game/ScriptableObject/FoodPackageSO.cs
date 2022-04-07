using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FoodItem", menuName = "New FoodItem")]
public class FoodPackageSO : ScriptableObject
{
    /// <summary>
    /// This scriptable objects holds info of each food item present in the game
    /// </summary>
    

    public int foodID = 00;
    public string foodName = "Name of the food";
    public string foodDescription = "Describe food here in one liner";
    public Sprite foodSprite;
    public int prepTime;

    public int GetFoodID()
    {
        return foodID;
    }

    public Sprite GetFoodSprite()
    {
        return foodSprite;
    }

    public string GetFoodName()
    {
        return foodName;
    }

    public int GetPrepTime()
    {
        return prepTime;
    }

    public string GetFoodDesc()
    {
        return foodDescription;
    }
    
}
