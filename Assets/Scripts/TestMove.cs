﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState 
{
	Running,
	Jumping,
	Diving,
	Swimming,
	Breaching
}

public class TestMove : MonoBehaviour {

	private Rigidbody2D rb2d;
	private CircleCollider2D col2d;
	private TestInput myInput;

    private HeadController head;

    public Action OnStateChange;
    public void RegisterOnStateChange(Action action) { OnStateChange += action;  }
    public void UnregisterOnStateChange(Action action) { OnStateChange -= action; }

    [SerializeField]
	private PlayerState _state;

	[SerializeField]
	private Vector2 dir;
    
	[SerializeField]
	private float jumpForce;
	[SerializeField]
	private float diveForce;
	[SerializeField]
	private float breachForce;
	[SerializeField]
	private float equilibriumFactor;
	[SerializeField]
	private float xHome = -4;

	[SerializeField]
	private float gravity = 1f;
	[SerializeField]
	private float buoyancy = -0.5f;
	[SerializeField]
	private float dragInAir = 0.15f;
	[SerializeField]
	private float dragInWater = 0.5f;

	[SerializeField]
	private float runSpeed;
	[SerializeField]
	private float moveRightForce;
	[SerializeField]
	private float moveRightBias;

	[SerializeField]
	private float swimForce;
	[SerializeField]
	private float swimSpeed;
	[SerializeField]
	private float swimStrokeTime = 0.2f;
	private float swimStrokeTimer;

    [SerializeField]
    private float obstacleForce = 100f;


    void Awake () {
		rb2d = GetComponent<Rigidbody2D> ();
		col2d = GetComponent<CircleCollider2D> ();
		myInput = GetComponent<TestInput> ();
	    head = GetComponentInChildren<HeadController>();
	}

	void Start () {
		_state = PlayerState.Running;
		rb2d.gravityScale = gravity;
		col2d.isTrigger = false;
        ParallaxManager.Instance.SetDrag (dragInAir);
	}

	public void ApplyImpulseForce (Vector2 dir) {
		rb2d.velocity = Vector2.zero;
		rb2d.AddForce (dir, ForceMode2D.Impulse);
	}

	void Update () {
		if (_state == PlayerState.Running) {
			Run ();
		} else if (_state == PlayerState.Swimming) {

            // hack to prevent swimming in air, probably a better way to handle
            if (transform.position.y > 0)
            {
                Breach();
            }

			swimStrokeTimer -= Time.deltaTime;
		}
	} 
		
	void FixedUpdate () {
		dir = myInput.GetPlayerInputVector(); 
		ApplyXEquilibriumForce ();
	}

	private void ApplyXEquilibriumForce() {
		float distFromHome = rb2d.position.x - xHome;
		rb2d.AddForce (Vector2.left * distFromHome * Time.deltaTime * equilibriumFactor);
	}

	public void ButtonA () {
		if (_state == PlayerState.Running) {
			ChangeState(PlayerState.Jumping);
			Jump ();
		} else if (_state == PlayerState.Jumping) {
			ChangeState(PlayerState.Diving);
			Dive ();
		} else if (_state == PlayerState.Swimming) {
			Swim ();
		}
	}

	void OnTriggerEnter2D (Collider2D other) {
		if (other.CompareTag ("SuperSurface")) {
			if (_state == PlayerState.Jumping ) {
				ChangeState(PlayerState.Running);
				col2d.isTrigger = false;
			} else if (_state == PlayerState.Breaching && rb2d.velocity.y <= 0f) {
				ChangeState(PlayerState.Running);
                ParallaxManager.Instance.SetDrag (dragInAir);
				col2d.isTrigger = false;
			} 
		}

		if (other.CompareTag ("SubSurface")) {
			if (_state == PlayerState.Diving) {
				ChangeState(PlayerState.Swimming);
				rb2d.gravityScale = buoyancy;
                ParallaxManager.Instance.SetDrag (dragInWater);
				swimStrokeTimer = 0f;
			} else if (_state == PlayerState.Swimming && rb2d.velocity.y >= 0) {
				Breach ();
			}
		}

        if (other.CompareTag ("Obstacle")) {
            ApplyImpulseForce(Vector2.left * obstacleForce);
        }
	}

	private void Jump () {
		ApplyImpulseForce (Vector2.up * jumpForce);
	}

	private void Dive () {
		ApplyImpulseForce (Vector2.down * diveForce);
		col2d.isTrigger = true;
	}

	private void Breach () {
		ChangeState(PlayerState.Breaching);
		ApplyImpulseForce (Vector2.up * breachForce);
		rb2d.gravityScale = gravity;
	}

	private void Run () {
        ParallaxManager.Instance.IncSpeed (runSpeed * Time.deltaTime);
		rb2d.AddForce (Vector2.right * (dir.x + moveRightBias) * moveRightForce * Time.deltaTime, ForceMode2D.Force);
	}

	private void Swim () {
		if (swimStrokeTimer <= 0f) {
			swimStrokeTimer = swimStrokeTime;
		    dir = new Vector2(Mathf.Cos(head.transform.eulerAngles.x*Mathf.Deg2Rad),
		                      Mathf.Sin(head.transform.eulerAngles.x*Mathf.Deg2Rad));
           // dir.Normalize();
		    rb2d.AddForce(dir * swimForce, ForceMode2D.Impulse);
            ParallaxManager.Instance.IncSpeed (swimSpeed);
		} 
	}

    public PlayerState GetPlayerState()
    {
        return _state;
    }

    private void ChangeState(PlayerState newState)
    {
        _state = newState;

        if (OnStateChange != null)
        {
            OnStateChange();
        }
    }
}
