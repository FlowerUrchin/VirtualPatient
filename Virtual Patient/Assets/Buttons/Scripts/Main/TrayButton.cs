using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrayButton : MonoBehaviour {

    public GameObject food, painkiller;
    bool on = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Click()
    {
        if (!on)
        {
            food.SetActive(true);
            painkiller.SetActive(true);
            GameManager.instance.Canceller(3);
        }
        else
        {
            food.SetActive(false);
            painkiller.SetActive(false);
        }
        on = !on;
    }
    public void Cancel()
    {
        food.SetActive(false);
        painkiller.SetActive(false);
        on = false;
    }
}
