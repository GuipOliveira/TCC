using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrastarLazerVertical : MonoBehaviour {

    public Camera _camera;
    RaycastHit hit;

    private void OnMouseDrag()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hit, 100))
        {
            transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
        }

    }
}
