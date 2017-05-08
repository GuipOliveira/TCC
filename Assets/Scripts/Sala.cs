using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using System.Text.RegularExpressions;
/*Esta classe controle o fluxo de objetos entre salas*/
public class Sala : MonoBehaviour {

    public GameObject[] objetos;
    public GameObject testeBola;
    private SocketIOComponent socket;

    public string sessao;
    public string nm_player;
    public string id_player;
    // Use this for initialization
    void Start () {

        carregaUser();//Carrega dados do usuário
        GameObject go = GameObject.Find("SocketIO");
        socket = go.GetComponent<SocketIOComponent>();
        addObj();//adiciona os objetos ao vetor
        
        socket.On("RECEBE_OBJ", receberOBJ);
        
    }
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.R))
        {
            PassaValor.idOBJ = 1;//Chumbando este valor pois por algum motivo o identificador de colisão não funciona no multiplayer
            transferirObj(PassaValor.idOBJ);

        }else //aguarda alguma entrada de objetos na sala
        {
            Dictionary<string, string> data = new Dictionary<string, string>();//pacote JSON
            data["id"] = id_player.ToString();
            data["sessao"] = sessao.ToString();
            socket.Emit("AGUARDAR", new JSONObject(data));
        }

	}
     void addObj()
    {
        Debug.LogWarning("Add objetos");
        objetos = new GameObject[2];//alterar tamanho conforme adição de objetos transportaves
        objetos[0] = GameObject.Find("radio");
        objetos[1] = GameObject.Find("Bola");
    }

    void receberOBJ(SocketIOEvent _obj)
    {
        Debug.LogWarning("Recebendo obj!");
        string idRecebe = JsonToString(_obj.data.GetField("id").ToString(), "\"");
        int idObj = int.Parse(JsonToString(_obj.data.GetField("idObj").ToString(), "\""));
        if (idRecebe != id_player)
        {
            instanciarObjetos(idObj);
            Debug.LogWarning("Objeto recebido!");
            
        }

    }

    void instanciarObjetos(int idObj)//escolhe qual objeto será instanciado
    {
        switch(idObj)
        {
            case 1:
                GameObject novoRadio = Instantiate(objetos[0]);//Radio gigante...pq?????
                break;
            case 2:
                GameObject novaBola = Instantiate(objetos[1]);
                break;
        }

    }


    public void transferirObj(int idObj)
    {
        if (idObj != 0)
        {
            PassaValor.idOBJ = 0;
            Dictionary<string, string> data = new Dictionary<string, string>();//pacote JSON
            data["idObj"] = idObj.ToString();
            data["id"] = id_player.ToString();
            
            socket.Emit("TRANSFERIR", new JSONObject(data));
        }
    }

    public void carregaUser()
    {
        sessao = PassaValor.sessao;
        nm_player = PassaValor.nm_player;
        id_player = PassaValor.id_player;

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
