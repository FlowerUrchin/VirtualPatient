using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    #region setup

    public static GameManager instance = null;
    public GameObject player;
    public GameObject light;

    //Objects
    public Text diagnoseResults;

    //Medical Condition
    bool sick, injured, internalinjured, mental;
    int state = 1; //1 = Bed, 2 = Weak, 3 = Recovering

    #region Game Variables
    //Status Variables
    public Status[] statuses;
    public Diagnosis[] diagnoses;

    //rates of decay
    public float decayWhileAwake;
    public float decayWhileAsleep;
    public float hungerDecay;
    public float thirstDecay;
    public float bladderDecay;
    public float fitnessDecay;
    public float hygieneDecay;
    public float tirednessDecay;

    //Diagnosis variables
    public int maxBedSores = 500;
    public int bedSoresDamagePercent = 40;
    public int bedSoresDamage = 10;
    public float BedSoreDecay;

    public int maxPain = 1000;
    public int painDamagePercent = 60;
    public int painDamage = 20;
    public float painDecay;

    public int maxOverDose = 1000;
    public int overDoseDamagePercent = 60;
    public int overDoseDamage = 30;
    public float overDoseDecay;

    public int maxBloodToxicity = 1000;
    public int bloodToxicityPercent = 80;
    public int bloodToxicityDamage = 50;
    public float bloodToxicityDecay;

    public List<Task> currentTask = new List<Task>();

    private float bedsore { get; set; }
    private float pain { get; set; }
    private float nausea { get; set; }
    private float sickness { get; set; }
    private float mentalill { get; set; }

    //Time Variables
    static float lastTime = 0;
    static float waitTime = 1;
    private float addValueLastTime = 0;
    private float addValueWaitTime = 1;

    public float lastDiagnoseTime = 0;

    //static float hunger, hungerMax = 3600, hungerChange = 1, full, tooFull; //Hunger
    //static float thirst, thirstMax = 3600, thirstChange = 2; //Thirst
    //static float bladder, bladderMax = 3600, bladderChange = 1.5f; //Bladder
    //static float hygiene = 3600, hygieneMax = 3600, hygieneChange = 0.3f; //Hygiene
    //static float fit = 10, fitMax = 50, fitChange;//Fitness88888888888888888888888888888888888888888888888888888888888
    //static float tire, tireMax = 3600, awakeChange = 1, sleepChange = 20; //Tiredness
    //static bool awake; //TirednessToggle
    //static float sore, soreMax = 3600, soreChange = 2.5f, soreShift = 800; //Bed Sores
    ////Major Game Variables
    //static float health = 25, healthMax = 25; //Health88888888888888888888888888888888888888888888888888888888888
    //static float happiness = 50, happinessMax = 100; //Happiness88888888888888888888888888888888888888888888888888888888888
    ////Object Variables
    //static float bedpan, bedpanMax = 1800; //bedpan
    //static public bool bedpanUse = false;
    //static float iv = 1800, ivMax = 1800; //IV Drip
    //static public bool ivUse = false;
    ////Other Variables
    //static float fortify, fortifyChange = 0.05f; //Resistance to Overdose88888888888888888888888888888888888888888888888888888888888
    ////NonGame Variables
    //static float timeCheck = 0, timeWait = 1;
    #endregion

    #region Disease Variables
    //static float pain = 0, painMax = 50, painChange = 0.2f; //Pain
    //static float nausea = 0, nauseaMax = 50, nauseaChange = 0.1f; //Nausea
    //static float sickness = 0, sickMax = 50, sickChange = 0.2f; //Sickness
    //static float mentalill = 0, mentalMax = 50, mentalChange = 0.2f; //Mentalilness
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

    ////Test Variables
    //public float tHunger, tThirst, tBladder, tHygiene, tPain, tOver, tTire, tSore, tIV, tBP;

    ////Whether in combat, in which stats stop decreasing over time, or not.
    //static bool inCombat = false;

    ////Source Buttons
    //public Button[] sourceButton = new Button[6];

    ////Togles IV Slider
    //bool IVActive = false, BedpanActive = false;
    //public GameObject IVSlider, BPSlider;

    //Called before start
    void Awake()
    {
        //setup the gamemanager
        if (instance == null)
        {
            instance = this;
        }
        else if (instance == this)
        {
            Destroy(gameObject);
        }
    }

	// Use this for initialization
	void Start ()
    {

        //setup initial variables
        statuses = new Status[]{
            new Status("hunger", hungerDecay),
            new Status("thirst", thirstDecay),
            new Status("bladder", bladderDecay),
            new Status("fitness", fitnessDecay),
            new Status("hygiene", hygieneDecay),
            new Status("tiredness", tirednessDecay),
            new Status("health", 0),
            new Status("happiness", 0)
        };

        diagnoses = new Diagnosis[]{
            new Diagnosis("Bed Sores", maxBedSores, bedSoresDamagePercent, bedSoresDamage, BedSoreDecay),
            new Diagnosis("Pain", maxPain, painDamagePercent, painDamage, painDecay),
            new Diagnosis("Over Dose", maxOverDose, overDoseDamagePercent, overDoseDamage, overDoseDecay),
            new Diagnosis("Blood Toxicity", maxBloodToxicity, bloodToxicityPercent, bloodToxicityPercent, bloodToxicityDecay)
        };

	}

    #endregion

    #region Time and Variable Change

    // Update is called once per frame
    void Update ()
    {

        //update depending where the player is
        if (player.GetComponent<Player>().playerState == Player.states.awake)
        {
            Decay(decayWhileAwake);
        }else if (player.GetComponent<Player>().playerState == Player.states.sleep){
            Decay(decayWhileAsleep);
        }

        //Task system
        TaskSystem();


	}

    //Task System
    void TaskSystem()
    {

        if (currentTask.Count > 0)
        {

            //If the player is asleep wake the player up.
            if (currentTask[0].statusEffects != "tiredness" && player.GetComponent<Player>().playerState == Player.states.sleep)
            {

                player.GetComponent<Player>().playerState = Player.states.awake;
                light.GetComponent<Light>().color = new Color(1, 0.95f, 0.839f, 1);

            }

            if (currentTask[0].atPosition())
            {

                if(currentTask[0].position == Player.position.empty)
                {
                    this.player.GetComponent<Player>().moveTo = currentTask[0].position;   
                }

                if(addValueLastTime <= Time.time)
                {

                    addValueLastTime = Time.time * waitTime;

                    //Status Task
                    if(currentTask[0].taskType == Task.type.status)
                    {

                        //If the task is to sleep and the player is in position then dim the lights and put the player to sleep.
                        if(currentTask[0].statusEffects == "tiredness" && player.GetComponent<Player>().playerState != Player.states.sleep)
                        {
                            player.GetComponent<Player>().playerState = Player.states.sleep;
                            light.GetComponent<Light>().color = new Color(0.09f, 0.07f, 0.03f, 1);
                        }

                        getStatus(currentTask[0].statusEffects).AddValue(currentTask[0].addValue);

                        if (getStatus(currentTask[0].statusEffects).statusValue == 100)
                        {

                            //Wake up if they were asleep
                            if (currentTask[0].statusEffects == "tiredness")
                            {
                                player.GetComponent<Player>().playerState = Player.states.awake;
                                light.GetComponent<Light>().color = new Color(255, 244, 214);
                            }

                            currentTask.RemoveAt(0);
                        }

                    }
                    //Diagnosis Task
                    else
                    {

                        getDiagnosis(currentTask[0].statusEffects).AddValue(currentTask[0].addValue);

                        if(getDiagnosis(currentTask[0].statusEffects).currentValue == 0)
                        {
                            currentTask.RemoveAt(0);
                        }

                    }

                }

            }
            else
            {
                this.player.GetComponent<Player>().moveTo = currentTask[0].position;
            }

        }

    }

    //Decaying Variables
    void Decay(float rateOfDecay){

        //Every second
        if(lastTime <= Time.time){

            lastTime = Time.time * waitTime;

            //Update every status with the decaying value * rate of decay
            foreach (Status status in statuses)
            {

                //Add decay value as negative
                status.AddValue(-((status.decayValue / 100) * rateOfDecay));

            }

            foreach (Diagnosis diagnosis in diagnoses)
            {
                
                if (diagnosis.currentValue > diagnosis.takeDamagePercent)
                {

                    this.getStatus("health").AddValue(-diagnosis.damage);

                }

                diagnosis.AddValue(((diagnosis.decayValue / 100) * rateOfDecay));

            }

        }

    }

    public Status getStatus(string name){

        foreach(Status status in statuses){
            if (status.statusName == name)
                return status;
        }

        throw new System.Exception("no status named: " + name);

    }

    public Diagnosis getDiagnosis(string name){

        foreach(Diagnosis diagnosis in diagnoses)
        {
            if (diagnosis.diagnosisName == name)
                return diagnosis;
        }

        throw new System.Exception("no status named: " + name);

    }

    ////GameVariable Section:
    //void GameVar(int timeMultiplier)//Updates all variables based on time
    //{
    //    //Hunger
    //    if(full == 0) //Checks if the patient was stuffed
    //    {
    //        hunger += hungerChange*timeMultiplier;
    //        if (hunger > hungerMax)
    //        {
    //            hunger = hungerMax;
    //        }
    //    }
    //    else
    //    {
    //        full -= hungerChange * timeMultiplier;
    //        if(full < 0)
    //        {
    //            hunger -= full;
    //            full = 0;
    //        }
    //    }
    //    //Thirst and iv
    //    if(ivUse && iv != 0)
    //    {
    //        iv -= thirstChange * timeMultiplier;
    //        thirst -= 0.1f * timeMultiplier;
    //        if(iv < 0)
    //        {
    //            thirst -= iv;
    //            iv = 0;
    //        }
    //        if(thirst < 0)
    //        {
    //            thirst = 0;
    //        }
    //    }
    //    else
    //    {
    //        thirst += thirstChange * timeMultiplier;
    //        if (thirst > thirstMax)
    //        {
    //            thirst = thirstMax;
    //        }
    //    }
    //    //Bladder and Bedpan
    //    if (bedpanUse && bedpan != bedpanMax)
    //    {
    //        bedpan += bladderChange * timeMultiplier;
    //        bladder -= bladderChange * timeMultiplier;
    //        if(bedpan > bedpanMax)
    //        {
    //            bladder -= bedpan;
    //            bedpan = bedpanMax;
    //        }
    //        if(bladder < 0)
    //        {
    //            bladder = 0;
    //        }
    //    }
    //    else
    //    {
    //        bladder += bladderChange * timeMultiplier;
    //        {
    //            if (bladder > bladderMax)
    //            {
    //                bladder = bladderMax;
    //            }
    //        }
    //    }

    //    //Hygiene
    //    hygiene -= hygieneChange * timeMultiplier;
    //    if(hygiene < 0)
    //    {
    //        hygiene = 0;
    //    }
    //    //Tiredness:
    //    if (awake)
    //    {
    //        tire += awakeChange * timeMultiplier;
    //        if(tire > tireMax)
    //        {
    //            tire = tireMax;
    //            awake = false;
    //        }
    //    }
    //    else
    //    {
    //        tire -= sleepChange * timeMultiplier;
    //        if(tire < 0)
    //        {
    //            tire = 0;
    //            awake = true;
    //        }
    //    }
    //    //Bedsores
    //    sore += soreChange * timeMultiplier;
    //    if(sore > soreMax)
    //    {
    //        sore = soreMax;
    //    }

    //    //Pain
    //    pain += painChange * timeMultiplier;
    //    if (pain > painMax)
    //    {
    //        pain = painMax;
    //    }
    //    //Nausea
    //    nausea -= nauseaChange * timeMultiplier;
    //    if(nausea < 0)
    //    {
    //        nausea = 0;
    //    }
    //    //Sickness
    //    sickness += sickChange * timeMultiplier;
    //    if(sickness > sickMax)
    //    {
    //        sickness = sickMax;
    //    }
    //    //Mental Illness
    //    mentalill += mentalChange;
    //    if(mentalill > mentalMax)
    //    {
    //        mentalill = mentalMax;
    //    }
    //    //Overdose
    //    over -= overChange * timeMultiplier;
    //    if (over < 0)
    //    {
    //        over = 0;
    //    }
    //    //Fortify/Resistance to overdose
    //    fortify -= fortifyChange * timeMultiplier;
    //    if(fortify < 0)
    //    {
    //        fortify = 0;
    //    }
    //}

    #endregion

    //#region Methods for Influencing Stats

    ////Action Methods for restoring values
    //public void Feed(float food, float resist)//lowers the Hunger Variable
    //{
    //    hunger -= food;
    //    if(hunger < 0)
    //    {
    //        full -= hunger; //Grows using the negative hunger amount
    //        hunger = 0;
    //    }
    //    fortify += resist;
    //}
    //public void Drink(float water) //Lowers Thirst Variable
    //{
    //    thirst -= water;
    //    if(thirst < 0)
    //    {
    //        thirst = 0;
    //    }
    //}
    //public void RefillIV() //Returns IV to Max
    //{
    //    iv = ivMax;
    //}
    //public void Toilet() //Resets Bladder Variable
    //{
    //    bladder = 0;
    //}
    //public void EmptyBP() //Returns Bedpan to empty;
    //{
    //    bedpan = 0;
    //}

    //public void TakeShower(){
    //    player.GetComponent<Player>().moveTo = Player.movement.shower;
    //}

    //public void Clean(float clean) //Raises Hygiene
    //{
    //    hygiene += clean;
    //    if(hygiene > hygieneMax)
    //    {
    //        hygiene = hygieneMax;
    //    }
    //}
    //public void ShiftPosition()
    //{
    //    sore -= soreShift;
    //    if(sore < 0)
    //    {
    //        sore = 0;
    //    }
    //}
    //public void Massage()//resets Bedsores
    //{
    //    sore = 0;
    //}
    ////Methods for toggling states
    //public void SleepState() //Toggle awake and asleep
    //{
    //    awake = !awake;
    //}
    //public void IVToggle()
    //{
    //    ivUse = !ivUse;
    //    IVActive = !IVActive;
    //    IVSlider.SetActive(IVActive);
    //}
    //public void BedpanToggle()
    //{
    //    bedpanUse = !bedpanUse;

    //    BedpanActive = !BedpanActive;
    //    BPSlider.SetActive(BedpanActive);
    //}

    //public void Medication()
    //{
    //    if(medType == "Painkiller")//Pain
    //    {
    //        Painkiller();
    //    }
    //    if(medType == "")//Sickness
    //    {
    //        if (sick)
    //        {

    //        }
    //        else//If not sick, bad effects increase
    //        {

    //        }
    //    }
    //    if (medType == "")//Nausea
    //    {

    //    }
    //    if (medType == "")//Mental
    //    {

    //    }
    //}
    //public void Painkiller()//Lowers Pain, Raises Overdose
    //{
    //    if (contents > 0)
    //    {
    //        pain -= strength * 5;
    //        float totalDose = overD - fortify;
    //        if (totalDose < 0)
    //        {
    //            totalDose = 0.1f;
    //        }
    //        over += totalDose;
    //        if (pain < 0)
    //        {
    //            pain = 0;
    //        }
    //        if (over > overMax)
    //        {
    //            over = overMax;
    //        }
    //    }
    //}

    //#endregion

    //#region Diagnosing

    ////Diagnosing Results Method
    //public void Diagnosed(int order)
    //{
    //    if(order == 1)//Bedsores
    //    {
    //        diagnoseResults.text = "Bedsores at " + (int)sore + " out of " + soreMax;
    //    }
    //    if(order == 2)//Pain
    //    {
    //        diagnoseResults.text = "Pain at " + (int)pain + " out of " + painMax;
    //    }
    //    if (order == 3)//Overdose
    //    {
    //        diagnoseResults.text = "Drug Overdose at " + (int)over + " out of " + overMax;
    //    }
    //    if (order == 4)//Resistance
    //    {
    //        diagnoseResults.text = "Overdose Resistance at " + (int)fortify;
    //    }
    //    if(order == 5)//Nausea
    //    {
    //        diagnoseResults.text = "Nausea at " + (int)nausea + " out of " + nauseaMax;
    //    }
    //    if(order == 6)//Illness
    //    {
    //        diagnoseResults.text = "Illness at " + (int)sickness + " out of " + sickMax;
    //    }
    //    if(order == 7)//Mental illness
    //    {
    //        diagnoseResults.text = "Mental Illness at " + (int)mentalill + " out of " + mentalMax;
    //    }
    //}

    //#endregion

    //#region medicalCabinet
    //public void FillMed(string name, int power, float od)
    //{
    //    medType = name;
    //    strength = power;
    //    overD = od;
    //    contents = contentsMax;
    //}
    //#endregion

    //#region Cancel Call
    //public void Canceller(int button) //Closes all other buttons
    //{
    //    for(int i = 0; i < sourceButton.Length; i++)
    //    {
    //        if(i != button)
    //        {
    //            if(sourceButton[i].name == "Button_Sink")
    //            {
    //                sourceButton[i].GetComponent<SinkScript>().Cancel();
    //            }
    //            if (sourceButton[i].name == "Button_Diagnose")
    //            {
    //                sourceButton[i].GetComponent<DiagnoseButton>().Cancel();
    //            }
    //            if (sourceButton[i].name == "Button_Bed")
    //            {
    //                sourceButton[i].GetComponent<BedButton>().Cancel();
    //            }
    //            if (sourceButton[i].name == "TrayButton")
    //            {
    //                sourceButton[i].GetComponent<TrayButton>().Cancel();
    //            }
    //            if (sourceButton[i].name == "IVButton")
    //            {
    //                sourceButton[i].GetComponent<IVButton>().Cancel();
    //            }
    //            if (sourceButton[i].name == "DoorButton")
    //            {
    //                sourceButton[i].GetComponent<DoorButton>().Cancel();
    //            }
    //        }
    //    }
    //}
    //#endregion

}

public class Status{

    public string statusName;
    public float statusValue;
    public float decayValue;

    public Status(string statusName, float decayValue){

        this.statusName = statusName;
        this.statusValue = 100;
        this.decayValue = decayValue;

    }

    //Set the status value
    public void SetValue(float value){

        if(value <= 100 || value >= 0){
            this.statusValue = value;
        }else
        {
            Debug.Log("Value outside of range. Must be between 100-0.");
        }

    }

    //Add value to the status
    public void AddValue(float value){

        if(statusValue + value > 100){
            statusValue = 100;
        }else if(statusValue + value < 0){
            statusValue = 0;
        }else{
            statusValue += value;
        }

    }

}

public class Diagnosis {

    public string diagnosisName;
    public int maxDiagnosisValue;
    public int takeDamagePercent;
    public int damage;
    public float currentValue;
    public float decayValue;

    public Diagnosis(string diagnosisName, int maxDiagnosisValue, int takeDamagePercent, int damage, float decayValue){

        this.diagnosisName = diagnosisName;
        this.maxDiagnosisValue = maxDiagnosisValue;
        this.takeDamagePercent = takeDamagePercent;
        this.damage = damage;
        this.currentValue = 0;
        this.decayValue = decayValue;

    }

    public void AddValue(float value){
        
        if (currentValue + value > maxDiagnosisValue)
        {
            currentValue = maxDiagnosisValue;
        }
        else if (currentValue + value < 0)
        {
            currentValue = 0;
        }
        else
        {
            currentValue += value;
        }
    }

}

public class Task{

    public string name;
    public Player.position position;
    public string statusEffects;
    public float addValue;
    private GameObject player;

    public enum type
    {
        status,
        diagnosis
    }

    public type taskType;

    public Task(string name, Player.position position, string statusEffects, float addValue, GameObject player, type taskType)
    {

        this.name = name;
        this.position = position;
        this.statusEffects = statusEffects;
        this.player = player;
        this.addValue = addValue;
        this.taskType = taskType;

    }

    public bool atPosition()
    {
        if (position == Player.position.empty)
            return true;
        if (position == this.player.GetComponent<Player>().moveTo && (this.player.GetComponent<PathFinding>().currentPath == null))
            return true;
        else
        {
            return false;
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