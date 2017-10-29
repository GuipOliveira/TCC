using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcenderNumero : MonoBehaviour {

    public Shader _shaderLit;

    // Use this for initialization
    void Start () {
        _shaderLit = Shader.Find("Unlit/Color");
    }

}
