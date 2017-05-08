#region License
/*
 * TestSocketIO.cs
 *
 * The MIT License
 *
 * Copyright (c) 2014 Fabio Panettieri
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */
#endregion

using System.Collections;
using UnityEngine;
using SocketIO;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class TestSocketIO : MonoBehaviour
{
    private SocketIOComponent socket;
    public InputField mainInputField;
    public GameObject canvas;
    public GameObject quarto1;
    public GameObject quarto2;
    public bool onLogin;
    public GameObject playerPrefab;
    public int players;
    public GameObject myPlayer;
	public GameObject bola;
	public GameObject trocaObjeto;
	public string idPlayer;
	public GameObject bolaPrefab;
	
    public void Start()
    {
        onLogin = false;
        mainInputField = FindObjectOfType(typeof(InputField)) as InputField;
		myPlayer = GameObject.Find("Player");
        canvas = GameObject.Find("Canvas");
        quarto1 = GameObject.Find("QuartoUm");
        quarto2 = GameObject.Find("QuartoDois");
		bola = GameObject.Find("Bola");
		bola.SetActive(false);
		myPlayer.SetActive(false);
       // quarto1.SetActive(false);
        //quarto2.SetActive(false);
        GameObject go = GameObject.Find("SocketIO");
        socket = go.GetComponent<SocketIOComponent>();

        socket.On("open", TestOpen);
        socket.On("boop", TestBoop);
        
        socket.On("LOGIN_SUCESS", OnLoginSucess);
		socket.On("ENVIA_SUCESS", recebeObjeto);
        
        socket.On("error", TestError);
        socket.On("close", TestClose);

        StartCoroutine("BeepBoop");
    }
	
	public void Update(){
	if (Input.GetKeyDown(KeyCode.R))
		enviaObjeto();
	}


    private IEnumerator BeepBoop()
    {
        // wait 1 seconds and continue
        yield return new WaitForSeconds(1);

        socket.Emit("beep");

        // wait 3 seconds and continue
        yield return new WaitForSeconds(3);

        socket.Emit("beep");

        // wait 2 seconds and continue
        yield return new WaitForSeconds(2);

        socket.Emit("beep");

        // wait ONE FRAME and continue
        yield return null;

        socket.Emit("beep");
        socket.Emit("beep");
    }

    public void TestOpen(SocketIOEvent e)
    {
        Debug.Log("[SocketIO] Open received: " + e.name + " " + e.data);
    }

    public void TestBoop(SocketIOEvent e)
    {
        Debug.Log("[SocketIO] Boop received: " + e.name + " " + e.data);

        if (e.data == null) { return; }

        Debug.Log(
            "#####################################################" +
            "THIS: " + e.data.GetField("this").str +
            "#####################################################"
        );
    }

    public void TestError(SocketIOEvent e)
    {
        Debug.Log("[SocketIO] Error received: " + e.name + " " + e.data);
    }

    public void TestClose(SocketIOEvent e)
    {
        Debug.Log("[SocketIO] Close received: " + e.name + " " + e.data);
    }
	
	public void enviaObjeto()
	{
	Debug.LogWarning("Enviando Objeto");
	PickUp pickup = myPlayer.GetComponent<PickUp>();
	if(pickup.isCarregando)
	{
            Debug.LogWarning("id_player " + idPlayer);
            Dictionary<string, string> data = new Dictionary<string, string>();//pacote JSON
            data["id"] = idPlayer;
			data["objeto"] = "1"; //bolinha
            socket.Emit("ENVIAR", new JSONObject(data));	
			bola.SetActive(false);//por enquanto chumbado para bola
			Debug.LogWarning("Bolinha Enviaada " + players);
	}
	}
	
	public void recebeObjeto(SocketIOEvent _objeto)
	{
		
		string id =  JsonToString(_objeto.data.GetField("id").ToString(), "\"");
		Debug.LogWarning("Objeto recebido no player: "+ id + " " + idPlayer);
		//if(id == idPlayer)
		//{
			//GameObject novaBola = GameObject.Instantiate(bolaPrefab) as GameObject;
			//Debug.LogWarning("Objeto recebido no playter: "+ id);
			bola.SetActive(true);//por enquanto chumbado para o objeto bola
			Debug.LogWarning("Objeto na posição:  " + bola.transform.position.x);

			bola.transform.position = new Vector3(14.47f,1.443f,-4.422f);
		
			
		//}
	}
    public void OnClickPlayBtn()//Chamada do botão de entrar
    {
        
        Debug.LogWarning("Quantidade de players " + players);
        if(mainInputField.text != "")
        {

                onLogin = true;
                Dictionary<string, string> data = new Dictionary<string, string>();//pacote JSON
                data["name"] = mainInputField.text;
				data["objeto"] = "0"; //sem objeto
                socket.Emit("LOGIN", new JSONObject(data));


                canvas.SetActive(false);
            Debug.LogWarning("Quantidade de players " + players);


        }
        else
        {
            
            mainInputField.text = "DIGITE";
        }


        

    }
    void OnLoginSucess(SocketIOEvent _myPlayer)
    {
		Debug.LogWarning("Instanciando quarto");
		players = int.Parse(JsonToString2(_myPlayer.data.GetField("players").ToString(), "\"")); //recebe a quantidade de players no servidor
        if (players < 1)//primeiro player a conectar entra no Quarto 1
        {
			idPlayer = JsonToString(_myPlayer.data.GetField("id").ToString(), "\"");
			Debug.LogWarning("id_player " + idPlayer);
            Debug.LogWarning("QUARTO 1 CRIADO");
			myPlayer.SetActive(true);
            quarto1.SetActive(true);
			bola.SetActive(true);
        }
        else if(players == 1) //segundo player a conectar entra no Quarto 2
        {
			myPlayer.transform.position = new Vector3(14.47f,2f,0f);
            idPlayer = JsonToString2(_myPlayer.data.GetField("id").ToString(), "\"");
			Debug.LogWarning("QUARTO 2 CRIADO");
			myPlayer.SetActive(true);
            quarto2.SetActive(true);
        }
		else
		{
			mainInputField.text = "Já existem 2 jogadores conectados";
			onLogin = false;
		}
	}

    void OnPlayers(SocketIOEvent count)
    {
        players = int.Parse(JsonToString(count.data.GetField("players").ToString(), "\""));
    }

    string JsonToString (string target, string s)
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
