using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrastaLazserPerVer : MonoBehaviour {
    ArrastarLazerVertical getTarget;
    bool isMouseDragging;
    Vector3 offsetValue;
    Vector3 positionOfScreen;
    ArrastarLaser _pegavel;
    RaycastHit hit;

    void Update()
    {

        //Mouse Button Press Down
        if (Input.GetMouseButton(0))
        {
            RaycastHit hitInfo;
            getTarget = ReturnClickedObject(out hitInfo);
            hit = hitInfo;
            if (getTarget != null)
            {
                getTarget.transform.position = new Vector3(getTarget.transform.position.x, hit.point.y, getTarget.transform.position.z);
            }

        }



    }

    //Method to Return Clicked Object
    ArrastarLazerVertical ReturnClickedObject(out RaycastHit hit)
    {
        ArrastarLazerVertical target = null;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray.origin, ray.direction * 20, out hit))
        {
            target = hit.transform.GetComponent<ArrastarLazerVertical>();
        }
        return target;
    }

}