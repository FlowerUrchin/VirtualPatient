using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clipboard : MonoBehaviour
{

    //the button
    public UnityEngine.UI.Button clipBoard;
    public GameObject status;

    //Default rect position
    private int xPos = 300;
    private int yPos = 150;

    //animation variables
    public bool Opening = false;
    public bool Finished = false;
    public bool Closing = false;

    public bool Opened = false;
    public bool Closed = true;

    // Use this for initialization
    void Start()
    {

        //Setup the onclick
        clipBoard.onClick.AddListener(() => OpenClickBoard());

    }

    // Update is called once per frame
    void Update()
    {

        if(Opening){
            
            //Get the current position
            Vector3 currentPosition = clipBoard.GetComponent<RectTransform>().anchoredPosition3D;
            //Scale the clipboard
            Vector3 currentScale = clipBoard.GetComponent<RectTransform>().localScale;

            //If the animation has finished
            if (currentScale.x >= 14 && currentScale.y >= 7 && currentPosition.x <= 0 && currentPosition.y <= 0)
            {
                Opening = false;
                Opened = true;
                Finished = true;
                Closed = false;
            }

            //Move the position
            currentPosition.x -= 10;
            currentPosition.y -= 5;
            //Increase the scale
            currentScale.x += 0.4f;
            currentScale.y += 0.2f;

            //If the clipboard is in the center of the screen then stop the animation
            if (currentPosition.x <= 0 && currentPosition.y <= 0)
            {
                //ensure whole numbers
                currentPosition = new Vector3(0, 0, 0);
            }

            //If the clipboard is large enough
            if(currentScale.x >= 14 && currentScale.y >= 7){
                currentScale = new Vector3(14, 7, 1);
            }

            clipBoard.GetComponent<RectTransform>().localScale = currentScale;
            clipBoard.GetComponent<RectTransform>().anchoredPosition3D = currentPosition;

        };

        if(Closing){

            if(status.active == true){
                status.SetActive(false);
            }

            //Get the current position
            Vector3 currentPosition = clipBoard.GetComponent<RectTransform>().anchoredPosition3D;
            //Scale the clipboard
            Vector3 currentScale = clipBoard.GetComponent<RectTransform>().localScale;

            //If the animation has finished
            if (currentScale.x <= 1 && currentScale.y <= 1 && currentPosition.x >= 300 && currentPosition.y >= 150)
            {
                Closing = false;
                Opened = false;
                Finished = true;
                Closed = true;
            }

            //Move the position
            currentPosition.x += 10;
            currentPosition.y += 5;
            //Increase the scale
            currentScale.x -= 0.4f;
            currentScale.y -= 0.2f;

            //If the clipboard is in the center of the screen then stop the animation
            if (currentPosition.x >= 300 && currentPosition.y >= 150)
            {
                //ensure whole numbers
                currentPosition = new Vector3(300, 150, 0);
            }

            //If the clipboard is large enough
            if (currentScale.x <= 1 && currentScale.y <= 1)
            {
                currentScale = new Vector3(1, 1, 1);
            }

            clipBoard.GetComponent<RectTransform>().localScale = currentScale;
            clipBoard.GetComponent<RectTransform>().anchoredPosition3D = currentPosition;

        }

        if(Finished && Opened){
            Debug.Log("setting status to active");
            status.SetActive(true);
            Finished = false;
        }

    }

    public void OpenClickBoard() {

        if(!Opened){
            //player animation to increase size of the screen
            this.Opening = true;   
        }else{
            this.Closing = true;
        }

    }

}
