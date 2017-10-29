using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControleBinario : MonoBehaviour {

    public bool ativo;
    public bool portaAberta;
    public BotaoBinario[] _botaoBinario;

    // Use this for initialization
    void Start () {
        _botaoBinario = FindObjectsOfType<BotaoBinario>();
	}

    bool AbrirPorta()
    {
        foreach(BotaoBinario bb in _botaoBinario)
        {
            if (bb.gameObject.name == "BotaoGO (7)" || bb.gameObject.name == "BotaoGO (4)")
            {
                if (!bb.ativo)
                {
                    return false;
                }
            }
            else
            {
                if (bb.ativo)
                {
                    return false;
                }
            }
        }
        return true;
    }
	
	// Update is called once per frame
	void Update () {
        if (!portaAberta)
        {
            portaAberta = AbrirPorta();
            if (portaAberta)
            {
               PassaValor.numPorta = 6; //DESCE O VIDRO
            }
        }
	}
}
