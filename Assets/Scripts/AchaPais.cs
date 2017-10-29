using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchaPais : MonoBehaviour {

    public LaserAcerta hit1;
    public LaserAcerta hit2;

	// Use this for initialization
	
	// Update is called once per frame
	void Update () {
        if (hit1.ativo && hit2.ativo)
        {
            PassaValor.numPorta = 2;                
        }
	}
}
