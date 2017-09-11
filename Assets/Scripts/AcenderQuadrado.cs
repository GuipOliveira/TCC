using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AcenderQuadrado : MonoBehaviour {

    public bool ativar;
    public bool ativo;
    public Shader _shader;
    public Shader _shaderLit;
    public Renderer _renderer;
    public AudioSource _audioSourceAcende;

    private Color _verde = Color.green;
    private Color _preto = Color.black;
    
    
    // Use this for initialization
    void Start()
    {
        _audioSourceAcende = GetComponent<AudioSource>();
        _renderer = GetComponent<Renderer>();
        _shaderLit = Shader.Find("Unlit/Color");      
    }


    public void Acende()
    {
        _renderer.material.SetColor("_Color", _verde);
        _renderer.material.shader = _shaderLit;
        ativar = false;
        _audioSourceAcende.Play();
    }


}
