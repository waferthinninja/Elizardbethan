using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<GameManager>();
            return instance;
        }
    }

    public float CurrentHealth { get; private set; }

    public float MaxHealth = 1000f;
    public float TirednessRate = -50f;
    public float HealthPerFly = 100f;


    // callbacks
    private Action OnHealthChange;
    public void RegisterOnHealthChange(Action action) { OnHealthChange += action; }
    public void UnregisterOnHealthChange(Action action) { OnHealthChange -= action; }


    private Action OnGameOver;
    public void RegisterOnGameOver(Action action) { OnGameOver += action; }
    public void UnregisterOnGameOver(Action action) { OnGameOver -= action; }

    void Start ()
    {
        CurrentHealth = MaxHealth;
	}
	
	// Update is called once per frame
	void Update ()
    {
        // health gradually falls
        // could potentially drop at different rates while underwater or whatever
        // but for now...
        ChangeHealth(TirednessRate * Time.deltaTime);
	}

    void GameOver()
    {
        if (OnGameOver != null)
        {
            OnGameOver();
        }
    }

    public void ConsumeFly()
    {
        ChangeHealth(HealthPerFly);
    }

    public void ChangeHealth(float deltaHealth)
    {
        CurrentHealth += deltaHealth;

        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);

        if (OnHealthChange != null)
        {
            OnHealthChange();
        }

        if (CurrentHealth <= 0)
        {
            GameOver();
        }

    }
}
