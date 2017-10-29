using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApertaBotaoFinal : MonoBehaviour {

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

    void Update()
    {
        if (ativo)
        {
            PassaValor.numPorta = 5;
        }
    }

}
