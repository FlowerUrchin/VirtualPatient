using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buttons : MonoBehaviour {

    bool awake;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        awake = GameManager.instance.IfAwake();
	}
    public void GiveFood()
    {
        if (awake)
        {
            GameManager.instance.Feed(10, 0.1f);
            //Work in Fullness, Fitness
        }
    }
    public void GiveDrink()
    {
        if (awake)
        {
            GameManager.instance.Drink(10);
        }
    }
    public void ToggleIV()
    {
        GameManager.instance.IVToggle();
    }
    public void FillIV()
    {
        GameManager.instance.RefillIV();
    }
    public void UseToilet()
    {
        if (awake)
        {
            GameManager.instance.Toilet();
        }
    }
    public void ToggleBedpan()
    {
        GameManager.instance.BedpanToggle();
    }
    public void EmptyBedpan()
    {
        GameManager.instance.EmptyBP();
    }
    public void Shower()
    {
        if (awake)
        {
            GameManager.instance.Clean(30); //Should be max clean
        }
    }
    public void Bedbath()
    {
        if (awake)
        {
            GameManager.instance.Clean(15);
        }
    }
    public void WashHands()
    {
        if (awake)
        {
            GameManager.instance.Clean(5);
        }
    }
    public void GivePainkiller()
    {
        if (awake)
        {
            //Get Strength and stuff from  Painkiller item
            GameManager.instance.Painkiller(10, 1);
        }
    }
    public void ShiftPatient()
    {
        GameManager.instance.ShiftPosition();
    }
    public void MassagePatient()
    {
        if (awake)
        {
            GameManager.instance.Massage();
        }
    }
    public void SleepingState()
    {
        GameManager.instance.SleepState();
    }
}
