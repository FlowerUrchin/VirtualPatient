using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class Play : MonoBehaviour {

    private UnityEngine.UI.Button thisButton;

	// Use this for initialization
	void Start () {
		
        thisButton = gameObject.GetComponent<UnityEngine.UI.Button>();
        thisButton.onClick.AddListener(OnClick);

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnClick()
    {

        int PatientType = Random.Range(0, 2);

        switch (PatientType)
        {
            case 0:
                PlayerPrefs.SetInt("PatientType", 0);
                break;
            case 1:
                PlayerPrefs.SetInt("PatientType", 1);
                break;
            case 2:
                PlayerPrefs.SetInt("PatientType", 2);
                break;
        }

        PlayerPrefs.SetInt("Continue", 0);

        SceneManager.LoadScene(1, LoadSceneMode.Single);

    }

}
