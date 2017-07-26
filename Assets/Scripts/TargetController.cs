using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour {

    
    public Sprite TargetSprite;
    public Sprite ArrowSprite;
    public Color TargetColor;
    public Color ArrowColor;

    private HeadController _headController;
    private SpriteRenderer _renderer;

    private bool _isTarget = true;

	// Use this for initialization
	void Start () {
        _renderer = GetComponent<SpriteRenderer>();
        _headController = GetComponentInParent<HeadController>();
        SetTargetSprite();
	}
	
	// Update is called once per frame
	void Update () {
        _renderer.enabled = !_headController.IsFiring;

        // temp - prob want to Observe player state instead
        if (transform.position.y >= 0 && !_isTarget)
        {
            SetTargetSprite();
        }
        else if (transform.position.y < 0 && _isTarget)
        {
            SetArrowSprite();
        }

	}

    public void SetTargetSprite()
    {
        _isTarget = true;
        _renderer.sprite = TargetSprite;
        _renderer.color = TargetColor;
    }

    public void SetArrowSprite()
    {
        _isTarget = false;
        _renderer.sprite = ArrowSprite;
        _renderer.color = ArrowColor;
    }
}
