using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ApertaBotao : MonoBehaviour {

    public bool ativar;
    public bool ativo;
    public Animator _animator;
    public AudioSource _audioSource;

    // Use this for initialization
    void Awake () {
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }


    public void Ligar()
    {
        ativo = true;
        _animator.Play("BotaoClick",-1);
        ativar = false;
        _audioSource.Play(22050);
        trocaShader(ativo);

    }

    public void Desligar()
    {
        if (ativo)
        {
            ativar = false;
            ativo = false;
            trocaShader(false);
        }

    }


    public void trocaShader(bool troca)
    {
        if(ativo)
            GetComponentInChildren<Acender>().ativar = true;
        else
            GetComponentInChildren<Acender>().ativar = false;
    } 


}
