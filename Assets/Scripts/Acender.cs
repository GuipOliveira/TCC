using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Acender : MonoBehaviour {

    public bool ativar;
    public Shader _shader;
    public Shader _shaderLit;
    public Renderer _renderer;

    // Use this for initialization
    void Start()
    {
        _renderer = GetComponent<Renderer>();
        _shaderLit = Shader.Find("Unlit/Color");
    }

    // Update is called once per frame
    void Update()
    {
        if (ativar)

            _renderer.material.shader = _shaderLit;
        if (!ativar)
            _renderer.material.shader = _shader;
    }

}
