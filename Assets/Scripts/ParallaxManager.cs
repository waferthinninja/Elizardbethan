﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxManager : MonoBehaviour {

    private static ParallaxManager instance;
    public static ParallaxManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<ParallaxManager>();
            return instance;
        }
    }

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

	private float lastPlayerY;
	private float yDelta = 0f;
	[SerializeField]
	private float yParallaxFactor;
	[SerializeField]
	private float xInputSpeedFactor;
	[SerializeField]
	private float xSpeed;
	[SerializeField]
	private float xMaxSpeed;

	[SerializeField]
	private float parallaxDrag;

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
		// iterate thru parallax layers and move them.
		for (int i = 0; i < layers.Length; i++) {
			layers [i].XParallax (xSpeed);
			layers [i].YParallax (yDelta * yParallaxFactor);
		}
	}

    public float GetXSpeed()
    {
        return xSpeed;
    }

	public void SetDir (Vector2 _dir) {
		inputDir = _dir;
	}

	public float GetLeftEdge(){
		return xLeftBounds;
	}

    public float GetRightEdge()
    {
        return xRightBounds;
    }

	public void SetDrag (float _drag) {
		parallaxDrag = _drag;
	}

	public void IncSpeed (float incBy) {
		xSpeed += incBy;
	}
}
