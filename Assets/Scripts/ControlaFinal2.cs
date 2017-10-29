using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlaFinal2 : MonoBehaviour {
    public bool ativo;
    public bool portaAberta;
    public BotaoBinario2[] _botaoBinario;

    // Use this for initialization
    void Start()
    {
        _botaoBinario = FindObjectsOfType<BotaoBinario2>();
    }

    bool AbrirPorta()
    {
        foreach (BotaoBinario2 bb in _botaoBinario)
        {
            if (bb.gameObject.name == "BotFin2" || bb.gameObject.name == "BotFin3" || bb.gameObject.name == "BotFin6" || bb.gameObject.name == "BotFin7")
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
    void Update()
    {
        if (!portaAberta)
        {
            portaAberta = AbrirPorta();
            if (portaAberta)
            {
                Debug.Log("anuuuuuuuuuubisssss");
                GameObject pt = GameObject.Find("ProtecaoVidro");
                Destroy(pt);
            }
        }
    }
}
