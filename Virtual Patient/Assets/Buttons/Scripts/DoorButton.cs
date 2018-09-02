using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorButton : MonoBehaviour {

    public GameObject shower, toilet;
    bool on;

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
            shower.SetActive(true);
            toilet.SetActive(true);
            GameManager.instance.Canceller(5);
        }
        else
        {
            shower.SetActive(false);
            toilet.SetActive(false);
        }
        on = !on;
    }
    public void Cancel()
    {
        shower.SetActive(false);
        toilet.SetActive(false);
        on = false;
    }
}
