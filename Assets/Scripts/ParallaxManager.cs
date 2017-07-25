using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxManager : MonoBehaviour {

	// a class to manage all our parallax layers.
	// takes player input as a vector and determines
	// how much of that vector is parallax movement, and how much is 
	// player character movement.

	[SerializeField]
	private TestMove playerMove;
	[SerializeField]
	private TestInput playerInput;

	[SerializeField]
	private ParallaxLayer[] layers;

	[SerializeField]
	private float xLeftBounds;
	[SerializeField]
	private float xRightBounds;
	[SerializeField]
	private float yTopBounds;
	[SerializeField]
	private float yBottomBounds;

	private float lastPlayerY;
	private float yDelta = 0f;
	[SerializeField]
	private float yParallaxFactor;
	[SerializeField]
	private float xInputSpeedFactor; // how much does input affect speed?
	[SerializeField]
	private float xSpeed;
	[SerializeField]
	private float xMaxSpeed;

	[SerializeField]
	private float parallaxDrag;

	private float yScreen = 10f;

	private Vector2 inputDir;

	void Awake () {
		layers = new ParallaxLayer[transform.childCount];
		for (int i = 0; i < layers.Length; i++) {
			layers [i] = transform.GetChild (i).GetComponent<ParallaxLayer>();
		}
	}

	void Start () {
		lastPlayerY = playerMove.transform.position.y;

		for (int i = 0; i < layers.Length; i++) {
			layers [i].SetUpLayer (xRightBounds - xLeftBounds);
		}
	}

	void Update () {

		// track player y axis movement delta and apply it below as vertical parallax
		yDelta = playerMove.transform.position.y - lastPlayerY;
		lastPlayerY = playerMove.transform.position.y;

		inputDir = playerInput.GetPlayerInputVector ();

		// modify speed by input vector.
		xSpeed += inputDir.x * xInputSpeedFactor * Time.deltaTime; 

		// apply drag
		if (xSpeed > 0f) {
			xSpeed -= parallaxDrag * Time.deltaTime;
		}
		if (xSpeed > xMaxSpeed) {
			xSpeed = xMaxSpeed;
		}
		if (xSpeed < 0) {
			xSpeed = 0f;
		}
		for (int i = 0; i < layers.Length; i++) {
			layers [i].XParallax (xSpeed);
			layers [i].YParallax (yDelta * yParallaxFactor);
		}
	}

	public void SetDir (Vector2 _dir) {
		inputDir = _dir;
	}

	public float GetLeftEdge(){
		return xLeftBounds;
	}

	public void SetDrag (float _drag) {
		parallaxDrag = _drag;
	}

	public void IncSpeed (float incBy) {
		xSpeed += incBy;
	}
}
