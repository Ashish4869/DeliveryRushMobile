using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    /// <summary>
    /// The NPC follows a predefined path
    /// </summary>
    

    [Header("WayPoints")]
    [SerializeField]
    Transform[] _waypoints;

    [Header("General")]
    [SerializeField]
    int NPCspeed;
    [SerializeField]
    float _RotationSpeed;


    Transform _targetPosition;
    int _currentIndex = 0;


    // Start is called before the first frame update
    void Start()
    {
        _targetPosition = _waypoints[0];
    }

    // Update is called once per frame
    void Update()
    {
        FindWayPointToGo();
    }

    void FindWayPointToGo()
    {
        if(SqrDistance(_targetPosition.position) < 1f) //if we have come close enough , start moving towards next waypoint
        {
            _currentIndex = (_currentIndex + 1) % (_waypoints.Length);
            _targetPosition = _waypoints[_currentIndex];
            GoToPoint();
        }
        else
        {
            GoToPoint();
        }
    }

    float SqrDistance(Vector3 target)
    {
        Vector3 offset = target - transform.position;
        float sqrLen = offset.sqrMagnitude;
        return sqrLen;
    }

    void GoToPoint()
    {
        transform.Translate(Vector3.right * NPCspeed * Time.deltaTime);
        RotateTowardsPoint();
    }

    //Makes the NPC rotate towards the next waypoint
    void RotateTowardsPoint()
    {
        float x = _targetPosition.position.x - transform.position.x;
        float y = _targetPosition.position.y - transform.position.y;

        float angle = Mathf.Atan2(y, x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Lerp(transform.rotation ,Quaternion.Euler(new Vector3(0, 0, angle)) , _RotationSpeed * Time.deltaTime);

    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < _waypoints.Length-1; i++)
        {
            Gizmos.DrawLine(_waypoints[i].position, _waypoints[i + 1].position);
        }

        Gizmos.DrawLine(_waypoints[_waypoints.Length-1].position, _waypoints[0].position);
    }
}
