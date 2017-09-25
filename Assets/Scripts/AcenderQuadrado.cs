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

    private Color _verde = Color.green; //Aceso
    private Color _preto = Color.black; //Apagado

    // Mesma Coluna
    public AcenderQuadrado _col1;
    public AcenderQuadrado _col2;
    public AcenderQuadrado _col3;

    // Mesma Linha
    public AcenderQuadrado _lin1;
    public AcenderQuadrado _lin2;
    public AcenderQuadrado _lin3;



    // Use this for initialization
    void Start()
    {
        _audioSourceAcende = GetComponent<AudioSource>();
        _renderer = GetComponent<Renderer>();
        _shaderLit = Shader.Find("Unlit/Color"); 
        _shader = Shader.Find("Standard");
    }

    public void Acende()
    {
        _renderer.material.SetColor("_Color", _verde);
        _renderer.material.shader = _shaderLit;
        ativar = false;
        _audioSourceAcende.Play();
        ativo = true;
    }

    public void Apagar() {
        _renderer.material.SetColor("_Color", _preto);
        _renderer.material.shader = _shader;
        ativo = false;
    }


    public void Controla()
    {
        if (!ativar)
        {
            Acende();
            ativar = true;
        }
        else
        {
            Apagar();
            ativar = false;
        }
    }

}
