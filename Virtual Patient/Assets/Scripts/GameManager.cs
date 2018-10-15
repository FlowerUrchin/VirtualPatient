using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static GameManager instance = null;
    public GameObject player;
    public GameObject light;

    //Objects
    public Text diagnoseResults;

    //Medical Condition
    bool sick, injured, internalinjured, mental;
    int state = 1; //1 = Bed, 2 = Weak, 3 = Recovering

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
    private int maxBedSores = 500;
    private int bedSoresDamagePercent = 40;
    private int bedSoresDamage = 10;
    private float BedSoreDecay;

    private int maxPain = 1000;
    private int painDamagePercent = 60;
    private int painDamage = 20;
    private float painDecay;

    private int maxOverDose = 1000;
    private int overDoseDamagePercent = 60;
    private int overDoseDamage = 30;
    private float overDoseDecay;

    private int maxBloodToxicity = 1000;
    private int bloodToxicityPercent = 80;
    private int bloodToxicityDamage = 50;
    private float bloodToxicityDecay;

    public List<Task> currentTask { get; set; }

    private float bedsore { get; set; }
    private float pain { get; set; }
    private float nausea { get; set; }
    private float sickness { get; set; }
    private float mentalill { get; set; }

    //Time Variables
    private float lastTime = 0;
    private float waitTime = 1;
    private float addValueLastTime = 0;
    private float addValueWaitTime = 1;

    public float lastDiagnoseTime { get; set; }

    //static float pain = 0, painMax = 50, painChange = 0.2f; //Pain
    //static float nausea = 0, nauseaMax = 50, nauseaChange = 0.1f; //Nausea
    //static float sickness = 0, sickMax = 50, sickChange = 0.2f; //Sickness
    //static float mentalill = 0, mentalMax = 50, mentalChange = 0.2f; //Mentalilness

    //Medicine Variables
    static float over, overMax = 50, overChange = 0.1f; //Overdose

    //Medicine Bottle
    static string medType, requiredType;
    static int strength; //1-3
    static float overD;
    static int contents = 0, contentsMax = 10; //How many pills left

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

        currentTask = new List<Task>();
        lastDiagnoseTime = 0;

        //setup initial statuses
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

        //Setup initial diagnoses
        diagnoses = new Diagnosis[]{
            new Diagnosis("Bed Sores", maxBedSores, bedSoresDamagePercent, bedSoresDamage, BedSoreDecay),
            new Diagnosis("Pain", maxPain, painDamagePercent, painDamage, painDecay),
            new Diagnosis("Over Dose", maxOverDose, overDoseDamagePercent, overDoseDamage, overDoseDecay),
            new Diagnosis("Blood Toxicity", maxBloodToxicity, bloodToxicityPercent, bloodToxicityPercent, bloodToxicityDecay)
        };

	}

    // Update is called once per frame
    void Update ()
    {

        //update depending where the player is
        if (player.GetComponent<Player>().playerState == Player.states.awake)
        {
            Decay(decayWhileAwake);
        }else if (player.GetComponent<Player>().playerState == Player.states.asleep){
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
            if (currentTask[0].name != "sleep" && player.GetComponent<Player>().playerState == Player.states.asleep)
            {

                player.GetComponent<Player>().playerState = Player.states.awake;
                light.GetComponent<Light>().color = new Color(1, 0.95f, 0.839f, 1);

            }

            if (currentTask[0].position != Player.position.Bed && !player.GetComponent<Player>().standing)
            {

                player.GetComponent<Player>().GetOutOfBed();

            }
            else
            {

                //Set the players position to the position of the task
                this.player.GetComponent<Player>().moveTo = currentTask[0].position;

                //If the player is at the correct position for the task.
                if (currentTask[0].atPosition())
                {

                    if (currentTask[0].position == Player.position.Bed && player.GetComponent<Player>().standing)
                    {
                        player.GetComponent<Player>().GetInBed();
                    }
                    else
                    {

                        //Commence the task on a time base increment
                        if (addValueLastTime <= Time.time)
                        {

                            addValueLastTime = Time.time * waitTime;

                            //If the task is to sleep and they're not already asleep
                            if (currentTask[0].name == "sleep" && player.GetComponent<Player>().playerState != Player.states.asleep)
                            {

                                //Change the players state to asleep and change the lighting to asleep mode
                                player.GetComponent<Player>().playerState = Player.states.asleep;
                                light.GetComponent<Light>().color = new Color(0.09f, 0.07f, 0.03f, 1);

                            }

                            //For each status update the value
                            for (int i = 0; i < currentTask[0].statusEffects.Count; i++)
                            {

                                if (currentTask[0].statusType(i) == Task.type.status)
                                    getStatus(currentTask[0].statusEffects[i]).AddValue(currentTask[0].addValue[i]);
                                else if (currentTask[i].statusType(i) == Task.type.diagnosis)
                                    getDiagnosis(currentTask[0].statusEffects[i]).AddValue(currentTask[0].addValue[i]);

                            }

                            //If all the conditions have been met
                            if (currentTask[0].taskConditions.TrueForAll(x => x.conditionMet()))
                            {

                                //If the task was to sleep
                                if (currentTask[0].name == "sleep")
                                {

                                    //Wake up the patient and turn on the lights
                                    player.GetComponent<Player>().playerState = Player.states.awake;
                                    light.GetComponent<Light>().color = new Color(1, 0.95f, 0.839f, 1);

                                }

                                //Remove the task from the list
                                currentTask[0].ExitTask();
                                currentTask.RemoveAt(0);
                                player.GetComponent<Player>().moveTo = Player.position.empty;

                            }

                        }

                    }

                }

            }

        }

    }

    public void SkipTime()
    {

        //Update every status with the decaying value * rate of decay
        foreach (Status status in statuses)
        {

            //Add decay value as negative
            status.AddValue(-((status.decayValue / 100) * 1000));

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
    public List<string> statusEffects;
    public float[] addValue;
    private GameObject player;
    public List<TaskCondition> taskConditions;
    private GameObject taskBar;

    public enum type
    {
        empty,
        status,
        diagnosis
    }

    public Task(string name, Player.position position, List<string> statusEffects, float[] addValue, GameObject player, List<TaskCondition> taskConditions)
    {

        this.name = name;
        this.position = position;
        this.statusEffects = statusEffects;
        this.player = player;
        this.addValue = addValue;
        this.taskConditions = taskConditions;

        //Create task bar
        GameObject clone = Resources.Load("Task") as GameObject;
        clone.GetComponent<TaskButton>().task = this.name;
        taskBar = Object.Instantiate(clone, GameObject.Find("UI").transform);
        taskBar.transform.localPosition = new Vector3(-365 + GameManager.instance.currentTask.Count * 50, 165, 0);

    }

    public void ExitTask()
    {

        Object.Destroy(taskBar);

        //Move the other buttons
        foreach(Task task in GameManager.instance.currentTask)
        {

            task.taskBar.transform.localPosition = new Vector3(-365 + (GameManager.instance.currentTask.IndexOf(task) - 1) * 50, 165, 0);

        }

    }

    public void CancelTask()
    {

        Object.Destroy(taskBar);
        var testList = GameManager.instance.currentTask;

        testList.Remove(this);

        //Move the other buttons
        foreach (Task task in testList)
        {

            task.taskBar.transform.localPosition = new Vector3(-365 + (GameManager.instance.currentTask.IndexOf(task)) * 50, 165, 0);

        }

    }

    public bool atPosition()
    {
        if (position == Player.position.empty)
            return true;
        if (position == this.player.GetComponent<Player>().currentPosition)
            return true;
        else
        {
            return false;
        }

    }

    public type statusType(int index)
    {

        var gameManager = GameManager.instance;

        //Check if it's a status
        for (int i = 0; i < gameManager.statuses.Length; i++)
        {

            if(gameManager.statuses[i].statusName == this.statusEffects[index])
                return type.status;

        }

        //Check if it's a status
        for (int i = 0; i < gameManager.diagnoses.Length; i++)
        {

            if (gameManager.diagnoses[i].diagnosisName == this.statusEffects[index])
                return type.diagnosis;

        }

        throw new System.Exception("No status or Diagnosis named: " + this.statusEffects[index]);
            
    }

}

public class TaskCondition
{
    
    public string conditionName;
    private float value;
    private Task.type type = Task.type.empty;

    public enum Condition
    {
        full,
        empty,
        value
    }

    public Condition condition;

    public TaskCondition(string conditionName, Condition condition, float value)
    {

        this.conditionName = conditionName;
        this.condition = condition;
        this.value = value;

        //Get the conditions type on initialisation
        var gameManager = GameManager.instance;

        //Check if it's a status
        for (int i = 0; i < gameManager.statuses.Length; i++)
        {

            if (gameManager.statuses[i].statusName == conditionName)
                this.type =  Task.type.status;

        }

        //Check if it's a status
        for (int i = 0; i < gameManager.diagnoses.Length; i++)
        {

            if (gameManager.diagnoses[i].diagnosisName == conditionName)
                this.type = Task.type.diagnosis;

        }

        if (this.type == Task.type.empty)
            throw new System.Exception("No status or diagnosis called: " + conditionName);

    }

    public bool conditionMet()
    {

        //Depending on the set condition
        switch(condition)
        {

            //Check if that condition is met
            case Condition.full:
                if(this.type == Task.type.status)
                {
                    if (GameManager.instance.getStatus(conditionName).statusValue >= 100)
                        return true;
                }
                else if(this.type == Task.type.status)
                {
                    if (GameManager.instance.getDiagnosis(conditionName).currentValue >= GameManager.instance.getDiagnosis(conditionName).maxDiagnosisValue)
                        return true;
                }
                break;

            case Condition.empty:
                if (this.type == Task.type.status)
                {
                    if (GameManager.instance.getStatus(conditionName).statusValue <= 0)
                        return true;
                }
                else if (this.type == Task.type.status)
                {
                    if (GameManager.instance.getDiagnosis(conditionName).currentValue <= 0)
                        return true;
                }
                break;

            case Condition.value:
                if (this.type == Task.type.status)
                {
                    if (GameManager.instance.getStatus(conditionName).statusValue == value)
                        return true;
                }
                else if (this.type == Task.type.status)
                {
                    if (GameManager.instance.getDiagnosis(conditionName).currentValue == value)
                        return true;
                }
                break;
        }

        return false;

    }

}