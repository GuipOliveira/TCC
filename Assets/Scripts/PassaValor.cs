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
public class PassaValor : MonoBehaviour
{

    public static string id_player; /*id aleatório gerado no servidor*/
    public static string sessao; /* nome da sessão */
    public static string nm_player; /* nome do usuário */
    public static string idOBJ;
    public static SocketIOComponent socket; /* objeto de conexão */
    public static int players;
    public static string idObjRec;
    public static GameObject go;
    public static GameObject connection; /* objeto de conexão */
    public static int numPorta; /*número da porta que será aberta (número do gameobject)*/
    public static bool sinalRecebido; /*indica se o sinal da porta foi recebido ou enviado*/
    public static bool vezMatriz; /*indica de quem é a vez para jogar no puzzle da matriz*/
    public static string posicaoSelecionadaMatriz; /*indica qual botão foi selecionado na Matriz*/
    public static string ranking;


    // Use this for initialization
    void Start()
    {

        numPorta = -1;
        idObjRec = "";
        go = GameObject.Find("SocketIO");
        socket = go.GetComponent<SocketIOComponent>();
        connection = GameObject.Find("SocketIO");

        socket.On("ATUALIZA_RANKING", atualizaRanking);
        socket.On("LOGIN_SUCESS", OnLoginSucess);
        socket.On("LOGIN_INSUCESS", OnLoginInsucess);
        socket.On("RECEBE_OBJ", receberOBJ);
        socket.On("RECEBE_SINALPORTA", recebeSinalPorta);
        socket.On("ATUALIZA_VEZMATRIZ", atualizaVezMatriz);
        socket.On("PONTEIRO_OK",ponteiroOk);
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public static void sair()
    {
        if (!string.IsNullOrEmpty(sessao) && !string.IsNullOrEmpty(id_player))
        {
            Dictionary<string, string> data = new Dictionary<string, string>();//pacote JSON
            data["sessao"] = sessao;
            data["id"] = id_player;

            socket.Emit("SAIR", new JSONObject(data));//RETIRANDO USER DO SERVER
        }
    }

    public static void entrar(string sessao, string name)
    {
        if (!string.IsNullOrEmpty(sessao) && !string.IsNullOrEmpty(name))
        {

            Dictionary<string, string> data = new Dictionary<string, string>();//pacote JSON
            data["sessao"] = sessao;
            data["id"] = name;

            socket.Emit("LOGIN", new JSONObject(data));//solicitando login ao servidor

        }

    }

    static void ponteiroOk(SocketIOEvent _myPlayer)
    {
        PassaValor.numPorta = 3;
        Debug.LogWarning("Concluindo Puzzle Relogio");
    }

    public static void setPonteiroRelogio(bool ponteiro)
    {
        string ponteiroRelogio = null;
        if (ponteiro)
            ponteiroRelogio = "V";
        else
            ponteiroRelogio = "F";

        Dictionary<string, string> data = new Dictionary<string, string>();
        data["sessao"] = sessao.ToString();
        data["id"] = id_player.ToString();
        data["ponteiroRelogio"] = ponteiroRelogio;

        socket.Emit("ATUALIZAR_PONTEIRO", new JSONObject(data));
    }

    public static void getRanking()
    {
        Dictionary<string, string> data = new Dictionary<string, string>();//pacote JSON
        data["x"] = "x";
        data["y"] = "y";
        socket.Emit("GET_RANKING",new JSONObject(data));
    }

    static void OnLoginInsucess(SocketIOEvent _myPlayer)
    {
        Debug.LogWarning("Entrada não autorizada");

    }

    static void OnLoginSucess(SocketIOEvent _myPlayer)
    {

        players = int.Parse(JsonToString2(_myPlayer.data.GetField("players").ToString(), "\"")); //recebe a quantidade de players no servidor
        id_player = JsonToString(_myPlayer.data.GetField("id").ToString(), "\"");
        sessao = JsonToString(_myPlayer.data.GetField("sessao").ToString(), "\"");
        Debug.LogWarning("Conectado a sessão: " +sessao);

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

    public static void enviaSinalPorta(int numPorta)
    {

        if (!string.IsNullOrEmpty(id_player))
        {
            Dictionary<string, string> data = new Dictionary<string, string>();//pacote JSON
            data["porta"] = numPorta.ToString();
            data["sessao"] = sessao.ToString();
            data["id"] = id_player.ToString();

            socket.Emit("SINALPORTA", new JSONObject(data));
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
    static void recebeSinalPorta(SocketIOEvent _obj)
    {

        if (!string.IsNullOrEmpty(id_player))
        {
            Debug.LogWarning("Recebendo Sinal Porta");
            string idRecebe = JsonToString(_obj.data.GetField("id").ToString(), "\"");
            string _sessao = JsonToString(_obj.data.GetField("sessao").ToString(), "\"");
            int _numPorta = int.Parse(JsonToString(_obj.data.GetField("porta").ToString(), "\""));
            if (idRecebe != id_player && sessao == _sessao)
            {
                sinalRecebido = true;
                numPorta = _numPorta; //porta que será aberta
                
            }
        }
    }

    /*atualiza nos clientes qual jogador deverá jogar no puzzle Matriz e pega qual a posição do ultimo botão apertado*/
    static void atualizaVezMatriz(SocketIOEvent _obj)
    {
        if (!string.IsNullOrEmpty(id_player))
        {
            
            string idRecebe = JsonToString(_obj.data.GetField("id").ToString(), "\"");
            string _sessao = JsonToString(_obj.data.GetField("sessao").ToString(), "\"");
            string vez = JsonToString(_obj.data.GetField("vezMatriz").ToString(), "\"");
            string posicao = JsonToString(_obj.data.GetField("posicaoSelecionada").ToString(), "\"");

            if (idRecebe == id_player && sessao == _sessao)
            {
                if (vez.Equals("V")) PassaValor.vezMatriz = true;
                else PassaValor.vezMatriz = false;

                posicaoSelecionadaMatriz = posicao;
            }
        }
     }

    static void atualizaRanking(SocketIOEvent _obj)
    {
        ranking = JsonToString(_obj.data.GetField("ranking").ToString(), "\"");

    }


    public static void aguardar()
    {
        Dictionary<string, string> data = new Dictionary<string, string>();//pacote JSON
        data["id"] = id_player.ToString();
        data["sessao"] = sessao.ToString();
        socket.Emit("AGUARDAR", new JSONObject(data));
    }


    /*envia para o servidor a posição do botão apertado na matriz*/
    public static void alterarJogadaMatriz(string posicao)
    {
        if (!string.IsNullOrEmpty(id_player))
        {
            Dictionary<string, string> data = new Dictionary<string, string>();//pacote JSON
          
            data["sessao"] = sessao.ToString();
            data["id"] = id_player.ToString();
            data["posicao"] = posicao;
            socket.Emit("ALTERAR_VEZMATRIZ", new JSONObject(data));
        }
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
