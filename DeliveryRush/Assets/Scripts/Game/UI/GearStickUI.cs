using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GearStickUI : MonoBehaviour
{
    [SerializeField]
    RectTransform markerPos;

    [SerializeField]
    RectTransform[] gears;
    
    public void HighGear()
    {
        markerPos.transform.position = new Vector3(markerPos.transform.position.x, gears[0].transform.position.y, markerPos.transform.position.z);
    }

    public void NormalGear()
    {
        markerPos.transform.position = new Vector3(markerPos.transform.position.x, gears[1].transform.position.y, markerPos.transform.position.z);
    }

    public void LowGear()
    {
        markerPos.transform.position = new Vector3(markerPos.transform.position.x, gears[2].transform.position.y, markerPos.transform.position.z);
    }
}
