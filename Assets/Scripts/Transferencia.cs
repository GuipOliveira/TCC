using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*Classe que envia objetos*/
public class Transferencia : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void transferir(GameObject objCarregado)
    {
       
        

        PassaValor.idOBJ = objCarregado.name.ToString().Replace("(Clone)","");//Se for um clone remove a palavra e seta na classe de valores
        Debug.LogWarning(PassaValor.idOBJ + " Game Object Transferido");

    }
}