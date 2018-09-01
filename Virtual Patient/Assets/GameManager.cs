﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;

    //Game Variables
    float hunger, hungerMax = 30, hungerChange = 0.2f, full, tooFull; //Hunger
    float thirst, thirstMax = 30, thirstChange = 0.5f; //Thirst
    float bladder, bladderMax = 30, bladderChange = 0.3f; //Bladder
    float hygiene, hygieneMax = 30, hygieneChange = 0.1f; //Hygiene
    float pain, painMax = 30, painChange = 0.2f; //Pain
    float over, overMax = 30, overChange = 0.1f; //Overdose
    float tire, tireMax = 30, awakeChange = 0.05f, sleepChange = 0.5f; //Tiredness
    bool awake; //TirednessToggle
    float sore, soreMax = 30, soreChange =0.8f, soreShift = 10; //Bed Sores
    //Major Game Variables
    float health, healthMax; //Health
    float happiness, happinessMax; //Happiness
    //Object Variables
    float bedpan, bedpanMax = 10; //bedpan
    public bool bedpanUse = false;
    float iv = 10, ivMax = 10; //IV Drip
    public bool ivUse = false;
    //Other Variables
    float fortify, fortifyChange = 0.1f; //Resistance to Overdose
    //NonGame Variables
    float timeCheck = 0, timeWait = 1;

    //Test Variables
    public float tHunger, tThirst, tBladder, tHygiene, tPain, tOver, tTire, tSore, tIV, tBP;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        tHunger = hunger;
        tThirst = thirst;
        tBladder = bladder;
        tHygiene = hygiene;
        tPain = pain;
        tOver = over;
        tTire = tire;
        tSore = sore;
        tIV = iv;
        tBP = bedpan;

        if(timeCheck <= Time.time)//Every Second 
        {
            GameVar();
            timeCheck = Time.time + timeWait;
        }
	}

    //GameVariable Section:
    void GameVar()//Updates all variables based on time
    {
        //Hunger
        if(full == 0) //Checks if the patient was stuffed
        {
            hunger += hungerChange;
            if (hunger > hungerMax)
            {
                hunger = hungerMax;
            }
        }
        else
        {
            full -= hungerChange;
            if(full < 0)
            {
                full = 0;
            }
        }
        //Thirst and iv
        if(ivUse && iv != 0)
        {
            iv -= thirstChange;
            if(iv < 0)
            {
                iv = 0;
            }
        }
        else
        {
            thirst += thirstChange;
            if (thirst > thirstMax)
            {
                thirst = thirstMax;
            }
        }
        //Bladder and Bedpan
        if (bedpanUse && bedpan != bedpanMax)
        {
            bedpan += bladderChange;
            if(bedpan > bedpanMax)
            {
                bedpan = bedpanMax;
            }
        }
        else
        {
            bladder += bladderChange;
            {
                if (bladder > bladderMax)
                {
                    bladder = bladderMax;
                }
            }
        }

        //Hygiene
        hygiene -= hygieneChange;
        if(hygiene < 0)
        {
            hygiene = 0;
        }
        //Pain
        pain += painChange;
        if(pain > painMax)
        {
            pain = painMax;
        }
        //Overdose
        over -= overChange;
        if(over < 0)
        {
            over = 0;
        }
        //Tiredness:
        if (awake)
        {
            tire += awakeChange;
            if(tire > tireMax)
            {
                tire = tireMax;
                awake = false;
            }
        }
        else
        {
            tire -= sleepChange;
            if(tire < 0)
            {
                tire = 0;
                awake = true;
            }
        }
        //Bedsores
        sore += soreChange;
        if(sore > soreMax)
        {
            sore = soreMax;
        }
        //Fortify/Resistance to overdose
        fortify -= fortifyChange;
        if(fortify < 0)
        {
            fortify = 0;
        }
    }

    //Action Methods for restoring values
    public void Feed(float food, float resist)//lowers the Hunger Variable
    {
        hunger -= food;
        if(hunger < 0)
        {
            full -= hunger; //Grows using the negative hunger amount
            hunger = 0;
        }
        fortify += resist;
    }
    public void Drink(float water) //Lowers Thirst Variable
    {
        thirst -= water;
        if(thirst < 0)
        {
            thirst = 0;
        }
    }
    public void RefillIV() //Returns IV to Max
    {
        iv = ivMax;
    }
    public void Toilet() //Resets Bladder Variable
    {
        bladder = 0;
    }
    public void EmptyBP() //Returns Bedpan to empty;
    {
        bedpan = 0;
    }
    public void Clean(float clean) //Raises Hygiene
    {
        hygiene += clean;
        if(hygiene > hygieneMax)
        {
            hygiene = hygieneMax;
        }
    }
    public void Painkiller(float relief, float overdose)//Lowers Pain, Raises Overdose
    {
        pain -= relief;
        float totalDose = overdose - fortify;
        if(totalDose < 0)
        {
            totalDose = 0.1f;
        }
        over += totalDose;
        if(pain < 0)
        {
            pain = 0;
        }
        if(over > overMax)
        {
            over = overMax;
        }
    }
    public void ShiftPosition()
    {
        sore -= soreShift;
    }
    public void Massage()//resets Bedsores
    {
        sore = 0;
    }
    //Methods for toggling states
    public void SleepState() //Toggle awake and asleep
    {
        awake = !awake;
    }
    public void IVToggle()
    {
        ivUse = !ivUse;
    }
    public void BedpanToggle()
    {
        bedpanUse = !bedpanUse;
    }

    //Get values
    public bool IfAwake()
    {
        return awake;
    }


    //Gamemanager Bit
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance == this)
        {
            Destroy(gameObject);
        }
    }
}
//Things to implement/Consider:
//tooFull having an affect. Lessen fitness, raise nausea.
//Maybe have both Full decrease and Hunger Increase?
//Bedsores only lessens when shifted, massage action added

//Required Implementations:
//Health based on variables
//Happiness based on variables