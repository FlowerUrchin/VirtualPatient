using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToiletButton : MonoBehaviour {

    private GameManager gameManager;
    public UnityEngine.UI.Button button;

    // Use this for initialization
    void Start()
    {

        //saves having to manually put the game manager in everytime
        gameManager = GameManager.instance;
        button = gameObject.GetComponent<UnityEngine.UI.Button>();
        button.onClick.AddListener(Toilet);

    }

    // Update is called once per frame
    void Update()
    {

    }

    void Toilet()
    {

        if (gameManager.currentTask.Count > 0)
        {
            gameManager.currentTask.Clear();
        }

        gameManager.currentTask.Add(new Task("Toilet", Player.position.Door, "bladder", 0.2f, gameManager.player, Task.type.status));

    }

}
