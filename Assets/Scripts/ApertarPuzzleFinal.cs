using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApertarPuzzleFinal : MonoBehaviour {

    public float range = 30f;
    public Camera _camera;
    public BotaoBinario2 bb;
    public BotaoBinario2[] arrayBb;

    // Use this for initialization
    void Start()
    {
        arrayBb = FindObjectsOfType<BotaoBinario2>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Aperta();
        }

    }

    void Aperta()
    {
        RaycastHit hit;
        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hit, range))
        {
            bb = hit.transform.GetComponent<BotaoBinario2>();
            if (bb != null)
            {
                bb.Controla();
            }
        }
    }
}
