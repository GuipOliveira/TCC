using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon.LoadBalancing;
using Voice = ExitGames.Client.Photon.Voice;

public class StreamingVoice : MonoBehaviour
{
    public GameObject o;
    // Use this for initialization
    void Start()
    {
        if (!PhotonNetwork.connected)
        {
            PhotonNetwork.ConnectUsingSettings("1"); //Configurações setadas no PhotonSettings
            PhotonNetwork.player.NickName = PassaValor.id_player;//Nome do Player definido na tela de login
        }

    }
    // Update is called once per frame
    void Update()
    {

    }

    void OnConnectedToMaster()
    {
        Debug.LogWarning("Conectado");

        if (PassaValor.players == 1)
        {
            
            PhotonNetwork.CreateRoom(PassaValor.sessao,new RoomOptions() { MaxPlayers = 2 },null); //Primeiro Player cria a sala com no maximo do jogadores
            
        }
        else
        {

            PhotonNetwork.JoinRoom(PassaValor.sessao); //Segundo Player se junta a sala

        }

    }
    public void OnJoinedRoom()
    {
        o = PhotonNetwork.Instantiate("PlayerVoice", new Vector3(0.0f, 0.0f), Quaternion.identity, 0); //Instancia o player e rec de voz
		o.GetComponent<PhotonVoiceSpeaker> ().enabled = false; //Para não reproduzir o próprio audio do jogador devo desligar o player
		o.GetComponent<AudioSource> ().enabled = false; //Para não reproduzir o próprio audio do jogador devo desligar o player

		
    }
    public void OnCreateRoom()
    {
        Debug.LogWarning("Sala criada");
    }

    public void OnPhotonCreateRoomFailed()
    {
        Debug.LogWarning("Falha na criação");
    }
}