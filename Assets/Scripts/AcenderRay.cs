using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcenderRay : MonoBehaviour {

    public float range = 30f;
    public Camera _camera;
    public int cont = 0;


    public int getContador()
    {
        return cont;
    }

    public void setContador(int valor)
    {
        cont = valor;
    }

    public void incrementaContador()
    {
        cont++;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Aperta();
        }
    }


    void Aperta()
    {
        RaycastHit hit;
        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hit, range))
        {
            AcenderQuadrado ac = hit.transform.GetComponent<AcenderQuadrado>();
            if (ac != null)
            {
                if (!ac.ativar)
                {
                    ac.Acende();
                    incrementaContador();
                }
            }
        }
    }

}
