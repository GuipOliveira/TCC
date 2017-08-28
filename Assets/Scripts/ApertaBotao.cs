using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApertaBotao : MonoBehaviour {

    public bool ativar;
    public Animator _animator;

	// Use this for initialization
	void Start () {
        _animator = GetComponent<Animator>();
    }


    // Update is called once per frame
    void Update () {
		if (ativar)
        {
            _animator.SetBool("isPressed", true);
        }
	}
}
