using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedButton : MonoBehaviour {

    public GameObject sleepButton, bedpanToggle, bedpanClean, bedBath;
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
            sleepButton.SetActive(true);
            bedpanToggle.SetActive(true);
            bedpanClean.SetActive(true);
            bedBath.SetActive(true);
            GameManager.instance.Canceller(2);
        }
        else
        {
            sleepButton.SetActive(false);
            bedpanToggle.SetActive(false);
            bedpanClean.SetActive(false);
            bedBath.SetActive(false);
        }
        on = !on;
    }
    public void Cancel()
    {
        sleepButton.SetActive(false);
        bedpanToggle.SetActive(false);
        bedpanClean.SetActive(false);
        bedBath.SetActive(false);
        on = false;
    }
}
