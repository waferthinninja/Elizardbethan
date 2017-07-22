using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInput : MonoBehaviour {

	// super simple movement input class

	public Vector2 dir;
	private float xAxis;
	private float yAxis;

	[SerializeField]
	private ParallaxManager parallax;

	void Awake () {
		dir = Vector2.zero;
	}
	
	// Update is called once per frame
	void Update () {
		dir = Vector2.zero;
		xAxis = Input.GetAxis ("Horizontal");
		yAxis = Input.GetAxis ("Vertical");
		if (xAxis != 0f || yAxis != 0f) {
			dir = new Vector2 (xAxis, yAxis);
		}
		// part of the input is applied to parallax, the vector is modified, and the mover uses what's left.
		dir = parallax.SetDir (dir);
	}
		
}
