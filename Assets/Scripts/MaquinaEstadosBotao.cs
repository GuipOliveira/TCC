using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaquinaEstadosBotao : MonoBehaviour
{

    public bool ativar;
    public ApertaBotao[] _apertaBotao;
    public GameObject player;
    public Apertar ap;
    public ControlaPortaAnimacao cpa;
    public GameObject _porta;
    public bool valendo;

    // Use this for initialization
    void Start()
    {
        ap = player.GetComponent<Apertar>();
        cpa = _porta.GetComponent<ControlaPortaAnimacao>();
        _apertaBotao = GetComponentsInChildren<ApertaBotao>();
    }

    public void Desativa()
    {
        foreach (ApertaBotao botao in _apertaBotao)
        {
            StartCoroutine(Espera());
            botao.Desligar();
        }
        ativar = false;
    }

    bool Acessa(int indice)
    {
        return _apertaBotao[indice].ativo;
    }

    int ChecarAtivo()
    {
        int indice = -1;
        foreach (ApertaBotao botao in _apertaBotao)
        {
            if (botao.ativo)
            {
                indice = System.Array.IndexOf(_apertaBotao, botao);
            }
        }
        //return(indice);
        return indice;
    }

    public void MaquinaEstados(int _contador)
    {
        if (_contador == 1)
            if (Acessa(0))
            {
                valendo = true;
                //ap.incrementaContador();
            }
            else
            {
                Desativa();
                ap.setContador(0);
                valendo = false;
            }

        if (_contador == 2 && valendo)
            if (!Acessa(8))
            {
                Desativa();
                ap.setContador(0);
            }

        if (_contador == 3 && valendo)
            if (!Acessa(4))
            {
                Desativa();
                ap.setContador(0);
            }

        if (_contador == 4 && valendo)
            if (!Acessa(1))
            {
                Desativa();
                ap.setContador(0);
                valendo = false;
            }

        if (_contador == 5 && valendo)
            if (!Acessa(9))
            {
                Desativa();
                ap.setContador(0);
                valendo = false;
            }

        if (_contador == 6 && valendo)
            if (Acessa(11))
            {
                Desativa();
                PassaValor.numPorta = 1;

            }
            else if (!Acessa(11))
            {
                Desativa();
                ap.setContador(0);
                valendo = false;
            }

    }

    public IEnumerator Espera()
    {
        yield return new WaitForSecondsRealtime(25f);
    }


    // Update is called once per frame
    void FixedUpdate()
    {
    }


}
