using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMove : MonoBehaviour {

	// super simple physics based movement to test stuff
	private Rigidbody2D rb2d;
	private TestInput myInput;

	[SerializeField]
	private ParallaxManager parallax;

	[SerializeField]
	private float movementForceFactor;
	[SerializeField]
	private float equilibriumFactor;


	[SerializeField]
	private float xHome = -4;

	void Awake () {
		rb2d = GetComponent<Rigidbody2D> ();
		myInput = GetComponent<TestInput> ();
	}

	public void ApplyForce (Vector2 dir) {
		rb2d.AddForce (dir * movementForceFactor * Time.deltaTime, ForceMode2D.Force);
	}

	void FixedUpdate () {
		ApplyForce (myInput.dir);
		// if we're to the right of position, move us back. overdoing this looks wrong!
		// perhaps compensate by passing the parallax manager some value corresponding to the force applied.
		if (rb2d.position.x > xHome){
			float distFromHome = rb2d.position.x - xHome;
			rb2d.AddForce (Vector2.left * distFromHome * Time.deltaTime * equilibriumFactor);
		}
	}
}
