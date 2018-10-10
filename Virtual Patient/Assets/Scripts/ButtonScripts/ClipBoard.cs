using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClipBoard : MonoBehaviour
{

    //the button
    public UnityEngine.UI.Button clipBoard;
    public GameObject Sliders;
    public GameObject Text;
    public UnityEngine.UI.Text timer;

    //Default rect position
    private int xPos = 300;
    private int yPos = 150;

    public float speed;

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

        if (Opening)
        {

            //Get the current position
            Vector3 currentPosition = clipBoard.GetComponent<RectTransform>().anchoredPosition3D;
            //Scale the clipboard
            Vector3 currentScale = clipBoard.GetComponent<RectTransform>().localScale;

            //Get the current position
            Vector3 currentPositionSliders = Sliders.GetComponent<RectTransform>().anchoredPosition3D;
            //Scale the Sliders
            Vector3 currentScaleSliders = Sliders.GetComponent<RectTransform>().localScale;

            //If the animation has finished
            if (currentScale.x >= 8 && currentScale.y >= 6 && currentPosition.x <= 0 && currentPosition.y <= 0 &&
                currentScaleSliders.x >= 6 && currentScaleSliders.y >= 6 && currentPositionSliders.x <= 0 && currentPositionSliders.y <= 0)
            {
                Opening = false;
                Opened = true;
                Finished = true;
                Closed = false;
            }

            //Move the position
            currentPosition.x -= 32 * speed;
            currentPosition.y -= 16 * speed;
            //Increase the scale
            currentScale.x += 1f * speed;
            currentScale.y += 0.7f * speed;

            //Move the position
            currentPositionSliders.x -= 45 * speed;
            currentPositionSliders.y -= 15 * speed;
            //Increase the scale
            currentScaleSliders.x += 0.7f * speed;
            currentScaleSliders.y += 0.7f * speed;

            //If the clipboard is in the center of the screen then stop the animation
            if (currentPosition.x <= 0 && currentPosition.y <= 0)
            {
                //ensure whole numbers
                currentPosition = new Vector3(0, 0, 0);
            }

            //If the clipboard is large enough
            if (currentScale.x >= 8 && currentScale.y >= 6)
            {
                currentScale = new Vector3(8, 6, 1);
            }

            //If the clipboard is in the center of the screen then stop the animation
            if (currentPositionSliders.x <= 0 && currentPositionSliders.y <= 0)
            {
                //ensure whole numbers
                currentPositionSliders = new Vector3(0, 0, 0);
            }

            //If the clipboard is large enough
            if (currentScaleSliders.x >= 6 && currentScaleSliders.y >= 6)
            {
                currentScaleSliders = new Vector3(6, 6, 1);
            }

            clipBoard.GetComponent<RectTransform>().localScale = currentScale;
            clipBoard.GetComponent<RectTransform>().anchoredPosition3D = currentPosition;

            Sliders.GetComponent<RectTransform>().localScale = currentScaleSliders;
            Sliders.GetComponent<RectTransform>().anchoredPosition3D = currentPositionSliders;

        };

        if (Closing)
        {

            if (Text.active == true)
            {
                Text.SetActive(false);
            }

            //Get the current position
            Vector3 currentPosition = clipBoard.GetComponent<RectTransform>().anchoredPosition3D;
            //Scale the clipboard
            Vector3 currentScale = clipBoard.GetComponent<RectTransform>().localScale;

            //Get the current position
            Vector3 currentPositionSliders = Sliders.GetComponent<RectTransform>().anchoredPosition3D;
            //Scale the Sliders
            Vector3 currentScaleSliders = Sliders.GetComponent<RectTransform>().localScale;

            //If the animation has finished
            if (currentScale.x <= 1 && currentScale.y <= 1 && currentPosition.x >= 300 && currentPosition.y >= 150 &&
                currentScaleSliders.x <= 1 && currentScaleSliders.y <= 1 && currentPositionSliders.x >= 365 && currentPositionSliders.y >= 145)
            {
                Closing = false;
                Opened = false;
                Finished = true;
                Closed = true;
            }

            //Move the position
            currentPosition.x += 35 * speed;
            currentPosition.y += 16 * speed;
            //Increase the scale
            currentScale.x -= 0.7f * speed;
            currentScale.y -= 0.7f * speed;

            //Move the position
            currentPositionSliders.x += 45 * speed;
            currentPositionSliders.y += 15 * speed;
            //Increase the scale
            currentScaleSliders.x -= 1f * speed;
            currentScaleSliders.y -= 0.7f * speed;

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

            //If the clipboard is in the center of the screen then stop the animation
            if (currentPositionSliders.x >= 365 && currentPositionSliders.y >= 145)
            {
                //ensure whole numbers
                currentPositionSliders = new Vector3(365, 145, 0);
            }

            //If the clipboard is large enough
            if (currentScaleSliders.x <= 1 && currentScaleSliders.y <= 1)
            {
                currentScaleSliders = new Vector3(1, 1, 1);
            }

            clipBoard.GetComponent<RectTransform>().localScale = currentScale;
            clipBoard.GetComponent<RectTransform>().anchoredPosition3D = currentPosition;

            Sliders.GetComponent<RectTransform>().localScale = currentScaleSliders;
            Sliders.GetComponent<RectTransform>().anchoredPosition3D = currentPositionSliders;

        }

        if (Finished && Opened)
        {

            if (GameManager.instance.lastDiagnoseTime == 0)
                timer.text = "Last Diagnosis\nNo Diagnosis";
            else if ((Time.time - GameManager.instance.lastDiagnoseTime) / 60 / 60 / 24 >= 1)
                timer.text = "Last Diagnosis\n" + Mathf.Floor((Time.time - GameManager.instance.lastDiagnoseTime) / 60 / 60 / 24) + " D";
            else if ((Time.time - GameManager.instance.lastDiagnoseTime) / 60 / 60 >= 1)
                timer.text = "Last Diagnosis\n" + Mathf.Floor((Time.time - GameManager.instance.lastDiagnoseTime) / 60 / 60) + " H";
            else
                timer.text = "Last Diagnosis\n" + Mathf.Floor((Time.time - GameManager.instance.lastDiagnoseTime) / 60) + " M";

            Text.SetActive(true);
            Finished = false;
        }

    }

    public void OpenClickBoard()
    {

        if (!Opened)
        {
            //player animation to increase size of the screen
            this.Opening = true;
        }
        else
        {
            this.Closing = true;
        }

    }

}