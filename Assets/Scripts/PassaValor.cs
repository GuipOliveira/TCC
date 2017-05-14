using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;
using SocketIO;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Text.RegularExpressions;
//ing UnityEditor;
using System.Net;
/*Esta classe passa valores entre cenas e outras classes e realiza comunicação com a classe JS do servidor via JSON*/
public class PassaValor : MonoBehaviour {

    public static string id_player;
    public static string sessao;
    public static string nm_player;
	public static string idOBJ;
    public static SocketIOComponent socket;
    public static int players;
    public static string idObjRec;
    public static GameObject go;
    public static GameObject connection;

    // Use this for initialization
    void Start () {


        idObjRec = "";
        go = GameObject.Find("SocketIO");
        socket = go.GetComponent<SocketIOComponent>();
        connection = GameObject.Find("SocketIO");
        socket.On("LOGIN_SUCESS", OnLoginSucess);
        socket.On("LOGIN_INSUCESS", OnLoginInsucess);
        socket.On("RECEBE_OBJ", receberOBJ);

    }
	
	// Update is called once per frame
	void Update () {
		
	}




    public static void entrar(string sessao,string name)
    {
        if (!string.IsNullOrEmpty(sessao) && !string.IsNullOrEmpty(name))
        {
           
            Dictionary<string, string> data = new Dictionary<string, string>();//pacote JSON
            data["sessao"] = sessao;
            data["name"] = name;

            socket.Emit("LOGIN", new JSONObject(data));//solicitando login ao servidor

        }

    }

    static void OnLoginInsucess(SocketIOEvent _myPlayer)
    {
        Debug.LogWarning("Entrada não autorizada");
        /* Ainda não sei por que a caixa de dialogo da erro no build do unity.
        EditorUtility.DisplayDialog("Atenção",
    "Olá "+ inputFields[1].text + ", existem dois usuários conectados a esta sessão: " + inputFields[0].text
    + ". Deseja conectar a outra sessão?", "Sim", "Não");*/
    }

    static void OnLoginSucess(SocketIOEvent _myPlayer)
    {

            players = int.Parse(JsonToString2(_myPlayer.data.GetField("players").ToString(), "\"")); //recebe a quantidade de players no servidor
            id_player = JsonToString(_myPlayer.data.GetField("id").ToString(), "\"");
            nm_player = JsonToString(_myPlayer.data.GetField("name").ToString(), "\"");
            sessao = JsonToString(_myPlayer.data.GetField("sessao").ToString(), "\"");

    }

    static void receberOBJ(SocketIOEvent _obj)
    {
        Debug.LogWarning("Recebendo obj!");
        string idRecebe = JsonToString(_obj.data.GetField("id").ToString(), "\"");
        string _idObjRec = JsonToString(_obj.data.GetField("idObj").ToString(), "\"");
        if (idRecebe != id_player)
        {
            idObjRec = _idObjRec; //altera atributo com o nome do objeto que será intanciado na sala
        }

    }


    public static void transferirObj(string _idObj)
    {
        idOBJ = "";
        if (!string.IsNullOrEmpty(_idObj))
        {

            Dictionary<string, string> data = new Dictionary<string, string>();//pacote JSON
            data["idObj"] = _idObj.ToString();
            data["id"] = id_player.ToString();

            socket.Emit("TRANSFERIR", new JSONObject(data));
        }
    }

    public static void aguardar()
    {
        Dictionary<string, string> data = new Dictionary<string, string>();//pacote JSON
        data["id"] = id_player.ToString();
        data["sessao"] = sessao.ToString();
        socket.Emit("AGUARDAR", new JSONObject(data));
    }


    //métodos úteis JSON
    public static string JsonToString(string target, string s)
    {
        string[] newString = Regex.Split(target, s);
        return newString[1];
    }

    public static string JsonToString2(string target, string s)
    {
        string[] newString = Regex.Split(target, s);
        return newString[0];
    }

   public static Vector3 JsonToVector3(string target)
    {
        Vector3 newVector;
        string[] newString = Regex.Split(target, ",");
        newVector = new Vector3(float.Parse(newString[0]), float.Parse(newString[1]), float.Parse(newString[2]));

        return newVector;
    }
}
