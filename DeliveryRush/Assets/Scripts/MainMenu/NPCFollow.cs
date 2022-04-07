using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCFollow : MonoBehaviour
{
    /// <summary>
    /// Follows the npc by keeping the car to the right
    /// </summary>

    [SerializeField]
    Transform _followobject;

    private void LateUpdate()
    {
        transform.position = new Vector3(_followobject.position.x - 10, _followobject.position.y, transform.position.z);
    }
}
