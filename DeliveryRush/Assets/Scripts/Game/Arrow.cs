using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    /// <summary>
    /// This scripts deals with arrow pointing towards the destination after the packaged is picked
    /// </summary>
    // Start is called before the first frame update

    [SerializeField] Transform delivery;
    [SerializeField] Transform player;

    bool _packagePicked = false;

    GameManager gameManager;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    void Start()
    {
        EventManager.OnPackagePicked += PackagePicked;
        EventManager.OnPackageDelivered += PackageDelivered;
    }

    private void Update()
    {
        if(_packagePicked == true)
        {
            PointToDestination(); //Makes the arrow point towards the destination
            TiltToDestination(); //Makes the arrow tilt towards direction of the destination
        }
    }


    void PointToDestination()
    {
        float x = transform.position.x - delivery.position.x;
        float y = transform.position.y - delivery.position.y;
        float angle = Mathf.Atan2(y, x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    void TiltToDestination()
    {
        transform.position = player.position - transform.right;
    }


    void PackagePicked(string foood)
    {
        _packagePicked = true;
    }

    void PackageDelivered()
    {
        _packagePicked = false;
        gameManager.RepositionElement(this.gameObject);
    }

    

    private void OnDestroy()
    {
        EventManager.OnPackagePicked -= PackagePicked;
        EventManager.OnPackageDelivered -= PackageDelivered;
    }


}
