using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TrocaCorLuz : MonoBehaviour {
    public bool ativo;

    public Light lt;
    // Use this for initialization
    void Start () {
        lt = GetComponent<Light>();
    }


    // Update is called once per frame
    void Update () {
		if (ativo)
        {
            lt.color = Color.green;      
        }
	}
}
