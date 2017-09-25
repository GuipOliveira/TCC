using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcenderRay : MonoBehaviour {

    public float range = 30f;
    public Camera _camera;
    public AcenderQuadrado aq;
    public AcenderQuadrado[] arrayAq;
    public int cont;

    public bool vez; //Se falso Player1, se Verdadeiro Player2


    private void Start()
    {
        arrayAq = FindObjectsOfType<AcenderQuadrado>();
    }


    void ChecarMatrizCompleta()
    {
        cont = 0;
        foreach(AcenderQuadrado item in arrayAq)
        {
            if (item.ativo)
            {
                cont++;
            }
        }
        if (cont == 16)
        {            
            PassaValor.numPorta = 2; //AbrirPorta
            Debug.Log(PassaValor.numPorta);
        }
    }

    // Update is called once per frame
    void Update()
    {
        ChecarMatrizCompleta();
        if (Input.GetButtonDown("Fire1"))
        {
            Aperta();
            if (aq != null)
            {
                if (!vez)
                {
                    aq._col1.Controla();
                    aq._col2.Controla();
                    aq._col3.Controla();
                    vez = true;
                }else if (vez)
                {
                    aq._lin1.Controla();
                    aq._lin2.Controla();
                    aq._lin3.Controla();
                    vez = false;
                }
                   
            }
        }
        Debug.Log(PassaValor.numPorta);
    }


    void Aperta()
    {
        RaycastHit hit;
        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hit, range))
        {
            AcenderQuadrado ac = hit.transform.GetComponent<AcenderQuadrado>();
            aq = ac;
            if (ac != null)
            {
                ac.Controla();
            }
        }
    }

}
