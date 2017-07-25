using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour {

    public Image HealthBar;
    public Text GameOverText;

	// Use this for initialization
	void Start ()
    {
        GameManager.Instance.RegisterOnHealthChange(OnHealthChange);
        GameManager.Instance.RegisterOnGameOver(OnGameOver);
    }
	
	// Update is called once per frame
	public void OnGameOver ()
    {
        GameOverText.enabled = true;		
	}

    public void OnHealthChange()
    {
        float perc = GameManager.Instance.CurrentHealth / GameManager.Instance.MaxHealth;
        //HealthBar.fillAmount = perc; // fillAmount not working?
        HealthBar.rectTransform.sizeDelta = new Vector2(512 * perc, 16); 
    }
}
