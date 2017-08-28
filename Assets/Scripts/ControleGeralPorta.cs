using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControleGeralPorta : MonoBehaviour {

    public bool controleGeral;
    private ControlaPortaAnimacao anim;
    private TocarAudio audio;


	// Use this for initialization
	void Start () {
 
    }

    private void Awake()
    {
    }

    // Update is called once per frame
    void Update () {
        gameObject.GetComponent<TocarAudio>().ativo = true;
        gameObject.GetComponentInChildren<TocarAudio>().ativo = true;
        if (controleGeral)
        {
            gameObject.GetComponent<ControlaPortaAnimacao>().ativar = true;          
            gameObject.GetComponent<TocarAudio>().ativo = false;
            gameObject.GetComponentInChildren<TrocaCorLuz>().ativo = true;
            gameObject.GetComponentInChildren<TocarAudio>().ativo = false;

        }
    }
}
