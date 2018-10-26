using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour {

    public UnityEngine.UI.Button Continue;

	// Use this for initialization
	void Start () {

        if (PlayerPrefs.GetInt("Continue") == 1)
            Continue.gameObject.SetActive(true);
        else
            Continue.gameObject.SetActive(false);

	}
	
	// Update is called once per frame
	void Update () {

        if (PlayerPrefs.GetInt("Continue") == 1)
            Continue.gameObject.SetActive(true);
        else
            Continue.gameObject.SetActive(false);
        
	}
}
