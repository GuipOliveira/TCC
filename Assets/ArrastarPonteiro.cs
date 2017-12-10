using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrastarPonteiro : MonoBehaviour {

    public Ponteiro getTarget;
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
                getTarget.transform.Rotate(Vector3.right , -hit.point.x * (Time.deltaTime * 15));
            }

        }



    }

    //Method to Return Clicked Object
    Ponteiro ReturnClickedObject(out RaycastHit hit)
    {
        Ponteiro target = null;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray.origin, ray.direction * 5, out hit, 40))
        {
            Debug.Log(hit.transform.name);
            target = hit.transform.GetComponent<Ponteiro>();
        }
        return target;
    }

}
