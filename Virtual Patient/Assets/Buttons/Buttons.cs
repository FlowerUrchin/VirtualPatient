using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buttons : MonoBehaviour {

    float X;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void GiveDrink()
    {
        GameManager.instance.Drink(10);
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
        GameManager.instance.Toilet();
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
        GameManager.instance.Clean(X);
    }
    public void Bedbath()
    {
        GameManager.instance.Clean(X);
    }
    public void WashHands()
    {
        GameManager.instance.Clean(X);
    }
    public void GivePainkiller()
    {
        //Get Strength and stuff from  Painkiller item
    }
    public void ShiftPatient()
    {
        GameManager.instance.ShiftPosition();
    }
    public void MassagePatient()
    {
        GameManager.instance.Massage();
    }
    public void SleepingState()
    {
        GameManager.instance.SleepState();
    }
}
