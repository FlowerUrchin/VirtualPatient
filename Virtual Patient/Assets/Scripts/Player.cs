using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public enum position
    {
        DiagnoseMachine,
        FoodTray,
        Bed,
        Sink,
        Door,
        Middle,
        empty
    };

    public enum evolution{
        bad,
        mild,
        good
    }

    public enum states
    {
        awake,
        asleep,
        fighting
    }

    public states playerState;

    public position moveTo;
    public position currentPosition;
    private position lastPosition;
    public evolution currentEvolution;

    public GameObject player;
    public GameObject wayPointsParent;
    public GameObject bed;

    private List<Transform> wayPoints;

    private int walkingSpeed = 100;
    //public int rotationSpeed = 10;

    //animaton variables;
    public bool standing = false;
    public bool actionCompleted = false;

    //Bed animations
    private enum GetinBedAnimation
    {
        FaceBed,
        Move,
        FaceScreen,
        GoUp,
        LieDown
    }

    private enum GetOutBedAnimation
    {
        StandUp,
        MoveDown,
        FaceRight,
        Move,
        FaceScreen
    }

    private GetinBedAnimation getinBedAnimation;
    private GetOutBedAnimation getOutBedAnimation;
    private float animationSpeed = 5.0f;


	// Use this for initialization
	void Start () {

        playerState = states.awake;
        currentEvolution = evolution.bad;
        moveTo = position.empty;
        wayPoints = new List<Transform>();

        foreach (Transform child in wayPointsParent.transform)
        {
            wayPoints.Add(child);
        }

        lastPosition = position.empty;

	}
	
	// Update is called once per frame
	void Update () {

        if(currentPosition != moveTo)
        {
            switch(moveTo)
            {
                case position.DiagnoseMachine:
                    //If not already nagivating
                    if(this.GetComponent<PathFinding>().movingTo != moveTo)
                    {
                        this.GetComponent<PathFinding>().movingTo = moveTo;
                        this.GetComponent<PathFinding>().NagivateTo(wayPoints.Find(x => x.gameObject.name == "DiagnoseMachineWP").transform.position);
                    }
                    break;
                case position.Bed:
                    if (this.GetComponent<PathFinding>().movingTo != moveTo)
                    {
                        this.GetComponent<PathFinding>().movingTo = moveTo;
                        this.GetComponent<PathFinding>().NagivateTo(wayPoints.Find(x => x.gameObject.name == "BedWP").transform.position);
                    }
                    break;
                case position.Door:
                    if (this.GetComponent<PathFinding>().movingTo != moveTo)
                    {
                        this.GetComponent<PathFinding>().movingTo = moveTo;
                        this.GetComponent<PathFinding>().NagivateTo(wayPoints.Find(x => x.gameObject.name == "DoorWP").transform.position);
                    }
                    break;
                case position.FoodTray:
                    if (this.GetComponent<PathFinding>().movingTo != moveTo)
                    {
                        this.GetComponent<PathFinding>().movingTo = moveTo;
                        this.GetComponent<PathFinding>().NagivateTo(wayPoints.Find(x => x.gameObject.name == "FoodTrayWP").transform.position);
                    }
                    break;
                case position.Middle:
                    if (this.GetComponent<PathFinding>().movingTo != moveTo)
                    {
                        this.GetComponent<PathFinding>().movingTo = moveTo;
                        this.GetComponent<PathFinding>().NagivateTo(wayPoints.Find(x => x.gameObject.name == "MiddleWP").transform.position);
                    }
                    break;
                case position.Sink:
                    if (this.GetComponent<PathFinding>().movingTo != moveTo)
                    {
                        this.GetComponent<PathFinding>().movingTo = moveTo;
                        this.GetComponent<PathFinding>().NagivateTo(wayPoints.Find(x => x.gameObject.name == "SinkWP").transform.position);
                    }
                    break;
                case position.empty:
                    if (this.GetComponent<PathFinding>().movingTo != moveTo)
                    {
                        this.GetComponent<PathFinding>().movingTo = moveTo;
                        this.GetComponent<PathFinding>().Stop();
                    }
                    break;
            }
        }

        if (this.GetComponent<PathFinding>().movingTo == moveTo && this.GetComponent<PathFinding>().currentPath == null)
            currentPosition = moveTo;

	}

    public void GetOutOfBed()
    {

        //Perform animation to get out of bed
        switch (getOutBedAnimation)
        {
            case GetOutBedAnimation.StandUp:
                if (player.transform.rotation.eulerAngles.x > 1 && player.transform.rotation.eulerAngles.x < 359)
                {
                    player.transform.Rotate(-5.0f * Time.deltaTime * animationSpeed, 0.0f, 0.0f, Space.World);
                }
                else
                {

                    float y = player.transform.eulerAngles.y;
                    float z = player.transform.eulerAngles.z;

                    player.transform.eulerAngles = new Vector3(0, y, z);
                    getOutBedAnimation = GetOutBedAnimation.MoveDown;

                }
                break;

            case GetOutBedAnimation.MoveDown:
                if (bed.transform.position.y > -63.1)
                {
                    bed.transform.position = (new Vector3(bed.transform.position.x, (bed.transform.position.y + (-10 * Time.deltaTime)), bed.transform.position.z));   
                }
                else
                {
                    bed.transform.position = new Vector3(bed.transform.position.x, -63.1f, bed.transform.position.z);
                }

                if (player.transform.position.y > 0)
                    player.transform.Translate(new Vector3(0, -10 * Time.deltaTime, 0), Space.World);
                else
                {
                    player.transform.localPosition = new Vector3(-330, 0, -25);
                }

                if((bed.transform.position.y >= -63.1f && bed.transform.position.y <= -63.1f) && (player.transform.position.y >= 0 && player.transform.position.y <= 0))
                {
                    getOutBedAnimation = GetOutBedAnimation.FaceRight;
                    toggleWalkAnimation(true);
                }

                break;

            case GetOutBedAnimation.FaceRight:
                if (player.transform.rotation.eulerAngles.y > 90)
                    player.transform.Rotate(0.0f, -10.0f * Time.deltaTime * animationSpeed, 0.0f, Space.World);
                else
                {

                    float x = player.transform.eulerAngles.x;
                    float z = player.transform.eulerAngles.z;

                    player.transform.eulerAngles = new Vector3(x, 90, z);
                    getOutBedAnimation = GetOutBedAnimation.Move;

                }
                break;

            case GetOutBedAnimation.Move:
                if (player.transform.position.x < -210)
                    player.transform.Translate(new Vector3(10 * Time.deltaTime * animationSpeed, 0, 0), Space.World);
                else
                {
                    player.transform.localPosition = new Vector3(-210, 0, -25);
                    getOutBedAnimation = GetOutBedAnimation.FaceScreen;
                }
                break;

            case GetOutBedAnimation.FaceScreen:
                //First rotate to look at the bed
                if (player.transform.rotation.eulerAngles.y < 180)
                    player.transform.Rotate(0.0f, 10.0f * Time.deltaTime * animationSpeed, 0.0f, Space.World);
                else
                {

                    float x = player.transform.eulerAngles.x;
                    float z = player.transform.eulerAngles.z;

                    player.transform.eulerAngles = new Vector3(x, 180, z);
                    getOutBedAnimation = GetOutBedAnimation.StandUp;
                    toggleWalkAnimation(false);
                    standing = true;

                }
                break;


        }

    }

    public void GetInBed()
    {

        //Perform animation to get into bed
        switch(getinBedAnimation)
        {
            case GetinBedAnimation.FaceBed:
                //First rotate to look at the bed
                if (player.transform.rotation.eulerAngles.y < 270)
                {
                    toggleWalkAnimation(true);
                    player.transform.Rotate(0.0f, 10.0f * Time.deltaTime * animationSpeed, 0.0f, Space.World);
                }
                else
                {

                    float x = player.transform.eulerAngles.x;
                    float z = player.transform.eulerAngles.z;

                    player.transform.eulerAngles = new Vector3(x, 270, z);
                    getinBedAnimation = GetinBedAnimation.Move;

                }
                break;

            case GetinBedAnimation.Move:
                if (player.transform.position.x > -330)
                    player.transform.Translate(new Vector3(-10 * Time.deltaTime * animationSpeed,0,0), Space.World);
                else
                {
                    player.transform.localPosition = new Vector3(-330, 0, -25);
                    getinBedAnimation = GetinBedAnimation.FaceScreen;
                }
                break;

            case GetinBedAnimation.FaceScreen:
                if (player.transform.rotation.eulerAngles.y > 180)
                    player.transform.Rotate(0.0f, -10.0f * Time.deltaTime * animationSpeed, 0.0f, Space.World);
                else
                {

                    float x = player.transform.eulerAngles.x;
                    float z = player.transform.eulerAngles.z;

                    player.transform.eulerAngles = new Vector3(x, 180, z);
                    getinBedAnimation = GetinBedAnimation.GoUp;
                    toggleWalkAnimation(false);

                }
                break;

            case GetinBedAnimation.GoUp:
                if (bed.transform.position.y < -14)
                {
                    bed.transform.position = (new Vector3(bed.transform.position.x, (bed.transform.position.y + (10 * Time.deltaTime)), bed.transform.position.z));
                }
                else
                {
                    bed.transform.position = new Vector3(bed.transform.position.x, -13.1f, bed.transform.position.z);
                }

                if (player.transform.position.y < 50)
                    player.transform.Translate(new Vector3(0, 10 * Time.deltaTime, 0), Space.World);
                else
                {
                    player.transform.localPosition = new Vector3(-330, 50, -25);
                }

                //if (player.transform.position.y == 50)
                //    Debug.Log("Player Done");

                //if (bed.transform.position.y < -13.0f && bed.transform.position.y > -13.2f)
                    //Debug.Log("Bed Done");

                if ((bed.transform.position.y < -13.0f && bed.transform.position.y > -13.2f) && (player.transform.position.y <= 50 && player.transform.position.y >= 50))
                {
                    getinBedAnimation = GetinBedAnimation.LieDown;
                }
                break;

            case GetinBedAnimation.LieDown:
                if (player.transform.rotation.eulerAngles.x > 313 || player.transform.rotation.eulerAngles.x < 311)
                {
                    player.transform.Rotate(10.0f * Time.deltaTime * animationSpeed, 0.0f, 0.0f, Space.World);
                }
                else
                {

                    float y = player.transform.eulerAngles.y;
                    float z = player.transform.eulerAngles.z;

                    player.transform.eulerAngles = new Vector3(-48, y, z);
                    //set standing and reset to starting action
                    standing = false;
                    getinBedAnimation = GetinBedAnimation.FaceBed;

                }
                break;

        }
        
    }

    public void toggleWalkAnimation(bool toggle){

        //if toggling walk
        if (toggle)
        {

            Animator anim = player.GetComponent<Animator>();

            //depending on the players state set the walking animation
            switch (currentEvolution)
            {
                case evolution.bad://bad
                    anim.SetLayerWeight(1, 1);
                    break;
                case evolution.mild://mild
                    anim.SetLayerWeight(3, 1);
                    break;

                case evolution.good://good
                    anim.SetLayerWeight(2, 1);
                    break;

            }

        }
        else //toggling walk off
        {

            Animator anim = player.GetComponent<Animator>();

            //incase the state changed while walking turn all off
            anim.SetLayerWeight(1, 0);
            anim.SetLayerWeight(3, 0);
            anim.SetLayerWeight(2, 0);

        }

    }

}
