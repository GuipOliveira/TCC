using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserAcerta : MonoBehaviour {

    public bool ativo;

    private void OnTriggerExit(Collider other)
    {
        ativo = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        ativo = true;
    }

}
