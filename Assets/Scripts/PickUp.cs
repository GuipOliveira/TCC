using UnityEngine;
using System.Collections;

public class PickUp: MonoBehaviour
{

    public GameObject mainCamera;
    public GameObject objCarregado;

    public bool isCarregando;
    public Transferencia trans;
    public float distancia;
    public float smooth;

    // Use this for initialization
    void Start()
    {
        trans = new Transferencia();
        mainCamera = GameObject.FindWithTag("MainCamera");

    }

    // Update is called once per frame
    void Update()
    {
        if (isCarregando)
        {

            carregar(objCarregado);
            if (Input.GetKeyDown(KeyCode.R))
            {
                trans.transferir(objCarregado);
                objCarregado.SetActive(false);
                dropObj();
  
                
            }
            isSolto();
        }
        else
        {
            int x = Screen.width / 2;
            int y = Screen.height / 2;

            pegar();
        }
    }



    

    void carregar(GameObject obj)
    {
        obj.transform.position = Vector3.Lerp(obj.transform.position, mainCamera.transform.position + mainCamera.transform.forward * distancia, Time.deltaTime * smooth);
        obj.transform.rotation = Quaternion.identity;
    }

    void pegar()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            int x = Screen.width / 2;
            int y = Screen.height / 2;
         
            Ray ray = mainCamera.GetComponent<Camera>().ScreenPointToRay(new Vector3(x, y));
            RaycastHit hit;
            Debug.LogWarning("Colisor: " + Physics.Raycast(ray, out hit));
            if (Physics.Raycast(ray, out hit))
            {
                Debug.LogWarning("Pegando 1");
                Pegavel p = hit.collider.GetComponent<Pegavel>();
      
                if (p != null)
                {
                    isCarregando = true;
                    objCarregado = p.gameObject;
                    //p.gameObject.GetComponent<Rigidbody>().isKinematic = false;
                    p.gameObject.GetComponent<Rigidbody>().useGravity = false;
                }
            }
        }
    }

    void isSolto()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            dropObj();
        }
    }

    void dropObj()
    {
        isCarregando = false;
        //objCarregado.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        objCarregado.gameObject.GetComponent<Rigidbody>().useGravity = true;
        objCarregado = null;
    }


}
