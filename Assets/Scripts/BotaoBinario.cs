using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotaoBinario : MonoBehaviour {

    public bool ativo;
    public Shader _shader;
    public Shader _shaderLit;
    public Renderer _renderer;


    void Start()
    {
        _renderer = gameObject.GetComponentInChildren<Renderer>();
        _shaderLit = Shader.Find("Unlit/Color");
        _shader = Shader.Find("Standard");
    }

    public void Acender()
    {
        _renderer.material.SetColor("_Color", Color.red);
        _renderer.material.shader = _shaderLit;
        ativo = true;
    }

    public void Apagar()
    {
        _renderer.material.SetColor("_Color", Color.red);
        _renderer.material.shader = _shader;
        ativo = false;
    }

    public void Controla()
    {
        if (!ativo)
        {
            Acender();
        }
        else if (ativo)
        {
            Apagar();
        }

    }

     void Update()
    {
        if (ativo)
        {
            Acender();
        }
        else
            Apagar();
    }

}
