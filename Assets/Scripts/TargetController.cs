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
    private TestMove _mover;

    private bool _isTarget = true;
    
	void Start ()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _headController = GetComponentInParent<HeadController>();
        _mover = GetComponentInParent<TestMove>();
        _mover.RegisterOnStateChange(OnStateChange);
        SetTargetSprite();
	}
	
	void Update ()
    {
        _renderer.enabled = !_headController.IsFiring;
	}

    public void OnStateChange()
    {
        PlayerState state = _mover.GetPlayerState();
        if (state != PlayerState.Swimming && !_isTarget)
        {
            SetTargetSprite();
        }
        else if (state == PlayerState.Swimming && _isTarget)
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
