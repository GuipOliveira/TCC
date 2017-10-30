using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoraCerta : MonoBehaviour {

    public bool ativo;
    
    void OnTriggerEnter(Collider other)
    {
        ativo = true;
    }

    void OnTriggerExit(Collider other)
    {
        ativo = false;
    }


}
