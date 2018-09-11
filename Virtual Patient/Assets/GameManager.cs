using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    #region setup

    public static GameManager instance = null;

    //Objects
    public Text diagnoseResults;

    //Medical Condition
    bool sick, injured, internalinjured, mental;
    int state = 1; //1 = Bed, 2 = Weak, 3 = Recovering

    #region Game Variables
    static float hunger, hungerMax = 3600, hungerChange = 1, full, tooFull; //Hunger
    static float thirst, thirstMax = 3600, thirstChange = 2; //Thirst
    static float bladder, bladderMax = 3600, bladderChange = 1.5f; //Bladder
    static float hygiene = 3600, hygieneMax = 3600, hygieneChange = 0.3f; //Hygiene
    static float fit = 10, fitMax = 50, fitChange;//Fitness88888888888888888888888888888888888888888888888888888888888
    static float tire, tireMax = 3600, awakeChange = 1, sleepChange = 20; //Tiredness
    static bool awake; //TirednessToggle
    static float sore, soreMax = 3600, soreChange = 2.5f, soreShift = 800; //Bed Sores
    //Major Game Variables
    static float health = 25, healthMax = 25; //Health88888888888888888888888888888888888888888888888888888888888
    static float happiness = 50, happinessMax = 100; //Happiness88888888888888888888888888888888888888888888888888888888888
    //Object Variables
    static float bedpan, bedpanMax = 1800; //bedpan
    static public bool bedpanUse = false;
    static float iv = 1800, ivMax = 1800; //IV Drip
    static public bool ivUse = false;
    //Other Variables
    static float fortify, fortifyChange = 0.05f; //Resistance to Overdose88888888888888888888888888888888888888888888888888888888888
    //NonGame Variables
    static float timeCheck = 0, timeWait = 1;
    #endregion

    #region Disease Variables
    static float pain = 0, painMax = 50, painChange = 0.2f; //Pain
    static float nausea = 0, nauseaMax = 50, nauseaChange = 0.1f; //Nausea
    static float sickness = 0, sickMax = 50, sickChange = 0.2f; //Sickness
    static float mentalill = 0, mentalMax = 50, mentalChange = 0.2f; //Mentalilness
    #endregion

    #region Medication

    //Medicine Variables
    static float over, overMax = 50, overChange = 0.1f; //Overdose

    //Medicine Bottle
    static string medType, requiredType;
    static int strength; //1-3
    static float overD;
    static int contents = 0, contentsMax = 10; //How many pills left

    #endregion

    //Test Variables
    public float tHunger, tThirst, tBladder, tHygiene, tPain, tOver, tTire, tSore, tIV, tBP;

    //Whether in combat, in which stats stop decreasing over time, or not.
    static bool inCombat = false;

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

        if (!inCombat)
        {
            if (timeCheck <= Time.time)//Every Second 
            {
                GameVar(1);
                timeCheck = Time.time + timeWait;
            }
        }
	}

    //GameVariable Section:
    void GameVar(int timeMultiplier)//Updates all variables based on time
    {
        //Hunger
        if(full == 0) //Checks if the patient was stuffed
        {
            hunger += hungerChange*timeMultiplier;
            if (hunger > hungerMax)
            {
                hunger = hungerMax;
            }
        }
        else
        {
            full -= hungerChange * timeMultiplier;
            if(full < 0)
            {
                hunger -= full;
                full = 0;
            }
        }
        //Thirst and iv
        if(ivUse && iv != 0)
        {
            iv -= thirstChange * timeMultiplier;
            thirst -= 0.1f * timeMultiplier;
            if(iv < 0)
            {
                thirst -= iv;
                iv = 0;
            }
            if(thirst < 0)
            {
                thirst = 0;
            }
        }
        else
        {
            thirst += thirstChange * timeMultiplier;
            if (thirst > thirstMax)
            {
                thirst = thirstMax;
            }
        }
        //Bladder and Bedpan
        if (bedpanUse && bedpan != bedpanMax)
        {
            bedpan += bladderChange * timeMultiplier;
            bladder -= bladderChange * timeMultiplier;
            if(bedpan > bedpanMax)
            {
                bladder -= bedpan;
                bedpan = bedpanMax;
            }
            if(bladder < 0)
            {
                bladder = 0;
            }
        }
        else
        {
            bladder += bladderChange * timeMultiplier;
            {
                if (bladder > bladderMax)
                {
                    bladder = bladderMax;
                }
            }
        }

        //Hygiene
        hygiene -= hygieneChange * timeMultiplier;
        if(hygiene < 0)
        {
            hygiene = 0;
        }
        //Tiredness:
        if (awake)
        {
            tire += awakeChange * timeMultiplier;
            if(tire > tireMax)
            {
                tire = tireMax;
                awake = false;
            }
        }
        else
        {
            tire -= sleepChange * timeMultiplier;
            if(tire < 0)
            {
                tire = 0;
                awake = true;
            }
        }
        //Bedsores
        sore += soreChange * timeMultiplier;
        if(sore > soreMax)
        {
            sore = soreMax;
        }

        //Pain
        pain += painChange * timeMultiplier;
        if (pain > painMax)
        {
            pain = painMax;
        }
        //Nausea
        nausea -= nauseaChange * timeMultiplier;
        if(nausea < 0)
        {
            nausea = 0;
        }
        //Sickness
        sickness += sickChange * timeMultiplier;
        if(sickness > sickMax)
        {
            sickness = sickMax;
        }
        //Mental Illness
        mentalill += mentalChange;
        if(mentalill > mentalMax)
        {
            mentalill = mentalMax;
        }
        //Overdose
        over -= overChange * timeMultiplier;
        if (over < 0)
        {
            over = 0;
        }
        //Fortify/Resistance to overdose
        fortify -= fortifyChange * timeMultiplier;
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

    public void Medication()
    {
        if(medType == "Painkiller")//Pain
        {
            Painkiller();
        }
        if(medType == "")//Sickness
        {
            if (sick)
            {

            }
            else//If not sick, bad effects increase
            {

            }
        }
        if (medType == "")//Nausea
        {

        }
        if (medType == "")//Mental
        {

        }
    }
    public void Painkiller()//Lowers Pain, Raises Overdose
    {
        if (contents > 0)
        {
            pain -= strength * 5;
            float totalDose = overD - fortify;
            if (totalDose < 0)
            {
                totalDose = 0.1f;
            }
            over += totalDose;
            if (pain < 0)
            {
                pain = 0;
            }
            if (over > overMax)
            {
                over = overMax;
            }
        }
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
    public float GetNausea()
    {
        return nausea;
    }
    public float GetIll()
    {
        return sickness;
    }
    public float GetMental()
    {
        return mentalill;
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
        if(order == 5)//Nausea
        {
            diagnoseResults.text = "Nausea at " + (int)nausea + " out of " + nauseaMax;
        }
        if(order == 6)//Illness
        {
            diagnoseResults.text = "Illness at " + (int)sickness + " out of " + sickMax;
        }
        if(order == 7)//Mental illness
        {
            diagnoseResults.text = "Mental Illness at " + (int)mentalill + " out of " + mentalMax;
        }
    }

    #endregion

    #region medicalCabinet
    public void FillMed(string name, int power, float od)
    {
        medType = name;
        strength = power;
        overD = od;
        contents = contentsMax;
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

    #region TimeSkip
    public void TimeJumpHour()
    {
        GameVar(3600);
    }
    public void TimeJumpMinute()
    {
        GameVar(60);
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

//Popup if content empty with medicine

//Required Implementations:
//Health based on variables
//Happiness based on variables
//Patient State and Locking Options
//Fitness stat
//Nausea stat
//Sickness Stat
//Antibiotics
//Anti-nausea
//
//
//
//
//
//