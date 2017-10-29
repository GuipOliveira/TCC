using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApertarPuzzleCinco : MonoBehaviour {


    public float range = 30f;
    public Camera _camera;
    public BotaoBinario bb;
    public BotaoBinario[] arrayBb;

    // Use this for initialization
    void Start () {
        arrayBb = FindObjectsOfType<BotaoBinario>();
    }
	
	// Update is called once per frame
	void Update () {
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
                 bb = hit.transform.GetComponent<BotaoBinario>();
                if (bb != null)
                {
                     bb.Controla();                  
                }
            }
        }
}
