using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlaPortaAnimacao : MonoBehaviour {

    public bool ativar;
    public Animator _animator;
    public AudioSource _audioSource;
    public AudioClip _audioClip;


    // Use this for initialization
    void Start () {
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update () {
        if (ativar)
        {
            gameObject.GetComponentInChildren<TrocaCorLuz>().ativo = true;
            _animator.SetBool("open", true);
            ativar = false;
            _audioSource.Play(22050);
            _audioSource.PlayOneShot(_audioClip,.3f);

        }
    }
}
