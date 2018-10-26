using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CancelButtons : MonoBehaviour {

    public GameObject buttons;
    private UnityEngine.UI.Button thisButton;

	// Use this for initialization
	void Start () {
		
        thisButton = gameObject.GetComponent<UnityEngine.UI.Button>();
        //Setup the method which is called on click
        thisButton.onClick.AddListener(OnClick);

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnClick()
    {

        foreach(UIButton button in buttons.GetComponentsInChildren<UIButton>())
        {

            button.unClick();

        }

    }

}
