using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiscaCntroller : MonoBehaviour {

    //Sequencia de cores { 0, 7, 9, 2, 5, 1 };

    public int i = 0;    
    public Piscar[] _piscar;
	
    // Use this for initialization
	void Start () {
        _piscar = GetComponentsInChildren<Piscar>();
        StartCoroutine("Sequencia");
    }
	
    void piscarAll()
    {
        foreach(Piscar piscar in _piscar)
        {
            piscar.StartCoroutine("Piscando");
        }
    }

    public IEnumerator Sequencia()
    {
        if (i == 0)
        {
            yield return new WaitForSecondsRealtime(3);
            _piscar[0].StartCoroutine("Piscando");
            i++;
        }

        if (i == 1)
        {
            yield return new WaitForSecondsRealtime(1);
            _piscar[7].StartCoroutine("Piscando");
            i++;
        }

        if (i == 2)           
        {
            yield return new WaitForSecondsRealtime(1);
            _piscar[9].StartCoroutine("Piscando");
            i++;
        }

        if (i == 3)
        {
            yield return new WaitForSecondsRealtime(1);
            _piscar[2].StartCoroutine("Piscando");
            i++;
        }

        if (i == 4)
        {
            yield return new WaitForSecondsRealtime(1);
            _piscar[5].StartCoroutine("Piscando");
            i++;
        }

        if (i == 5)
        {
            yield return new WaitForSecondsRealtime(1);
            _piscar[1].StartCoroutine("Piscando");
            i++;
        }

        if(i == 6)
        {
            yield return new WaitForSecondsRealtime(1);
            piscarAll();
            i = 0;
            StartCoroutine("Sequencia");
        }

    }

}
