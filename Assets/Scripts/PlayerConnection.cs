using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;
using SocketIO;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Text.RegularExpressions;
//ing UnityEditor;
using System.Net;

/*Classe responsável por conectar o usuário ao servidor*/
public class PlayerConnection : MonoBehaviour {


    public InputField[] inputFields;
    public Button btnEntrar;
    public GameObject canvas;
    public int players;
    public bool fecharCanvas;
    public bool logar;
    public string sessao;
    public string nm_player;
    public string id_player;
    public GameObject passaValor;
   
    // Use this for initialization
    void Start () {

        btnEntrar = FindObjectOfType(typeof(Button)) as Button;
        btnEntrar.enabled = false;
        inputFields = FindObjectsOfType(typeof(InputField)) as InputField[];

        canvas = GameObject.Find("Canvas");
        passaValor = GameObject.Find("PassaValor");

    }
	
	// Update is called once per frame
	void Update () {
        if (!fecharCanvas)
        {
            if (inputFields[0].text != "" && inputFields[1].text != "")
            {
                btnEntrar.enabled = true;
            }
            else
            {
                btnEntrar.enabled = false;
            }
            if(PassaValor.players == 1 && logar)
            {
             
                logar = false;//

                canvas.SetActive(false);//fecha canvas

                //Não destruir objetos de conexao para serem passados para nova cena
                DontDestroyOnLoad(passaValor);
                DontDestroyOnLoad(PassaValor.socket);
                DontDestroyOnLoad(PassaValor.go);
                DontDestroyOnLoad(PassaValor.connection);

                SceneManager.LoadScene("Assets/Scenes/Sala1.unity", LoadSceneMode.Single); //carrega sala

                Scene sala = SceneManager.GetSceneAt(1);//pega noga cena

                //Joga objetos de conexao da cena anterior na nova cena
                SceneManager.MoveGameObjectToScene(passaValor, sala);
                SceneManager.MoveGameObjectToScene(PassaValor.go, sala);
                SceneManager.MoveGameObjectToScene(PassaValor.connection, sala);


            }
            else if (PassaValor.players == 2 && logar)
            {
                logar = false;

                canvas.SetActive(false);

                DontDestroyOnLoad(passaValor);
                DontDestroyOnLoad(PassaValor.socket);
                DontDestroyOnLoad(PassaValor.go);
                DontDestroyOnLoad(PassaValor.connection);

                SceneManager.LoadSceneAsync("Assets/Scenes/Sala2.unity", LoadSceneMode.Single); //carrega sala

                Scene sala = SceneManager.GetSceneAt(1);

                SceneManager.MoveGameObjectToScene(passaValor, sala);
                SceneManager.MoveGameObjectToScene(PassaValor.go, sala);
                SceneManager.MoveGameObjectToScene(PassaValor.connection, sala);
            }
        }

    }

    
    public void entrar()
    {
        logar = true;
        PassaValor.entrar(inputFields[0].text, inputFields[1].text);//sala e usuario

    }





  


}
