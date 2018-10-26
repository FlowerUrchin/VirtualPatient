using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager instance = null;
    public GameObject player;
    public GameObject lighting;
    public GameObject scanner;

    //Objects
    public Text diagnoseResults;

    //Medical Condition
    bool sick, injured, internalinjured, mental;
    int state = 1; //1 = Bed, 2 = Weak, 3 = Recovering

    //Status Variables
    public Status[] statuses;
    public List<Diagnosis> diagnoses;

    //rates of decay
    public float decayWhileAwake;
    public float decayWhileAsleep;
    public float hungerDecay;
    public float thirstDecay;
    public float bladderDecay;
    public float hygieneDecay;
    public float tirednessDecay;

    //Diagnosis variables
    public float BedSoreDecay = 0.0069f;

    public float painDecay = 0.0046f;

    public float bloodToxicityDecay = 0.0011f;

    public float nauseDecay = 0.0046f;

    public List<Task> currentTask { get; set; }

    //Time Variables
    private float lastTime = 0;
    private float waitTime = 1;
    private float addValueLastTime = 0;
    private float addValueWaitTime = 1;

    public float lastDiagnoseTime { get; set; }

    public bool bedSoresRemoved { get; set; }
    public bool NauseaRemove { get; set; }

    public bool Continue { get; set; }

    //Diagnosis animation stage
    private float diagnosisAnimation = 0;

    //Called before start
    void Awake()
    {
        //setup the gamemanager
        if (instance == null)
        {
            instance = this;

            //Check if the game is a continue or not
            if(PlayerPrefs.GetInt("Continue") == 1)
            {
                Continue = true;
            }else
            {
                Continue = false;
            }

            //Load the correct player
            switch (PlayerPrefs.GetInt("PatientType"))
            {

                case 0:
                    this.player = GameObject.FindGameObjectWithTag("Patient1");
                    GameObject.FindGameObjectWithTag("Patient2").SetActive(false);
                    GameObject.FindGameObjectWithTag("Patient3").SetActive(false);
                    break;

                case 1:
                    this.player = GameObject.FindGameObjectWithTag("Patient2");
                    GameObject.FindGameObjectWithTag("Patient1").SetActive(false);
                    GameObject.FindGameObjectWithTag("Patient3").SetActive(false);
                    break;

                case 2:
                    this.player = GameObject.FindGameObjectWithTag("Patient3");
                    GameObject.FindGameObjectWithTag("Patient1").SetActive(false);
                    GameObject.FindGameObjectWithTag("Patient2").SetActive(false);
                    break;

            }

        }
        else if (instance == this)
        {
            Destroy(gameObject);
        }
    }

	// Use this for initialization
	void Start ()
    {



        //Bugfix for the player going spaz at the start of the game
        player.GetComponent<Player>().currentPosition = Player.position.empty;
        player.GetComponent<Player>().moveTo = Player.position.Bed;
        player.transform.localEulerAngles = new Vector3(312, 180, 0);

        currentTask = new List<Task>();
        lastDiagnoseTime = 0;

            bedSoresRemoved = false;
            player.GetComponent<Player>().Happiness = 100;
            //setup initial statuses
            statuses = new Status[]{
                                    new Status("hunger", hungerDecay),
                                    new Status("thirst", thirstDecay),
                                    new Status("bladder", bladderDecay),
                                    new Status("hygiene", hygieneDecay),
                                    new Status("tiredness", tirednessDecay),
                                    new Status("strength", 0)
                                   };

            //Set strength equal to 0;
            getStatus("strength").statusValue = 50;

            //Setup initial diagnoses
            diagnoses = new List<Diagnosis>(){
                                              new Diagnosis("Bed Sores", BedSoreDecay),
                                              new Diagnosis("Pain", painDecay),
                                              new Diagnosis("Nausea", nauseDecay),
                                              new Diagnosis("Blood Toxicity", bloodToxicityDecay)
                                             };

            //Add the player specific issue
            switch (player.GetComponent<Player>().patientType)
            {
                case Player.PatientType.phsyical:
                    diagnoses.Add(new Diagnosis("Injury", 0.0011f));
                    getDiagnosis("Injury").toggledDecay = true;
                    break;
                case Player.PatientType.mental:
                    diagnoses.Add(new Diagnosis("MentalState", 0.0011f));
                    getDiagnosis("MentalState").toggledDecay = true;
                    break;
                case Player.PatientType.ill:
                    diagnoses.Add(new Diagnosis("Illness", 0.0011f));
                    getDiagnosis("Illness").toggledDecay = true;
                    break;
            }

        //If this game is a continue the set the variables
        if(Continue)
        {

            getStatus("hunger").statusValue = PlayerPrefs.GetFloat("hungerValue");
            getStatus("thirst").statusValue = PlayerPrefs.GetFloat("thirstValue");
            getStatus("bladder").statusValue = PlayerPrefs.GetFloat("bladderValue");
            getStatus("hygiene").statusValue = PlayerPrefs.GetFloat("hygieneValue");
            getStatus("tiredness").statusValue = PlayerPrefs.GetFloat("tirednessValue");
            getStatus("strength").statusValue = PlayerPrefs.GetFloat("Strength");

            getDiagnosis("Pain").currentValue = PlayerPrefs.GetFloat("Pain");
            getDiagnosis("Blood Toxicity").currentValue = PlayerPrefs.GetFloat("BloodToxicity");

            //Setup the players evolution
            switch(PlayerPrefs.GetInt("evolution"))
            {
                case 0:
                    getDiagnosis("Nausea").currentValue = PlayerPrefs.GetFloat("Nausea");
                    getDiagnosis("Bed Sores").currentValue = PlayerPrefs.GetFloat("BedSores");
                    player.GetComponent<Player>().currentEvolution = Player.evolution.bad;
                    break;

                case 1:
                    getDiagnosis("Nausea").currentValue = PlayerPrefs.GetFloat("Nausea");
                    player.GetComponent<Player>().currentEvolution = Player.evolution.mild;
                    break;

                case 2:
                    player.GetComponent<Player>().currentEvolution = Player.evolution.good;
                    break;
            }

            //setup the patient type value
            switch(PlayerPrefs.GetInt("patientType"))
            {
                case 0:
                    getDiagnosis("Illness").currentValue = PlayerPrefs.GetFloat("Illness");
                    break;

                case 1:
                    getDiagnosis("MentalState").currentValue = PlayerPrefs.GetFloat("MentalState");
                    break;

                case 2:
                    getDiagnosis("Injury").currentValue = PlayerPrefs.GetFloat("Injury");
                    break;
            }

        }

	}

    //Save data
    private void OnApplicationQuit()
    {

        //Before the software is closed save all the variables
        //Save the status values
        PlayerPrefs.SetFloat("hungerValue", getStatus("hunger").statusValue);
        PlayerPrefs.SetFloat("thirstValue", getStatus("thirst").statusValue);
        PlayerPrefs.SetFloat("bladderValue", getStatus("bladder").statusValue);
        PlayerPrefs.SetFloat("hygieneValue", getStatus("hygiene").statusValue);
        PlayerPrefs.SetFloat("tirednessValue", getStatus("tiredness").statusValue);
        PlayerPrefs.SetFloat("Strength", getStatus("strength").statusValue);

        //Save the diagnosis values
        PlayerPrefs.SetFloat("Pain", getDiagnosis("Pain").currentValue);
        PlayerPrefs.SetFloat("BloodToxicity", getDiagnosis("Blood Toxicity").currentValue);

        //Save the current players state
        switch(player.GetComponent<Player>().currentEvolution)
        {
            case Player.evolution.bad:
                PlayerPrefs.SetFloat("Nausea", getDiagnosis("Nausea").currentValue);
                PlayerPrefs.SetFloat("BedSores", getDiagnosis("Bed Sores").currentValue);
                PlayerPrefs.SetInt("evolution", 0);
                break;

            case Player.evolution.mild:
                PlayerPrefs.SetFloat("Nausea", getDiagnosis("Nausea").currentValue);
                PlayerPrefs.SetInt("evolution", 1);
                break;

            case Player.evolution.good:
                PlayerPrefs.SetInt("evolution", 2);
                break;
        }

        switch(player.GetComponent<Player>().patientType)
        {
            case Player.PatientType.ill:
                PlayerPrefs.SetInt("patientType", 0);
                PlayerPrefs.SetFloat("Illness", getDiagnosis("Illness").currentValue);
                break;

            case Player.PatientType.mental:
                PlayerPrefs.SetInt("patientType", 1);
                PlayerPrefs.SetFloat("MentalState", getDiagnosis("MentalState").currentValue);
                break;

            case Player.PatientType.phsyical:
                PlayerPrefs.SetInt("patientType", 2);
                PlayerPrefs.SetFloat("Injury", getDiagnosis("Injury").currentValue);
                break;
        }

        //Set this to a continue game
        PlayerPrefs.SetInt("Continue", 1);

    }

    // Update is called once per frame
    void Update ()
    {

        //Determine if diagnosises should be enabled or not
        DiagnosisConditions();

        //Evolution Check
        EvolutionCondition();

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
                lighting.GetComponent<Light>().color = new Color(1, 0.95f, 0.839f, 1);

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

                    //If the current task is to excersie then continue the walking animation
                    if (currentTask[0].name == "exercise")
                    {
                        if (!player.GetComponent<Player>().walking)
                            player.GetComponent<Player>().toggleWalkAnimation(true);
                    }

                    if (currentTask[0].position == Player.position.Bed && player.GetComponent<Player>().standing)
                    {
                        player.GetComponent<Player>().GetInBed();
                    }
                    else
                    {

                        //If the player is at the door then make them invisible
                        if(currentTask[0].position == Player.position.Door)
                        {

                            player.transform.localScale = Vector3.zero;

                        }

                        //Commence the task on a time base increment
                        if (addValueLastTime <= Time.time)
                        {

                            addValueLastTime = Time.time + waitTime;

                            //If the task is to sleep and they're not already asleep
                            if (currentTask[0].name == "sleep" && player.GetComponent<Player>().playerState != Player.states.asleep)
                            {

                                //Change the players state to asleep and change the lighting to asleep mode
                                player.GetComponent<Player>().playerState = Player.states.asleep;
                                lighting.GetComponent<Light>().color = new Color(0.09f, 0.07f, 0.03f, 1);

                            }

                            if (currentTask[0].name == "diagnose")
                            {
                                //Diagnose the patient
                                if(scanner.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                                {
                                    scanner.GetComponent<Animator>().Play("MoveArm");
                                }

                                if(scanner.GetComponent<Animator>().GetAnimatorTransitionInfo(0).IsName("MoveArm -> Exit"))
                                {
                                    //Remove the task from the list
                                    diagnosisAnimation = 0;
                                    lastDiagnoseTime = Time.deltaTime;
                                    currentTask[0].ExitTask();
                                    currentTask.RemoveAt(0);
                                    player.GetComponent<Player>().moveTo = Player.position.empty;
                                }

                            }
                            else
                            {

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
                                        lighting.GetComponent<Light>().color = new Color(1, 0.95f, 0.839f, 1);

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
                else
                {

                    //If the player is invisible then make them visible
                    if (player.GetComponent<Player>().currentPosition != Player.position.Door && currentTask[0].position != Player.position.Door && player.transform.localScale == Vector3.zero)
                    {
                        player.transform.localScale = new Vector3(1, 1, 1);
                    }

                }

            }

        }
        else
        {

            //If the player is invisible then make them visible
            if(player.transform.localScale == Vector3.zero)
            {
                player.transform.localScale = new Vector3(1,1,1);
            }

        }

    }

    //This method checks to determine whether the player should evolve or not
    public void EvolutionCondition()
    {

        //Check if the player should evolve
        if(getStatus("strength").statusValue >= 100)
        {
            getStatus("strength").statusValue = 50;
            //ensure the player cannot go above good.
            if(player.GetComponent<Player>().currentEvolution != Player.evolution.good)
                player.GetComponent<Player>().currentEvolution += 1;
        }
        //Check if the player should devolve
        else if(getStatus("strength").statusValue <= 0)
        {
            getStatus("strength").statusValue = 50;
            //Check if the player should die
            if (player.GetComponent<Player>().currentEvolution != Player.evolution.bad)
                player.GetComponent<Player>().currentEvolution -= 1;
            else
            {

                PlayerPrefs.SetInt("Continue", 0);
                SceneManager.LoadScene(3, LoadSceneMode.Single);

            }

        }

    }

    //Skips forward in time
    public void SkipTime()
    {

        //Update every status with the decaying value * rate of decay
        foreach (Status status in statuses)
        {

            //Add decay value as negative
            status.AddValue(-((status.decayValue) * 6000));

        }

        foreach (Diagnosis diag in diagnoses)
        {

            //Add decay value as negative
            diag.AddValue(-((diag.decayValue) * 6000));

        }

    }

    //toggles diagnosis conditions
    void DiagnosisConditions()
    {

        //If the player is currently in the first state then the bedsores diagnosis is toggled
        if (player.GetComponent<Player>().currentEvolution == Player.evolution.bad)
        {

            //Make sure it exists
            if(diagnoses.Any(x => x.diagnosisName == "Bed Sores") == false)
            {
                diagnoses.Add(new Diagnosis("Bed Sores", BedSoreDecay));
                bedSoresRemoved = false;
            }

            //If the player is in bed
            if(player.GetComponent<Player>().currentPosition == Player.position.Bed)
            {
                getDiagnosis("Bed Sores").toggledDecay = true;  
            }else
            {
                getDiagnosis("Bed Sores").toggledDecay = false;
            }
        }
        else
        {

            if(!bedSoresRemoved)
            {
                //The player has evolved so you don't have to deal with bed sores anymore.
                bedSoresRemoved = true;
                diagnoses.Remove(getDiagnosis("Bed Sores"));
            }

        }

        //If the player is currently in the first state then the bedsores diagnosis is toggled
        if (player.GetComponent<Player>().currentEvolution == Player.evolution.good)
        {
            if (!NauseaRemove)
            {
                //The player has evolved so you don't have to deal with bed sores anymore.
                NauseaRemove = true;
                diagnoses.Remove(getDiagnosis("Nausea"));
            }
        }
        else
        {
            //Make sure it exists
            if (diagnoses.Any(x => x.diagnosisName == "Nausea") == false)
            {
                diagnoses.Add(new Diagnosis("Nausea", nauseDecay));
                NauseaRemove = false;
            }
        }

    }

    //Decaying Variables
    void Decay(float rateOfDecay){

        //Debug.Log(player.GetComponent<Player>().Happiness);
        //Debug.Log(getDiagnosis("Bed Sores").currentValue);

        //Every second
        if(lastTime <= Time.time){

            lastTime = Time.time + waitTime;

            //Update every status with the decaying value * rate of decay
            foreach (Status status in statuses)
            {

                //If the status is - then decrease the players happiness
                if(status.statusValue <= 30 && status.statusValue > 0)
                {
                    //Ensure happiness doesn't get less than 0
                    if (player.GetComponent<Player>().Happiness - 0.01f <= 0)
                        player.GetComponent<Player>().Happiness = 0;
                    else
                        player.GetComponent<Player>().Happiness -= 0.01f;
                }
                else if(status.statusValue <= 0)
                {
                    //Ensure happiness doesn't get less than 0
                    if (player.GetComponent<Player>().Happiness - 0.1f <= 0)
                        player.GetComponent<Player>().Happiness = 0;
                    else
                        player.GetComponent<Player>().Happiness -= 0.1f;
                }

                //Add decay value as negative
                status.AddValue(-((status.decayValue) * rateOfDecay));

            }

            foreach (Diagnosis diagnosis in diagnoses)
            {
                
                //If the diagnosis is toggled
                if(diagnosis.toggledDecay)
                {

                    //If the diagnosis is at 30% then decrease happiness
                    if (diagnosis.currentValue <= 0 || (diagnosis.currentValue) < 30)
                    {

                        //Ensure happiness doesn't get less than 0
                        if (player.GetComponent<Player>().Happiness - 0.01f <= 0)
                            player.GetComponent<Player>().Happiness = 0;
                        else
                            player.GetComponent<Player>().Happiness -= 0.01f;

                    }

                    //Decay value
                    diagnosis.AddValue(-((diagnosis.decayValue) * rateOfDecay));

                    Debug.Log(diagnosis.diagnosisName + ":" + diagnosis.currentValue);

                }

            }

            //If all the diagnoses are above 30% then increase happiness instead
            if(diagnoses.All(x => (x.currentValue) > 30))
            {
                //Ensure happiness doesn't get greater than 100
                if (player.GetComponent<Player>().Happiness + 0.01f >= 100)
                    player.GetComponent<Player>().Happiness = 100;
                else
                    player.GetComponent<Player>().Happiness += 0.005f;
            }

            //Increase strength based on happiness
            if (player.GetComponent<Player>().Happiness >= 75)
                getStatus("strength").AddValue(0.01f);
            else if (player.GetComponent<Player>().Happiness <= 25)
                getStatus("strength").AddValue(-0.01f);

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
    public float currentValue;
    public float decayValue;
    public bool toggledDecay;

    public Diagnosis(string diagnosisName, float decayValue){

        this.diagnosisName = diagnosisName;
        this.decayValue = decayValue;
        this.toggledDecay = true;
        this.currentValue = 100;

    }

    public void AddValue(float value){

        if (currentValue + value > 100)
        {
            currentValue = 100;
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

    public Task(string name, Player.position position, List<string> statusEffects, float[] addValue, GameObject player, List<TaskCondition> taskConditions, Sprite image)
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
        clone.GetComponent<Image>().sprite = image;
        taskBar = Object.Instantiate(clone, GameObject.Find("UI").transform);
        taskBar.transform.localPosition = new Vector3(-365 + GameManager.instance.currentTask.Count * 50, 165, 0);

    }

    public void ExitTask()
    {

        Object.Destroy(taskBar);

        GameManager.instance.player.GetComponent<Player>().toggleWalkAnimation(false);

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

        GameManager.instance.player.GetComponent<Player>().toggleWalkAnimation(false);

        if(this.position == Player.position.Door && GameManager.instance.player.transform.localScale == Vector3.zero)
        {
            player.transform.localScale = new Vector3(1, 1, 1);
        }

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

        if (this.name == "medicine")
            return type.diagnosis;

        //Check if it's a status
        for (int i = 0; i < gameManager.statuses.Length; i++)
        {

            if(gameManager.statuses[i].statusName == this.statusEffects[index])
                return type.status;

        }

        //Check if it's a status
        for (int i = 0; i < gameManager.diagnoses.Count; i++)
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
        value,
        instant
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
                type =  Task.type.status;

        }

        //Check if it's a status
        for (int i = 0; i < gameManager.diagnoses.Count; i++)
        {

            if (gameManager.diagnoses[i].diagnosisName == conditionName)
                type = Task.type.diagnosis;

        }

        if (type == Task.type.empty)
            throw new System.Exception("No status or diagnosis called: " + conditionName);

    }

    public bool conditionMet()
    {

        if (condition == Condition.instant)
            return true;

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
                else if(this.type == Task.type.diagnosis)
                {
                    if (GameManager.instance.getDiagnosis(conditionName).currentValue >= 100)
                        return true;
                }
                break;

            case Condition.empty:
                if (this.type == Task.type.status)
                {
                    if (GameManager.instance.getStatus(conditionName).statusValue <= 0)
                        return true;
                }
                else if (this.type == Task.type.diagnosis)
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
                else if (this.type == Task.type.diagnosis)
                {
                    if (GameManager.instance.getDiagnosis(conditionName).currentValue == value)
                        return true;
                }
                break;
                              
        }

        return false;

    }

}