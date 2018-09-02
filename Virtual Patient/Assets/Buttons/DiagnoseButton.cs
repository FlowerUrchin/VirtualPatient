using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiagnoseButton : MonoBehaviour {

    public GameObject soreDia, painDia, overDia, fortDia;
    bool on = false;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
    public void DignoseOptions()
    {
        if (!on)
        {
            soreDia.SetActive(true);
            painDia.SetActive(true);
            overDia.SetActive(true);
            fortDia.SetActive(true);
            on = !on;
        }
        else
        {
            soreDia.SetActive(false);
            painDia.SetActive(false);
            overDia.SetActive(false);
            fortDia.SetActive(false);
            on = !on;
        }
    }
    public void Close()
    {
        soreDia.SetActive(false);
        painDia.SetActive(false);
        overDia.SetActive(false);
        fortDia.SetActive(false);
    }
}
