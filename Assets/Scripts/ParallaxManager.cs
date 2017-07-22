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
	private float xInputToParallax; // how much of player's move vector goes into parallax movement?
	[SerializeField]
	private float xInputSpeedFactor; // affects overall speed
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

		yDelta = playerMove.transform.position.y - lastPlayerY;
		lastPlayerY = playerMove.transform.position.y;

		xSpeed += inputDir.x * xInputToParallax * xInputSpeedFactor * Time.deltaTime;

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
			layers [i].XParallax (xSpeed * xInputToParallax);
			layers [i].YParallax (yDelta * yParallaxFactor);
		}
	}
		
	public Vector2 SetDir (Vector2 _dir) {
		// this is the main input to the manager from the input class.
		inputDir = _dir;
		return new Vector2 (inputDir.x * (1 - xInputToParallax), inputDir.y);
	}

	public float GetLeftEdge(){
		return xLeftBounds;
	}
}
