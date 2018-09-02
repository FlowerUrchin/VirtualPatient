using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiagnosingButtons : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void DiagSores()//Bedsores
    {
        GameManager.instance.Diagnosed(1);
    }
    public void DiagPain()
    {
        GameManager.instance.Diagnosed(2);
    }
    public void DiagOver()
    {
        GameManager.instance.Diagnosed(3);
    }
    public void DiagFort()
    {
        GameManager.instance.Diagnosed(4);
    }
}
