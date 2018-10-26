using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClipBoard : MonoBehaviour
{

    //the button
    public UnityEngine.UI.Button clipBoard;
    public GameObject Sliders;
    public GameObject Text;
    public UnityEngine.UI.Text diagnosisText;
    public UnityEngine.UI.Text timer;

    public UnityEngine.UI.Image HappinessImage;
    public Sprite[] happyImages;

    //Default rect position
    private int xPos = 300;
    private int yPos = 150;

    public float animationMultiplier;

    //animation variables
    public bool Opening = false;
    public bool Finished = false;
    public bool Closing = false;

    public bool Opened = false;
    public bool Closed = true;

    private float animationLength = 60;

    // Use this for initialization
    void Start()
    {

        //Setup the onclick
        clipBoard.onClick.AddListener(() => OpenClickBoard());

    }

    // Update is called once per frame
    void Update()
    {

        if(GameManager.instance.player.GetComponent<Player>().Happiness >= 60)
        {
            HappinessImage.sprite = happyImages[0];
        }
        else if(GameManager.instance.player.GetComponent<Player>().Happiness < 60 && GameManager.instance.player.GetComponent<Player>().Happiness >= 30)
        {
            HappinessImage.sprite = happyImages[1];
        }
        else
        {
            HappinessImage.sprite = happyImages[2];
        }

        if(GameManager.instance.lastDiagnoseTime > 0)
        {

            diagnosisText.text = "Diagnosis:\n\n";

            foreach (Diagnosis diagnosis in GameManager.instance.diagnoses)
            {
                string value = "";

                if (diagnosis.currentValue > 60)
                    value = "good";
                else if (diagnosis.currentValue <= 60 && diagnosis.currentValue >= 30)
                    value = "mild";
                else
                    value = "bad";

                diagnosisText.text += diagnosis.diagnosisName + ": " + value + "\n"; 
            }

        }

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
                currentScaleSliders.x >= 3 && currentScaleSliders.y >= 6 && currentPositionSliders.x <= -220 && currentPositionSliders.y <= 0)
            {
                Opening = false;
                Opened = true;
                Finished = true;
                Closed = false;
            }

            //Move the position
            currentPosition.x += ((0 - 300) / animationLength) * animationMultiplier;
            currentPosition.y += ((0 - 150) / animationLength) * animationMultiplier;
            //Increase the scale
            currentScale.x += ((8-0) / animationLength) * animationMultiplier;
            currentScale.y += ((6-0) / animationLength) * animationMultiplier;

            //Move the position
            currentPositionSliders.x += ((0 - 365 - 220) / animationLength) * animationMultiplier;
            currentPositionSliders.y += ((0 - 145) / animationLength) * animationMultiplier;
            //Increase the scale
            currentScaleSliders.x += ((3) / animationLength) * animationMultiplier;
            currentScaleSliders.y += ((6) / animationLength) * animationMultiplier;

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
            if (currentPositionSliders.x <= -220 && currentPositionSliders.y <= 0)
            {
                //ensure whole numbers
                currentPositionSliders = new Vector3(-220, 0, 0);
            }

            //If the clipboard is large enough
            if (currentScaleSliders.x >= 3 && currentScaleSliders.y >= 6)
            {
                currentScaleSliders = new Vector3(3, 6, 1);
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
            currentPosition.x += ((300 - 0) / animationLength) * animationMultiplier;
            currentPosition.y += ((300 - 0) / animationLength) * animationMultiplier;
            //Increase the scale
            currentScale.x += ((0 - 8) / animationLength) * animationMultiplier;
            currentScale.y += ((0 - 6) / animationLength) * animationMultiplier;

            //Move the position
            currentPositionSliders.x += ((0 + 365 + 220) / animationLength) * animationMultiplier;
            currentPositionSliders.y += ((0 + 145) / animationLength) * animationMultiplier;
            //Increase the scale
            currentScaleSliders.x += ((0 - 3) / animationLength) * animationMultiplier;
            currentScaleSliders.y += ((0 - 6) / animationLength) * animationMultiplier;

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
            else if ((Time.time - GameManager.instance.lastDiagnoseTime) / 60 >= 1)
                timer.text = "Last Diagnosis\n" + Mathf.Floor((Time.time - GameManager.instance.lastDiagnoseTime) / 60) + " M";
            else
                timer.text = "Last Diagnosis\nLess than";

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