using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public enum movement
    {
        idle,
        bed,
        shower
    };

    public enum states{
        bad,
        mild,
        good
    }

    public movement moveTo;
    public states playerState;
    public GameObject player;

    private int walkingSpeed = 100;
    //public int rotationSpeed = 10;

    //animaton variables;
    public bool standing = false;
    public bool actionCompleted = false;

	// Use this for initialization
	void Start () {

        playerState = states.bad;

	}
	
	// Update is called once per frame
	void Update () {

        if(moveTo == movement.shower){

            //animate the player to move to the shower
            if (!standing)
                StandUp();

            if (standing)
                walkTowards(new Vector3(-330, 10, -450));

        }

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

    void walkTowards(Vector3 location){

        float step = walkingSpeed * Time.deltaTime;
        player.transform.position = Vector3.MoveTowards(player.transform.position, location, step);

        Animator anim = player.GetComponent<Animator>();

        toggleWalkAnimation(true);

        if(player.transform.position == location){
            toggleWalkAnimation(false);
            this.moveTo = movement.idle;
        }

    }

    void toggleWalkAnimation(bool toggle){

        //if toggling walk
        if (toggle)
        {

            Animator anim = player.GetComponent<Animator>();

            //depending on the players state set the walking animation
            switch (playerState)
            {
                case states.bad://bad
                    anim.SetLayerWeight(1, 1);
                    break;
                case states.mild://mild
                    anim.SetLayerWeight(3, 1);
                    break;

                case states.good://good
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
