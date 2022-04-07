using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSoundController : MonoBehaviour
{
    /// <summary>
    /// Plays a horn and hit effect on collision
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            
            FindObjectOfType<AudioManager>().Play("CarHorn");
            FindObjectOfType<AudioManager>().Play("CarHit");
            
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("hit");
            FindObjectOfType<PlayerDamage>().CalcDamage();
        }
       
    }
}
