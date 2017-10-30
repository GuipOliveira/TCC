using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;
using System;
/*Esta classe controle o fluxo de objetos entre salas*/
public class Sala : MonoBehaviour
{

    public GameObject[] objetos;
    public string sessao;
    public string nm_player;
    public string id_player;



    // Use this for initialization
    void Start()
    {
        Scene menu = SceneManager.GetSceneAt(0);
        SceneManager.UnloadSceneAsync(menu);
       
        carregaUser();//Carrega dados do usuário


        addObj();//adiciona os objetos ao vetor



    }

    // Update is called once per frame
    void Update()
    {
        if (!string.IsNullOrEmpty(PassaValor.nm_player))
        {
            if (!string.IsNullOrEmpty(PassaValor.idOBJ)) //Enviando objeto para outra sala
            {

                PassaValor.transferirObj(PassaValor.idOBJ);

            }
            else //aguarda alguma entrada de objetos na sala
            {
                
                PassaValor.aguardar();
            }
            if (!string.IsNullOrEmpty(PassaValor.idObjRec))//Se houver algum idObjeRec significa que algum objeto foi enviado para esta sala
            {
                instanciarObjetos(PassaValor.idObjRec);
            }
            if (PassaValor.numPorta > 0)
            {
                controlePortas(PassaValor.numPorta);
                PassaValor.numPorta = -1;
            }
        }

    }
    void addObj()
    {
        Debug.LogWarning("Add objetos");

        objetos = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[];

    }



    void instanciarObjetos(string idObj)//escolhe qual objeto será instanciado
    {
        PassaValor.idObjRec = "";


        foreach (GameObject go in objetos)
        {
            if (idObj == go.name)//Necessário ter o mesmo game objects na mesma sala, mesmo que estejam inativos
            {

                GameObject Clone = Instantiate(go, new Vector3(-3.56f, 2.36f, -2.93f), new Quaternion(0f, 90.00001f, 0f, 0f));//Radio gigante...pq?????

                Clone.SetActive(true);
            }
        }




    }

    public void controlePortas(int numPorta) /*Controla o fluxo de abertura das portas*/
    {


        if (numPorta == 1)
        {


            GameObject porta = GameObject.Find("PortaGO (1)");
            porta.GetComponent<ControlaPortaAnimacao>();

            ControlaPortaAnimacao cpa = porta.GetComponent<ControlaPortaAnimacao>();
            cpa.ativar = true;

            if (!PassaValor.sinalRecebido) /*Se o sinal foi enviado e não recebido, devo enviar um sinal com o número da porta para o outro cliente*/
            {
                PassaValor.enviaSinalPorta(numPorta);
            }
            PassaValor.sinalRecebido = false;
            PassaValor.numPorta = -1;

        }
        else if (numPorta == 2)
        {
            GameObject porta = GameObject.Find("PortaGO (2)");
            porta.GetComponent<ControlaPortaAnimacao>();

            ControlaPortaAnimacao cpa = porta.GetComponent<ControlaPortaAnimacao>();
            cpa.ativar = true;

            if (!PassaValor.sinalRecebido)
            {
                PassaValor.enviaSinalPorta(numPorta);
            }
            PassaValor.sinalRecebido = false;
            PassaValor.numPorta = -1;
        }
        else if (numPorta == 3)
        {
            GameObject porta = GameObject.Find("PortaGO (3)");
            porta.GetComponent<ControlaPortaAnimacao>();

            ControlaPortaAnimacao cpa = porta.GetComponent<ControlaPortaAnimacao>();
            cpa.ativar = true;

            if (!PassaValor.sinalRecebido)
            {
                PassaValor.enviaSinalPorta(numPorta);
            }
            PassaValor.sinalRecebido = false;
            PassaValor.numPorta = -1;
        }
        else if (numPorta == 4)
        {
            GameObject porta = GameObject.Find("PortaGO (4)");
            porta.GetComponent<ControlaPortaAnimacao>();

            ControlaPortaAnimacao cpa = porta.GetComponent<ControlaPortaAnimacao>();
            cpa.ativar = true;

            if (!PassaValor.sinalRecebido)
            {
                PassaValor.enviaSinalPorta(numPorta);
            }
            PassaValor.sinalRecebido = false;
            PassaValor.numPorta = -1;
        }
        else if (numPorta == 5)
        {
            GameObject porta = GameObject.Find("PortaGO");
            porta.GetComponent<ControlaPortaAnimacao>();

            ControlaPortaAnimacao cpa = porta.GetComponent<ControlaPortaAnimacao>();
            cpa.ativar = true;

            if (!PassaValor.sinalRecebido)
            {
                PassaValor.enviaSinalPorta(numPorta);
            }
            PassaValor.sinalRecebido = false;
            PassaValor.numPorta = -1;
        }
        else if (numPorta == 6)
        {
            GameObject vidro = GameObject.Find("Vidro");
            Destroy(vidro);

            if (!PassaValor.sinalRecebido)
            {
                PassaValor.enviaSinalPorta(numPorta);
            }
            PassaValor.sinalRecebido = false;
            PassaValor.numPorta = -1;
        }
    }



    public void carregaUser()
    {
        if (!string.IsNullOrEmpty(PassaValor.nm_player))
        {
            PassaValor.idOBJ = "";
            sessao = PassaValor.sessao;
            nm_player = PassaValor.nm_player;
            id_player = PassaValor.id_player;
        }


    }




}
