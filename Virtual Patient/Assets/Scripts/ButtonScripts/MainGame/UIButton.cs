using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButton : MonoBehaviour {

    private UnityEngine.UI.Button thisButton;
    public GameObject[] childButtons;
    private Player player;

    public bool active;

	// Use this for initialization
	void Start ()
    {

        active = false;

        thisButton = gameObject.GetComponent<UnityEngine.UI.Button>();

        //Get the count of the child buttons
        int childButtonsCount = gameObject.transform.childCount;
        //Setup the array that holds the child buttons
        childButtons = new GameObject[childButtonsCount];
        //Add the child buttons to the array
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            childButtons[i] = gameObject.transform.GetChild(i).gameObject;
        }

        //Setup the method which is called on click
        thisButton.onClick.AddListener(OnClick);

        this.player = GameManager.instance.player.GetComponent<Player>();

	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void OnClick()
    {

        //If this button isn't already activated
        if(!active)
        {
            //Set this button to active
            this.active = true;

            //For all the buttons in the button UI
            for (int i = 0; i < this.transform.parent.childCount; i ++)
            {

                //If the button isn't this button
                if(this.transform.parent.GetChild(i).gameObject != this.gameObject)
                {
                    this.transform.parent.GetChild(i).GetComponent<UIButton>().unClick();
                }

            }

            //For all the sub buttons
            foreach(GameObject button in childButtons)
            {
                switch(player.currentEvolution)
                {
                        //If the player is currently at the bad state
                    case Player.evolution.bad:
                        //Set all the buttons in stage1 active
                        if(button.name == "stage1")
                        {
                            button.SetActive(true);
                        }
                        break;

                        //If the the player is currently at the mild state
                    case Player.evolution.mild:
                        //Set all the buttons in stage2 active
                        if (button.name == "stage2")
                        {
                            button.SetActive(true);
                        }
                        break;

                        //If the player is currently at the good state
                    case Player.evolution.good:
                        //Set all the buttons in stage3 active
                        if (button.name == "stage3")
                        {
                            button.SetActive(true);
                        }
                        break;

                }
            }

        }
        //This button is already clicked (has been double clicked)
        else
        {

            //Unactivate
            this.active = false;

            //For all child buttons unactive
            foreach(GameObject child in childButtons)
            {

                child.SetActive(false);

            }

        }

    }

    public void unClick()
    {

        if (active == true)
        {

            this.active = false;
            //For all child buttons unactive
            foreach (GameObject child in childButtons)
            {

                child.SetActive(false);

            }

        }

    }

}
