using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sliders : MonoBehaviour {

    public Slider slider;
    public string role;

    // Use this for initialization
    void Start ()
    {
        slider = this.GetComponent<Slider>();
        role = this.gameObject.name;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(role == "Hunger Slider")
        {
            slider.value = GameManager.instance.getStatus("hunger").statusValue;
        }
        else if(role == "Thirst Slider")
        {
            slider.value = GameManager.instance.getStatus("thirst").statusValue;
        }
        else if (role == "IV Slider")
        {
            slider.value = 0;// GameManager.instance.GetIV();
        }
        else if (role == "Bladder Slider")
        {
            slider.value = GameManager.instance.getStatus("bladder").statusValue;
        }
        else if (role == "Bedpan Slider")
        {
            slider.value = 0;//GameManager.instance.GetBedpan();
        }
        else if (role == "Hygiene Slider")
        {
            slider.value = GameManager.instance.getStatus("hygiene").statusValue;
        }
        else if(role == "Sleep Slider")
        {
            slider.value = GameManager.instance.getStatus("tiredness").statusValue;
        }
	}
}
