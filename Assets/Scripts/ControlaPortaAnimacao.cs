using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlaPortaAnimacao : MonoBehaviour {

    public bool ativar;
    public Animator _animator;


	// Use this for initialization
	void Start () {
        _animator = GetComponent<Animator>();

	}

    private void OnTriggerEnter(Collider other)
    {

    }

    // Update is called once per frame
    void Update () {
        if (ativar)
        {
            _animator.SetBool("open", true);
        }
    }
}
