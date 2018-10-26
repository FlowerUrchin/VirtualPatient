using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskButton : MonoBehaviour {

    private GameManager gameManager;
    public UnityEngine.UI.Button button;
    public string task;

    // Use this for initialization
    void Start()
    {

        //saves having to manually put the game manager in everytime
        gameManager = GameManager.instance;
        button = gameObject.GetComponent<UnityEngine.UI.Button>();
        button.onClick.AddListener(CancelTask);

        this.button.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = task;

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CancelTask()
    {
        
        Task thistask = gameManager.currentTask.Find(x => x.name == task);
        thistask.CancelTask();
        gameManager.currentTask.Remove(thistask);
        gameManager.player.GetComponent<Player>().moveTo = Player.position.empty;
        if(gameManager.player.GetComponent<Player>().halfStanding)
        {
            gameManager.player.GetComponent<Player>().standing = !gameManager.player.GetComponent<Player>().standing;
        }

    }

}
