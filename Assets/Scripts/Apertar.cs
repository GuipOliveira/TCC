using UnityEngine;

public class Apertar : MonoBehaviour {

    public float range = 50f;
    public Camera _camera;
    public int cont = 0;
    public GameObject _maquina;
    public MaquinaEstadosBotao maq;

    private void Start()
    {
         maq = _maquina.GetComponent<MaquinaEstadosBotao>();
    }


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
    void Update () {
        if (Input.GetButtonDown("Fire1"))
        {
            Aperta();
            maq.MaquinaEstados(getContador());
        }		
	}


    void Aperta()
    {
        RaycastHit hit;
        if(Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hit, range))
        {
            ApertaBotao ab = hit.transform.GetComponent<ApertaBotao>();
            if (ab != null)
            {
                if (!ab.ativar)
                {
                    ab.Ligar();
                    incrementaContador();
                }                 
            }
        }   
    }

}
