using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowerButton : MonoBehaviour {

    private GameManager gameManager;
    public UnityEngine.UI.Button button;

    // Use this for initialization
    void Start()
    {

        //saves having to manually put the game manager in everytime
        gameManager = GameManager.instance;
        button = gameObject.GetComponent<UnityEngine.UI.Button>();
        button.onClick.AddListener(Shower);

    }

    // Update is called once per frame
    void Update()
    {

    }

    void Shower()
    {

        if (gameManager.currentTask.Count > 0)
        {
            gameManager.currentTask.Clear();
        }

        gameManager.currentTask.Add(new Task("Shower", Player.position.Door, "hygiene", 0.2f, gameManager.player, Task.type.status));

    }

}
