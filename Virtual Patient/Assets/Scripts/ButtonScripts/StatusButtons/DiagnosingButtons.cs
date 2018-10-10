using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiagnosingButtons : MonoBehaviour {

    private GameManager gameManager;
    public UnityEngine.UI.Button button;

	// Use this for initialization
	void Start () {
	
        //saves having to manually put the game manager in everytime
        gameManager = GameManager.instance;
        button = gameObject.GetComponent<UnityEngine.UI.Button>();
        button.onClick.AddListener(Diagnose);

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void Diagnose()
    {
        
        gameManager.lastDiagnoseTime = Time.time - gameManager.lastDiagnoseTime;

    }

}
