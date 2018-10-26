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
        Treadmill,
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

    public enum PatientType
    {
        phsyical,
        mental,
        ill
    }

    public PatientType patientType;
    public states playerState;

    public position moveTo;
    public position currentPosition;
    private position lastPosition;
    public evolution currentEvolution;

    public GameObject player;
    public GameObject wayPointsParent;
    public GameObject bed;

    public float Happiness { get; set; }

    private List<Transform> wayPoints;

    private int walkingSpeed = 100;
    //public int rotationSpeed = 10;

    //animaton variables;
    public bool standing = false;
    public bool halfStanding = false;
    public bool walking = false;

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

        if(GameManager.instance.Continue)
        {
            
        }
        else
        {
         
            playerState = states.awake;
            currentEvolution = evolution.bad;
            moveTo = position.empty;

        }

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
                case position.Treadmill:
                    if (this.GetComponent<PathFinding>().movingTo != moveTo)
                    {
                        this.GetComponent<PathFinding>().movingTo = moveTo;
                        this.GetComponent<PathFinding>().NagivateTo(wayPoints.Find(x => x.gameObject.name == "TreadmillWP").transform.position);
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
        {
            currentPosition = moveTo;
        }

	}

    public void GetOutOfBed()
    {

        Vector3 lookAt = new Vector3();

        switch(getOutBedAnimation)
        {

            //Stage 1
            case GetOutBedAnimation.StandUp:
                //Make sure the player isn't walking
                toggleWalkAnimation(false);
                //Rotate the player so that they're standing up.
                if (player.transform.rotation.eulerAngles.x > 1 && player.transform.rotation.eulerAngles.x < 359)
                {
                    //The player has executed the standing procedure
                    halfStanding = true;
                    player.transform.Rotate(-5.0f * Time.deltaTime * animationSpeed, 0.0f, 0.0f, Space.World);

                    getinBedAnimation = GetinBedAnimation.LieDown;

                }
                else
                {

                    //The player is not standing up
                    float y = player.transform.eulerAngles.y;
                    float z = player.transform.eulerAngles.z;

                    player.transform.eulerAngles = new Vector3(0, y, z);
                    getOutBedAnimation = GetOutBedAnimation.MoveDown;
                    getinBedAnimation = GetinBedAnimation.LieDown;

                }
                break;

                //Stage 2
            case GetOutBedAnimation.MoveDown:
                //Make sure the player isn't walking
                toggleWalkAnimation(false);

                //Move the bed down
                if (bed.transform.position.y > -63.1)
                {
                    getinBedAnimation = GetinBedAnimation.GoUp;
                    halfStanding = true;
                    bed.transform.position = (new Vector3(bed.transform.position.x, (bed.transform.position.y + (-30 * Time.deltaTime)), bed.transform.position.z));
                }
                else
                {
                    bed.transform.position = new Vector3(bed.transform.position.x, -63.1f, bed.transform.position.z);
                }

                //Move the player down
                if (player.transform.position.y > 0)
                {
                    getinBedAnimation = GetinBedAnimation.GoUp;
                    halfStanding = true;
                    player.transform.Translate(new Vector3(0, -30 * Time.deltaTime, 0), Space.World);
                }
                else
                {
                    player.transform.localPosition = new Vector3(-330, 0, -25);
                }

                if ((bed.transform.position.y >= -63.1f && bed.transform.position.y <= -63.1f) && (player.transform.position.y >= 0 && player.transform.position.y <= 0))
                {
                    getinBedAnimation = GetinBedAnimation.GoUp;
                    getOutBedAnimation = GetOutBedAnimation.Move;
                }

                break;

            case GetOutBedAnimation.Move:
                //Toggle walking animation
                toggleWalkAnimation(true);

                //MoveTowards
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(-210, 0, -25), walkingSpeed * Time.deltaTime);

                //Look at the position to walk to
                lookAt = new Vector3(-200, 0, -25) - transform.position;
                lookAt.y = 0;
                if (Vector3.Angle(transform.forward, new Vector3(-200, 0, -25) - new Vector3(transform.position.x, 0, transform.position.y)) > 1f)
                {
                    getinBedAnimation = GetinBedAnimation.Move;
                    halfStanding = true;
                    var lookRotation = Quaternion.LookRotation(lookAt);
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 5 * Time.deltaTime);
                }

                //If i've reached the position
                if (transform.localPosition.x == -210f && transform.localPosition.z == -25f)
                {
                    getinBedAnimation = GetinBedAnimation.Move;
                    getOutBedAnimation = GetOutBedAnimation.FaceScreen;
                }
                break;

            case GetOutBedAnimation.FaceScreen:
                //Toggle walking animation
                toggleWalkAnimation(true);

                //Look at the position to walk to
                lookAt = new Vector3(-210, 0, -100) - this.transform.position;
                lookAt.y = 0;

                if (Vector3.Angle(transform.forward, new Vector3(-210, 0, -100) - new Vector3(transform.position.x, 0, transform.position.y)) > 1f)
                {
                    halfStanding = true;
                    getinBedAnimation = GetinBedAnimation.Move;
                    var lookRotation = Quaternion.LookRotation(lookAt);
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 5 * Time.deltaTime);
                }
                //Looking towards the screen
                else
                {
                    getinBedAnimation = GetinBedAnimation.Move;
                    toggleWalkAnimation(false);
                    standing = true;
                    halfStanding = false;
                }
                break;


        }

    }

    public void GetInBed()
    {

        Vector3 lookAt = Vector3.zero;

        switch(getinBedAnimation)
        {
            //Stage 1
            case GetinBedAnimation.Move:
                //Toggle walking animation
                toggleWalkAnimation(true);

                //MoveTowards
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(-330, 0, -25), walkingSpeed * Time.deltaTime);

                //Look at the position to walk to
                lookAt = new Vector3(-340, 0, -25) - transform.position;
                lookAt.y = 0;
                if (Vector3.Angle(transform.forward, new Vector3(-340, 0, -25) - new Vector3(transform.position.x, 0, transform.position.y)) > 1f)
                {
                    halfStanding = true;
                    getOutBedAnimation = GetOutBedAnimation.Move;
                    var lookRotation = Quaternion.LookRotation(lookAt);
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 5 * Time.deltaTime);
                }

                //If i've reached the position
                if(transform.localPosition.x == -330f && transform.localPosition.z == -25f)
                {
                    getOutBedAnimation = GetOutBedAnimation.Move;
                    getinBedAnimation = GetinBedAnimation.FaceScreen;
                }
                break;

                //Stage 2
            case GetinBedAnimation.FaceScreen:
                //Toggle walking animation
                toggleWalkAnimation(true);

                //Look at the position to walk to
                lookAt = new Vector3(-330, 0, -100) - transform.position;
                lookAt.y = 0;
                if (Vector3.Angle(transform.forward, new Vector3(-330, 0, -100) - new Vector3(transform.position.x, 0, transform.position.y)) > 1f)
                {
                    halfStanding = true;
                    getOutBedAnimation = GetOutBedAnimation.Move;
                    var lookRotation = Quaternion.LookRotation(lookAt);
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 5 * Time.deltaTime);
                }
                //Looking towards the screen
                else
                {
                    getOutBedAnimation = GetOutBedAnimation.Move;
                    getinBedAnimation = GetinBedAnimation.GoUp;
                }
                break;

                //Stage 3
            case GetinBedAnimation.GoUp:
                //Make sure the player isn't walking
                toggleWalkAnimation(false);

                if (bed.transform.position.y < -14)
                {
                    getOutBedAnimation = GetOutBedAnimation.MoveDown;
                    halfStanding = true;
                    bed.transform.position = (new Vector3(bed.transform.position.x, (bed.transform.position.y + (30 * Time.deltaTime)), bed.transform.position.z));
                }
                else
                {
                    bed.transform.position = new Vector3(bed.transform.position.x, -13.1f, bed.transform.position.z);
                }

                if (player.transform.position.y < 50)
                {
                    getOutBedAnimation = GetOutBedAnimation.MoveDown;
                    halfStanding = true;
                    player.transform.Translate(new Vector3(0, 30 * Time.deltaTime, 0), Space.World);

                }
                else
                {
                    player.transform.localPosition = new Vector3(-330, 50, -25);
                }

                if ((bed.transform.position.y < -13.0f && bed.transform.position.y > -13.2f) && (player.transform.position.y <= 50 && player.transform.position.y >= 50))
                {
                    getOutBedAnimation = GetOutBedAnimation.MoveDown;
                    getinBedAnimation = GetinBedAnimation.LieDown;
                }

                break;
            
                //Stage 4
            case GetinBedAnimation.LieDown:
                //Make sure the player isn't walking
                toggleWalkAnimation(false);

                if (player.transform.rotation.eulerAngles.x > 313 || player.transform.rotation.eulerAngles.x < 311)
                {
                    halfStanding = true;
                    getOutBedAnimation = GetOutBedAnimation.StandUp;
                    player.transform.Rotate(10.0f * Time.deltaTime * animationSpeed, 0.0f, 0.0f, Space.World);
                }
                else
                {

                    float y = player.transform.eulerAngles.y;
                    float z = player.transform.eulerAngles.z;

                    player.transform.eulerAngles = new Vector3(-48, y, z);
                    //set standing and reset to starting action
                    halfStanding = false;
                    standing = false;
                    getOutBedAnimation = GetOutBedAnimation.StandUp;

                }
                   
                break;

        }
        
    }

    public void toggleWalkAnimation(bool toggle){

        //if toggling walk
        if (toggle)
        {

            walking = true;

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

            walking = false;

            Animator anim = player.GetComponent<Animator>();

            //incase the state changed while walking turn all off
            anim.SetLayerWeight(1, 0);
            anim.SetLayerWeight(3, 0);
            anim.SetLayerWeight(2, 0);

        }

    }

}
