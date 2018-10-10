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
        sleep,
        fighting
    }

    public states playerState;

    public position moveTo;
    private position lastPosition;
    public evolution currentEvolution;

    public GameObject player;
    public GameObject wayPointsParent;

    private List<Transform> wayPoints;

    private int walkingSpeed = 100;
    //public int rotationSpeed = 10;

    //animaton variables;
    public bool standing = false;
    public bool actionCompleted = false;

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

        if(lastPosition != moveTo)
        {
            switch(moveTo)
            {
                case position.DiagnoseMachine:
                    this.GetComponent<PathFinding>().NagivateTo(wayPoints.Find(x => x.gameObject.name == "DiagnoseMachineWP").transform.position);
                    break;
                case position.Bed:
                    //Debug.Log(wayPoints.Find(x => x.gameObject.name == "BedWP").transform.position);
                    this.GetComponent<PathFinding>().NagivateTo(wayPoints.Find(x => x.gameObject.name == "BedWP").transform.position);
                    break;
                case position.Door:
                    this.GetComponent<PathFinding>().NagivateTo(wayPoints.Find(x => x.gameObject.name == "DoorWP").transform.position);
                    break;
                case position.FoodTray:
                    this.GetComponent<PathFinding>().NagivateTo(wayPoints.Find(x => x.gameObject.name == "FoodTrayWP").transform.position);
                    break;
                case position.Middle:
                    this.GetComponent<PathFinding>().NagivateTo(wayPoints.Find(x => x.gameObject.name == "MiddleWP").transform.position);
                    break;
                case position.Sink:
                    this.GetComponent<PathFinding>().NagivateTo(wayPoints.Find(x => x.gameObject.name == "SinkWP").transform.position);
                    break;
                case position.empty:
                    this.GetComponent<PathFinding>().Stop();
                    break;
            }
        }

        if (this.GetComponent<PathFinding>().currentPath != null && this.GetComponent<PathFinding>().currentPath.Count != 0)
            toggleWalkAnimation(true);
        else
            toggleWalkAnimation(false);

        lastPosition = moveTo;

        //if(moveTo == movement.shower){

        //    //animate the player to move to the shower
        //    if (!standing)
        //        StandUp();

        //    if (standing)
        //        walkTowards(new Vector3(-330, 10, -450));

        //}

	}

    void StandUp(){

        player.transform.Rotate(new Vector3(1, 0, 0));

        if (player.transform.rotation.eulerAngles.x >= 0 && player.transform.rotation.eulerAngles.x <= 5){

            float y = player.transform.rotation.y;
            float z = player.transform.rotation.z;
            float w = player.transform.rotation.w;

            player.transform.rotation = new Quaternion(0, y, z, w);
            standing = true;
        }
            
    }

    //void walkTowards(Vector3 location){

        //float step = walkingSpeed * Time.deltaTime;
        //player.transform.position = Vector3.MoveTowards(player.transform.position, location, step);

        //Animator anim = player.GetComponent<Animator>();

        //toggleWalkAnimation(true);

        //if(player.transform.position == location){
        //    toggleWalkAnimation(false);
        //    this.moveTo = position.idle;
        //}

    //}

    void toggleWalkAnimation(bool toggle){

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
