using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piscar : MonoBehaviour {

    public Shader _shader;
    public Shader _shaderLit;
    public Renderer _renderer;

    // Use this for initialization
    void Start () {
        _renderer = GetComponent<Renderer>();
        _shaderLit = Shader.Find("Unlit/Color");
    }


    public IEnumerator Piscando()
    {
        yield return new WaitForSecondsRealtime(1);
        _renderer.material.shader = _shaderLit;
        yield return new WaitForSecondsRealtime(1);
        _renderer.material.shader = _shader;
    }

}
