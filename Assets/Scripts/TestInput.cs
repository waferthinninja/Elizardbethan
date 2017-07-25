using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInput : MonoBehaviour {

	// super simple movement input class

	public Vector2 dir;
	private float xAxis;
	private float yAxis;

	private TestMove mover;

	[SerializeField]
	private string buttonA = "space";

	void Awake () {
		dir = Vector2.zero;
		mover = GetComponent<TestMove> ();
	}
	
	void Update () {
		dir = Vector2.zero;
		xAxis = Input.GetAxis ("Horizontal");
		yAxis = Input.GetAxis ("Vertical");
		if (xAxis != 0f || yAxis != 0f) {
			dir = new Vector2 (xAxis, yAxis);
		}

		if (Input.GetKeyDown (buttonA)) {
			mover.ButtonA ();
		}
	}
		
	public Vector2 GetPlayerInputVector() {
		return dir;
	}
}
