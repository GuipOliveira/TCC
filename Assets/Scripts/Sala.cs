using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;
/*Esta classe controle o fluxo de objetos entre salas*/
public class Sala : MonoBehaviour {

    public GameObject[] objetos;
    public string sessao;
    public string nm_player;
    public string id_player;
    public GameObject novaInstancia;


    // Use this for initialization
    void Start () {

        carregaUser();//Carrega dados do usuário
   
        
        addObj();//adiciona os objetos ao vetor
        

        
    }
	
	// Update is called once per frame
	void Update () {
        if (!string.IsNullOrEmpty(PassaValor.idOBJ)) //Enviando objeto para outra sala
        {

            PassaValor.transferirObj(PassaValor.idOBJ);

        }else //aguarda alguma entrada de objetos na sala
        {
            PassaValor.aguardar();
        }
        if(!string.IsNullOrEmpty(PassaValor.idObjRec))//Se houver algum idObjeRec significa que algum objeto foi enviado para esta sala
        {
            instanciarObjetos(PassaValor.idObjRec);
        }

	}
     void addObj()
    {
        Debug.LogWarning("Add objetos");
        objetos = new GameObject[2];//alterar tamanho conforme adição de objetos transportaves
        objetos[0] = GameObject.Find("radio");
        objetos[1] = GameObject.Find("BolaGO");
    }



    void instanciarObjetos(string idObj)//escolhe qual objeto será instanciado
    {
        PassaValor.idObjRec = "";

        
        foreach(GameObject go in objetos)
        {
            if(idObj == go.name)//Necessário ter o mesmo game objects na mesma sala, mesmo que estejam inativos
            {
              
                Instantiate(go);//Radio gigante...pq?????
                go.SetActive(true);
            }
        }
        
            
        

    }




    public void carregaUser()
    {

        PassaValor.idOBJ = "";
        sessao = PassaValor.sessao;
        nm_player = PassaValor.nm_player;
        id_player = PassaValor.id_player;

    }




}
