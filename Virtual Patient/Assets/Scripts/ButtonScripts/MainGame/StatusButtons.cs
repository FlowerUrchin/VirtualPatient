using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusButtons : MonoBehaviour {

    private GameManager gameManager;
    public UnityEngine.UI.Button button { get; set; }

    public string taskName;
    public Sprite taskImage;
    public Player.position taskPosition;
    public List<string> statusEffects;
    public float[] statusDamage;

    public List<string> conditionNames;
    public List<TaskCondition.Condition> conditions;
    public List<float> conditionValues;
    public List<TaskCondition> taskConditions;

    // Use this for initialization
    void Start()
    {

        //saves having to manually put the game manager in everytime
        gameManager = GameManager.instance;
        button = gameObject.GetComponent<UnityEngine.UI.Button>();
        button.onClick.AddListener(OnClick);

        //setup the conditions
        taskConditions = new List<TaskCondition>();
        for (int i = 0; i < conditionNames.Count; i++)
        {

            taskConditions.Add(new TaskCondition(conditionNames[i], conditions[i], conditionValues[i]));

        }

        if (this.taskName == "medicine")
        {
            switch (gameManager.player.GetComponent<Player>().patientType)
            {
                case Player.PatientType.ill:
                    this.statusEffects = new List<string>() { "Illness" };
                    this.conditionNames = new List<string>() { "Illness" };
                    break;

                case Player.PatientType.mental:
                    this.statusEffects = new List<string>() { "MentalState" };
                    this.conditionNames = new List<string>() { "MentalState" };
                    break;

                case Player.PatientType.phsyical:
                    this.statusEffects = new List<string>() { "Injury" };
                    this.conditionNames = new List<string>() { "Injury" };
                    break;
            }
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnClick()
    {

        if(this.taskName == "medicine")
        {
            switch(gameManager.player.GetComponent<Player>().patientType)
            {
                case Player.PatientType.ill:
                    this.statusEffects = new List<string>(){"Illness"};
                    this.conditionNames = new List<string>() { "Illness" };
                    break;

                case Player.PatientType.mental:
                    this.statusEffects = new List<string>() { "MentalState" };
                    this.conditionNames = new List<string>() { "MentalState" };
                    break;

                case Player.PatientType.phsyical:
                    this.statusEffects = new List<string>() { "Injury" };
                    this.conditionNames = new List<string>() { "Injury" };
                    break;
            }
        }

        //If there isn't already 3 tasks
        if (gameManager.currentTask.Count <= 2)
        {

            //If this task isn't already in the list
            if(!gameManager.currentTask.Exists((Task obj) => obj.name == taskName))
            {

                gameManager.currentTask.Add(new Task(taskName,
                                                     taskPosition,
                                                     statusEffects,
                                                     statusDamage,
                                                     gameManager.player,
                                                     taskConditions,
                                                     taskImage
                                                     ));

            }

        }

        //gameManager.currentTask.Add(new Task("Feed", Player.position.FoodTray, "hunger", 0.2f, gameManager.player, Task.type.status));

    }

}
