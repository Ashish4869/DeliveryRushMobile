using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField]
    Transform _player;
   

    void LateUpdate()
    {
        transform.position = new Vector3(_player.position.x, _player.position.y, transform.position.z);
    }
}
