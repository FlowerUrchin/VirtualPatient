using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinkScript : MonoBehaviour {

    public GameObject GDrink, WHands;
    bool on = false;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void Click()
    {
        if (!on)
        {
            GDrink.SetActive(true);
            WHands.SetActive(true);
        }
        else
        {
            GDrink.SetActive(false);
            WHands.SetActive(false);
        }
        on = !on;
    }
    public void Cancel()
    {
        GDrink.SetActive(false);
        WHands.SetActive(false);
        on = false;
    }
}
