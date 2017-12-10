using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {

    public Color _color = Color.red;
    public int range = 50;
    public float _startWidth = 0.02f, _endWidth = 0.1f;
    public LineRenderer _lineRender;
    private GameObject lightCollision;
    private Light _light;
    private Vector3 lightpos;
    public ControlaPortaAnimacao cpa;
    public GameObject _porta;


    // Use this for initialization
    void Start () {

        cpa = _porta.GetComponent<ControlaPortaAnimacao>();

    }


    void procuraHit()
    {

        Vector3 lazerEnd = transform.position + ( - 1 * transform.forward * range);
        RaycastHit hit;
        if(Physics.Raycast(transform.position, - transform.forward, out hit, range))
        {
            HitBoxMapa hbm = hit.transform.GetComponent<HitBoxMapa>();
            if (hbm != null)
            {

                // cpa.ativar = true;
                PassaValor.numPorta = 3;
            }
        }

    }

    // Update is called once per frame
    void Update () {
        procuraHit();

    }


 
}
