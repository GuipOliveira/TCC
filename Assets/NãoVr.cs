using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;

public class NãoVr : MonoBehaviour {

	// Use this for initialization
	public void Start () {
        StartCoroutine(AtivarVr("cardboard"));
    }
	
    public IEnumerator AtivarVr(string name)
    {
        UnityEngine.XR.XRSettings.LoadDeviceByName(name);
        yield return null;
        UnityEngine.XR.XRSettings.enabled = true;
    }
}
