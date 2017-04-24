using UnityEngine;
using System.Collections;

public class ChecarMaterial : MonoBehaviour {

    public GameObject GO;
    public Material M;

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "BolaVerde")
        {
            print("funcionou!!");
        }
    }


}
