using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApertarBotaoFinal : MonoBehaviour {

    public bool controle;
    public float range = 30f;
    public Camera _camera;
    public ApertaBotaoFinal abf;

    // Use this for initialization
    void Start () {
		
	}

    void Aperta()
    {
        RaycastHit hit;
        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hit, range))
        {
            abf = hit.transform.GetComponent<ApertaBotaoFinal>();
            if (abf != null)
            {
                abf.Acender();
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Aperta();
        }
    }
}
