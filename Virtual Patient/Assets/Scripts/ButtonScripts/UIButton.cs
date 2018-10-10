using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButton : MonoBehaviour {

    private UnityEngine.UI.Button thisButton;
    public UnityEngine.UI.Button[] childButtons;

    public bool active;

	// Use this for initialization
	void Start ()
    {

        active = false;

        thisButton = gameObject.GetComponent<UnityEngine.UI.Button>();

        //Get the count of the child buttons
        int childButtonsCount = gameObject.transform.childCount;
        //Setup the array that holds the child buttons
        childButtons = new UnityEngine.UI.Button[childButtonsCount];
        //Add the child buttons to the array
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            childButtons[i] = gameObject.transform.GetChild(i).GetComponent<UnityEngine.UI.Button>();
        }

        //Setup the method which is called on click
        thisButton.onClick.AddListener(OnClick);

	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void OnClick()
    {

        if(active)
        {
            this.active = false;
            foreach (UnityEngine.UI.Button button in childButtons)
            {
                button.gameObject.SetActive(false);
            }
        }
        else
        {
            //Disable all other active buttons
            GameObject parent = this.transform.parent.gameObject;
            UIButton[] buttons = parent.GetComponentsInChildren<UIButton>();
            foreach (UIButton button in buttons)
            {
                button.unClick();
            }

            active = true;

            foreach (UnityEngine.UI.Button button in childButtons)
            {
                button.gameObject.SetActive(true);
            }

        }

    }

    public void unClick()
    {

        if (active == true)
        {
            
            foreach (UnityEngine.UI.Button button in childButtons)
            {
                button.gameObject.SetActive(false);
                active = false;
            }

        }

    }

}
