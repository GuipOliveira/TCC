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

    private SocketIOComponent socket;
    public InputField[] inputFields;
    public Button btnEntrar;
    public GameObject canvas;
    public int players;
    public bool fecharCanvas;
    public string sessao;
    public string nm_player;
    public string id_player;

    // Use this for initialization
    void Start () {

        btnEntrar = FindObjectOfType(typeof(Button)) as Button;
        btnEntrar.enabled = false;

        inputFields = FindObjectsOfType(typeof(InputField)) as InputField[];

        canvas = GameObject.Find("Canvas");

        GameObject go = GameObject.Find("SocketIO");
        socket = go.GetComponent<SocketIOComponent>();

        socket.On("LOGIN_SUCESS", OnLoginSucess);
        socket.On("LOGIN_INSUCESS", OnLoginInsucess);

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
        }

    }

    public void entrar()
    {
        if (inputFields[0].text != "" && inputFields[1].text != "")
        {
            string nome = Dns.GetHostName();
            Dictionary<string, string> data = new Dictionary<string, string>();//pacote JSON
            data["sessao"] = inputFields[0].text;
            data["name"] = inputFields[1].text;

            socket.Emit("LOGIN", new JSONObject(data));

        }

    }
    void OnLoginInsucess(SocketIOEvent _myPlayer)
    {
        Debug.LogWarning("Entrada não autorizada");
        /* Ainda não sei por que a caixa de dialogo da erro no build do unity.
        EditorUtility.DisplayDialog("Atenção",
    "Olá "+ inputFields[1].text + ", existem dois usuários conectados a esta sessão: " + inputFields[0].text
    + ". Deseja conectar a outra sessão?", "Sim", "Não");*/
    }
    void OnLoginSucess(SocketIOEvent _myPlayer)
    {
        canvas.SetActive(false);
        fecharCanvas = true;
        Debug.LogWarning("Instanciando quarto");
        players = int.Parse(JsonToString2(_myPlayer.data.GetField("players").ToString(), "\"")); //recebe a quantidade de players no servidor
        if (players == 1)//primeiro player a conectar entra no Quarto 1
        {
            id_player = JsonToString(_myPlayer.data.GetField("id").ToString(), "\"");
            nm_player = JsonToString(_myPlayer.data.GetField("name").ToString(), "\"");
            sessao = JsonToString(_myPlayer.data.GetField("sessao").ToString(), "\"");
            Debug.LogWarning("id_player " + id_player);
            Debug.LogWarning("QUARTO 1 CRIADO");

            PassaValor.id_player = id_player;
            PassaValor.sessao = sessao;
            PassaValor.nm_player = nm_player;

            SceneManager.LoadScene("Assets/Scenes/Sala1.unity", LoadSceneMode.Additive);
        }
        else if (players == 2) //segundo player a conectar entra no Quarto 2
        {

            id_player = JsonToString(_myPlayer.data.GetField("id").ToString(), "\"");
            nm_player = JsonToString(_myPlayer.data.GetField("name").ToString(), "\"");
            sessao = JsonToString(_myPlayer.data.GetField("sessao").ToString(), "\"");
            Debug.LogWarning("QUARTO 2 CRIADO. Id_player " + nm_player);
       
            PassaValor.id_player = id_player;
            PassaValor.sessao = sessao;
            PassaValor.nm_player = nm_player;
            SceneManager.LoadScene("Assets/Scenes/Sala2.unity", LoadSceneMode.Additive);
        }
        else
        {
            inputFields[0].text = "Já existem 2 jogadores conectados";

        }
    }

    string JsonToString(string target, string s)
    {
        string[] newString = Regex.Split(target, s);
        return newString[1];
    }

    string JsonToString2(string target, string s)
    {
        string[] newString = Regex.Split(target, s);
        return newString[0];
    }

    Vector3 JsonToVector3(string target)
    {
        Vector3 newVector;
        string[] newString = Regex.Split(target, ",");
        newVector = new Vector3(float.Parse(newString[0]), float.Parse(newString[1]), float.Parse(newString[2]));

        return newVector;
    }




}
