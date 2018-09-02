using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    #region setup

    public static GameManager instance = null;

    //Objects
    public Text diagnoseResults;

    //Game Variables
    float hunger, hungerMax = 50, hungerChange = 0.2f, full, tooFull; //Hunger
    float thirst, thirstMax = 50, thirstChange = 0.5f; //Thirst
    float bladder, bladderMax = 50, bladderChange = 0.3f; //Bladder
    float hygiene = 50, hygieneMax = 50, hygieneChange = 0.1f; //Hygiene
    float pain, painMax = 50, painChange = 0.2f; //Pain
    float over, overMax = 50, overChange = 0.1f; //Overdose
    float tire, tireMax = 50, awakeChange = 0.05f, sleepChange = 0.5f; //Tiredness
    bool awake; //TirednessToggle
    float sore, soreMax = 50, soreChange =0.4f, soreShift = 15; //Bed Sores
    //Major Game Variables
    float health, healthMax; //Health
    float happiness, happinessMax; //Happiness
    //Object Variables
    float bedpan, bedpanMax = 15; //bedpan
    public bool bedpanUse = false;
    float iv = 10, ivMax = 15; //IV Drip
    public bool ivUse = false;
    //Other Variables
    float fortify, fortifyChange = 0.05f; //Resistance to Overdose
    //NonGame Variables
    float timeCheck = 0, timeWait = 1;

    //Test Variables
    public float tHunger, tThirst, tBladder, tHygiene, tPain, tOver, tTire, tSore, tIV, tBP;

    //Source Buttons
    public Button[] sourceButton = new Button[6];

    //Togles IV Slider
    bool IVActive = false, BedpanActive = false;
    public GameObject IVSlider, BPSlider;

	// Use this for initialization
	void Start ()
    {
		
	}

    #endregion

    #region Time and Variable Change

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
            thirst -= 0.1f;
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
            bladder -= bladderChange;
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

    #endregion

    #region Methods for Influencing Stats

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
        if(sore < 0)
        {
            sore = 0;
        }
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
        IVActive = !IVActive;
        IVSlider.SetActive(IVActive);
    }
    public void BedpanToggle()
    {
        bedpanUse = !bedpanUse;

        BedpanActive = !BedpanActive;
        BPSlider.SetActive(BedpanActive);
    }

    #endregion

    #region Getting Stat Values

    //Get values
    public bool IfAwake()
    {
        return awake;
    }
    public float GetHunger()
    {
        return hunger;
    }
    public float GetFull()
    {
        return full;
    }
    public float GetTooFull()
    {
        return tooFull;
    }
    public float GetThirst()
    {
        return thirst;
    }
    public float GetIV()
    {
        return iv;
    }
    public float GetBladder()
    {
        return bladder;
    }
    public float GetBedpan()
    {
        return bedpan;
    }
    public float GetHygiene()
    {
        return hygiene;
    }
    public float GetPain()
    {
        return pain;
    }
    public float GetOver()
    {
        return over;
    }
    public float GetTire()
    {
        return tire;
    }
    public float GetSore()
    {
        return sore;
    }
    public float GetHealth()
    {
        return health;
    }
    public float GetHappiness()
    {
        return happiness;
    }
    //Return Lesser Variables
    public float Resistance()
    {
        return fortify;
    }

    #endregion

    #region Diagnosing

    //Diagnosing Results Method
    public void Diagnosed(int order)
    {
        if(order == 1)//Bedsores
        {
            diagnoseResults.text = "Bedsores at " + (int)sore + " out of " + soreMax;
        }
        if(order == 2)//Pain
        {
            diagnoseResults.text = "Pain at " + (int)pain + " out of " + painMax;
        }
        if (order == 3)//Overdose
        {
            diagnoseResults.text = "Drug Overdose at " + (int)over + " out of " + overMax;
        }
        if (order == 4)//Resistance
        {
            diagnoseResults.text = "Overdose Resistance at " + (int)fortify;
        }
    }

    #endregion

    #region Cancel Call
    public void Canceller(int button) //Closes all other buttons
    {
        for(int i = 0; i < sourceButton.Length; i++)
        {
            if(i != button)
            {
                if(sourceButton[i].name == "Button_Sink")
                {
                    sourceButton[i].GetComponent<SinkScript>().Cancel();
                }
                if (sourceButton[i].name == "Button_Diagnose")
                {
                    sourceButton[i].GetComponent<DiagnoseButton>().Cancel();
                }
                if (sourceButton[i].name == "Button_Bed")
                {
                    sourceButton[i].GetComponent<BedButton>().Cancel();
                }
                if (sourceButton[i].name == "TrayButton")
                {
                    sourceButton[i].GetComponent<TrayButton>().Cancel();
                }
                if (sourceButton[i].name == "IVButton")
                {
                    sourceButton[i].GetComponent<IVButton>().Cancel();
                }
                if (sourceButton[i].name == "DoorButton")
                {
                    sourceButton[i].GetComponent<DoorButton>().Cancel();
                }
            }
        }
    }
    #endregion

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