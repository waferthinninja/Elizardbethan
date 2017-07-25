using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TongueController : MonoBehaviour
{

    public Transform DeadFlyParent; // this will be the collider object that we attach flies to when they are caught
    public Collider2D TongueCollider;

    private Animator _animator;
    private bool _isFiring;

    // Use this for initialization
    void Start()
    {
        _animator = GetComponent<Animator>();
        _isFiring = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isFiring)
        {
            LookAtMouse();
        }
        if (Input.GetButton("Fire1"))
        {
            ShootTongue();
        }

    }

    private void LookAtMouse()
    {
        // look at the mouse        
        Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float AngleRad = Mathf.Atan2(mouse.y - transform.position.y, mouse.x - transform.position.x);
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * AngleRad);
    }


    private void ShootTongue()
    {
        _isFiring = true;
        TongueCollider.enabled = true;
        _animator.Play("TongueLash");
    }

    public void ConsumeFlies()
    {
        foreach (Transform t in DeadFlyParent)
        {
            Destroy(t.gameObject); // replace with pooling at some point
                                   
            // do other stuff - give score, energy or whatever
            GameManager.Instance.ConsumeFly();
        }

        // turn off the collider
        TongueCollider.enabled = false;

        // reactivate head movement
        _isFiring = false;

        
    }
}
