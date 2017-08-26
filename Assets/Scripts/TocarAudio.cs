using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class TocarAudio : MonoBehaviour {

    public bool ativo;
    public AudioSource audio_source;

    // Use this for initialization
    void Start () {
        audio_source = GetComponent<AudioSource>();
    }


    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update () {
        if (ativo)
        {
            audio_source.Play();
        }
    }
}
