using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IVButton : MonoBehaviour {

    public GameObject ivToggle, ivRefill;
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
            ivToggle.SetActive(true);
            ivRefill.SetActive(true);
        }
        else
        {
            ivToggle.SetActive(false);
            ivRefill.SetActive(false);
        }
        on = !on;
    }
    public void Cancel()
    {
        ivToggle.SetActive(false);
        ivRefill.SetActive(false);
        on = false;
    }
}
