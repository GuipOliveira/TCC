using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoraCerta : MonoBehaviour {

    public bool ativo;
    
    void OnTriggerEnter(Collider other)
    {
        ativo = true;
        Debug.LogWarning("Bateu Ponteiro");
        PassaValor.setPonteiroRelogio(ativo);
    }

    void OnTriggerExit(Collider other)
    {
        ativo = false;
        Debug.LogWarning("Não Bateu Ponteiro");
        PassaValor.setPonteiroRelogio(ativo);
    }


}
