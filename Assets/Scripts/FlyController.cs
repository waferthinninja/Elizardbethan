using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum FlyState
{
    Dead,
    Hovering,
    Bimbling,
    Moving
}

public class FlyController : MonoBehaviour
{

    private FlyState _state;
    private float _timeToStateChange;
    private Vector3 _direction;
    private float _speed;

    public float bimbleSpeed = 2f;
    public float maxDeltaX = 2f;
    public float maxDeltaY = 1f;
    public float maxY = 5f;
    public float minY = -5f;
    public float minTimeInState = 0.5f;
    public float maxTimeInState = 1.5f;

    // Use this for initialization
    void Start()
    {
        PickRandomState();
    }

    // Update is called once per frame
    void Update()
    {

        if (_state == FlyState.Dead)
        {
            return;
        }

        _timeToStateChange -= Time.deltaTime;
        if (_timeToStateChange < 0)
        {
            PickRandomState();
        }

        if (_state == FlyState.Moving)
        {
            transform.Translate(_direction * _speed * Time.deltaTime);
        }
        else if (_state == FlyState.Bimbling)
        {
            float angle = Random.Range(0f, 360f);
            var direction = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle));
            transform.Translate(direction * bimbleSpeed * Time.deltaTime);
            if (transform.position.y < minY)
                transform.position = new Vector3(transform.position.x, minY);
            if (transform.position.y > maxY)
                transform.position = new Vector3(transform.position.x, maxY);
        }
    }

    private void PickRandomState()
    {
        // schedule next state change
        _timeToStateChange = Random.Range(minTimeInState, maxTimeInState);

        float r = Random.Range(0, 1f);
        if (r < 0.15)
        {
            _state = FlyState.Hovering;
        }
        else if (r < 0.5)
        {
            _state = FlyState.Bimbling;
        }
        else
        {
            _state = FlyState.Moving;
            SetRandomTarget();
        }

    }

    private void SetRandomTarget()
    {
        // pick a point within a box around current position
        float newX = Random.Range(transform.position.x - maxDeltaX, transform.position.x + maxDeltaX);
        float newY = Random.Range(transform.position.y - maxDeltaY, transform.position.y + maxDeltaY);

        // if new Y is off the top of the world (might not == screen) move it back on
        while (newY < minY) { newY += 1f; }
        while (newY > maxY) { newY -= 1f; }

        var target = new Vector3(newX, newY);

        // set speed and direction
        _speed = Vector3.Distance(transform.position, target) / _timeToStateChange;
        _direction = target - transform.position;
        _direction.Normalize();
    }

    public void Die()
    {
        _state = FlyState.Dead;
        // TODO disable animation
    }
}
