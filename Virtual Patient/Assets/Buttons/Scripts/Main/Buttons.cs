﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    #region Buttons
    public void GiveFood()
    {
        if (awake)
        {
            GameManager.instance.Feed(1800, 1);
            //Work in Fullness, Fitness
            GameManager.instance.Canceller(10);//closes all buttons
        }
    }
    public void GiveDrink()
    {
        if (awake)
        {
            GameManager.instance.Drink(900);
            GameManager.instance.Canceller(10);//closes all buttons
        }
    }
    public void ToggleIV()
    {
        GameManager.instance.IVToggle();
        GameManager.instance.Canceller(10);//closes all buttons

    }
    public void FillIV()
    {
        GameManager.instance.RefillIV();
        GameManager.instance.Canceller(10);//closes all buttons
    }
    public void UseToilet()
    {
        if (awake)
        {
            GameManager.instance.Toilet();
            GameManager.instance.Canceller(10);//closes all buttons
        }
    }
    public void ToggleBedpan()
    {
        GameManager.instance.BedpanToggle();
        GameManager.instance.Canceller(10);//closes all buttons
    }
    public void EmptyBedpan()
    {
        GameManager.instance.EmptyBP();
        GameManager.instance.Canceller(10);//closes all buttons
    }
    public void Shower()
    {
        if (awake)
        {
            GameManager.instance.Clean(3600); //Should be max clean
            GameManager.instance.Canceller(10);//closes all buttons
        }
    }
    public void Bedbath()
    {
        if (awake)
        {
            GameManager.instance.Clean(1600);
            GameManager.instance.Canceller(10);//closes all buttons
        }
    }
    public void WashHands()
    {
        if (awake)
        {
            GameManager.instance.Clean(600);
            GameManager.instance.Canceller(10);//closes all buttons
        }
    }
    public void GivePainkiller()
    {
        if (awake)
        {
            //Get Strength and stuff from  Painkiller item
            GameManager.instance.Painkiller();
            GameManager.instance.Canceller(10);//closes all buttons
        }
    }
    public void ShiftPatient()
    {
        GameManager.instance.ShiftPosition();
        GameManager.instance.Canceller(10);//closes all buttons
    }
    public void MassagePatient()
    {
        if (awake)
        {
            GameManager.instance.Massage();
            GameManager.instance.Canceller(10);//closes all buttons
        }
    }
    public void SleepingState()
    {
        GameManager.instance.SleepState();
        GameManager.instance.Canceller(10);//closes all buttons
    }
    public void MedicalCabinet()
    {
        SceneManager.LoadScene("Medicine Cabinet");
    }
    #endregion

    #region Timeskip
    public void TimeSkipper()
    {
        GameManager.instance.TimeJumpMinute();
    }
    #endregion
}